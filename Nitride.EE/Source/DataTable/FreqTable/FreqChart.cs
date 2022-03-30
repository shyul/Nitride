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
    public sealed class FreqChart : ChartWidget
    {
        public FreqChart(string name, FreqTable st) : base(name)
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
            /*
            AddArea(MainArea = new OscillatorArea(this, "Main", 0.3f)
            {
                HasXAxisBar = true,
                Reference = 0,
                FixedTickStep_Right = 10,
            });*/


            AddArea(MainArea = new Area(this, "Main", 0.3f)
            {
                Reference = 0,
                HasXAxisBar = true,
            });

            MainArea.AxisY(AlignType.Right).TickStep = 10;

            EnableChartShift = false;

            ResumeLayout(false);
            PerformLayout();
        }

        public override int RightBlankAreaWidth => 0;

        public FreqTable FreqTable { get; private set; }

        public Area MainArea { get; }

        public override string this[int i]
        {
            get
            {
                if (FreqTable[i] is FreqRow sp && sp.Frequency is double d)
                    return (d / 1e6).ToString("0.######") + "MHz";
                else
                    return string.Empty;
            }
        }

        public override ITable Table
        {
            get => FreqTable;

            set
            {
                if (value is FreqTable st)
                    FreqTable = st;
                else
                    FreqTable = null;
            }
        }

        public override bool ReadyToShow { get => m_ReadyToShow; set { m_ReadyToShow = value; } }

        public double[] TickDacades { get; set; } = new double[]
            { 0.1, 0.2, 0.25, 0.3, 0.4, 0.5, 0.6, 0.8, 1 };

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
                lock (Table.DataLockObject)
                    lock (GraphicsLockObject)
                    {
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
                            //Console.WriteLine("totalTicks =" + totalTicks);
                            //double freqSpan = FreqTable[StopPt - 1].Frequency - FreqTable[StartPt].Frequency;
                            //double tickFreqSpan = (freqSpan / totalTicks).FitDacades(TickDacades);

                            //Console.WriteLine("tickFreqSpan = " + tickFreqSpan);

                            // TODO: Fix THIS!!
                            for (int i = StartPt; i < Math.Min(FreqTable.Count, StopPt); i++)
                            {
                                //DateTime time = m_BarTable.IndexToTime(i);
                                //if ((time.Month - 1) % MajorTick.Length == 0) AxisX.TickList.CheckAdd(px, (Importance.Major, time.ToString("MMM-YY")));
                                //if ((time.Month - 1) % MinorTick.Length == 0) AxisX.TickList.CheckAdd(px, (Importance.Minor, time.ToString("MM")));

                                double freq = FreqTable[i].Frequency;

                                //if ((freq % tickFreqSpan) < (tickFreqSpan / 10D)) AxisX.TickList.CheckAdd(px, (Importance.Major, freq.ToString()));
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
