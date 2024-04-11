using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace Nitride.OpenGL
{
    public partial class Chart
    {
        public partial class Area : IAxisArea
        {
            public Area(Chart c)
            {
                Chart = c;
                Axis_Left = new Axis(this);
                Axis_Right = new Axis(this);
            }

            public int Index { get; set; }

            private Chart Chart { get; }

            public float Weight { get; set; } = 1.0f;

            public float Ratio_Bottom;
            public float Ratio_Top;
            private float Ratio_Height;

            public float Ratio_AxisMin => Ratio_Bottom;
            public float Ratio_AxisRange => Ratio_Height;

            private float Ratio_Left => Chart.Ratio_Left;
            private float Ratio_Right => Chart.Ratio_Right;
            private float Ratio_Width => Chart.Ratio_Width;

            // ###############################################################

            public bool HasXAxisStrip { get; set; }
            private float X_Max => Chart.X_Axis.Range.Maximum;
            private float X_Min => Chart.X_Axis.Range.Minimum;

            public int Y_Pix_XAxisStrip = 0;
            public float Ratio_XAxisStrip;
            private float Ratio_XAxisTickHigh;
            private float Ratio_XAxisTickLow;
            private float Ratio_XAxisBottom;

            // ###############################################################

            public int Y_Pix_Max;
            public int Y_Pix_Min;
            public int TotalAxisPix => Y_Pix_Max - Y_Pix_Min;

            public Axis Axis_Left { get; }
            public Axis Axis_Right { get; }

            //private readonly VecPoint[] AreaBox = new VecPoint[4];
            //private int AreaBoxBufferHandle;
            //private int AreaBoxArrayHandle;



            // ###############################################################

            public List<ChartLine> Lines_Left { get; } = new();
            public List<ChartLine> Lines_Right { get; } = new();

            public void CoordinateLayout()
            {
                GLGraphics g = Chart.Graphics;

                if (HasXAxisStrip) 
                {
                    Ratio_XAxisStrip = 1.0f - (Y_Pix_XAxisStrip * 2.0f / (float)Chart.Height);
                    Y_Pix_XAxisStrip = Y_Pix_Max + (XAxisStripHeight / 2);
                }
                else
                {
                    Y_Pix_XAxisStrip = 0;
                }

                Ratio_Bottom = 1.0f - (Y_Pix_Max * 2.0f / (float)Chart.Height);
                Ratio_Height = (TotalAxisPix * 2.0f / (float)Chart.Height);

               
                Ratio_Top = Ratio_Bottom + Ratio_Height;
                /*
                (AreaBox[0].Vec.X, AreaBox[0].Vec.Y) = (Ratio_Left, Ratio_Bottom);
                (AreaBox[1].Vec.X, AreaBox[1].Vec.Y) = (Ratio_Left, Ratio_Top);
                (AreaBox[2].Vec.X, AreaBox[2].Vec.Y) = (Ratio_Right, Ratio_Top);
                (AreaBox[3].Vec.X, AreaBox[3].Vec.Y) = (Ratio_Right, Ratio_Bottom);
                */
                Ratio_XAxisTickHigh = g.GetRatioY(Y_Pix_Max + MajorTickLength); // 1.0f - ((Y_Pix_Max + MajorTickLength) * 2.0f / (float)Chart.Height);
                Ratio_XAxisBottom = g.GetRatioY(Y_Pix_Max + XAxisStripHeight); // 1.0f - ((Y_Pix_Max + XAxisStripHeight) * 2.0f / (float)Chart.Height);
                Ratio_XAxisTickLow = g.GetRatioY(Y_Pix_Max + XAxisStripHeight - MajorTickLength); // 1.0f - ((Y_Pix_Max + XAxisStripHeight - MajorTickLength) * 2.0f / (float)Chart.Height);

                if (!Axis_Left.FixedRange)
                {
                    var list = Lines_Left.SelectMany(n => n.PointList).Where(n => n.Vec.X >= X_Min && n.Vec.X <= X_Max).Select(n => n.Vec.Y);

                    if (list.Count() > 0)
                    {
                        Axis_Left.Range.Reset(list.Min(), list.Max());
                        Console.WriteLine("Axis_Left Min = " + list.Min() + " | Max = " + list.Max());
                    }
                }

                if (!Axis_Right.FixedRange)
                {
                    var list = Lines_Right.SelectMany(n => n.PointList).Where(n => n.Vec.X >= X_Min && n.Vec.X <= X_Max).Select(n => n.Vec.Y);

                    if (list.Any())
                    {
                        Axis_Right.Range.Reset(list.Min(), list.Max());
                        Console.WriteLine("Axis_Right Min = " + list.Min() + " | Max = " + list.Max());
                    }
                }

                Axis_Left.GenerateTicks();
                Axis_Right.GenerateTicks();

                // Generate vectors for axis lines.
            }

            // ###############################################################

            public virtual void Render()
            {
                // GL.Viewport(Chart.Margin.Left, Y_Pix_Min, Chart.X_Pix_Total, Y_Pix_Max - Y_Pix_Min);
                GLGraphics g = Chart.Graphics;

                g.DrawRectangle(Ratio_Left, Ratio_Right, Ratio_Top, Ratio_Bottom, 3.0f, new Color4(0.5f, 0.5f, 0.5f, 1.0f));
                g.DrawAxis(Axis_Right, Ratio_Left, Ratio_Right, Chart.RatioMajorTick_Right, Chart.MainFont, Chart.RatioAxisLabel_Right, 1.0f, true);

                lock (Chart.X_Axis.Ticks)
                {
                    for (int i = 0; i < Chart.X_Axis.Ticks.Count; i++)
                    {
                        AxisTick tick = Chart.X_Axis.Ticks[i];
                        /*
                        (axisLine[0].Vec.X, axisLine[0].Vec.Y) = (tick.Ratio, Ratio_Bottom);
                        (axisLine[1].Vec.X, axisLine[1].Vec.Y) = (tick.Ratio, Ratio_Top);
                        GLTools.UpdateBuffer(Chart.AxisLinesBufferHandle, Chart.AxisLinesArrayHandle, axisLine, axisLine.Length);
                        */
                        // Console.WriteLine(i + " Tick Y " + tick.AxisLine[0].Vec.Y);

                        switch (tick.Importance)
                        {
                            case Importance.Minor:
                                g.DrawLine(tick.Ratio, Ratio_Bottom, tick.Ratio, Ratio_Top, 1.2f, new Vector3(0.5f, 0.5f, 0.5f), 1.0f, 3, 0x5555);

                                break;

                            case Importance.Major:
                                g.DrawLine(tick.Ratio, Ratio_Bottom, tick.Ratio, Ratio_Top, 1.8f, new Vector3(0.3f, 0.3f, 0.3f), 1.0f, 2, 0xFF3C);
                                break;

                            default:

                                break;
                        }
                    }

                    if (HasXAxisStrip)
                    {
                        for (int i = 0; i < Chart.X_Axis.Ticks.Count; i++)
                        {
                            AxisTick tick = Chart.X_Axis.Ticks[i];

                            g.DrawLine(tick.Ratio, Ratio_Bottom, tick.Ratio, Ratio_XAxisTickHigh, 1.5f, new Vector3(0.5f, 0.5f, 0.5f));

                            if (Index < Chart.Areas.Count - 1)
                            {
                                g.DrawLine(tick.Ratio, Ratio_XAxisBottom, tick.Ratio, Ratio_XAxisTickLow, 1.5f, new Vector3(0.5f, 0.5f, 0.5f));
                            }
                        }

                        for (int i = 0; i < Chart.X_Axis.Ticks.Count; i++)
                        {
                            AxisTick tick = Chart.X_Axis.Ticks[i];
                            g.DrawString(tick.Label, Chart.MainFont, new Vector3(0.3f, 0.3f, 0.3f), tick.Ratio, Ratio_XAxisStrip, AlignType.Center);
                        }
                    }

                    
                    for (int i = 0; i < Axis_Right.Ticks.Count; i++)
                    {
                        AxisTick tick = Axis_Right.Ticks[i];
                        g.DrawString(tick.Label, Chart.MainFont, new Vector3(0.3f, 0.3f, 0.3f), Chart.RatioAxisLabel_Right, tick.Ratio, AlignType.Left);
                    }
                }

                GL.Enable(EnableCap.Blend);
                GL.BlendFuncSeparate(
                    BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha, // For color channels
                    BlendingFactorSrc.One, BlendingFactorDest.SrcAlphaSaturate // For alpha channel
                );

                GL.Enable(EnableCap.ScissorTest);
                GL.Scissor(Chart.Margin.Left, Chart.Height - Y_Pix_Max, Chart.X_Pix_Total, TotalAxisPix); // Chart.Height); //  + 30

                // ################################################################

                g.DrawWaveFormStart(Ratio_Left, Ratio_Right, Ratio_Bottom, Ratio_Top, X_Min, X_Max, Axis_Right.Range.Minimum, Axis_Right.Range.Maximum);

                for (int i = 0; i < Lines_Right.Count; i++)
                {
                    ChartLine line = Lines_Right[i];
                    g.DrawWaveForm(line);
                }

                g.DrawWaveFormStart(Ratio_Left, Ratio_Right, Ratio_Bottom, Ratio_Top, X_Min, X_Max, Axis_Left.Range.Minimum, Axis_Left.Range.Maximum);

                for (int i = 0; i < Lines_Left.Count; i++)
                {
                    ChartLine line = Lines_Left[i];
                    g.DrawWaveForm(line);
                }

                /*
                    GL.UseProgram(WaveFormShaderProgramHandle);

                // Area U and V
                GL.Uniform1(uni_waveform_left, Ratio_Left); // - 1.0f);
                GL.Uniform1(uni_waveform_width, Ratio_Width); // 2.0f);
                GL.Uniform1(uni_waveform_bottom, Ratio_Bottom); //  - 1.0f);
                GL.Uniform1(uni_waveform_height, Ratio_Height); // 2.0f);

                // Line value
                GL.Uniform1(uni_waveform_x_min, X_Min); // - 1.0f);
                GL.Uniform1(uni_waveform_x_range, X_Max - X_Min); // 2.0f);



                GL.Uniform1(uni_waveform_y_min, Axis_Right.Range.Minimum); // Y_Min);
                GL.Uniform1(uni_waveform_y_range, Axis_Right.Range.Maximum - Axis_Right.Range.Minimum); // Y_Range);

                for (int i = 0; i < Lines_Right.Count; i++)
                {
                    ChartLine line = Lines_Right[i];
                    GL.Uniform1(uni_waveform_intensity, line.Intensity);
                    GL.Uniform3(uni_waveform_lineColor, new Vector3(line.LineColor.R / 255.0f, line.LineColor.G / 255.0f, line.LineColor.B / 255.0f));
                    line.Render();

                    // Console.WriteLine("Render Right Line " + i);
                }

                GL.Uniform1(uni_waveform_y_min, Axis_Left.Range.Minimum); // Y_Min);
                GL.Uniform1(uni_waveform_y_range, Axis_Left.Range.Maximum - Axis_Left.Range.Minimum); // Y_Range);

                for (int i = 0; i < Lines_Left.Count; i++)
                {
                    ChartLine line = Lines_Left[i];
                    GL.Uniform1(uni_waveform_intensity, line.Intensity);
                    GL.Uniform3(uni_waveform_lineColor, new Vector3(line.LineColor.R / 255.0f, line.LineColor.G / 255.0f, line.LineColor.B / 255.0f));
                    line.Render();

                    // Console.WriteLine("Render Left Line " + i);
                }*/

                // ################################################################

                GL.Disable(EnableCap.Blend);
                GL.Disable(EnableCap.ScissorTest);
            }

            // ###############################################################



            // ###############################################################

        }
    }
}
