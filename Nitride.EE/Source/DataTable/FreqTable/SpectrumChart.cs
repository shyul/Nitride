using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Nitride;
using Nitride.Chart;

namespace Nitride.EE
{
    public sealed class SpectrumChart : ChartWidget
    {
        public SpectrumChart(string name, FreqTable st, NumericColumn mainCol) : base(name)
        {
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

            FreqTable = st;
            FreqTable.Status = TableStatus.Loading;
            FreqTable.AddDataConsumer(this);
            TabName = Name = name;

            AddArea(MainArea = new Area(this, "Main", 0.3f)
            {
                Reference = 0,
                HasXAxisBar = true,
            });

            ContinuousAxis MainAxis = MainArea.AxisY(AlignType.Right);

            MainAxis.TickStep = 10;
            MainAxis.FixedRange = true;
            MainAxis.FixedMinimum = -120;
            MainAxis.FixedMaximum = 0;

            EnableChartShift = false;

            MainColumn = mainCol;

            MainLineSeries = new LineSeries(MainColumn)
            {
                Order = 0,
                Importance = Importance.Major,
                Name = "FFT Spectrum",
                LegendName = "FFT Spectrum",
                Color = Color.FromArgb(128, 128, 128, 128),
                IsAntialiasing = true,
                Tension = 0,
                HasTailTag = false
            };

            MainArea.AddSeries(MainLineSeries);

            ReadyToShow = true;
            Location = new Point(0, 0);
            Dock = DockStyle.Fill;

            
            List<NumericColumn> persistCol_L = new();
            List<NumericColumn> persistCol_H = new();
            List<BandSeries> persistSeries = new();
            
            for (int i = 0; i < 8; i++) 
            {
                NumericColumn h_col = new("Persist H" + i);
                NumericColumn l_col = new("Persist L" + i);
                BandSeries s = new(h_col, l_col, Color.FromArgb(32, 128, 128, 128))
                {
                    EdgeColor = Color.FromArgb(64, 128, 128, 128),
                    FillColor = Color.FromArgb(32, 128, 128, 128),
                    LowerEdgeColor = Color.FromArgb(8, 128, 128, 128),
                    Order = i + 2,
                    Importance = Importance.Minor,
                    Name = "FFT Persist " + i,
                    // LegendName = "FFT Persist " + i,
                    Color = Color.FromArgb(8, 128, 128, 128),
                    IsAntialiasing = true,
                    Tension = 0,
                    HasTailTag = false,
                    
                };

                persistSeries.Add(s);
                MainArea.AddSeries(s);

                persistCol_L.Add(l_col);
                persistCol_H.Add(h_col);

            }

            PersistColumnL = persistCol_L.ToArray();
            PersistColumnH = persistCol_H.ToArray();
            PersisSeries = persistSeries.ToArray();

            PixelTable.AddDataConsumer(this);

            UpdatePixelTable(true);

            ResumeLayout(false);
            PerformLayout();
        }

        public override int RightBlankAreaWidth => 0;

        public FreqTable FreqTable { get; }

        public NumericColumn MainColumn { get; }

        public LineSeries MainLineSeries { get; }

        public NumericColumn[] PersistColumnL { get; }

        public NumericColumn[] PersistColumnH { get; }

        public BandSeries[] PersisSeries { get; } // = new LineSeries[32];

        public FreqTable PixelTable { get; } = new();

        public Area MainArea { get; }

        public override string this[int i]
        {
            get
            {
                if (PixelTable[i] is FreqRow sp && sp.Frequency is double d)
                    return (d / 1e6).ToString("0.######") + "MHz";
                else
                    return string.Empty;
            }
        }

        public override ITable Table => PixelTable; // FreqTable; // PixelTable;

        public override bool ReadyToShow { get => m_ReadyToShow; set { m_ReadyToShow = value; } }

        public double[] TickDacades { get; set; } = new double[]
            { 0.1, 0.2, 0.25, 0.3, 0.4, 0.5, 0.6, 0.8, 1 };

        private int PersistIndex { get; set; } = 0;

        public void UpdatePixelTable(bool force = false)
        {
            //Console.WriteLine("UpdatePixelTable");

            if (PixelTable.Count != ChartBounds.Width || PixelTable.StartFreq != FreqTable.StartFreq || PixelTable.StopFreq != FreqTable.StopFreq || force)
            {
                PixelTable.Configure(FreqTable.StartFreq, FreqTable.StopFreq, ChartBounds.Width); // Count is pegged with window size, OR it can be manually set.
                IndexCount = Table.Count;
                StopPt = Table.Count;

                //Console.WriteLine("IndexCount = " + IndexCount);
            }

            if (FreqTable.Count > PixelTable.Count)
            {
                int j = 0;
                
                NumericColumn h_col = PersistColumnH[PersistIndex];
                NumericColumn l_col = PersistColumnL[PersistIndex];

                PersistIndex++;
                if (PersistIndex >= PersistColumnL.Length)
                    PersistIndex = 0;
                
                for (int i = 0; i < PixelTable.Count; i++)
                {
                    FreqRow prow = PixelTable[i];

                    double freq_min = prow.Frequency - (PixelTable.FreqStep / 2);
                    double freq_max = prow.Frequency + (PixelTable.FreqStep / 2);
                    
                    prow[MainColumn] = double.MinValue;// Peak detection!
                    
                    prow[h_col] = double.MinValue;
                    prow[l_col] = double.MaxValue;
                    
                    while (j < FreqTable.Count)
                    {
                        FreqRow drow = FreqTable[j];

                        if (drow.Frequency >= freq_min && drow.Frequency < freq_max)
                        {
                            // Peak detection!
                            if (prow[MainColumn] < drow[MainColumn])
                            {
                                prow[MainColumn] = drow[MainColumn];
                                prow[h_col] = drow[MainColumn];
                            }

                            //if (prow[h_col] < drow[MainColumn]) prow[h_col] = drow[MainColumn];

                            if (prow[l_col] > drow[MainColumn]) prow[l_col] = drow[MainColumn];
                        }
                        else
                        {
                            break;
                        }

                        j++;


                    }
                }
            }
            else if (FreqTable.Count == PixelTable.Count)
            {
                for (int i = 0; i < PixelTable.Count; i++)
                {
                    FreqRow prow = PixelTable[i];
                    FreqRow drow = FreqTable[i];
                    prow[MainColumn] = drow[MainColumn];
                }
            }
            else if (FreqTable.Count > 0)// Spline interpolation.
            {
                CubicSpline cs = new(FreqTable.FreqRows.Select(n => n.Frequency), FreqTable.FreqRows.Select(n => n[MainColumn]));
                var result = cs.Evaluate(PixelTable.FreqRows.Select(n => n.Frequency));

                for (int i = 0; i < PixelTable.Count; i++)
                {
                    FreqRow prow = PixelTable[i];
                    prow[MainColumn] = result.ElementAt(i);
                }
            }

        }

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
                lock (PixelTable.DataLockObject)
                    lock (GraphicsLockObject)
                    {
                        UpdatePixelTable();

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
                            for (int i = StartPt; i < Math.Min(PixelTable.Count, StopPt); i++)
                            {
                                double freq = PixelTable[i].Frequency;

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
    }
}
