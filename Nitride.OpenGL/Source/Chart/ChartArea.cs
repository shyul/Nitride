using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using Nitride.Chart;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Nitride.OpenGL
{
    public class ChartArea
    {
        public ChartArea(Chart c)
        {
            Chart = c;
            Axis_Left = new ChartAxis(this);
            Axis_Right = new ChartAxis(this);
        }

        public Chart Chart { get; }

        public float Weight { get; set; } = 1.0f;
        public float Ratio_Bottom { get; set; }
        public float Ratio_Height { get; set; }
        public float Ratio_Left => Chart.Ratio_Left;
        public float Ratio_Width => Chart.Ratio_Width;

        // ###############################################################

        public bool HasXAxisStrip { get; set; }
        public float X_Max => Chart.X_Max;
        public float X_Min => Chart.X_Min;

        // ###############################################################

        public int Y_Pix_Max { get; set; }
        public int Y_Pix_Min { get; set; }
        public int Y_Pix => Y_Pix_Max - Y_Pix_Min;

        public ChartAxis Axis_Left { get; set; }
        public ChartAxis Axis_Right { get; set; }

        VecPoint[] AreaBox = new VecPoint[4];
        private int AreaBoxBufferHandle;
        private int AreaBoxArrayHandle;

        // ###############################################################

        public List<ChartLine> Lines_Left { get; } = new();
        public List<ChartLine> Lines_Right { get; } = new();

        public void CoordinateLayout()
        {
            (AreaBox[0].Vec.X, AreaBox[0].Vec.Y) = (Ratio_Left, Ratio_Bottom);
            (AreaBox[1].Vec.X, AreaBox[1].Vec.Y) = (Ratio_Left, Ratio_Bottom + Ratio_Height);
            (AreaBox[2].Vec.X, AreaBox[2].Vec.Y) = (Ratio_Left + Ratio_Width, Ratio_Bottom + Ratio_Height);
            (AreaBox[3].Vec.X, AreaBox[3].Vec.Y) = (Ratio_Left + Ratio_Width, Ratio_Bottom);

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

                if (list.Count() > 0)
                {
                    Axis_Right.Range.Reset(list.Min(), list.Max());
                    Console.WriteLine("Axis_Right Min = " + list.Min() + " | Max = " + list.Max());
                }
            }

            Axis_Left.GenerateTicks();
            Axis_Right.GenerateTicks();
        }

        // ###############################################################

        public void CreateBuffer()
        {
            (AreaBoxBufferHandle, AreaBoxArrayHandle) = GLTools.CreateBuffer(AreaBox, AreaBox.Length);

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
            GL.Enable(EnableCap.ScissorTest);
            GL.Scissor(Chart.Margin.Left, Y_Pix_Min, Chart.X_Pix_Total, Y_Pix_Max - Y_Pix_Min);
            GL.Enable(EnableCap.Blend);
            GL.BlendFuncSeparate(
                BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha, // For color channels
                BlendingFactorSrc.One, BlendingFactorDest.SrcAlphaSaturate // For alpha channel
            );

            GL.UseProgram(LineShaderProgramHandle);
            GL.Uniform1(uni_line_intensity, 1.0f);
            GL.Uniform3(uni_line_lineColor, new Vector3(0.5f, 0.5f, 0.5f));


            GLTools.UpdateBuffer(AreaBoxBufferHandle, AreaBoxArrayHandle, AreaBox, AreaBox.Length);

            GL.LineWidth(5f);
            GL.DrawArrays(PrimitiveType.LineLoop, 0, AreaBox.Length);



            /*
            GL.Begin(PrimitiveType.LineLoop);
            GL.Vertex3(Ratio_Left, Ratio_Bottom, 0f);
            GL.Vertex3(Ratio_Left, Ratio_Bottom + Ratio_Height, 0f);
            GL.Vertex3(Ratio_Left + Ratio_Width, Ratio_Bottom + Ratio_Height, 0f);
            GL.Vertex3(Ratio_Left + Ratio_Width, Ratio_Bottom, 0f);
            GL.End();

            GL.PointSize(5f);
            GL.Begin(PrimitiveType.Points);
            GL.Vertex3(Ratio_Left, Ratio_Bottom, 0f);
            GL.Vertex3(Ratio_Left, Ratio_Bottom + Ratio_Height, 0f);
            GL.Vertex3(Ratio_Left + Ratio_Width, Ratio_Bottom + Ratio_Height, 0f);
            GL.Vertex3(Ratio_Left + Ratio_Width, Ratio_Bottom, 0f);
            GL.End();*/

            // ################

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
