using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
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
            while (GetFrameTask.Status == TaskStatus.Running) ;
            FreqTable.Dispose();
        }

        public bool PauseUpdate
        {
            get => m_PauseUpdate;

            set
            {
                Clear();
                Enable = !value;
                m_PauseUpdate = value;
                //FreqTraceBuffer.Clear();
                //FrameBuffer.Clear();
            }
        }

        public bool m_PauseUpdate = false;

        #region Basic Settings

        /// <summary>
        /// A.k.a Number of Points
        /// </summary>
        public int TracePoint
        {
            get => m_TracePointCount;
            private set
            {
                if (value < 3)
                {
                    m_TracePointCount = 3;
                }
                else if (value % 2 == 0)
                {
                    m_TracePointCount = value + 1;
                }
                else
                {
                    m_TracePointCount = value;
                }
            }
        }
        private int m_TracePointCount = -1;

        public double CenterFreq { get; private set; }

        public double Span { get; private set; }

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
            get => Span / (TracePoint - 1);
            private set => TracePoint = Convert.ToInt32(Math.Ceiling(Span / value));
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

        public Dictionary<int, double> Cursors { get; } = new();

        public ColorTheme CursorTheme { get; } = new(Color.White, Color.Green, Color.DarkGreen);

        public DatumColumn CursorTagColumn { get; } = new DatumColumn("Cursor", typeof(TagInfo));

        public void AddCursor(double freq)
        {
            if (freq >= StartFreq && freq <= StopFreq)
            {
                // double d = (freq - StartFreq) * (FreqTable.Count - 1.0) / Span;
                // int i = Convert.ToInt32(Math.Round(d, MidpointRounding.AwayFromZero));

                int i = FreqTable.GetIndex(freq);

                Console.WriteLine("AddCursor: freq = " + freq + " | index = " + i);

                if (!Cursors.ContainsKey(i))
                {
                    Cursors.Add(i, double.NaN);
                    FreqTable[i][CursorTagColumn] = new TagInfo(i, "NaN", DockStyle.Top, CursorTheme);
                }
            }
        }

        public void ClearCursors() 
        {
            foreach(var i in Cursors.Keys)
            {
                FreqTable[i][CursorTagColumn] = null;
            }
            Cursors.Clear();
        }

        public void ConfigureFreqRange(double center, double span)
        {
            lock (FreqTable.DataLockObject)
            {
                //if (count % 2 == 0) count++;

    
                CenterFreq = center;
                Span = span;

                // Console.WriteLine("Count = " + Count);

                FreqTable.Configure(StartFreq, StopFreq, TracePoint);
                FreqPoints = FreqTable.Rows.Select(n => n.Frequency).OrderBy(n => n).ToArray();

                // Console.WriteLine(FreqPoints.ToStringWithIndex());
            }
        }

        public void ConfigureCorrection(double offset, double multiply = 20)
        {
            // Generate Correction Header
            for (int i = 0; i < FreqTable.Count; i++)
            {
                FreqRow prow = FreqTable[i];
                prow[MultiCorrColumn] = multiply;
                prow[OffsetCorrColumn] = offset;
            }
        }

        public void ConfigureHistoDepth(int depth, int numOfPts = 800, int height = 1000)
        {
            if (depth != HistoDepth || numOfPts != TracePoint || PersistBufferHeight != height)
            {
                lock (FreqTable.DataLockObject)
                {
                    TracePoint = numOfPts;
                    PersistBufferHeight = height;

                    if (TracePoint > 2)
                    {
                        if (depth != HistoDepth) 
                        {
                            HistoFrames = new TraceFrame[depth];
                            HistoDepth = depth;
                        }

                        //List <TraceFrame> frames = new();
                        for (int i = 0; i < HistoDepth; i++)
                        {
                            HistoFrames[i] = new(i, TracePoint, PersistBufferHeight);
                            //frames.Add(new(i, TracePoint, PersistBufferHeight));
                        }
                        //HistoFrames = frames.ToArray();
                    }
                }

                GC.Collect();
            }

            HistoIndex = 0;
        }

        public void ConfigurePersist(int persistDepth = 64)
        {
            if (persistDepth != PersistDepth)
            {
                lock (FreqTable.DataLockObject)
                {
                    PersistDepth = persistDepth;
                    PersistColor = ColorTool.GetThermalGradient(PersistDepth, 58);
                }
            }
        }

        public bool Enable { get; set; }

        public bool EnableHisto { get; set; } = true;
        public int HistoIndex { get; private set; }
        public int HistoDepth { get; private set; }
        public TraceFrame[] HistoFrames { get; private set; }

        public bool EnablePersist { get; set; }
        public int PersistDepth { get; private set; }
        public Color[] PersistColor { get; private set; }
        public int PersistBufferHeight { get; private set; }

        #endregion Data

        #region Add Data

        public void RemoveStaleFreqTrace() 
        {
            while (FreqTraceQueue.Count > 2)
            {
                FreqTraceQueue.TryDequeue(out var tr);
                tr.IsUpdated = false;
                // Console.WriteLine("SpectrumData Overflow!");
            }
        }

        public void FreqTraceEnqueue(FreqTrace trace)
        {
            RemoveStaleFreqTrace();

            if (Enable && !PauseUpdate)
            {
                FreqTraceQueue.Enqueue(trace);
            }
        }

        public void Clear()
        {
            Enable = false;

            while (FreqTraceQueue.Count > 0)
                FreqTraceQueue.TryDequeue(out var _);

            while (FrameBuffer.Count > 0)
                FrameBuffer.TryDequeue(out var _);

            CurrentTraceFrame = null;
            PersistBitmapBuffer.Clear();

            if (HistoFrames is not null)
                for (int i = 0; i < HistoFrames.Length; i++)
                {
                    var frame = HistoFrames[i];

                    //frame.ClearPersistBuffer();
                    //frame.ClearPersistBitmap();

                    Parallel.For(0, TracePoint, i => {
                        FreqRow row = FreqTable[i];
                        //row.Clear();

                        row[frame.MagnitudeHighColumn] = double.NaN;
                        row[frame.MagnitudeLowColumn] = double.NaN;
                        row[frame.HighPixColumn] = double.NaN;
                        row[frame.LowPixColumn] = double.NaN;
                    });
                }
        }

        private CancellationTokenSource GetFrameCancellationTokenSource { get; } = new();

        private ConcurrentQueue<FreqTrace> FreqTraceQueue { get; } = new();

        public ConcurrentQueue<TraceFrame> FrameBuffer { get; } = new();

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

                if (FreqTraceQueue.Count > 0 && Enable && !PauseUpdate)
                {
                    if (FrameBuffer.Count < 3)
                    {
                        FreqTraceQueue.TryDequeue(out var tr);
                        CurrentTraceFrame = GetFrame(tr);
                        FrameBuffer.Enqueue(CurrentTraceFrame);
                    }
                    else if (FrameBuffer.Count > 2)
                    {
                        Console.WriteLine("FrameBuffer overflow!");
                        FrameBuffer.TryDequeue(out var _);
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

        public bool IsLog { get; } = true;

        public static NumericColumn MultiCorrColumn { get; } = new("MultiCorr");

        public static NumericColumn OffsetCorrColumn { get; } = new("OffsetCorr");

        private TraceFrame GetFrame(FreqTrace trace)
        {
            TraceFrame frame = HistoFrames[HistoIndex];

            lock (frame)
            {
                TraceDetector det;
                int traceCount = trace.Length;
                if (traceCount > TracePoint)
                {
                    det = Detector switch
                    {
                        TraceDetectorType.NegativePeak => new NegativePeakTraceDetector(trace),
                        TraceDetectorType.Average => new AverageTraceDetector(trace),
                        TraceDetectorType.Mean => new MeanTraceDetector(trace),
                        TraceDetectorType.RMS => new RmsTraceDetector(trace),
                        TraceDetectorType.Spline => new SplineTraceDetector(trace),
                        _ => new PeakTraceDetector(trace),
                    };
                }
                else if (traceCount == TracePoint)
                {
                    det = new TraceDetector(trace);
                }
                else
                {
                    det = new SplineTraceDetector(trace);
                }

                trace.IsUpdated = false;

                det.Evaluate(this, frame);

                foreach (int ci in Cursors.Keys) 
                {
                    double mag = Cursors[ci] = FreqTable[ci][frame.MagnitudeColumn];

                    if (FreqTable[ci][CursorTagColumn] is TagInfo ti) 
                    {
                        ti.Text = mag.ToString("0.##");
                    }
                }

                if (EnablePersist)
                {
                    for (int i = 0; i < TracePoint; i++)
                    // Parallel.For(0, Count, i =>
                    {
                        FreqRow row = FreqTable[i];

                        double h_value = row[frame.MagnitudeHighColumn];
                        double l_value = row[frame.MagnitudeLowColumn];

                        if (h_value > Y_Max) h_value = Y_Max;
                        if (l_value < Y_Min) l_value = Y_Min;

                        double full_height = Y_Range > 0 ? (PersistBufferHeight - 1) / Y_Range : 0;

                        double h_pix = Math.Round(full_height * (Y_Max - h_value), MidpointRounding.AwayFromZero);
                        double l_pix = Math.Round(full_height * (Y_Max - l_value), MidpointRounding.AwayFromZero);

                        /*
                        if (h_pix >= 800 || h_pix < 0)
                            Console.WriteLine("h_value = " + h_value + " | l_value = " + l_value + " | h_pix = " + h_pix + " | l_pix = " + l_pix);
                        */

                        if (h_pix > 799) h_pix = 800;
                        else if (h_pix < 0) h_pix = -1;
                        row[frame.HighPixColumn] = h_pix;

                        if (l_pix > 799) l_pix = 800;
                        else if (l_pix < 0) l_pix = -1;
                        row[frame.LowPixColumn] = l_pix;
                    }//);

                    // Getting the Histo BMP

                    int histo_index = HistoIndex;

                    frame.ClearPersistBuffer();

                    for (int z = 0; z < PersistDepth; z++) // Parallel.For(0, PersistDepth, z =>
                    {
                        TraceFrame histo_frame = HistoFrames[histo_index];
                        for (int x = 0; x < TracePoint; x++)
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

                if (CurrentTraceFrame is TraceFrame frame && (!frame.PersistBitmapValid) && Enable && EnablePersist && !PauseUpdate)
                {
                    GetPersistBitmap(frame);
                    PersistBitmapBuffer.Enqueue(frame);

                    if (cnt == 50)
                    {
                        TimeSpan span = DateTime.Now - time;
                        double fps = 50 / span.TotalSeconds;
                        Console.WriteLine("PersistBitmap: " + fps.ToString("0.###") + " fps");
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

        public void GetPersistBitmap(TraceFrame frame)
        {
            Bitmap bp = frame.PersistBitmap; // new(Count, PersistBufferHeight);

            frame.ClearPersistBitmap();

            lock (frame)
            {
                for (int x = 0; x < TracePoint; x++)
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
