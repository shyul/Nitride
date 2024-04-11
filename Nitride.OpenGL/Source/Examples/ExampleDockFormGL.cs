using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System.Drawing;
using System.Windows.Forms;

namespace Nitride.OpenGL
{
    public class ExampleDockFormGL : DockFormGL
    {
        public ExampleDockFormGL() : base("Example GL Form", true)
        {
            _timer = new Timer();
            _timer.Tick += (sender, e) =>
            {
                iTime += 0.1f;
                _a += 0.01f;

                if (_a >= 1.0f) _a = 0f;

                float b = _a;// * 0.5f;

                VertexBuffer[0].Position.X = -b;
                VertexBuffer[0].Position.Y = -b;
                VertexBuffer[1].Position.X = -b;
                VertexBuffer[1].Position.Y = b;
                VertexBuffer[2].Position.X = b;
                VertexBuffer[2].Position.Y = b;
                VertexBuffer[3].Position.X = b;
                VertexBuffer[3].Position.Y = -b;
                VertexBuffer[4].Position.X = -b;
                VertexBuffer[4].Position.Y = -b;



                AsyncUpdateUI = true;
            };
            _timer.Interval = 10;   // 1000 ms per sec / 50 ms per frame = 20 FPS
            _timer.Start();

            ResumeLayout(false);
        }

        private float _a = 0.0f;
        private float iTime = 0;
        private Timer _timer = null!;

        ColorVertex[] VertexBuffer = new ColorVertex[5];

        private int ShaderProgram1;
        private int VertexBufferHandle;
        private int VertexArrayHandle;

     

        public GLFont MainFont = new (new Font("Consolas", 50F, FontStyle.Regular, GraphicsUnit.Point, 0), true);

        public override void CreateBuffer()
        {
            
            MainFont.CreateTexture();

            VertexBuffer[0] = new ColorVertex(new Vector3(-0.5f, -0.5f, 0.0f), new Vector4(1, 0, 0, 1));
            VertexBuffer[1] = new ColorVertex(new Vector3(-0.5f, 0.5f, 0.0f), new Vector4(0, 1, 0, 1));
            VertexBuffer[2] = new ColorVertex(new Vector3(0.5f, 0.5f, 0.0f), new Vector4(0, 0, 1, 1));
            VertexBuffer[3] = new ColorVertex(new Vector3(0.5f, -0.5f, 0.0f), new Vector4(0, 1, 1, 1));
            VertexBuffer[4] = new ColorVertex(new Vector3(-0.5f, -0.5f, 0.0f), new Vector4(1, 1, 0, 1));

            // Vertex Shader
            string vertexShaderSource = @"
            #version 330 core

            layout(location = 0) in vec3 aPosition;
            layout(location = 1) in vec4 aColor;

            out vec4 vColor;

            void main()
            {
                vColor = aColor;
                gl_Position = vec4(aPosition, 1.0f);
            }";

            // Fragment Shader  // vec4(0.75, 0.75, 0.75, 1.0); // White color
            string fragmentShaderSource = @"
            #version 330 core

            in vec4 vColor;
            out vec4 FragColor;

            void main()
            {
                FragColor = vColor;
            }";

            ShaderProgram1 = GLTools.CreateProgram(vertexShaderSource, fragmentShaderSource);
            (VertexBufferHandle, VertexArrayHandle) = GLTools.CreateBuffer(VertexBuffer, VertexBuffer.Length);



        }

        public override void Render()
        {            
            // GL.ClearColor(new Color4(0.6f, 0.4f, 0.5f, 1f));
            GL.ClearColor(new Color4(1f, 0.99215686f, 0.96078431f, 1f));
            // GL.ClearColor(Color4.Transparent);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // ############################################################################################

            GL.LineWidth(5.0f);
            GLTools.UpdateBuffer(VertexBufferHandle, VertexArrayHandle, VertexBuffer, VertexBuffer.Length);
            GL.UseProgram(ShaderProgram1);

            // GL.DrawArrays(PrimitiveType.LineStrip, 0, 5);
            // GL.DrawArrays(PrimitiveType.Triangles, 0, 4);

            // GL.DrawArrays(PrimitiveType.Lines, 0, 5);
            GL.DrawArrays(PrimitiveType.LineLoop, 0, 4);
            // GL.DrawArrays(PrimitiveType.Polygon, 0, 4);

            GL.PointSize(5.0f);
            GL.DrawArrays(PrimitiveType.Points, 0, 4);

            // ############################################################################################

            GL.Enable(EnableCap.Blend);
            GL.BlendFuncSeparate(
                BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha, // For color channels
                BlendingFactorSrc.One, BlendingFactorDest.SrcAlphaSaturate // For alpha channel
            );



            GL.Disable(EnableCap.Blend);

            // ############################################################################################

            Graphics.DrawString("Hello World! " + iTime.ToString("0.00") + " | " + MouseIndex, MainFont, new Vector3(0.5f), 0.25f, 0.5f);
        }

        public override void DeleteBuffer()
        {
            GL.DeleteVertexArray(VertexArrayHandle);
            GL.DeleteBuffer(VertexBufferHandle);
            GL.DeleteProgram(ShaderProgram1);
        }
    }
}
