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
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nitride;
using Nitride.Chart;

namespace Nitride.EE
{
    public sealed class SpectrumChart : ChartWidget
    {
        public SpectrumChart(string name, SpectrumData sd) : base(name)
        {
            TabName = Name = name;
            Data = sd;

            LeftYAxisLabelWidth = 5;
            Margin = new Padding(60, 60, 5, 5); // new Padding(5, 15, 5, 5); 
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

            Style[Importance.MajorText].Font = Main.Theme.FontBold;
            Style[Importance.MajorText].HasLabel = true;
            Style[Importance.MajorText].HasLine = false;
            // Style[Importance.MajorText].Theme.EdgePen.DashPattern = new float[] { 1, 2 };

            AddArea(MainArea = new Area(this, "Main", 0.3f)
            {
                Reference = 0,
                HasXAxisBar = true,
            });

            MainAxis = MainArea.AxisY(AlignType.Right);

            //MainAxis.TickStep = 10;
            MainAxis.FixedRange = true;
            // MainAxis.FixedMinimum = -120;
            // MainAxis.FixedMaximum = 0;

            EnableChartShift = false;

            MainLineSeries = new LineSeries(LineType.Default, 1, 0)
            {
                Order = 0,
                Importance = Importance.Major,
                Name = "FFT Spectrum",
                LegendName = string.Empty, //"FFT Spectrum",
                Color = Color.FromArgb(128, 128, 128, 128),
                IsAntialiasing = true,
                HasTailTag = false,
                
            };

            MainArea.AddSeries(MainLineSeries);

            ReadyToShow = true;
            Location = new Point(0, 0);
            Dock = DockStyle.Fill;

            ResumeLayout(false);
            PerformLayout();
        }

        protected override void Dispose(bool disposing)
        {
            //DataUpdateCancellationTokenSource.Cancel();
            if (AsyncUpdateUITask_Cts is CancellationTokenSource cts && cts.IsContinue()) 
            {
                cts.Cancel();
            }
            base.Dispose(disposing);
        }

        private SpectrumData Data { get; }

        public void UpdateConfiguration(double tickStep) 
        {
            MinimumTextWidth = TextRenderer.MeasureText("0.0000MHz", Style[Importance.Major].Font).Width * 1D;
            CoordinatedSize = new Size(0, 0);

            MainArea.Reference = Data.Reference;
            MainAxis.FixedMaximum = Data.Y_Max;
            MainAxis.FixedMinimum = Data.Y_Min;

            MainAxis.TickStep = tickStep;
            MainAxis.TickDacades = new double[] { 0.1, 0.2, 0.5, 1 };
        }

        private FreqTable FreqTable => Data.FreqTable;

        private Area MainArea { get; }

        private Area HistoArea { get; }

        private ContinuousAxis MainAxis { get; }

        public LineSeries MainLineSeries { get; }

        #region Data Update

        private double MinimumTextWidth;

        protected override void AsyncUpdateUIWorker()
        {
            while (AsyncUpdateUITask_Cts.IsContinue())
            {
                if (Data is not null && Data.FrameBuffer.Count > 0 && m_AsyncUpdateUI == false) // && Graphics is not busy!!
                {
                    CurrentTraceFrame = Data.FrameBuffer.Dequeue();
                    MainLineSeries.AssignMainDataColumn(CurrentTraceFrame.MagnitudeColumn);

                    if (Data.PersistBitmapBuffer.Count > 0 && Data.Enable && Data.EnableHisto)
                    {
                        if (PersistBitmapFrame is not null)
                        {
                            PersistBitmapFrame.PersistBitmapValid = false;
                        }

                        PersistBitmapFrame = Data.PersistBitmapBuffer.Dequeue();
                    }
                    else if (!Data.EnableHisto) 
                    {
                        PersistBitmapFrame = null;
                    }

                    m_AsyncUpdateUI = true;
                }
                else
                {
                    Thread.Sleep(5);
                }

                if (m_AsyncUpdateUI)
                {
                    try
                    {
                        this?.Invoke(() =>
                        {
                            CoordinateLayout();
                            Invalidate(true);
                        });
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("DockForm AsyncUpdateUIWorker(): " + e.Message);
                    }

                    m_AsyncUpdateUI = false;
                }
                else
                    Thread.Sleep(5);
            }
        }

