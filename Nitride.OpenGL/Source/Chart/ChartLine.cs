using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Nitride.OpenGL
{
    public class ChartLine
    {
        public ChartLine(int length = 1024) 
        {
            PointList = new VecPoint[length];
        }

        public Color LineColor { get; set; } = Color.DarkGreen;
        public float Intensity { get; set; } = 0.25f;
        public float LineWidth { get; set; } = 1.0f;


        public VecPoint[] PointList;
        public int Length => PointList.Length;
        public float this[int i] 
        {
            get => PointList[i].Vec.Y;
            set => PointList[i].Vec.Y = value;
        }

        Random rnd = new Random();

        public void UpdateBuffer()
        {
            float x_wavestep = 2.0f / (PointList.Length - 1);
            float x = -1.0f;

            for (int i = 0; i < PointList.Length; i++)
            {

                PointList[i] = new VecPoint(x, (rnd.NextSingle() - 0.5f) * 2.0f);
                x += x_wavestep;
            }
        }

        private int WaveBufferHandle;
        private int WaveArrayHandle;

        // ###############################################################

        public void CreateBuffer()
        {
            (WaveBufferHandle, WaveArrayHandle) = GLTools.CreateBuffer(PointList, PointList.Length);
            // Console.WriteLine("Create Line Buffer! " + WaveBufferHandle + " / " + WaveArrayHandle);
        }

        public void Render() 
        {
            //GL.LineStipple(3, 0xF0F0);
            //GL.Enable(EnableCap.LineStipple);
            GLTools.UpdateBuffer(WaveBufferHandle, WaveArrayHandle, PointList, PointList.Length);
            GL.LineWidth(LineWidth);
            GL.DrawArrays(PrimitiveType.LineStrip, 0, Length);
            //GL.Disable(EnableCap.LineStipple);
        }
    }
}
