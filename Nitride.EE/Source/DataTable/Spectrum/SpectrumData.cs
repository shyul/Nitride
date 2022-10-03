﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Nitride;

namespace Nitride.EE
{
    public sealed class SpectrumData : IDisposable //: IDataProvider
    {
        public SpectrumData()
        {
            //GetScaledTraceTask = new(() => GetGetScaledTraceWorker());
            GetPersistBitmapTask = new(() => GetPersistBitmapWorker());
            GetFrameTask = new(() => GetFrameWorker());

            //GetScaledTraceTask.Start();
            GetPersistBitmapTask.Start();
            GetFrameTask.Start();
        }

        ~SpectrumData() => Dispose();

        public void Dispose()
        {
            GetFrameCancellationTokenSource.Cancel();
            FreqTable.Dispose();
        }

        #region Basic Settings

        public double CenterFreq { get; private set; }

        public double Span { get; private set; }

        /// <summary>
        /// A.k.a Number of Points
        /// </summary>
        public int Count
        {
            get => m_Count;
            private set
            {
                if (value < 3)
                {
                    m_Count = 3;
                }
                else if (value % 2 == 0)
                {
                    m_Count = value + 1;
                }
                else
                {
                    m_Count = value;
                }
            }
        }
        private int m_Count = -1;

        public double StartFreq
        {
            get => CenterFreq - (Span / 2);
            private set
            {
                if (value < 0) value = 0;

                double start = value < StopFreq ? value : StopFreq;
                double stop = StopFreq;
                CenterFreq = (stop + start) / 2;
                Span = stop - start;
            }
        }

        public double StopFreq
        {
            get => CenterFreq + (Span / 2);
            private set
            {
                double stop = value > StartFreq ? value : StartFreq;
                double start = StartFreq;
                CenterFreq = (stop + start) / 2;
                Span = stop - start;
            }
        }

        public double FreqStep
        {
            get => Span / (Count - 1);
            private set => Count = Convert.ToInt32(Math.Ceiling(Span / value));
        }

        public TraceDetectorType Detector { get; set; } = TraceDetectorType.Peak;

        #endregion Basic Settings

        #region Y Axis

        public double Reference { get; private set; }

        public double Y_Range { get; private set; }

        public double Y_Max => Reference;

        public double Y_Min => Reference - Y_Range;

        public void ConfigureLevel(double reference, double range)
        {
            Reference = reference;
            Y_Range = range;
        }

        #endregion Y Axis

        #region Data

        public FreqTable FreqTable { get; } = new();

        public double[] FreqPoints { get; private set; }

        public void ConfigureRange(double center, double span, int count)
        {
            lock (FreqTable.DataLockObject)
            {
                //if (count % 2 == 0) count++;

                Count = count;
                CenterFreq = center;
                Span = span;

                // Console.WriteLine("Count = " + Count);

                FreqTable.Configure(StartFreq, StopFreq, Count);
                FreqPoints = FreqTable.Rows.Select(n => n.Frequency).OrderBy(n => n).ToArray();

                // Console.WriteLine(FreqPoints.ToStringWithIndex());
            }
        }

        public void ConfigureDepth(int depth, int persistDepth = 64, int height = 1000)
        {
            lock (FreqTable.DataLockObject)
            {
                if (Count > 2)
                {
                    HistoDepth = depth;
                    PersistDepth = persistDepth;
                    PersistBufferHeight = height;

                    List<TraceFrame> frames = new();

                    for (int i = 0; i < HistoDepth; i++)
                    {
                        frames.Add(new(i, Count, PersistBufferHeight));
                    }

                    HistoFrames = frames.ToArray();

                    /*
                    List<Color> persistColor = new();
                    // int colorStep = 256 / PersistDepth;
                    for (int i = 0; i < PersistDepth; i++)
                    {
                        //persistColor.Add(Color.FromArgb((colorStep * (i + 1) - 1), 96, 96, 96));
                        persistColor.Add(ColorTool.GetGradient(Color.FromArgb(96, 60, 119, 177), Color.FromArgb(128, 254, 135, 149), i * 1.0D / PersistDepth));
                    }*/
                    PersistColor = ColorTool.GetThermalGradient(PersistDepth, 58); // persistColor.ToArray();
                    //PersistBuffer = new int[Count, PersistBufferHeight];
                    //PersistBitmap = new(Count, PersistBufferHeight);

                }

                HistoIndex = 0;
            }
        }
        


