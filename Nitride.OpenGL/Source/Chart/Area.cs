using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
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

            private Chart Chart { get; }

            public float Weight { get; set; } = 1.0f;

            private float Ratio_Bottom;
            private float Ratio_Top;
            private float Ratio_Height;

            public float Ratio_AxisMin => Ratio_Bottom;
            public float Ratio_AxisRange => Ratio_Height;

            private float Ratio_Left => Chart.Ratio_Left;
            private float Ratio_Right;
            private float Ratio_Width => Chart.Ratio_Width;

            // ###############################################################

            public bool HasXAxisStrip { get; set; }
            private float X_Max => Chart.X_Axis.Range.Maximum;
            private float X_Min => Chart.X_Axis.Range.Minimum;

            public int Y_Pix_XAxisStrip = 0;
            private float Ratio_XAxisStrip;

            // ###############################################################

            public int Y_Pix_Max;
            public int Y_Pix_Min;
            public int TotalAxisPix => Y_Pix_Max - Y_Pix_Min;

            public Axis Axis_Left { get; }
            public Axis Axis_Right { get; }

            private readonly VecPoint[] AreaBox = new VecPoint[4];
            private int AreaBoxBufferHandle;
            private int AreaBoxArrayHandle;

            private int AxisLinesBufferHandle;
            private int AxisLinesArrayHandle;

            // ###############################################################

            public List<ChartLine> Lines_Left { get; } = new();
            public List<ChartLine> Lines_Right { get; } = new();

            public void CoordinateLayout()
            {
                if (HasXAxisStrip) 
                {
                    Ratio_XAxisStrip = 1.0f - (Y_Pix_XAxisStrip * 2.0f / (float)Chart.Height);
                }

                Ratio_Bottom = 1.0f - (Y_Pix_Max * 2.0f / (float)Chart.Height);
                Ratio_Height = (TotalAxisPix * 2.0f / (float)Chart.Height);

                Ratio_Right = Ratio_Left + Ratio_Width;
                Ratio_Top = Ratio_Bottom + Ratio_Height;

                (AreaBox[0].Vec.X, AreaBox[0].Vec.Y) = (Ratio_Left, Ratio_Bottom);
                (AreaBox[1].Vec.X, AreaBox[1].Vec.Y) = (Ratio_Left, Ratio_Top);
                (AreaBox[2].Vec.X, AreaBox[2].Vec.Y) = (Ratio_Right, Ratio_Top);
                (AreaBox[3].Vec.X, AreaBox[3].Vec.Y) = (Ratio_Right, Ratio_Bottom);

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

            public void CreateBuffer()
            {
                (AreaBoxBufferHandle, AreaBoxArrayHandle) = GLTools.CreateBuffer(AreaBox, AreaBox.Length);

                VecPoint[] axisLines = new VecPoint[2];
                (AxisLinesBufferHandle, AxisLinesArrayHandle) = GLTools.CreateBuffer(axisLines, axisLines.Length);

                LineShaderProgramHandle = GLTools.CreateProgram(LineVertexShader, ColorFragmentShader);
                uni_line_intensity = GL.GetUniformLocation(LineShaderProgramHandle, "intensity");
                uni_line_lineColor = GL.GetUniformLocation(LineShaderProgramHandle, "lineColor");

                WaveFormShaderProgramHandle = GLTools.CreateProgram(WaveFormVertexShader, ColorFragmentShader);
                uni_waveform_x_min = GL.GetUniformLocation(WaveFormShaderProgramHandle, "x_min");
                uni_waveform_x_range = GL.GetUniformLocation(WaveFormShaderProgramHandle, "x_range");
                uni_waveform_y_min = GL.GetUniformLocation(WaveFormShaderProgramHandle, "y_min");
                uni_waveform_y_range = GL.GetUniformLocation(WaveFormShaderProgramHandle, "y_range");
                uni_waveform_left = GL.GetUniformLocation(WaveFormShaderProgramHandle, "left");
                uni_waveform_width = GL.GetUniformLocation(WaveFormShaderProgramHandle, "width");
                uni_waveform_bottom = GL.GetUniformLocation(WaveFormShaderProgramHandle, "bottom");
                uni_waveform_height = GL.GetUniformLocation(WaveFormShaderProgramHandle, "height");
                uni_waveform_intensity = GL.GetUniformLocation(WaveFormShaderProgramHandle, "intensity");
                uni_waveform_lineColor = GL.GetUniformLocation(WaveFormShaderProgramHandle, "lineColor");

                Console.WriteLine("WaveFormShaderProgramHandle = " + WaveFormShaderProgramHandle);

                for (int i = 0; i < Lines_Left.Count; i++)
                {
                    ChartLine line = Lines_Left[i];
                    line.CreateBuffer();
                }

                for (int i = 0; i < Lines_Right.Count; i++)
                {
                    ChartLine line = Lines_Right[i];
                    line.CreateBuffer();
                }
            }

            public virtual void Render()
            {
                // GL.Viewport(Chart.Margin.Left, Y_Pix_Min, Chart.X_Pix_Total, Y_Pix_Max - Y_Pix_Min);

                GL.UseProgram(LineShaderProgramHandle);
                GL.Uniform1(uni_line_intensity, 1.0f);
                GL.Uniform3(uni_line_lineColor, new Vector3(0.5f, 0.5f, 0.5f));
                GLTools.UpdateBuffer(AreaBoxBufferHandle, AreaBoxArrayHandle, AreaBox, AreaBox.Length);

                GL.LineWidth(3f);
                GL.DrawArrays(PrimitiveType.LineLoop, 0, AreaBox.Length);
                GL.PointSize(3f);
                GL.DrawArrays(PrimitiveType.Points, 0, AreaBox.Length);

                VecPoint[] axisLine = new VecPoint[2];

                lock (Axis_Right.Ticks)
                {
                    // Console.WriteLine("Axis_Right.Ticks.Count = " + Axis_Right.Ticks.Count);
                    // Console.WriteLine("Axis_Left.Ticks.Count = " + Axis_Left.Ticks.Count);

                    for (int i = 0; i < Axis_Right.Ticks.Count; i++)
                    {
                        AxisTick tick = Axis_Right.Ticks[i];

                        (axisLine[0].Vec.X, axisLine[0].Vec.Y) = (Ratio_Left, tick.Ratio);
                        (axisLine[1].Vec.X, axisLine[1].Vec.Y) = (Ratio_Right, tick.Ratio);
                        GLTools.UpdateBuffer(AxisLinesBufferHandle, AxisLinesArrayHandle, axisLine, axisLine.Length);

                        // Console.WriteLine(i + " Tick Y " + tick.AxisLine[0].Vec.Y);

                        switch (tick.Importance)
                        {
                            case Importance.Minor:
                                GL.LineWidth(1.2f);
                                GL.LineStipple(3, 0x5555);
                                GL.Uniform3(uni_line_lineColor, new Vector3(0.5f, 0.5f, 0.5f));
                                break;

                            case Importance.Major:
                                GL.LineWidth(1.8f);
                                GL.LineStipple(2, 0xFF3C);// 0x7F38);
                                GL.Uniform3(uni_line_lineColor, new Vector3(0.3f, 0.3f, 0.3f));
                                break;

                            default:

                                break;
                        }

                        GL.Enable(EnableCap.LineStipple);
                        GL.DrawArrays(PrimitiveType.LineStrip, 0, 2);
                        GL.Disable(EnableCap.LineStipple);

                        // Draw Ticks!!
                        GL.LineWidth(1.5f);
                        (axisLine[0].Vec.X, axisLine[0].Vec.Y) = (Chart.RatioMajorTick_Right, tick.Ratio);
                        GLTools.UpdateBuffer(AxisLinesBufferHandle, AxisLinesArrayHandle, axisLine, axisLine.Length);
                        GL.DrawArrays(PrimitiveType.LineStrip, 0, 2);
                    }

                    for (int i = 0; i < Chart.X_Axis.Ticks.Count; i++)
                    {
                        AxisTick tick = Chart.X_Axis.Ticks[i];

                        (axisLine[0].Vec.X, axisLine[0].Vec.Y) = (tick.Ratio, Ratio_Bottom);
                        (axisLine[1].Vec.X, axisLine[1].Vec.Y) = (tick.Ratio, Ratio_Top);
                        GLTools.UpdateBuffer(AxisLinesBufferHandle, AxisLinesArrayHandle, axisLine, axisLine.Length);

                        // Console.WriteLine(i + " Tick Y " + tick.AxisLine[0].Vec.Y);

                        switch (tick.Importance)
                        {
                            case Importance.Minor:
                                GL.LineWidth(1.2f);
                                GL.LineStipple(3, 0x5555);
                                GL.Uniform3(uni_line_lineColor, new Vector3(0.5f, 0.5f, 0.5f));
                                break;

                            case Importance.Major:
                                GL.LineWidth(1.8f);
                                GL.LineStipple(2, 0xFF3C);// 0x7F38);
                                GL.Uniform3(uni_line_lineColor, new Vector3(0.3f, 0.3f, 0.3f));
                                break;

                            default:

                                break;
                        }

                        GL.Enable(EnableCap.LineStipple);
                        GL.DrawArrays(PrimitiveType.LineStrip, 0, 2);
                        GL.Disable(EnableCap.LineStipple);


                    }

                    if (HasXAxisStrip) 
                    {
                        (axisLine[0].Vec.X, axisLine[0].Vec.Y) = (Ratio_Left, Ratio_XAxisStrip);
                        (axisLine[1].Vec.X, axisLine[1].Vec.Y) = (Ratio_Right, Ratio_XAxisStrip);
                        GL.LineWidth(1f);
                        GLTools.UpdateBuffer(AxisLinesBufferHandle, AxisLinesArrayHandle, axisLine, axisLine.Length);
                        GL.DrawArrays(PrimitiveType.LineStrip, 0, 2);
                    }
               

                    for (int i = 0; i < Axis_Right.Ticks.Count; i++)
                    {
                        AxisTick tick = Axis_Right.Ticks[i];
                        Chart.DrawString(tick.Label, Chart.MainFont, Color4.DimGray, Chart.RatioAxisLabel_Right, tick.Ratio, AlignType.Left);
                    }
                }

                GL.Enable(EnableCap.Blend);
                GL.BlendFuncSeparate(
                    BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha, // For color channels
                    BlendingFactorSrc.One, BlendingFactorDest.SrcAlphaSaturate // For alpha channel
                );

                // ################

                GL.Enable(EnableCap.ScissorTest);
                GL.Scissor(Chart.Margin.Left, Chart.Height - Y_Pix_Max, Chart.X_Pix_Total, TotalAxisPix); // Chart.Height); //  + 30

                // Console.WriteLine("Y_Pix_Min = " + Y_Pix_Min);

                GL.UseProgram(WaveFormShaderProgramHandle);

                // Area U and V
                GL.Uniform1(uni_waveform_left, Ratio_Left); // - 1.0f);
                GL.Uniform1(uni_waveform_width, Ratio_Width); // 2.0f);
                GL.Uniform1(uni_waveform_bottom, Ratio_Bottom); //  - 1.0f);
                GL.Uniform1(uni_waveform_height, Ratio_Height); // 2.0f);

                // Line value
                GL.Uniform1(uni_waveform_x_min, X_Min); // - 1.0f);
                GL.Uniform1(uni_waveform_x_range, X_Max - X_Min); // 2.0f);

                GL.Uniform1(uni_waveform_y_min, Axis_Left.Range.Minimum); // Y_Min);
                GL.Uniform1(uni_waveform_y_range, Axis_Left.Range.Maximum - Axis_Left.Range.Minimum); // Y_Range);

                for (int i = 0; i < Lines_Left.Count; i++)
                {
                    ChartLine line = Lines_Left[i];
                    GL.Uniform1(uni_waveform_intensity, line.Intensity);
                    GL.Uniform3(uni_waveform_lineColor, new Vector3(line.LineColor.R / 255.0f, line.LineColor.G / 255.0f, line.LineColor.B / 255.0f));
                    line.Render();

                    // Console.WriteLine("Render Left Line " + i);
                }

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

                GL.Disable(EnableCap.Blend);
                GL.Disable(EnableCap.ScissorTest);
            }

            // ###############################################################

            private const string LineVertexShader = @"

            #version 330 core

            in vec3 aPosition;

            void main()
            {
                gl_Position = vec4(aPosition, 1.0f);
            }";

            private const string ColorFragmentShader = @"

            #version 330 core

            uniform float intensity;
            uniform vec3 lineColor;

            out vec4 FragColor;

            void main()
            {
                FragColor = vec4(lineColor, intensity);
            }";

            private int LineShaderProgramHandle = 0;
            private int uni_line_intensity;
            private int uni_line_lineColor;

            // ###############################################################

            private const string WaveFormVertexShader = @"

            #version 330 core

            uniform float x_min;
            uniform float x_range;
            uniform float y_min;
            uniform float y_range;

            uniform float left;
            uniform float width;
            uniform float bottom;
            uniform float height;
            
            in vec2 wave;

            void main()
            {
                float p_x = (wave.x - x_min) / x_range;
                float x = left + (p_x * width);

                float p_y = (wave.y - y_min) / y_range;
                float y = bottom + (p_y * height);

                gl_Position = vec4(x, y, 0.0f, 1.0f);
            }";

            private int WaveFormShaderProgramHandle = 0;

            private int uni_waveform_x_min;
            private int uni_waveform_x_range;
            private int uni_waveform_y_min;
            private int uni_waveform_y_range;

            private int uni_waveform_left;
            private int uni_waveform_width;
            private int uni_waveform_bottom;
            private int uni_waveform_height;

            private int uni_waveform_intensity;
            private int uni_waveform_lineColor;

            // ###############################################################

        }
    }
}
