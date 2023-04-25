/// ***************************************************************************
/// Nitride Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Axis:
/// 1. Settings for labels styles
/// 2. Location for labels
/// 3. Pixel locatons for drawing axis elements
/// 4. Drawing all axis elements: label and line
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace Nitride.Chart
{
    public class ContinuousAxis
    {
        public ContinuousAxis(IIndexArea area, AlignType labelSide, AlignType align, double heightRatio)
        {
            HeightRatio = heightRatio;
            LabelSide = labelSide;
            Align = align;
            Area = area;
            Theme = area.Theme;
        }

        public readonly Dictionary<Importance, AxisTickStyle> Style = new()
        {
            { Importance.Tiny, new AxisTickStyle() },
            { Importance.Micro, new AxisTickStyle() },
            { Importance.Minor, new AxisTickStyle() },
            { Importance.Major, new AxisTickStyle() },
        };

        public IIndexArea Area { get; protected set; }

        public ColorTheme Theme { get; }

        public Range<double> Range { get; } = new(double.MaxValue, double.MinValue);

        public double Delta => Range.Maximum - Range.Minimum;

        public bool FixedRange { get; set; } = false;

        public double FixedMinimum { get; set; } = 0;

        public double FixedMaximum { get; set; } = 0;

        public bool IsLogarithmic { get; set; } = false;

        public double LogarithmicShiftFactor { get; set; } = 1; // Keep the actual minimum above 1

        public double HeightRatio { get; set; } = 1;

        public AlignType Align { get; protected set; } = AlignType.Right;

        public AlignType LabelSide { get; protected set; } = AlignType.Left;

        public Dictionary<double, (Importance Importance, string Label)> TickList { get; } = new Dictionary<double, (Importance Importance, string Label)>();

        protected virtual double GetPixelRatio(double val)
        {
            if (Range.Maximum > Range.Minimum)
            {
                if (IsLogarithmic && val != Range.Minimum)
                {
                    //val *= LogarithmicShiftFactor;
                    double ratio = Math.Log10(LogarithmicShiftFactor * (val - Range.Minimum)) / Math.Log10(LogarithmicShiftFactor * Delta);

                    //double ratio = Math.Log10(val - Range.Minimum) / Math.Log10(Delta);

                    //if (val < 1)
                    // Console.WriteLine("log ratio = " + ratio + "; Max = " + Range.Maximum + "; Min = " + Range.Minimum + "; val = " + val);

                    return ratio;
                }
                else
                    return (val - Range.Minimum) / Delta;
            }
            else
                return 0;
        }

        public virtual int ValueToPixel(double val)
        {
            return Align switch
            {
                AlignType.Left => (Pixel_Near + (Pixel_Count * GetPixelRatio(val))).ToInt32(),
                _ => (Pixel_Far - (Pixel_Count * GetPixelRatio(val))).ToInt32(),
            };
        }


        public virtual double PixelToValue(int pix)
        {
            double ratio = Align switch
            {
                AlignType.Left => (pix - Pixel_Near) * 1.0D / Pixel_Count,
                _ => (Pixel_Far - pix) * 1.0D / Pixel_Count,
            };

            if (IsLogarithmic)
            {
                return (Range.Minimum + Math.Pow(10, ratio * Math.Log10(LogarithmicShiftFactor * Delta))) / LogarithmicShiftFactor;
            }
            else
                return Range.Minimum + (ratio * Delta);

            /*
            return Align switch
            {
                AlignType.Left => Range.Minimum + ((pix - Pixel_Near) * Delta / Pixel_Count),
                _ => Range.Minimum + ((Pixel_Far - pix) * Delta / Pixel_Count),
            };*/

        }

        public virtual string PixelToString(int pix) => PixelToValue(pix).ToUnitPrefixNumber3String("G4").String;

        public double Reference { get => double.IsNaN(m_Reference) ? Area.Reference : m_Reference; set => m_Reference = value; }
        public double m_Reference = double.NaN;

        public double TickStep { get; set; } = double.NaN;

        public double[] TickDacades { get; set; } = new double[]
            { 0.1, 0.2, 0.25, 0.3, 0.4, 0.5, 0.6, 0.8, 1 };

        public virtual int MinimumTickHeight { get; set; } = 30;

        public double MinRange { get; set; } = double.NaN;

        public virtual void GenerateTicks()
        {
            if (!double.IsNaN(MinRange))
            {
                Range.Insert(MinRange);
                Range.Insert(-MinRange);
            }

            //Console.WriteLine("GenerateTicks(): " + Range.Minimum);
            if (!double.IsNaN(Reference) && (!FixedRange))
            {
                Range.Insert(Reference);
                TickList.CheckAdd(Reference, (Importance.Major, Reference.ToUnitPrefixNumber3String("0.##").String));
            }

            int tickCount = (1.0 * HeightRatio * Area.Height / MinimumTickHeight).ToInt32(); // It needs at least 10 pixel for a tick

            if (tickCount > 0)
            {
                double tickStep = (!double.IsNaN(TickStep) && (Delta / TickStep) >= 1 && (Delta / TickStep) <= tickCount) ? TickStep : (Delta / tickCount).FitDacades(TickDacades);

                if (tickStep > 0)
                {
                    if (!double.IsNaN(Reference))
                    {
                        double tickVal = Reference;
                        while (tickVal >= Range.Minimum)
                        {
                            TickList.CheckAdd(tickVal, (Importance.Minor, tickVal.ToUnitPrefixNumber3String("0.##").String));
                            tickVal -= tickStep;
                        }

                        if (tickVal < Range.Minimum && Reference > Range.Minimum && (!FixedRange)) Range.Insert(tickVal);

                        tickVal = Reference;
                        while (tickVal <= Range.Maximum)
                        {
                            TickList.CheckAdd(tickVal, (Importance.Minor, tickVal.ToUnitPrefixNumber3String("0.##").String));
                            tickVal += tickStep;
                        }

                        if (tickVal > Range.Maximum && Reference < Range.Minimum && (!FixedRange)) Range.Insert(tickVal);
                    }
                    else
                    {
                        if (!FixedRange) 
                        {
                            Range.Insert(Range.Minimum - (Range.Minimum % tickStep));

                            double max_remainder = Range.Maximum % tickStep;

                            if (max_remainder > 0)
                                Range.Insert(Range.Maximum - max_remainder + tickStep); // * 1.0001); // Fix the last tick
                            else if (max_remainder < -0)
                                Range.Insert(Range.Maximum + max_remainder - tickStep); // * 1.0001); // Fix the last tick
                        }

                        double tickVal = Range.Minimum;

                        while (tickVal <= Range.Maximum)
                        {
                            TickList.CheckAdd(tickVal, (Importance.Minor, tickVal.ToUnitPrefixNumber3String("0.##").String));
                            tickVal += tickStep;
                        }
                    }
                }
            }
        }

        public virtual void Reset()
        {
            Range.Reset(double.MaxValue, double.MinValue);
            TickList.Clear();
        }

        public int Pixel_Near { get; protected set; }

        public int Pixel_Far { get; protected set; }

        public int Pixel_Count => Pixel_Far - Pixel_Near;

        public virtual void Coordinate(int size, int offset)
        {
            if (Delta == 0)
            {
                if (Range.Minimum > double.MinValue + 1) Range.Insert(Range.Minimum - 1);
                if (Range.Maximum < double.MaxValue - 1) Range.Insert(Range.Minimum + 1);
            }

            if (FixedRange) 
            {
                Range.Set(FixedMinimum, FixedMaximum);
            }

            switch (Align)
            {
                case AlignType.Left:
                    Pixel_Near = offset;
                    Pixel_Far = Pixel_Near + size;
                    break;

                case AlignType.Center:
                    int ct = offset + (size / 2.0).ToInt32();
                    Pixel_Near = ct - (size / 2.0).ToInt32();
                    Pixel_Far = ct + (size / 2.0).ToInt32();
                    break;

                case AlignType.Right:
                    Pixel_Far = offset + size;
                    Pixel_Near = Pixel_Far - size;
                    break;
            }
        }

        public virtual void Draw(Graphics g, Rectangle bounds, int labelOffset)
        {
            foreach (var tk in TickList.OrderBy(n => n.Value.Importance))
            {
                AxisTickStyle style = Style[tk.Value.Importance];
                int location = ValueToPixel(tk.Key);


                if (style.HasLine)
                    g.DrawLine(style.Theme.EdgePen, bounds.Left, location, bounds.Right, location);

                if (location < bounds.Bottom && location > bounds.Top)
                    if (LabelSide == AlignType.Left)
                    {
                        g.DrawLine(Theme.EdgePen, bounds.Left, location, bounds.Left - 2, location);

                        if (style.HasLabel)
                        {
                            if (tk.Value.Importance > Importance.Minor)
                            {

                            }

                            g.DrawString(tk.Value.Label, style.Font, style.Theme.ForeBrush,
                                new Point(bounds.Left - labelOffset, location), AppTheme.TextAlignRight);


                        }

                    }
                    else
                    {
                        g.DrawLine(Theme.EdgePen, bounds.Right, location, bounds.Right + 2, location);

                        if (style.HasLabel)
                        {
                            if (tk.Value.Importance > Importance.Minor)
                            {
                                Size text_size = TextRenderer.MeasureText(tk.Value.Label, style.Font);
                                using GraphicsPath gp = ShapeTool.Tag(new Point(bounds.Right + labelOffset - 1 + text_size.Width / 2, location - 1), new Size(text_size.Width - 2, text_size.Height - 2), 1);
                                g.FillPath(Area.Chart.Theme.FillBrush, gp);
                                g.DrawPath(Theme.EdgePen, gp);
                                g.DrawString(tk.Value.Label, style.Font, style.Theme.ForeBrush, new Point(bounds.Right + labelOffset, location), AppTheme.TextAlignLeft);
                            }
                            else
                            {
                                g.DrawString(tk.Value.Label, style.Font, style.Theme.ForeBrush,
                                    new Point(bounds.Right + labelOffset, location), AppTheme.TextAlignLeft);
                            }
                        }

                    }
            }
        }
    }
}
