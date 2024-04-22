using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Nitride.OpenGL
{
    public class ChartColorMap
    {
        public ChartColorMap() { }

        public float X_Min { get; private set; }
        public float X_Max { get; private set; }
        public float Z_Max { get; private set; }
        public float Z_Min { get; private set; }

        public float[] Data;

        public float[] ColorPalette = new float[0];

        public int Height { get; private set; }
        public int Width { get; private set; }
        public void UpdateSettings(int width, int height, float x_min, float x_max, float z_min, float z_max)
        {
            Width = width;
            Height = height;
            Data = new float[width * height];

            Color[] colorList = ColorTool.GetThermalGradient(100, 62);

            lock (ColorPalette)
            {
                ColorPalette = new float[colorList.Length * 4];

                for (int i = 0; i < colorList.Length; i++)
                {
                    Color c = colorList[i]; // colorList[colorList.Length - i - 1];  // colorList[i]; // colorList.Length - i - 1];
                    ColorPalette[(4 * i) + 0] = c.R / 255.0f;
                    ColorPalette[(4 * i) + 1] = c.G / 255.0f;
                    ColorPalette[(4 * i) + 2] = c.B / 255.0f;
                    ColorPalette[(4 * i) + 3] = c.A / 255.0f;
                }
            }

            X_Max = x_max;
            X_Min = x_min;
            
            Z_Max = z_max;
            Z_Min = z_min;
        }
    }
}
