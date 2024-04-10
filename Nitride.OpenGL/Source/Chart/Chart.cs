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
    public partial class Chart : DockFormGL, IAxisArea
    {
        public Chart(string name) : base(name, true)
        {
            HasIcon = true;
            Btn_Pin.Enabled = true;
            Btn_Close.Enabled = true;

            X_Axis = new Axis(this);

            // ResumeLayout(false);
        }

        public GLFont MainFont = new(new Font("Consolas", 12F, FontStyle.Regular, GraphicsUnit.Point, 0), true);

        public const int XAxisStripHeight = 30;
        public const int MajorTickLength = 5;
        public const int MinorTickLength = 2;
        public const int AxisLabelOffset = 8;

        // ################

        public float Ratio_AxisMin => Ratio_Left;
        public float Ratio_AxisRange => Ratio_Width;
        public int TotalAxisPix => X_Pix_Total;
        public Axis X_Axis { get; }

        // ################

        private int X_Pix_Total = 0;
        private int Y_Pix_Total = 0;

        private float Ratio_Left = 0f;
        private float Ratio_Width = 0f;

        private float RatioMajorTick_Left = 0f;
        private float RatioMajorTick_Right = 0f;

        private float RatioMinorTick_Left = 0f;
        private float RatioMinorTick_Right = 0f;

        private float RatioAxisLabel_Left = 0f;
        private float RatioAxisLabel_Right = 0f;

        public List<Area> Areas { get; } = new();

        public override void CreateBuffer()
        {
            InitTextShader();
            MainFont.CreateTexture();

            for (int i = 0; i < Areas.Count; i++)
            {
                Areas[i].CreateBuffer();
            }
        }

        public override void CoordinateLayout()
        {
            X_Pix_Total = Width - Margin.Left - Margin.Right;
            Y_Pix_Total = Height - (Areas.Where(n => n.HasXAxisStrip).Count() * XAxisStripHeight) - Margin.Top - Margin.Bottom;

            int bottom = Height - Margin.Bottom;

            if (Y_Pix_Total > 0 && X_Pix_Total > 0)
            {
                ReadyToShow = true;

                //Console.WriteLine("Y_Pix_Total = " + Y_Pix_Total + " | bottom = " + bottom);

                // ################

                Ratio_Left = (Margin.Left * 2.0f / Width) - 1.0f;
                Ratio_Width = (X_Pix_Total * 2.0f / Width);

                RatioMajorTick_Left = ((Margin.Left - MajorTickLength) * 2.0f / Width) - 1.0f;
                RatioMajorTick_Right = ((Width - Margin.Right + MajorTickLength) * 2.0f / Width) - 1.0f;

                RatioMinorTick_Left = ((Margin.Left - MinorTickLength) * 2.0f / Width) - 1.0f;
                RatioMinorTick_Right = ((Width - Margin.Right + MinorTickLength) * 2.0f / Width) - 1.0f;

                RatioAxisLabel_Left = ((Margin.Left - AxisLabelOffset) * 2.0f / Width) - 1.0f;
                RatioAxisLabel_Right = ((Width - Margin.Right + AxisLabelOffset) * 2.0f / Width) - 1.0f;

                // ################

                float total_weight = Areas.Select(n => n.Weight).Sum();
                int y_pix = Margin.Top;

                Area area;

                for (int i = 0; i < Areas.Count; i++)
                {
                    area = Areas[i];
                    area.Y_Pix_Min = y_pix;
                    y_pix += Convert.ToInt32(area.Weight * Y_Pix_Total / total_weight);
                    area.Y_Pix_Max = y_pix;

                    // Console.WriteLine("1# Y_Pix_Min = " + area.Y_Pix_Min + " | Y_Pix_Max = " + area.Y_Pix_Max);

                    if (area.HasXAxisStrip) 
                    {
                        area.Y_Pix_XAxisStrip = y_pix + (XAxisStripHeight / 2);
                        y_pix += XAxisStripHeight;
                    }
                    else
                    {
                        area.Y_Pix_XAxisStrip = 0;
                    }
                }

                area = Areas.Last();
                area.Y_Pix_Max = (area.HasXAxisStrip) ? bottom - XAxisStripHeight : bottom;

                for (int i = 0; i < Areas.Count; i++)
                {
                    Areas[i].CoordinateLayout();
                }

                X_Axis.GenerateTicks();
            }
            else
            {
                ReadyToShow = false;
            }
        }

        public override void Render()
        {
            GL.ClearColor(new Color4(1f, 0.99215686f, 0.96078431f, 1f));
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // #######################################################################

            if (ReadyToShow)
            {
                foreach (Area area in Areas)
                {
                    area.Render();
                }
            }


        }

        public override void DeleteBuffer()
        {

        }
    }
}
