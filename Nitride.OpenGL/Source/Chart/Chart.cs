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
        public GLFont MainBoldFont = new(new Font("Consolas", 15F, FontStyle.Bold, GraphicsUnit.Point, 0), true);

        public const int XAxisStripHeight = 30;
        public const int MajorTickLength = 5;
        public const int MinorTickLength = 2;
        public const int AxisLabelOffset = 8;

        // ###############################################################

        public float Ratio_AxisMin => Ratio_Left;
        public float Ratio_AxisRange => Ratio_Width;
        public int TotalAxisPix => X_Pix_Total;
        public Axis X_Axis { get; }

        // ###############################################################

        private int X_Pix_Total = 0;
        private int Y_Pix_Total = 0;

        public float Ratio_Left = 0f;
        public float Ratio_Right = 0f;
        private float Ratio_Width = 0f;

        public float Ratio_Bottom = 0f;
        public float Ratio_Top = 0f;

        private float RatioMajorTick_Left = 0f;
        private float RatioMajorTick_Right = 0f;

        private float RatioMinorTick_Left = 0f;
        private float RatioMinorTick_Right = 0f;

        public float RatioAxisLabel_Left = 0f;
        public float RatioAxisLabel_Right = 0f;

        public List<Area> Areas { get; } = new();

        public override void CreateBuffer()
        {
            MainFont.CreateTexture();
            MainBoldFont.CreateTexture();


        }

        public override void CoordinateLayout()
        {
            ReadyToShow = false;

            if (!IsBufferReady) return;

            X_Pix_Total = Width - Margin.Left - Margin.Right;
            Y_Pix_Total = Height - (Areas.Where(n => n.HasXAxisStrip).Count() * XAxisStripHeight) - Margin.Top - Margin.Bottom;

            int bottom = Height - Margin.Bottom;

            if (Y_Pix_Total > 0 && X_Pix_Total > 0)
            {
                //Console.WriteLine("Y_Pix_Total = " + Y_Pix_Total + " | bottom = " + bottom);

                // ################

                Ratio_Left = (Margin.Left * 2.0f / Width) - 1.0f;
                Ratio_Width = (X_Pix_Total * 2.0f / Width);
                Ratio_Right = Ratio_Left + Ratio_Width;

                RatioMajorTick_Left = Graphics.GetRatioX(Margin.Left - MajorTickLength); // ((Margin.Left - MajorTickLength) * 2.0f / Width) - 1.0f;
                RatioMajorTick_Right = Graphics.GetRatioX(Width - Margin.Right + MajorTickLength); // ((Width - Margin.Right + MajorTickLength) * 2.0f / Width) - 1.0f;

                RatioMinorTick_Left = Graphics.GetRatioX(Margin.Left - MinorTickLength); //((Margin.Left - MinorTickLength) * 2.0f / Width) - 1.0f;
                RatioMinorTick_Right = Graphics.GetRatioX(Width - Margin.Right + MinorTickLength); //((Width - Margin.Right + MinorTickLength) * 2.0f / Width) - 1.0f;

                RatioAxisLabel_Left = Graphics.GetRatioX(Margin.Left - AxisLabelOffset); //((Margin.Left - AxisLabelOffset) * 2.0f / Width) - 1.0f;
                RatioAxisLabel_Right = Graphics.GetRatioX(Width - Margin.Right + AxisLabelOffset); //((Width - Margin.Right + AxisLabelOffset) * 2.0f / Width) - 1.0f;

                // ################

                float total_weight = Areas.Select(n => n.Weight).Sum();
                int y_pix = Margin.Top;

                Area area;

                for (int i = 0; i < Areas.Count; i++)
                {
                    area = Areas[i];
                    area.Index = i;
                    area.Y_Pix_Min = y_pix;
                    y_pix += Convert.ToInt32(area.Weight * Y_Pix_Total / total_weight);
                    area.Y_Pix_Max = y_pix;

                    // Console.WriteLine("1# Y_Pix_Min = " + area.Y_Pix_Min + " | Y_Pix_Max = " + area.Y_Pix_Max);

                    if (area.HasXAxisStrip) 
                    {
                        y_pix += XAxisStripHeight;
                    }
                }

                area = Areas.Last();
                area.Y_Pix_Max = y_pix = (area.HasXAxisStrip) ? bottom - XAxisStripHeight : bottom;

                for (int i = 0; i < Areas.Count; i++)
                {
                    Areas[i].CoordinateLayout();
                }

                Ratio_Top = Areas.First().Ratio_Top;
                Ratio_Bottom = Areas.Last().Ratio_Bottom;

                X_Axis.GenerateTicks();
                ReadyToShow = true;
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

                if (MouseActive && MouseRatioX > Ratio_Left && MouseRatioX < Ratio_Right)
                {
                    //VecPoint[] axisLine = new VecPoint[2];
                    float mouse_x = MouseRatioX;
                    /*
                    GL.UseProgram(LineShaderProgramHandle);
                    GL.Uniform1(uni_line_intensity, 1.0f);
                    GL.Uniform3(uni_line_lineColor, new Vector3(0.2f, 0.4f, 0.45f)); //new Vector3(0.12549f, 0.27451f, 0.313725f));
                    */

                    if (mouse_x > Ratio_Left && mouse_x < Ratio_Right)
                    {
                        /*
                        (axisLine[0].Vec.X, axisLine[0].Vec.Y) = (mouse_x, Ratio_Bottom);
                        (axisLine[1].Vec.X, axisLine[1].Vec.Y) = (mouse_x, Ratio_Top);
                        GLTools.UpdateBuffer(AxisLinesBufferHandle, AxisLinesArrayHandle, axisLine, axisLine.Length);
                        GL.DrawArrays(PrimitiveType.LineStrip, 0, 2);*/

                        Graphics.DrawLine(mouse_x, Ratio_Bottom, mouse_x, Ratio_Top, 1.0f, new Vector3(0.2f, 0.4f, 0.45f), 1.0f);
                        Graphics.DrawChartCursor(this, MainBoldFont, new Vector3(0.2f, 0.4f, 0.45f), new Vector3(0.784314f, 0.929412f, 0.921568f));
                        /*
                        foreach (Area area in Areas)
                        {
                            area.RenderCursor();
                        }*/
                    }



                }
            }


        }

        public override void DeleteBuffer()
        {

        }
    }
}
