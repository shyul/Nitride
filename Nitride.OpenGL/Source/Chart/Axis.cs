using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Nitride.OpenGL
{
    public partial class Chart
    {
        public struct AxisTick
        {
            public AxisTick(float value, float ratio, Func<float, string> toString, Importance importance = Importance.Minor)
            {
                Value = value;
                Ratio = ratio;
                Label = toString(value);
                Importance = importance;
            }

            public float Value;
            public float Ratio;
            public string Label;
            public Importance Importance;
        }

        public class Axis
        {
            public Axis(IAxisArea area)
            {
                Area = area;
            }

            private IAxisArea Area { get; }

            private int TotalPix => Area.TotalAxisPix;
            private float Ratio_Min => Area.Ratio_AxisMin;
            private float Ratio_Range => Area.Ratio_AxisRange;


            public float GetRatio(float value)
            {
                float p_value = (value - Range.Minimum) / (Range.Maximum - Range.Minimum);
                return Ratio_Min + (p_value * Ratio_Range);
            }

            public void AddTick(float value, Importance importance)
            {
                AxisTick tick = new(value, GetRatio(value), GetTickLabel, importance);
                Ticks.Add(tick);

                // Console.WriteLine("AddTick = " + value);
            }

            public bool FixedRange { get; set; } = false;
            public Range<float> Range { get; } = new Range<float>();
            public float Reference { get; set; } = float.NaN;
            public float TickStep { get; set; } = float.NaN;
            public List<AxisTick> Ticks { get; } = new();

            static string GetTickLabel(float value) => Convert.ToDouble(value).ToUnitPrefixNumber3String("0.##").String;


            public virtual int MinimumTickHeight { get; set; } = 30;

            public void GenerateTicks()
            {
                lock (Ticks)
                {
                    Ticks.Clear();

                    /*
                    if (!FixedRange) 
                    {
                        Range.Reset(float.MaxValue, float.MinValue);

                    }*/

                    if (!float.IsNaN(Reference))
                    {
                        AddTick(Reference, Importance.Major);
                        Range.Insert(Reference);
                    }

                    float value;

                    if (!float.IsNaN(TickStep))
                    {
                        if (!float.IsNaN(Reference))
                        {
                            value = Reference - TickStep;
                            while (value > Range.Minimum)
                            {
                                AddTick(value, Importance.Minor);
                                value -= TickStep;
                            }

                            value = Reference + TickStep;
                            while (value < Range.Maximum)
                            {
                                AddTick(value, Importance.Minor);
                                value += TickStep;
                            }
                        }
                        else
                        {
                            value = Range.Maximum - TickStep;
                            while (value > Range.Minimum)
                            {
                                AddTick(value, Importance.Minor);
                                value -= TickStep;
                            }
                        }
                    }
                    else // Generate Ticks According to 
                    {
                        int tickCount = (1.0 * TotalPix / MinimumTickHeight).ToInt32();
                    }
                }
            }
        }
    }
}
