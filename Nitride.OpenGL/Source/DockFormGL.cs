using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Windows.Forms;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

using NativeWindow = OpenTK.Windowing.Desktop.NativeWindow;

namespace Nitride.OpenGL
{
    [DesignerCategory("Code")]
    public abstract class DockFormGL : DockForm
    {
        public DockFormGL(string formName, bool enableUiUpdate = false) : base(formName, enableUiUpdate)
        {
            SetStyle(ControlStyles.Opaque, true); // false); //  true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            Dock = DockStyle.Fill;
            DoubleBuffered = false;


            
        }

        public abstract void CreateBuffer();

        public abstract void DeleteBuffer();

        public abstract void Render();

        int i = 0;
        double opsTxed = 50;
        DateTime time = DateTime.Now;

        protected virtual void Draw()
        {
            if (NativeWindow is NativeWindow win && IsBufferReady)
            {
                EnsureCreated();
                win.MakeCurrent();
                Render();
                EnsureCreated();
                win.Context.SwapBuffers();

                if (i >= opsTxed)
                {
                    TimeSpan ts = DateTime.Now - time;
                    opsTxed = i / ts.TotalSeconds;

                    Console.WriteLine("DockFormGL: " + opsTxed.ToString("0.##") + " FPS");
                
                    i = 0;
                    time = DateTime.Now;
                }
                else
                {
                    i++;
                    // opsTxed++;
                }

            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Draw();
        }

        protected override void AsyncUpdateUIWorker()
        {
            while (AsyncUpdateUITask_Cts.IsContinue())
            {
                if (AsyncUpdateUI && IsHandleCreated)
                {
                    // CoordinateLayout();
                    //Draw();
                    this?.Invoke(Draw);

                    AsyncUpdateUI = false;
                    Thread.Sleep(AsyncUpdateDeley);
                }
                else
                    Thread.Sleep(2);
            }
        }

        private void ResizeNativeWindow()
        {
            if (NativeWindow is NativeWindow win)
            {
                if (Height > 0 && Width > 0)
                {
                    // win.Location = new Vector2i(0, 0); 
                    win.ClientRectangle = new Box2i(0, 0, Width, Height);
                }
            }
        }

        private bool IsResizeEventCancelled { get; set; } = false;

        protected override void OnResize(EventArgs e)
        {
            if (!IsHandleCreated)
            {
                IsResizeEventCancelled = true;
                return;
            }

            ResizeNativeWindow();

            if (NativeWindow is NativeWindow win)
            {
                win.MakeCurrent();

                GL.Viewport(0, 0, ClientSize.Width, ClientSize.Height);

                float aspect_ratio = Math.Max(ClientSize.Width, 1) / (float)Math.Max(ClientSize.Height, 1);
                Matrix4 perpective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspect_ratio, 1, 64);
                GL.MatrixMode(MatrixMode.Projection);
                GL.LoadMatrix(ref perpective);

                // Console.WriteLine("NativeWindow Size = " + win.ClientRectangle + " | Location = " + win.Location);
            }

            // Coordinate();
            base.OnResize(e);
        }

        #region Focus

        private void ForceFocusToCorrectWindow()
        {
            unsafe
            {
                if (IsNativeInputEnabled(NativeWindow))
                {
                    // Focus should be on the NativeWindow inside the GLControl.
                    NativeWindow.Focus();
                }
                else
                {
                    // Focus should be on the GLControl itself.
                    Focus();
                }
            }
        }

        /// <summary>
        /// These EventArgs are used as a safety check to prevent unexpected recursion
        /// in OnGotFocus.
        /// </summary>
        private static readonly EventArgs _noRecursionSafetyArgs = new();

        private void OnNativeWindowFocused(FocusedChangedEventArgs e)
        {
            if (e.IsFocused)
            {
                ForceFocusToCorrectWindow();
                OnGotFocus(_noRecursionSafetyArgs);
            }
            else
            {
                OnLostFocus(EventArgs.Empty);
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);

            if (!ReferenceEquals(e, _noRecursionSafetyArgs))
            {
                ForceFocusToCorrectWindow();
            }
        }


        #endregion Focus

        #region GL Components

        protected NativeWindow NativeWindow = null!;

