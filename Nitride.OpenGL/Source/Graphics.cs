﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using Microsoft.VisualBasic.Devices;

namespace Nitride.OpenGL
{
    public abstract partial class DockFormGL
    {
        public class GLGraphics
        {
            public GLGraphics(Control c)
            {
                Control = c;
                InitTextShader();

                VecPoint[] axisLines = new VecPoint[2];
                (AxisLinesBufferHandle, AxisLinesArrayHandle) = GLTools.CreateBuffer(axisLines, axisLines.Length, BufferUsageHint.StaticDraw);

                uint[] Shapeindices = { 0, 1, 2, 3, 7, 8, 9, 10 };
                AxisLinesElementHandle = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, AxisLinesElementHandle);
                GL.BufferData(BufferTarget.ElementArrayBuffer, Shapeindices.Length * sizeof(uint), Shapeindices, BufferUsageHint.StaticDraw);


                LineShaderProgramHandle = GLTools.CreateProgram(LineVertexShader, ColorFragmentShader);
                uni_line_intensity = GL.GetUniformLocation(LineShaderProgramHandle, "intensity");
                uni_line_lineColor = GL.GetUniformLocation(LineShaderProgramHandle, "lineColor");
            }

            Control Control { get; }

            float Width => Control.Width;

            float Height => Control.Height;

            public float GetRatioX(float pix_x)
            {
                return (pix_x * 2.0f / Width) - 1.0f;
            }

            public float GetRatioY(float pix_y)
            {
                return 1.0f - (pix_y * 2.0f / Height);
            }

            public VecPoint GetVector(PointF pt, float x_offset, float y_offset)
            {
                return new VecPoint((pt.X * 2.0f / Width) + x_offset, (pt.Y * 2.0f / Height) + y_offset);
            }

            public VecPoint[] GetVector(PointF[] pt, float x_offset, float y_offset)
            {
                VecPoint[] axisLine = new VecPoint[pt.Length];

                for (int i = 0; i < axisLine.Length; i++)
                {
                    axisLine[i] = GetVector(pt[i], x_offset, y_offset);
                }

                return axisLine;
            }

            #region Font

            private const string FontVertexShader = @"

            #version 330 core
            layout (location = 0) in vec3 aPosition;
            layout (location = 1) in vec2 aTexCoord;

            out vec2 TexCoord;

            void main()
            {
                gl_Position = vec4(aPosition, 1.0);
                TexCoord = aTexCoord;
            }";

            private const string FontFragShader = @"

            #version 330 core
            out vec4 FragColor;

            in vec2 TexCoord;

            uniform vec3 fontColor;
            uniform sampler2D texture1;

            void main()
            {
                vec4 texColor = texture(texture1, TexCoord);
                FragColor = vec4 (fontColor, texColor.a);
                // FragColor = vec4 (1.0f, 0.75f, 0f, texColor.a);
            }";

            private TextureVertex[] FontVertices = {

                new (new Vector3 (-1.0f, 1.0f, 0.0f), new Vector2 (0.0f, 0.0f)),
                new (new Vector3 (1.0f, 1.0f, 0.0f), new Vector2 (1.0f, 0.0f)),
                new (new Vector3 (1.0f, -1.0f, 0.0f), new Vector2 (1.0f, 1.0f)),
                new (new Vector3 (-1.0f, -1.0f, 0.0f), new Vector2 (0.0f, 1.0f)),
            };

            private int FontVerticesBufferHandle;
            private int FontVerticesArrayHandle;
            private int TextShaderProgramHandle;
            private int TextShaderFontColorUniform;

            private void InitTextShader()
            {
                (FontVerticesBufferHandle, FontVerticesArrayHandle) = GLTools.CreateBuffer(FontVertices, FontVertices.Length, BufferUsageHint.StreamDraw);
                TextShaderProgramHandle = GLTools.CreateProgram(FontVertexShader, FontFragShader);
                TextShaderFontColorUniform = GL.GetUniformLocation(TextShaderProgramHandle, "fontColor");
            }