        public int HistoIndex { get; private set; }
        public int HistoDepth { get; private set; }
        public TraceFrame[] HistoFrames { get; private set; }

        public bool PersistEnable { get; set; }
        public int PersistDepth { get; private set; }
        public Color[] PersistColor { get; private set; }
        //private int[,] PersistBuffer { get; set; }
        public int PersistBufferHeight { get; private set; }

        #endregion Data

        #region Add Data

        public void AppendTrace(IEnumerable<(double Freq, double Value)> trace)
        {
            if (Enable) 
            {
                //var frame = GetGetScaledTrace(trace);

                if (FreqTraceBuffer.Count < 3)
                    FreqTraceBuffer.Enqueue(trace.ToArray());
                else
                    FreqTraceBuffer.Dequeue();
            }
        }

        public void Clear()
        {
            Enable = false;

            FreqTraceBuffer.Clear();
            FrameBuffer.Clear();

            CurrentTraceFrame = null;
            PersistBitmapBuffer.Clear();

            if (HistoFrames is not null)
            for (int i = 0; i < HistoFrames.Length; i++)
            {
                var frame = HistoFrames[i];

                //frame.ClearPersistBuffer();
                //frame.ClearPersistBitmap();

                Parallel.For(0, Count, i => {
                    FreqRow row = FreqTable[i];
                    //row.Clear();

                    row[frame.HighValueColumn] = double.NaN;
                    row[frame.LowValueColumn] = double.NaN;
                    row[frame.HighPixColumn] = double.NaN;
                    row[frame.LowPixColumn] = double.NaN;
                });
            }
        }

        private CancellationTokenSource GetFrameCancellationTokenSource { get; } = new();

        private Queue<(double Freq, double Value)[]> FreqTraceBuffer { get; } = new();

        public Queue<TraceFrame> FrameBuffer { get; } = new();

        private Task GetFrameTask { get; }

        public void GetFrameWorker()
        {
            int cnt = 0;
            DateTime time = DateTime.Now;

            while (true)
            {
                if (GetFrameCancellationTokenSource.IsCancellationRequested)
                {
                    CurrentTraceFrame = null;
                    return;
                }  

                if (FreqTraceBuffer.Count > 0 && Enable)
                {
                    if (FrameBuffer.Count < 3)
                    {
                        CurrentTraceFrame = GetFrame(FreqTraceBuffer.Dequeue());
                        FrameBuffer.Enqueue(CurrentTraceFrame);
                    }
                    else if (FrameBuffer.Count > 2)
                    {
                        Console.WriteLine("FrameBuffer overflow!");
                        FrameBuffer.Dequeue();
                    }

                    if (cnt == 50)
                    {
                        TimeSpan span = DateTime.Now - time;
                        double fps = 50 / span.TotalSeconds;
                        Console.WriteLine("Time for Frame: " + fps.ToString("0.###") + " fps");
                        time = DateTime.Now;
                        cnt = 0;
                    }
                    else
                        cnt++;
                }
                else
                {
                    // if(!Enable) ClearFrame();
                    Thread.Sleep(10);
                }
            }


        }