        public TraceFrame CurrentTraceFrame { get; private set; }


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

        public override bool ReadyToShow { get => m_ReadyToShow && IsActive && CurrentTraceFrame is not null && Table.Count > 0; set { m_ReadyToShow = value; } } // && Data.Enable

        public double[] TickDacades { get; set; } = new double[]
            { 0.1, 0.2, 0.25, 0.3, 0.4, 0.5, 0.6, 0.8, 1 };

        private Size CoordinatedSize { get; set;  } = new Size(0, 0);

        public override void CoordinateOverlay()
        {
            ResumeLayout(true);
            ChartBounds = new Rectangle(
                LeftYAxisLabelWidth + Margin.Left,
                Margin.Top,
                ClientRectangle.Width - LeftYAxisLabelWidth - Margin.Left - RightYAxisLabelWidth - Margin.Right,
                ClientRectangle.Height - Margin.Top - Margin.Bottom
                );

            if (ReadyToShow && (CoordinatedSize != Size || AxisX.IndexCount != IndexCount))
            {
         
                lock (FreqTable.DataLockObject)
                    lock (GraphicsLockObject)
                    {
                        //if (ChartBounds.Width > RightBlankAreaWidth)

                        if (true)
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

                        if (CoordinatedSize.Width != Size.Width)
                        {
                            //TickStripWidth = Width;

                            AxisX.TickList.Clear();

                            double tickCount = ChartBounds.Width / MinimumTextWidth;

                            if (tickCount > 0)
                            {
                                int tickStep = Math.Round((StopPt - StartPt) / tickCount).ToInt32();

                                int px;
                                int centerPt = (FreqTable.Count / 2); // (StartPt + Math.Min(FreqTable.Count, StopPt)) / 2;
                                double freq, centerFreq;
                                centerFreq = FreqTable[centerPt].Frequency;
                                AxisX.TickList.CheckAdd(centerPt, (Importance.Major, (centerFreq / 1e6).ToString("0.###") + "MHz"));
                                freq = FreqTable[0].Frequency;
                                AxisX.TickList.CheckAdd(0, (Importance.MajorText, (freq / 1e6).ToString("0.###") + "MHz"));
                                freq = FreqTable[FreqTable.Count - 1].Frequency;
                                AxisX.TickList.CheckAdd(FreqTable.Count - 1, (Importance.MajorText, (freq / 1e6).ToString("0.###") + "MHz"));

                                for (int i = 1; i < (tickCount / 2); i++)
                                {
                                    px = centerPt - (i * tickStep);

                                    if (px >= StartPt)
                                    {
                                        freq = centerFreq - FreqTable[px].Frequency;
                                        AxisX.TickList.CheckAdd(px, (Importance.Minor, "-" + (freq / 1e6).ToString("0.0") + "M"));
                                    }

                                    px = centerPt + (i * tickStep);

                                    if (px < StopPt)
                                    {
                                        freq = FreqTable[px].Frequency - centerFreq;
                                        AxisX.TickList.CheckAdd(px, (Importance.Minor, "+" + (freq / 1e6).ToString("0.0") + "M"));
                                    }
                                }
                            }
                        }


                        CoordinatedSize = Size;
                    }
            }
        }

        TraceFrame PersistBitmapFrame { get; set; }

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
                        if (PersistBitmapFrame is not null)
                        {
                            g.DrawImage(PersistBitmapFrame.PersistBitmap, MainArea.DataBounds);
                        }
               
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
