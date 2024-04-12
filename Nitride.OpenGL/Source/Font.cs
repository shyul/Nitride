using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Nitride.OpenGL
{
    public class GLFont
    {
        public GLFont(Font font, bool antiAlias)
        {
            Font = font;
            Size sz = TextRenderer.MeasureText("W", Font);
            GlyphSize = new Size(Convert.ToInt32(sz.Width / 1.7f), Convert.ToInt32(sz.Height));

            int bitmapWidth = MaxGlyphs * GlyphSize.Width;
            int bitmapHeight = GlyphSize.Height;

            Bitmap = new Bitmap(bitmapWidth, bitmapHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            using Graphics g = Graphics.FromImage(Bitmap);

            if (antiAlias)
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            }
            else
            {
                g.SmoothingMode = SmoothingMode.None;
                g.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
            }

            for (int i = 0; i < MaxGlyphs; i++)
            {
                char c = Convert.ToChar(i + 33);
                Point Pos = new(Convert.ToInt32((float)(i - 0.21f) * (float)GlyphSize.Width), 0);
                g.DrawString(c.ToString(), Font, Brushes.White, Pos);
            }

            TextureWidth = Bitmap.Width;
            TextureHeight = Bitmap.Height;

            // U_Step = (float)GlyphSize.Width * 1.0f / (float)TextureWidth;
            U_Step = 1.0f / ((float)MaxGlyphs);
        }

        public Font Font { get; }
        public Bitmap Bitmap { get; }
        public void SaveBitmap(string filename) => Bitmap.Save(filename);

        public const int MaxGlyphs = 256;
        public Size GlyphSize { get; }

        public int TextureID;
        public int TextureWidth;
        public int TextureHeight;
        public float U_Step;

        public void CreateTexture() 
        {
            TextureID = GLTools.CreateTexture(Bitmap);
        }

        public const string VertexShader = @"

            #version 330 core
            layout (location = 0) in vec3 aPosition;
            layout (location = 1) in vec2 aTexCoord;

            out vec2 TexCoord;

            void main()
            {
                gl_Position = vec4(aPosition, 1.0);
                TexCoord = aTexCoord;
            }";

        public const string FragShader = @"

            #version 330 core
            out vec4 FragColor;

            in vec2 TexCoord;

            uniform sampler2D texture1;

            void main()
            {
                vec4 texColor = texture(texture1, TexCoord);

                FragColor = vec4 (1.0f, 0.75f, 0f, texColor.a);

                // FragColor = texture(texture1, TexCoord);
            }";
    }
}