        private TraceFrame GetFrame((double Freq, double Value)[] traceData)
        {
            TraceFrame frame = HistoFrames[HistoIndex];

            lock (frame)
            {
                TraceDetector ft;
                int traceCount = traceData.Count();
                if (traceCount > Count)
                {
                    ft = Detector switch
                    {
                        TraceDetectorType.NegativePeak => new NegativePeakTraceDetector(traceData),
                        TraceDetectorType.Average => new AverageTraceDetector(traceData),
                        TraceDetectorType.Mean => new MeanTraceDetector(traceData),
                        TraceDetectorType.RMS => new RmsTraceDetector(traceData),
                        TraceDetectorType.Spline => new SplineTraceDetector(traceData),
                        _ => new PeakTraceDetector(traceData),
                    };
                }
                else if (traceCount == Count)
                {
                    ft = new TraceDetector(traceData);
                }
                else
                {
                    ft = new SplineTraceDetector(traceData);
                }

                ft.Evaluate(FreqTable, frame);

                for (int i = 0; i < Count; i++)
                // Parallel.For(0, Count, i =>
                {
                    FreqRow row = FreqTable[i];
                    double h_value = row[frame.HighValueColumn];
                    double l_value = row[frame.LowValueColumn];

                    if (h_value > Y_Max) h_value = Y_Max;
                    if (l_value < Y_Min) l_value = Y_Min;

                    double full_height = (PersistBufferHeight - 1) / Y_Range;

                    double h_pix = Math.Round(full_height * (Y_Max - h_value), MidpointRounding.AwayFromZero);
                    double l_pix = Math.Round(full_height * (Y_Max - l_value), MidpointRounding.AwayFromZero);

                    // Console.WriteLine("h_value = " + h_value + " | l_value = " + l_value + " | h_pix = " + h_pix + " | l_pix = " + l_pix);

                    row[frame.HighPixColumn] = h_pix;
                    row[frame.LowPixColumn] = l_pix;
                }//);

                // Getting the Histo BMP

                int histo_index = HistoIndex;

                frame.ClearPersistBuffer();

                for (int z = 0; z < PersistDepth; z++)
                //Parallel.For(0, PersistDepth, z =>
                {
                    TraceFrame histo_frame = HistoFrames[histo_index];
                    for (int x = 0; x < Count; x++)
                    {
                        FreqRow prow = FreqTable[x];

                        double h_pix = prow[histo_frame.HighPixColumn];
                        double l_pix = prow[histo_frame.LowPixColumn];

                        // Console.WriteLine("h_pix = " + h_pix + " | l_pix = " + l_pix);

                        if (!double.IsNaN(h_pix) && !double.IsNaN(l_pix))
                        {
                            for (int y = Convert.ToInt32(h_pix); y <= l_pix; y++)  // h and l swapped
                            {
                                frame.PersistBuffer[x, y]++;
                            }
                        }
                    }

                    //Console.Write(" " + histo_index);

                    histo_index--;
                    if (histo_index < 0) histo_index = HistoDepth - 1;
                }//);

                //Console.Write("\n\r");


            }

            HistoIndex++;
            if (HistoIndex >= HistoDepth)
                HistoIndex = 0;

            return frame;
        }

        public TraceFrame CurrentTraceFrame { get; private set; }

        public Queue<TraceFrame> PersistBitmapBuffer { get; } = new();

        private Task GetPersistBitmapTask { get; }

        public void GetPersistBitmapWorker()
        {
            int cnt = 0;
            DateTime time = DateTime.Now;

            while (true)
            {
                if (GetFrameCancellationTokenSource.IsCancellationRequested)
                    return;

                if (CurrentTraceFrame is TraceFrame frame && (!frame.PersistBitmapValid) && Enable)
                {
                    GetPersistBitmap(frame);
                    PersistBitmapBuffer.Enqueue(frame);

                    if (cnt == 50)
                    {
                        TimeSpan span = DateTime.Now - time;
                        double fps = 50 / span.TotalSeconds;
                        Console.WriteLine("Time for PersistBitmap: " + fps.ToString("0.###") + " fps");
                        time = DateTime.Now;
                        cnt = 0;
                    }
                    else
                        cnt++;
                }
                else
                {
                    Thread.Sleep(5);
                }
            }
        }

        public bool Enable { get; set; }

        public void GetPersistBitmap(TraceFrame frame)
        {
            Bitmap bp = frame.PersistBitmap; // new(Count, PersistBufferHeight);

            frame.ClearPersistBitmap();

            lock (frame)
            {
                for (int x = 0; x < Count; x++)
                {
                    for (int y = 0; y < PersistBufferHeight; y++)
                    {
                        int z = frame.PersistBuffer[x, y];
                        if (z > 0)
                        {
                            bp.SetPixel(x, y, PersistColor[z - 1]);
                        }
                    }
                }
                frame.PersistBitmapValid = true;
            } 
        }

        #endregion Add Data
    }
}
