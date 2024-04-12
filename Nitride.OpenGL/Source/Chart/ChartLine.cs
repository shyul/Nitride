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
        public ChartLine()
        {
        }

        public ChartLine(int length = 1024) 
        {
            PointList = new VecPoint[length];
        }

        public Color LineColor { get; set; } = Color.DarkGreen;
        public float Intensity { get; set; } = 0.25f;
        public float LineWidth { get; set; } = 1.0f;
        public bool Enabled { get; set; } = true;


        public VecPoint[] PointList = new VecPoint[2];
        public int Length => PointList.Length;
        public float this[int i] 
        {
            get => PointList[i].Vec.Y;
            set => PointList[i].Vec.Y = value;
        }

        public void UpdateBuffer(float[] x_ticks) 
        {
            if (Length != x_ticks.Length)
                PointList = new VecPoint[x_ticks.Length];

            for (int i = 0; i < Length; i++)
            {
                PointList[i] = new VecPoint(x_ticks[i], 0.0f);
            }
        }

        public void UpdateBuffer(int length, float startValue, float stopValue)
        {
            if (PointList.Length != length)
                PointList = new VecPoint[length];

            float valueRange = stopValue - startValue;
            float x_step = valueRange / (length - 1);
            float x = startValue;

            for (int i = 0; i < Length; i++)
            {
                PointList[i] = new VecPoint(x, 0.0f);
                x += x_step;
            }
        }

        Random rnd = new Random();
        public void UpdateBuffer()
        {
            float x_wavestep = 2.0f / (PointList.Length - 1);
            float x = -1.0f;

            for (int i = 0; i < Length; i++)
            {
                PointList[i] = new VecPoint(x, (rnd.NextSingle() - 0.5f) * 2.0f);
                x += x_wavestep;
            }
        }

        // ###############################################################
    }
}
