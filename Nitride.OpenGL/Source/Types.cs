using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using OpenTK.Mathematics;

namespace Nitride.OpenGL
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ColorVertex
    {
        public ColorVertex(Vector3 pos, Vector4 color)
        {
            Position = pos;
            Color = color;
        }

        public Vector3 Position;
        public Vector4 Color;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TextureVertex
    {
        public TextureVertex(Vector3 pos, Vector2 texPos)
        {
            Position = pos;
            TexCoord = texPos;
        }

        public Vector3 Position;
        public Vector2 TexCoord;
    }
}
