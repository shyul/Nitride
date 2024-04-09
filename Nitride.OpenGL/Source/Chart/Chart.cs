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
    public class Chart : DockFormGL
    {
        public Chart(string name) : base(name, true)
        {
            HasIcon = true;
            Btn_Pin.Enabled = true;
            Btn_Close.Enabled = true;

            // ResumeLayout(false);
        }

        public int XAxisStripHeight { get; set; } = 30;

        public float X_Min { get; set; }
        public float X_Max { get; set; }
        public int X_Pix_Total { get; set; }
        public int Y_Pix_Total { get; set; }

        public float Ratio_Left = 0f;
        public float Ratio_Width = 0f;

        public List<ChartArea> Areas { get; } = new();

        public override void CreateBuffer()
        {
            InitTextShader();
            // MainFont.CreateTexture();

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

                // ################

                float total_weight = Areas.Select(n => n.Weight).Sum();
                int y_pix = Margin.Top;

                ChartArea area;

                for (int i = 0; i < Areas.Count; i++)
                {
                    area = Areas[i];
                    area.Y_Pix_Min = y_pix;
                    y_pix += Convert.ToInt32(area.Weight * Y_Pix_Total / total_weight);
                    area.Y_Pix_Max = y_pix;

                    //Console.WriteLine("1# Y_Pix_Min = " + area.Y_Pix_Min + " | Y_Pix_Max = " + area.Y_Pix_Max);

                    if (area.HasXAxisStrip) y_pix += XAxisStripHeight;
                }

                area = Areas.Last();
                area.Y_Pix_Max = (area.HasXAxisStrip) ? bottom - XAxisStripHeight : bottom;

                for (int i = 0; i < Areas.Count; i++)
                {
                    area = Areas[i];

                    //Console.WriteLine("2# Y_Pix_Min = " + area.Y_Pix_Min + " | Y_Pix_Max = " + area.Y_Pix_Max);

                    area.Ratio_Bottom = 1.0f - (area.Y_Pix_Max * 2.0f / (float)Height);
                    area.Ratio_Height = (area.Y_Pix * 2.0f / (float)Height);

                    //Console.WriteLine("2# Ratio_Bottom = " + area.Ratio_Bottom + " | Ratio_Height = " + area.Ratio_Height);
                    area.CoordinateLayout();
                }
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

            foreach (ChartArea area in Areas)
            {
                area.Render();
            }

        }

        public override void DeleteBuffer()
        {

        }
    }
}
