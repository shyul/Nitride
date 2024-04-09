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
    public struct AxisTick
    {
        public AxisTick(float value, Func<float, string> toString, Importance importance = Importance.Minor)
        {
            Value = value;
            Label = toString(value);
            Importance = importance;
        }

        public float Value;
        public string Label;
        public Importance Importance;
    }

    public class ChartAxis
    {
        public ChartAxis(ChartArea area)
        {
            Area = area;
        }

        public ChartArea Area { get; }

        public int Pix => Area.Y_Pix;

        public bool FixedRange { get; set; } = false;
        public Range<float> Range { get; } = new Range<float>();
        public float Reference { get; set; } = float.NaN;
        public float TickStep { get; set; } = float.NaN;
        public List<AxisTick> Ticks { get; } = new();

        static string GetTickLabel(float value) => Convert.ToDouble(value).ToUnitPrefixNumber3String("0.##").String;


        public virtual int MinimumTickHeight { get; set; } = 30;

        public void GenerateTicks()
        {
            Ticks.Clear();

            /*
            if (!FixedRange) 
            {
                Range.Reset(float.MaxValue, float.MinValue);
            
            }*/

            if (!float.IsNaN(Reference))
            {
                Ticks.Add(new AxisTick(Reference, GetTickLabel, Importance.Major));
                Range.Insert(Reference);
            }

            if (!float.IsNaN(TickStep)) 
            {
            
            
            }
            else // Generate Ticks According to 
            {
                int tickCount = (1.0 * Pix / MinimumTickHeight).ToInt32();
            }
        }
    }
}
