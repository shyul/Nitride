using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Nitride.OpenGL
{
    public class WaveForm
    {
        public WaveForm(int length = 1024) 
        {
            if (ShaderProgramHandle == 0) 
            {
                ShaderProgramHandle = GLTools.CreateProgram(VertexShaderSource, FragmentShaderSource);
                uni_x_min = GL.GetUniformLocation(ShaderProgramHandle, "x_min");
                uni_x_range = GL.GetUniformLocation(ShaderProgramHandle, "x_range");
                uni_y_min = GL.GetUniformLocation(ShaderProgramHandle, "y_min");
                uni_y_range = GL.GetUniformLocation(ShaderProgramHandle, "y_range");
                uni_left = GL.GetUniformLocation(ShaderProgramHandle, "left");
                uni_width = GL.GetUniformLocation(ShaderProgramHandle, "width");
                uni_bottom = GL.GetUniformLocation(ShaderProgramHandle, "bottom");
                uni_height = GL.GetUniformLocation(ShaderProgramHandle, "height");
                uni_intensity = GL.GetUniformLocation(ShaderProgramHandle, "intensity");
                uni_lineColor = GL.GetUniformLocation(ShaderProgramHandle, "lineColor");
            }

            WaveFormBuffer = new Point[length];
        }

        // ###############################################################

        private static string VertexShaderSource = @"

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

        private static string FragmentShaderSource = @"

            #version 330 core

            uniform float intensity;
            uniform vec3 lineColor;

            out vec4 FragColor;

            void main()
            {
                FragColor = vec4(lineColor, intensity);
            }";

        private static int ShaderProgramHandle = 0;

        private static int uni_x_min;
        private static int uni_x_range;
        private static int uni_y_min;
        private static int uni_y_range;

        private static int uni_left;
        private static int uni_width;
        private static int uni_bottom;
        private static int uni_height;

        private static int uni_intensity;
        private static int uni_lineColor;

        // ###############################################################

        private int WaveBufferHandle;
        private int WaveArrayHandle;

        // ###############################################################

        public float Intensity { get; set; } = 0.15f;
        public Color LineColor { get; set; } = Color.DarkGreen;
        public float LineWidth { get; set; } = 1.0f;

        public int Length => WaveFormBuffer.Length;

        Point[] WaveFormBuffer;

        Random rnd = new Random();

        public void UpdateBuffer()
        {
            for (int i = 0; i < WaveFormBuffer.Length; i++)
            {
                WaveFormBuffer[i].Data.Y = rnd.NextSingle() - 0.5f;
                //Console.WriteLine("Y = " + WaveFormBuffer[i].Data.Y);
            }
        }

        public void InitBuffer()
        {
            float x_wavestep = 2.0f / WaveFormBuffer.Length;

            float x = -1.0f;
            for (int i = 0; i < WaveFormBuffer.Length; i++)
            {
                WaveFormBuffer[i] = new Point(x, rnd.NextSingle() - 0.5f);
                x += x_wavestep;
            }

            (WaveBufferHandle, WaveArrayHandle) = GLTools.CreateBuffer(WaveFormBuffer, WaveFormBuffer.Length);
        }

        public void DrawLine() 
        {
            GL.UseProgram(ShaderProgramHandle);

            // Area U and V
            GL.Uniform1(uni_left, -1.0f);
            GL.Uniform1(uni_width, 2.0f);
            GL.Uniform1(uni_bottom, -1.0f);
            GL.Uniform1(uni_height, 2.0f);

            // Line value
            GL.Uniform1(uni_x_min, -1.0f);
            GL.Uniform1(uni_x_range, 2.0f);
            GL.Uniform1(uni_y_min, -1.0f);
            GL.Uniform1(uni_y_range, 2.0f);

            GL.Uniform1(uni_intensity, Intensity);
            GL.Uniform3(uni_lineColor, new Vector3(LineColor.R / 255.0f, LineColor.G / 255.0f, LineColor.B / 255.0f));

            GLTools.UpdateBuffer(WaveBufferHandle, WaveArrayHandle, WaveFormBuffer, WaveFormBuffer.Length);
            GL.LineWidth(LineWidth);
            GL.DrawArrays(PrimitiveType.LineStrip, 0, Length);

            //GL.LineStipple(3, 0xF0F0);
            //GL.Enable(EnableCap.LineStipple);
            //GL.Disable(EnableCap.LineStipple);
        }

        public void LoadBuffer()
        {
            GLTools.UpdateBuffer(WaveBufferHandle, WaveArrayHandle, WaveFormBuffer, WaveFormBuffer.Length);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Point
        {
            public Point(float x, float y)
            {
                Data = new Vector2(x, y);
            }

            public Vector2 Data;
        }
    }
}
