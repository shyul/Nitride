/// ***************************************************************************
/// Nitride Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// SpectrumChart
/// 
/// ***************************************************************************
/// 
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nitride;
using Nitride.Chart;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace Nitride.EE
{
    public sealed class SpectrumChart : ChartWidget
    {
        public SpectrumChart(string name, SpectrumData sd) : base(name)
        {
            TabName = Name = name;
            Data = sd;

            Margin = new Padding(5, 15, 5, 5);
            Theme.FillColor = BackColor = Color.FromArgb(255, 255, 253, 245);
            Theme.EdgeColor = Theme.ForeColor = Color.FromArgb(192, 192, 192);

            Style[Importance.Major].Font = Main.Theme.FontBold;
            Style[Importance.Major].HasLabel = true;
            Style[Importance.Major].HasLine = true;
            Style[Importance.Major].Theme.EdgePen.DashPattern = new float[] { 5, 3 };
            Style[Importance.Major].Theme.EdgePen.Color = Color.FromArgb(180, 180, 180);

            Style[Importance.Minor].Font = Main.Theme.Font;
            Style[Importance.Minor].HasLabel = true;
            Style[Importance.Minor].HasLine = true;
            Style[Importance.Minor].Theme.EdgePen.DashPattern = new float[] { 1, 2 };

            AddArea(MainArea = new Area(this, "Main", 0.3f)
            {
                Reference = 0,
                HasXAxisBar = true,
            });

            MainAxis = MainArea.AxisY(AlignType.Right);

            MainAxis.TickStep = 10;
            MainAxis.FixedRange = true;
            MainAxis.FixedMinimum = -120;
            MainAxis.FixedMaximum = 0;

            EnableChartShift = false;

            MainLineSeries = new LineSeries(LineType.Default, 1, 0)
            {
                Order = 0,
                Importance = Importance.Major,
                Name = "FFT Spectrum",
                LegendName = "FFT Spectrum",
                Color = Color.FromArgb(128, 128, 128, 128),
                IsAntialiasing = true,
                HasTailTag = false
            };

            MainArea.AddSeries(MainLineSeries);

            ReadyToShow = true;
            Location = new Point(0, 0);
            Dock = DockStyle.Fill;


            DataUpdateTask = new(() => DataUpdateWorker());
            DataUpdateTask.Start();

            ResumeLayout(false);
            PerformLayout();
        }

        protected override void Dispose(bool disposing)
        {
            DataUpdateCancellationTokenSource.Cancel();
            base.Dispose(disposing);
        }

        private SpectrumData Data { get; }

        private FreqTable FreqTable => Data.FreqTable;

        private Area MainArea { get; }

        private Area HistoArea { get; }

        private ContinuousAxis MainAxis { get; }

        public LineSeries MainLineSeries { get; }

        #region Data Update

        private Task DataUpdateTask { get; }

        private CancellationTokenSource DataUpdateCancellationTokenSource { get; } = new();

        public void DataUpdateWorker()
        {
            while (true)
            {
                if (DataUpdateCancellationTokenSource.IsCancellationRequested)
                    return;

                if (Data.FrameBuffer.Count > 0 && m_AsyncUpdateUI == false) // && Graphics is not busy!!
                {
                    CurrentTraceFrame = Data.FrameBuffer.Dequeue();
                    MainLineSeries.AssignMainDataColumn(CurrentTraceFrame.TraceColumn);



                    m_AsyncUpdateUI = true;
                }
                else
                {
                    Thread.Sleep(10);
                }
            }
        }

        public TraceFrame CurrentTraceFrame { get; private set; }

        public override void DataIsUpdated(IDataProvider _) => m_AsyncUpdateUI = true;

        #endregion Data Update

        public override int RightBlankAreaWidth => 0;

        public override string this[int i]
        {
            get
            {
                if (Data.FreqTable[i] is FreqRow sp && sp.Frequency is double d)
                    return (d / 1e6).ToString("0.###") + "MHz";
                else
                    return string.Empty;
            }
        }

        public override ITable Table => Data.FreqTable;

        public override bool ReadyToShow { get => m_ReadyToShow && CurrentTraceFrame is not null && Table.Count > 0 ; set { m_ReadyToShow = value; } } // && Data.Enable

        public double[] TickDacades { get; set; } = new double[]
            { 0.1, 0.2, 0.25, 0.3, 0.4, 0.5, 0.6, 0.8, 1 };

        static SolidBrush TransparentBrush { get; } = new(Color.Transparent);

        public override void CoordinateOverlay()
        {
            ResumeLayout(true);
            ChartBounds = new Rectangle(
                LeftYAxisLabelWidth + Margin.Left,
                Margin.Top,
                ClientRectangle.Width - LeftYAxisLabelWidth - Margin.Left - RightYAxisLabelWidth - Margin.Right,
                ClientRectangle.Height - Margin.Top - Margin.Bottom
                );

            if (ReadyToShow)
            {
                lock (FreqTable.DataLockObject)
                    lock (GraphicsLockObject)
                    {
                        /*
                        Bitmap pf = Data.PersistBitmap;

                        for (int x = 0; x < Data.Count; x++)
                        {
                            for (int y = 0; y < Data.PersistBufferHeight; y++)
                            {
                                int z = CurrentTraceFrame.PersistBuffer[x, y] - 1;
                                if (z >= 0)
                                {
                                    //Console.WriteLine("############### z = " + z);
                                    pf.SetPixel(x, y, Data.PersistColor[z]);
                                }
                                else
                                {
                                    // frame.PersistBitmap.SetPixel(x, y, Color.Transparent);
                                }
                            }
                        }*/

                        if (Data.PersistBitmapBuffer.Count > 0 && Data.Enable)
                        {
                            if (PersistBitmap is not null)
                            {
                                PersistBitmap.Dispose();
                                /*
                                lock (PersistBitmap) 
                                {
                                    using Graphics gb = Graphics.FromImage(PersistBitmap);
                                    gb.FillRectangle(TransparentBrush, 0, 0, Data.Count, Data.PersistBufferHeight);
                                }*/
                            }

                            PersistBitmap = Data.PersistBitmapBuffer.Dequeue();
                        }

                        //UpdatePersistBuffer();
                        AxisX.TickList.Clear();

                        //int tickMulti = 1;
                        //int tickWidth = AxisX.TickWidth;
                        double minTextWidth = TextRenderer.MeasureText("0.0000MHz", Style[Importance.Major].Font).Width * 1D;

                        //while (tickWidth <= minTextWidth) { tickWidth += tickWidth; tickMulti++; }


                        int px = 0;

                        //int totalTicks = (StopPt - StartPt) / tickMulti;

                        double tickNum = Width / minTextWidth;

                        if (tickNum > 0)
                        {
                            int tickStep = Math.Round((StopPt - StartPt) / tickNum).ToInt32();

                            // Console.WriteLine("tickStep = " + tickStep);

                            // TODO: Fix THIS!!
                            for (int i = StartPt; i < Math.Min(FreqTable.Count, StopPt); i++)
                            {
                                double freq = FreqTable[i].Frequency;

                                if (i % tickStep == 0) AxisX.TickList.CheckAdd(px, (Importance.Major, (freq / 1e6).ToString("0.###") + "MHz"));



                                px++;
                            }

                            if (ChartBounds.Width > RightBlankAreaWidth)
                            {
                                AxisX.IndexCount = IndexCount;
                                AxisX.Coordinate(ChartBounds.Width - RightBlankAreaWidth);

                                int ptY = ChartBounds.Top;
                                float totalY = TotalAreaHeightRatio;

                                if (AutoScaleFit)
                                {
                                    foreach (Area ca in Areas)
                                    {
                                        if (ca.Visible && ca.Enabled)
                                        {
                                            if (ca.HasXAxisBar)
                                            {
                                                ca.Bounds = new Rectangle(ChartBounds.X, ptY, ChartBounds.Width, (ChartBounds.Height * ca.HeightRatio / totalY - AxisXLabelHeight).ToInt32());
                                                ptY += ca.Bounds.Height + AxisXLabelHeight;
                                                ca.TimeLabelY = ca.Bounds.Bottom + AxisXLabelHeight / 2 + 1;
                                            }
                                            else
                                            {
                                                ca.Bounds = new Rectangle(ChartBounds.X, ptY, ChartBounds.Width, (ChartBounds.Height * ca.HeightRatio / totalY).ToInt32());
                                                ptY += ca.Bounds.Height;
                                            }
                                            ca.Coordinate();
                                        }
                                    }
                                }
                                else { }
                            }
                        }





                    }
            }
        }

        Bitmap PersistBitmap { get; set; }

        protected override void OnPaint(PaintEventArgs pe)
        {
            Graphics g = pe.Graphics;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            if (Table is null || DataCount < 1)
            {
                g.DrawString("No Data", Main.Theme.FontBold, Main.Theme.GrayTextBrush, new Point(Bounds.Width / 2, Bounds.Height / 2), AppTheme.TextAlignCenter);
            }
            else if (IsActive && !ReadyToShow)
            {
                g.DrawString("Preparing Data... Stand By.", Main.Theme.FontBold, Main.Theme.GrayTextBrush, new Point(Bounds.Width / 2, Bounds.Height / 2), AppTheme.TextAlignCenter);
            }
            else if (ReadyToShow && ChartBounds.Width > 0 && Table is ITable t)
            {
                lock (t.DataLockObject)
                    lock (GraphicsLockObject)
                    {
                        //g.DrawImage(CurrentTraceFrame.PersistBitmap, MainArea.Bounds.Left, MainArea.Bounds.Top);
                        //g.DrawImage(CurrentTraceFrame.PersistBitmap, MainArea.Bounds.Left + 2, MainArea.Bounds.Top + 2);
                        //g.DrawImage(CurrentTraceFrame.PersistBitmap, 0, 0);

                        // Bitmap pf = CurrentTraceFrame.PersistBitmap;

                        //lock (pf) 

                        //using Bitmap pf = new(Data.Count, Data.PersistBufferHeight); // Data.PersistBitmap;

                        try
                        {
                            if (PersistBitmap is not null)
                                lock (PersistBitmap)
                                    g.DrawImage(PersistBitmap, MainArea.DataBounds);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        //using Graphics gb = Graphics.FromImage(pf);
                        //gb.FillRectangle(TransparentBrush, 0, 0, pf.Width, pf.Height);



                        var areas = Areas.Where(n => n.Enabled && n.Visible).OrderBy(n => n.Order);

                        int i = 0;
                        foreach (var ca in areas)
                        {
                            ca.Draw(g);
                            if (ca.HasXAxisBar)
                            {
                                for (int j = 0; j < IndexCount; j++)
                                {
                                    int x = IndexToPixel(j);
                                    int y = ca.Bottom;
                                    g.DrawLine(ca.Theme.EdgePen, x, y, x, y + 1);

                                    if (i < Areas.Count - 1)
                                    {
                                        y = Areas[i + 1].Top;
                                        g.DrawLine(ca.Theme.EdgePen, x, y, x, y - 1);
                                    }
                                }
                            }

                            i++;
                        }
                    }
            }
        }
    }
}