            public void DrawString(string s, GLFont font, Vector3 color, float x, float y, AlignType align = AlignType.Center)
            {
                GL.UseProgram(TextShaderProgramHandle);
                GL.Uniform3(TextShaderFontColorUniform, color); //color.R / 255.0f, color.G / 255.0f, color.B / 255.0f));
                GL.BindTexture(TextureTarget.Texture2D, font.TextureID);
                GL.Enable(EnableCap.Blend);
                // GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

                float half_width = (float)font.GlyphSize.Width * 1.0f / Width;
                float half_height = (float)font.GlyphSize.Height * 1.0f / Height;
                float text_step = half_width * 1.9f;
                //Console.WriteLine("width = " + width + " | height = " + height);

                switch (align)
                {
                    case AlignType.Center:
                        x -= text_step * (s.Length - 1.0f) / 2.0f; // Center Alignment!
                        break;

                    case AlignType.Left:
                        x += text_step / 2.0f; // Center Alignment!
                        break;

                    case AlignType.Right:
                        x -= text_step * (s.Length - 0.5f); // Center Alignment!
                        break;
                }

                foreach (char c in s)
                {
                    float glyphOffset = (c - 33) * font.U_Step;
                    FontVertices[0].TexCoord.X = FontVertices[3].TexCoord.X = glyphOffset;
                    FontVertices[1].TexCoord.X = FontVertices[2].TexCoord.X = glyphOffset + font.U_Step;

                    FontVertices[0].Position.X = FontVertices[3].Position.X = x - half_width;
                    FontVertices[1].Position.X = FontVertices[2].Position.X = x + half_width;

                    FontVertices[0].Position.Y = FontVertices[1].Position.Y = y + half_height;
                    FontVertices[2].Position.Y = FontVertices[3].Position.Y = y - half_height;

                    GLTools.UpdateBuffer(FontVerticesBufferHandle, FontVerticesArrayHandle, FontVertices, FontVertices.Length);
                    GL.DrawArrays(PrimitiveType.Quads, 0, 4);

                    x += text_step;
                }

                GL.BindTexture(TextureTarget.Texture2D, 0);
                GL.Disable(EnableCap.Blend);
            }

            #endregion Font

            #region Draw Line

            private int AxisLinesBufferHandle;
            private int AxisLinesElementHandle;
            private int AxisLinesArrayHandle;

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

            public void DrawLine(float x1, float y1, float x2, float y2, float lineWidth, Vector3 color, float intensity = 1.0f, int stippleStep = 0, ushort stipplePattern = 0)
            {
                VecPoint[] vecLine = new VecPoint[2];
                (vecLine[0].Vec.X, vecLine[0].Vec.Y) = (x1, y1);
                (vecLine[1].Vec.X, vecLine[1].Vec.Y) = (x2, y2);

                GL.UseProgram(LineShaderProgramHandle);
                GL.Uniform1(uni_line_intensity, intensity);
                GL.Uniform3(uni_line_lineColor, color); //new Vector3(0.12549f, 0.27451f, 0.313725f));
                GLTools.UpdateBuffer(AxisLinesBufferHandle, AxisLinesArrayHandle, vecLine, vecLine.Length);
                GL.LineWidth(lineWidth);

                if (stippleStep > 0)
                {
                    GL.Enable(EnableCap.LineStipple);
                    GL.LineStipple(stippleStep, stipplePattern);
                }

                GL.DrawArrays(PrimitiveType.LineStrip, 0, vecLine.Length);
                GL.Disable(EnableCap.LineStipple);
            }

            public void DrawShape(VecPoint[] shapeVec, Vector3 foreColor, Vector3 backColor, float lineWidth = 2.0f, float intensity = 1.0f)
            {
                GL.UseProgram(LineShaderProgramHandle);
                GL.Uniform1(uni_line_intensity, intensity);
                GL.Uniform3(uni_line_lineColor, foreColor);
                GLTools.UpdateBuffer(AxisLinesBufferHandle, AxisLinesArrayHandle, shapeVec, shapeVec.Length);
                GL.Uniform3(uni_line_lineColor, backColor);
                GL.DrawArrays(PrimitiveType.Polygon, 0, shapeVec.Length);
                GL.Uniform3(uni_line_lineColor, foreColor);
                GL.LineWidth(lineWidth);
                GL.DrawArrays(PrimitiveType.LineLoop, 0, shapeVec.Length);
                GL.PointSize(lineWidth);
                GL.DrawArrays(PrimitiveType.Points, 0, shapeVec.Length);
            }

