using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Nitride.OpenGL
{
    public class ExampleChart : Chart
    {
        public ExampleChart(string name, int nr = 256, int p = 8) : base(name)
        {
            Margin = new Padding(50, 20, 50, 20);

            NR = nr;
            P = p;
            Lines = new ChartLine[P];

            for (int i = 0; i < Lines.Length; i++)
            {
                Lines[i] = new(NR);
                Lines[i].UpdateBuffer();
            }

            X_Min = -0.8f;
            X_Max = 0.8f;

            ChartArea area = new (this)
            { 
                HasXAxisStrip = true
            };

            area.Axis_Left.Range.Reset(-1.0f, 1.0f);
            area.Axis_Left.FixedRange = true;

            area.Axis_Right.Range.Reset(-1.0f, 1.0f);
            area.Axis_Right.FixedRange = true;

            area.Lines_Right.AddRange(Lines);

            Areas.Add(area);

            
            ChartArea area2 = new (this)
            {
                HasXAxisStrip = true
            };

            Areas.Add(area2);

            _timer = new Timer();
            _timer.Tick += (sender, e) =>
            {
                for (int i = 0; i < Lines.Length; i++)
                {
                    Lines[i].UpdateBuffer();
                    // CoordinateLayout();
                }

                AsyncUpdateUI = true;
            };
            _timer.Interval = 10;   // 1000 ms per sec / 50 ms per frame = 20 FPS
            _timer.Start();

            ResumeLayout(false);
        }

        private Timer _timer = null!;

        private ChartLine[] Lines;

        public int NR { get; }

        public int P { get; }

        public void UpdateValue(int p, int nr, float value)
        {
            Lines[p][nr] = value;
        }

        public override void DeleteBuffer()
        {

        }
    }
}
