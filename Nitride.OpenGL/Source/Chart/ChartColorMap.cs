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

        public float[] Data;
        
        public float[] ColorPalette;

        public int X { get; private set; }
        public int Y { get; private set; }
        public void UpdateSettings(int x, int y) 
        {
            X = x; 
            Y = y;
            Data = new float[x * y];

            Color[] colorList = ColorTool.GetThermalGradient(100, 58);

            ColorPalette = new float[colorList.Length * 4];

            for (int i =  0; i < colorList.Length; i++) 
            {
                Color c = colorList[i]; // colorList[colorList.Length - i - 1];  // colorList[i]; // colorList.Length - i - 1];
                ColorPalette[(4 * i) + 0] = c.R / 255.0f;
                ColorPalette[(4 * i) + 1] = c.G / 255.0f;
                ColorPalette[(4 * i) + 2] = c.B / 255.0f;
                ColorPalette[(4 * i) + 3] = c.A / 255.0f;
            }

        }
    }
}