        private NativeWindowSettings NativeWindowSettings { get; } = new NativeWindowSettings()
        {
            API = ContextAPI.OpenGL,
            APIVersion = new Version(4, 6, 0, 0),

            Flags = ContextFlags.Default,
            Profile = ContextProfile.Compatability,
            AutoLoadBindings = true,
            IsEventDriven = true,

            // SharedContext = null,
            // NumberOfSamples = 0,

            StencilBits = 8,
            DepthBits = 24,
            RedBits = 8,
            GreenBits = 8,
            BlueBits = 8,
            AlphaBits = 8,
            SrgbCapable = true,

            NumberOfSamples = 8,

            TransparentFramebuffer = true,

            StartFocused = false,
            StartVisible = false,
            WindowBorder = WindowBorder.Hidden,
            WindowState = WindowState.Normal
        };

        private unsafe bool IsNativeInputEnabled(NativeWindow nativeWindow)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                IntPtr hWnd = GLFW.GetWin32Window(nativeWindow.WindowPtr);
                IntPtr style = Win32.GetWindowLongPtr(hWnd, Win32.WindowLongs.GWL_STYLE);
                return ((Win32.WindowStyles)(long)style & Win32.WindowStyles.WS_DISABLED) == 0;
            }
            else
                throw new NotSupportedException("The current operating system is not supported by this control.");
        }

        /// <summary>
        /// A fix for the badly-broken DesignMode property, this answers (somewhat more
        /// reliably) whether this is DesignMode or not.  This does *not* work when invoked
        /// from the GLControl's constructor.
        /// </summary>
        /// <returns>True if this is in design mode, false if it is not.</returns>
        private bool DetermineIfThisIsInDesignMode()
        {
            // The obvious test.
            if (DesignMode)
                return true;

            // This works on .NET Framework but no longer seems to work reliably on .NET Core.
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return true;

            // Try walking the control tree to see if any ancestors are in DesignMode.
            for (Control control = this; control != null; control = control.Parent)
            {
                if (control.Site != null && control.Site.DesignMode)
                    return true;
            }

            // Try checking for `IDesignerHost` in the service collection.
            if (GetService(typeof(System.ComponentModel.Design.IDesignerHost)) != null)
                return true;

            // Last-ditch attempt:  Is the process named `devenv` or `VisualStudio`?
            // These are bad, hacky tests, but they *can* work sometimes.
            if (System.Reflection.Assembly.GetExecutingAssembly().Location.Contains("VisualStudio", StringComparison.OrdinalIgnoreCase))
                return true;
            if (string.Equals(System.Diagnostics.Process.GetCurrentProcess().ProcessName, "devenv", StringComparison.OrdinalIgnoreCase))
                return true;

            // Nope.  Not design mode.  Probably.  Maybe.
            return false;
        }

        public bool IsDesignMode => _isDesignMode ??= DetermineIfThisIsInDesignMode();
        private bool? _isDesignMode;

        /// <summary>
        /// Ensure that the required underlying GLFW window has been created.
        /// </summary>
        private void EnsureCreated()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(GetType().Name);

            if (!IsHandleCreated)
            {
                CreateControl();

                if (NativeWindow is null)
                    throw new InvalidOperationException("Failed to create GLControl."
                        + " This is usually caused by trying to perform operations on the GLControl"
                        + " before its containing form has been fully created.  Make sure you are not"
                        + " invoking methods on it before the Form's constructor has completed.");
            }

            if (NativeWindow is null && !DesignMode)
            {
                RecreateHandle();

                if (NativeWindow is null)
                    throw new InvalidOperationException("Failed to recreate GLControl :-(");
            }
        }

        private bool IsBufferReady = false;

        public void CreateNativeWindow()
        {
            NativeWindow = new(NativeWindowSettings);
            NativeWindow.FocusedChanged += OnNativeWindowFocused;

            /// <summary>
            /// Reparent the given NativeWindow to be a child of this GLControl.  This is a
            /// non-portable operation, as its name implies:  It works wildly differently
            /// between OSes.  The current implementation only supports Microsoft Windows.
            /// </summary>
            /// <param name="nativeWindow">The NativeWindow that must become a child of
            /// this control.</param>
            unsafe
            {
                IntPtr hWnd = GLFW.GetWin32Window(NativeWindow.WindowPtr);

                // Reparent the real HWND under this control.
                Win32.SetParent(hWnd, Handle);

                // Change the real HWND's window styles to be "WS_CHILD | WS_DISABLED" (i.e.,
                // a child of some container, with no input support), and turn off *all* the
                // other style bits (most of the rest of them could cause trouble).  In
                // particular, this turns off stuff like WS_BORDER and WS_CAPTION and WS_POPUP
                // and so on, any of which GLFW might have turned on for us.
                IntPtr style = (IntPtr)(long)(Win32.WindowStyles.WS_CHILD | Win32.WindowStyles.WS_DISABLED);
                Win32.SetWindowLongPtr(hWnd, Win32.WindowLongs.GWL_STYLE, style);

                // Change the real HWND's extended window styles to be "WS_EX_NOACTIVATE", and
                // turn off *all* the other extended style bits (most of the rest of them
                // could cause trouble).  We want WS_EX_NOACTIVATE because we don't want
                // Windows mistakenly giving the GLFW window the focus as soon as it's created,
                // regardless of whether it's a hidden window.
                style = (IntPtr)(long)Win32.WindowStylesEx.WS_EX_NOACTIVATE;
                Win32.SetWindowLongPtr(hWnd, Win32.WindowLongs.GWL_EXSTYLE, style);
            }

            // Force the newly child-ified GLFW window to be resized to fit this control.
            ResizeNativeWindow();

            CreateBuffer();

            // And now show the child window, since it hasn't been made visible yet.
            NativeWindow.IsVisible = true;

            // NativeWindow.Refresh

            IsBufferReady = true;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            CreateNativeWindow();

            base.OnHandleCreated(e);

            if (IsResizeEventCancelled)
            {
                OnResize(EventArgs.Empty);
                IsResizeEventCancelled = false;
            }

            if (Focused || (NativeWindow is NativeWindow nwin && nwin.IsFocused)) // (_nativeWindow?.IsFocused ?? false)
            {
                ForceFocusToCorrectWindow();
            }
        }

        protected override void OnParentChanged(EventArgs e)
        {
            ResizeNativeWindow();
            base.OnParentChanged(e);
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            if (NativeWindow is NativeWindow win)
            {
                DeleteBuffer();
                win.Dispose();
                NativeWindow = null!;
            }

            base.OnHandleDestroyed(e);
        }

        #endregion GL Components

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

            new TextureVertex(new Vector3 (-1.0f, 1.0f, 0.0f), new Vector2 (0.0f, 0.0f)),
            new TextureVertex(new Vector3 (1.0f, 1.0f, 0.0f), new Vector2 (1.0f, 0.0f)),
            new TextureVertex(new Vector3 (1.0f, -1.0f, 0.0f), new Vector2 (1.0f, 1.0f)),
            new TextureVertex(new Vector3 (-1.0f, -1.0f, 0.0f), new Vector2 (0.0f, 1.0f)),
        };

        private int FontVerticesBufferHandle;
        private int FontVerticesArrayHandle;
        private int TextShaderProgramHandle;
        private int TextShaderFontColorUniform;

        protected void InitTextShader()
        {
            (FontVerticesBufferHandle, FontVerticesArrayHandle) = GLTools.CreateBuffer(FontVertices, FontVertices.Length, BufferUsageHint.StreamDraw);
            TextShaderProgramHandle = GLTools.CreateProgram(FontVertexShader, FontFragShader);
            TextShaderFontColorUniform = GL.GetUniformLocation(TextShaderProgramHandle, "fontColor");
        }

        public void DrawString(string s, GLFont font, Color4 color, float x, float y, AlignType align = AlignType.Center)
        {
            GL.UseProgram(TextShaderProgramHandle);
            GL.Uniform3(TextShaderFontColorUniform, new Vector3(color.R, color.G, color.B)); //color.R / 255.0f, color.G / 255.0f, color.B / 255.0f));
            GL.BindTexture(TextureTarget.Texture2D, font.TextureID);
            GL.Enable(EnableCap.Blend);
            // GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            float half_width = (float)font.GlyphSize.Width * 1.0f / (float)ClientSize.Width;
            float half_height = (float)font.GlyphSize.Height * 1.0f / (float)ClientSize.Height;
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

        #region Draw WaveForm


        #endregion Draw WaveForm

        #region Mouse

        protected Point MousePt = new Point(0, 0);

        protected int MouseIndex = 0;

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            //Console.WriteLine("OnMouseMove: " + e.X + " | " + e.Y);

            MousePt = e.Location;
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            //Console.WriteLine("OnMouseWheel: " + e.Delta);

            if (e.Delta > 0)
            {
                MouseIndex++;
            }
            else
            {
                MouseIndex--;
            }
        }

        #endregion Mouse
    }
}