            public void DrawAxis(Chart.Axis axis, float farSideRatio, float tickSideRatio, float tickRatio, GLFont font, float tickLabelRatio, float intensity = 1.0f, bool drawTick = false)
            {
                lock (axis.Ticks)
                {
                    VecPoint[] vecLine = new VecPoint[2];

                    GL.UseProgram(LineShaderProgramHandle);
                    GL.Uniform1(uni_line_intensity, intensity);

                    for (int i = 0; i < axis.Ticks.Count; i++)
                    {
                        Chart.AxisTick tick = axis.Ticks[i];

                        (vecLine[0].Vec.X, vecLine[0].Vec.Y) = (farSideRatio, tick.Ratio);
                        (vecLine[1].Vec.X, vecLine[1].Vec.Y) = (tickSideRatio, tick.Ratio);
                        GLTools.UpdateBuffer(AxisLinesBufferHandle, AxisLinesArrayHandle, vecLine, vecLine.Length);

                        switch (tick.Importance)
                        {
                            case Importance.Minor:
                                GL.LineWidth(1.2f);
                                GL.Enable(EnableCap.LineStipple);
                                GL.LineStipple(3, 0x5555);
                                GL.Uniform3(uni_line_lineColor, new Vector3(0.5f, 0.5f, 0.5f));
                                //DrawLine(farSideRatio, tick.Ratio, tickSideRatio, tick.Ratio, 1.2f, new Color4(0.5f, 0.5f, 0.5f, 1.0f), 1.0f, 3, 0x5555);
                                break;

                            case Importance.Major:
                                GL.LineWidth(1.8f);
                                GL.Enable(EnableCap.LineStipple);
                                GL.LineStipple(2, 0xFF3C);
                                GL.Uniform3(uni_line_lineColor, new Vector3(0.5f, 0.5f, 0.5f));
                                //DrawLine(farSideRatio, tick.Ratio, tickSideRatio, tick.Ratio, 1.8f, new Color4(0.3f, 0.3f, 0.3f, 1.0f), 1.0f, 2, 0xFF3C);
                                break;

                            default:

                                break;
                        }

                        GL.DrawArrays(PrimitiveType.LineStrip, 0, vecLine.Length);
                        GL.Disable(EnableCap.LineStipple);

                        if (drawTick)
                        {
                            (vecLine[0].Vec.X, vecLine[0].Vec.Y) = (tickRatio, tick.Ratio);
                            GLTools.UpdateBuffer(AxisLinesBufferHandle, AxisLinesArrayHandle, vecLine, vecLine.Length);

                            GL.LineWidth(1.8f);
                            GL.Uniform3(uni_line_lineColor, new Vector3(0.5f, 0.5f, 0.5f));
                            GL.DrawArrays(PrimitiveType.LineStrip, 0, vecLine.Length);

                            // Draw Ticks!!
                            // DrawLine(tickRatio, tick.Ratio, tickSideRatio, tick.Ratio, 1.5f, new Color4(0.5f, 0.5f, 0.5f, 1.0f));
                        }
                    }

                    for (int i = 0; i < axis.Ticks.Count; i++)
                    {
                        Chart.AxisTick tick = axis.Ticks[i];
                        DrawString(tick.Label, font, new Vector3(0.5f, 0.5f, 0.5f), tickLabelRatio, tick.Ratio, AlignType.Left);
                    }
                }
            }

            public void DrawRectangle(float left, float right, float bottom, float top, float lineWidth, Color4 color, float intensity = 1.0f, int stippleStep = 0, ushort stipplePattern = 0)
            {
                VecPoint[] axisLine = new VecPoint[4];
                (axisLine[0].Vec.X, axisLine[0].Vec.Y) = (left, bottom);
                (axisLine[1].Vec.X, axisLine[1].Vec.Y) = (left, top);
                (axisLine[2].Vec.X, axisLine[2].Vec.Y) = (right, top);
                (axisLine[3].Vec.X, axisLine[3].Vec.Y) = (right, bottom);

                GL.UseProgram(LineShaderProgramHandle);
                GL.Uniform1(uni_line_intensity, intensity);
                GL.Uniform3(uni_line_lineColor, new Vector3(color.R, color.G, color.B)); //new Vector3(0.12549f, 0.27451f, 0.313725f));

                GLTools.UpdateBuffer(AxisLinesBufferHandle, AxisLinesArrayHandle, axisLine, axisLine.Length);

                if (stippleStep > 0)
                {
                    GL.Enable(EnableCap.LineStipple);
                    GL.LineStipple(stippleStep, stipplePattern);
                }

                GL.LineWidth(lineWidth);
                GL.DrawArrays(PrimitiveType.LineLoop, 0, axisLine.Length);

                GL.PointSize(lineWidth);
                GL.DrawArrays(PrimitiveType.Points, 0, axisLine.Length);

                GL.Disable(EnableCap.LineStipple);
            }

            #endregion Draw Line

            #region Shape

