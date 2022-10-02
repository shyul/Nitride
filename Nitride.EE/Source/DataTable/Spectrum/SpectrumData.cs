using System;
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
                else if (value % 2 != 0)
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
            get => Span / Count;
            private set => Count = Convert.ToInt32(Math.Ceiling(Span / value));
        }

        public TraceDetectorType Detector { get; set; } = TraceDetectorType.Peak;

        #endregion Basic Settings

        #region Y Axis

        public double Reference { get; set; }

        public double Y_Range { get; set; }

        public double Y_Max => Reference;

        public double Y_Min => Reference - Y_Range;

        public double Y_DivRange { get; set; }

        #endregion Y Axis

        #region Data

        public FreqTable FreqTable { get; } = new();

        public double[] FreqPoints { get; private set; }

        public void ConfigureRange(double center, double span, int count)
        {
            lock (FreqTable.DataLockObject)
            {
                Count = count;
                CenterFreq = center;
                Span = span;

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

        public void AppendTrace((double Freq, double Value)[] trace)
        {
            if (Enable) 
            {
                var frame = GetGetScaledTrace(trace);
                ScaledTraceBuffer.Enqueue(frame);
            }


            /*
            if (TraceBuffer.Count < 3)
                TraceBuffer.Enqueue(trace);
            else
                TraceBuffer.Dequeue();*/
        }

        public void Clear()
        {
            Enable = false;
            //TraceBuffer.Clear();

            ScaledTraceBuffer.Clear();
            FrameBuffer.Clear();

            CurrentTraceFrame = null;

            for (int i = 0; i < HistoFrames.Length; i++)
            {
                var frame = HistoFrames[i];

                frame.ClearPersistBuffer();
                //frame.ClearPersistBitmap();

                Parallel.For(0, Count, i => {
                    FreqRow row = FreqTable[i];
                    row.Clear();

                    // row[frame.HighValueColumn] = double.NaN;
                    // row[frame.LowValueColumn] = double.NaN;
                    // row[frame.HighPixColumn] = double.NaN;
                    // row[frame.LowPixColumn] = double.NaN;
                });
            }

            PersistBitmapBuffer.Clear();

            /*
            HistoFrames.RunEach(n => {
                lock (n)
                {
                    n.ClearPersistBuffer();

                    lock (n.PersistBitmap)
                        n.ClearPersistBitmap();
                }
            });*/
        }

        private CancellationTokenSource GetFrameCancellationTokenSource { get; } = new();

        //private Queue<(double Freq, double Value)[]> TraceBuffer { get; } = new();

        private Queue<TraceFrame> ScaledTraceBuffer { get; } = new();

        //private Task GetScaledTraceTask { get; }
        /*
        public void GetGetScaledTraceWorker()
        {
            int cnt = 0;
            DateTime time = DateTime.Now;

            while (true)
            {
                if (GetFrameCancellationTokenSource.IsCancellationRequested)
                    return;

                if (TraceBuffer.Count > 0)
                {
                    if (FrameBuffer.Count < 3)
                    {
                        var frame = GetGetScaledTrace(TraceBuffer.Dequeue());
                        ScaledTraceBuffer.Enqueue(frame);
                    }
                    else if (FrameBuffer.Count > 2)
                    {
                        Console.WriteLine("ScaledTraceBuffer overflow!");
                        ScaledTraceBuffer.Dequeue();
                    }

                    if (cnt == 50)
                    {
                        TimeSpan span = DateTime.Now - time;
                        double fps = 50 / span.TotalSeconds;
                        Console.WriteLine("Time for Scaled Trace: " + fps.ToString("0.###") + " fps");
                        time = DateTime.Now;
                        cnt = 0;
                    }
                    else
                        cnt++;
                }
                else
                {
                    Thread.Sleep(10);
                }
            }
        }*/

        private TraceFrame GetGetScaledTrace(IEnumerable<(double Freq, double Value)> traceData)
        {
            FreqTrace ft;
            TraceFrame frame = HistoFrames[HistoIndex];
            int traceCount = traceData.Count();

            if (traceCount > Count)
            {
                ft = Detector switch
                {
                    TraceDetectorType.NegativePeak => new FreqTraceNegativePeak(traceData),
                    TraceDetectorType.Average => new FreqTraceAverage(traceData),
                    TraceDetectorType.Mean => new FreqTraceMean(traceData),
                    TraceDetectorType.RMS => new FreqTraceRms(traceData),
                    TraceDetectorType.Spline => new FreqTraceSpline(traceData),
                    _ => new FreqTracePeak(traceData),
                };
            }
            else if (traceCount == Count)
            {
                ft = new FreqTrace(traceData);
            }
            else
            {
                ft = new FreqTraceSpline(traceData);
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

            return frame;
        }

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

                if (ScaledTraceBuffer.Count > 0 && Enable)
                {
                    if (FrameBuffer.Count < 3)
                    {

                        //Task.Run(() => GetFrame(ScaledTraceBuffer.Dequeue()));

                        //Thread.Sleep(100);


                        CurrentTraceFrame = GetFrame(ScaledTraceBuffer.Dequeue());
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

        private TraceFrame GetFrame(TraceFrame frame)
        {
            lock (frame)
            {
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


                //frame.ApplyPersistBitmap(PersistColor);

                /*
                lock (frame.PersistBitmap) 
                {
                   // PersistBitmap

                    for (int x = 0; x < Count; x++)
                    {
                        for (int y = 0; y < PersistBufferHeight; y++)
                        {
                            int z = frame.PersistBuffer[x, y] - 1;
                            if (z >= 0)
                            {
                                //Console.WriteLine("############### z = " + z);
                                frame.PersistBitmap.SetPixel(x, y, PersistColor[z]);
                            }
                            else
                            {
                               // frame.PersistBitmap.SetPixel(x, y, Color.Transparent);
                            }
                        }
                    }
                }*/

                //FrameBuffer.Enqueue(frame);


                HistoIndex++;
                if (HistoIndex >= HistoDepth)
                    HistoIndex = 0;
            }
            return frame;
        }



        public TraceFrame CurrentTraceFrame { get; private set; }

        public Queue<Bitmap> PersistBitmapBuffer { get; } = new();

        private Task GetPersistBitmapTask { get; }

        public void GetPersistBitmapWorker()
        {
            int cnt = 0;
            DateTime time = DateTime.Now;

            while (true)
            {
                if (GetFrameCancellationTokenSource.IsCancellationRequested)
                    return;

                if (CurrentTraceFrame is TraceFrame frame && Enable)
                {
                    var bp = GetPersistBitmap(frame);
                    PersistBitmapBuffer.Enqueue(bp);

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
                


            }
        }

        public bool Enable { get; set; }

        public Bitmap GetPersistBitmap(TraceFrame frame)
        {
            //Bitmap bp = frame.PersistBitmap;
            Bitmap bp = new(Count, PersistBufferHeight);
            try
            {
                lock (frame)
                {
                    
                    lock (bp)
                    {
                        //using Graphics gb = Graphics.FromImage(bp);
                        //gb.FillRectangle(TransparentBrush, 0, 0, Count, PersistBufferHeight);
                        //frame.ClearPersistBitmap();
                        for (int x = 0; x < Count; x++)
                        {
                            for (int y = 0; y < PersistBufferHeight; y++)
                            {
                                int z = frame.PersistBuffer[x, y];
                                if (z > 0)
                                {
                                    //Console.WriteLine("############### z = " + z);
                                    bp.SetPixel(x, y, PersistColor[z - 1]);
                                }
                                else
                                {
                                    //bp.SetPixel(x, y, Color.Transparent);
                                }
                            }
                        }
                    }

                
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return bp;
        }

        #endregion Add Data
    }
}