            public void DrawUpDownTag(string tagString, GLFont font, float offset_x, float offset_y, Vector3 foreColor, Vector3 backColor, bool isLast, float intensity = 1.0f, float lineWidth = 2.0f, float arrowSize = 5, float cornerSize = 3)
            {
                float tagWidth = (tagString.Length + 1.0f) * font.GlyphSize.Width;
                float half_width = tagWidth / 2.0f;
                float half_height = Chart.XAxisStripHeight / 2.0f;

                PointF[] tagPts = new[]
                {
                new PointF (half_width - cornerSize, -half_height), // 0
                new PointF (half_width, -(half_height - cornerSize)), // 1

                new PointF (half_width, half_height - cornerSize), // 2
                new PointF (half_width - cornerSize, half_height), // 3

                new PointF (arrowSize, half_height), // 4
                new PointF (0.0f, half_height + arrowSize), // 5
                new PointF (-arrowSize, half_height), // 6

                new PointF (-half_width + cornerSize, half_height), // 7
                new PointF (-half_width, half_height - cornerSize), // 8

                new PointF (-half_width, -(half_height - cornerSize)), // 9
                new PointF (-(half_width - cornerSize), -half_height), // 10

                new PointF (-arrowSize, -half_height), // 11
                new PointF (0.0f, -half_height - arrowSize), // 12
                new PointF (arrowSize, -half_height) // 13
            };

                VecPoint[] shapeVec = GetVector(tagPts, offset_x, offset_y);

                GL.UseProgram(LineShaderProgramHandle);
                GL.Uniform1(uni_line_intensity, intensity);
                GL.Uniform3(uni_line_lineColor, foreColor);
                GLTools.UpdateBuffer(AxisLinesBufferHandle, AxisLinesArrayHandle, shapeVec, shapeVec.Length);
                GL.Uniform3(uni_line_lineColor, backColor);

                uint[] shapeindices = { 0, 1, 2, 0, 2, 3, 0, 3, 7, 0, 7, 10, 8, 9, 10, 7, 8, 10, 4, 5, 6, 11, 12, 13 };
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, AxisLinesElementHandle);
                GL.BufferData(BufferTarget.ElementArrayBuffer, shapeindices.Length * sizeof(uint), shapeindices, BufferUsageHint.StreamDraw);
                GL.DrawElements(PrimitiveType.Triangles, isLast ? (shapeindices.Length - 3) : shapeindices.Length, DrawElementsType.UnsignedInt, 0);

                GL.Uniform3(uni_line_lineColor, foreColor);
                GL.LineWidth(lineWidth);
                GL.DrawArrays(PrimitiveType.LineLoop, 0, isLast ? (shapeVec.Length - 3) : shapeVec.Length);
                GL.PointSize(lineWidth);
                GL.DrawArrays(PrimitiveType.Points, 0, isLast ? (shapeVec.Length - 3) : shapeVec.Length);

                DrawString(tagString, font, foreColor, offset_x, offset_y);
            }

            public void DrawChartCursor(Chart c, GLFont font, Vector3 foreColor, Vector3 backColor, float arrowSize = 5, float cornerSize = 3)
            {
                float mouse_x = c.MouseRatioX;
                float mouse_y = c.MouseRatioY;

                string tagString = c.X_Axis.GetValue(mouse_x).ToString();
                float half_height = Chart.XAxisStripHeight / 2.0f;

                // X Cursor Line
                DrawLine(mouse_x, c.Ratio_Top, mouse_x, c.Ratio_Bottom, 1.5f, foreColor);

                foreach (Chart.Area area in c.Areas)
                {
                    if (mouse_y > area.Ratio_Bottom && mouse_y < area.Ratio_Top)
                    {
                        // Y Cursor Line
                        DrawLine(c.Ratio_Left, mouse_y, c.Ratio_Right, mouse_y, 1.5f, foreColor);

                        string y_axis_string = area.Axis_Right.GetValue(mouse_y).ToString("0.00");
                        float cursorWidth = (y_axis_string.Length + 1f) * font.GlyphSize.Width;
                        PointF[] rightCursorPts = new[]
                        {
                        new PointF (cornerSize, half_height),
                        new PointF (-arrowSize, 0),
                        new PointF (cornerSize, -half_height),

                        new PointF (cursorWidth - cornerSize, -half_height),
                        new PointF (cursorWidth, -(half_height - cornerSize)),

                        new PointF (cursorWidth, half_height - cornerSize),
                        new PointF (cursorWidth - cornerSize, half_height)
                    };

                        VecPoint[] cursorVec = GetVector(rightCursorPts, c.Ratio_Right, mouse_y);
                        DrawShape(cursorVec, foreColor, backColor);
                        DrawString(y_axis_string, font, foreColor, c.RatioAxisLabel_Right, mouse_y, AlignType.Left);
                    }

                    if (area.HasXAxisStrip)
                    {
                        DrawUpDownTag(tagString, font, mouse_x, area.Ratio_XAxisStrip, foreColor, backColor, area == c.Areas.Last());
                    }
                }
            }

            #endregion Shape
        }
    }
}