using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Runtime.InteropServices;
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
    public abstract partial class DockFormGL : DockForm
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

        public GLGraphics Graphics { get; private set; }

        public abstract void CreateBuffer();

        public abstract void DeleteBuffer();

        public abstract void Render();

        int i = 0;
        double FpSpeed = 50;
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

                if (i >= 100) // FpSpeed)
                {
                    TimeSpan ts = DateTime.Now - time;
                    FpSpeed = i / ts.TotalSeconds;

                    Console.Write("\rDockFormGL: " + FpSpeed.ToString("0.00") + " FPS");
                
                    i = 0;
                    time = DateTime.Now;
                }
                else
                {
                    i++;
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

        protected bool IsBufferReady = false;

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

            Graphics = new GLGraphics(this);
            CreateBuffer();
            IsBufferReady = true;

            // Force the newly child-ified GLFW window to be resized to fit this control.
            ResizeNativeWindow();
            CoordinateLayout();

            // And now show the child window, since it hasn't been made visible yet.
            NativeWindow.IsVisible = true;

            // NativeWindow.Refresh
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



        #region Mouse

        protected bool MouseActive = false;
        protected Point MousePt = new(-1, -1);

        public float MouseRatioX = -2.0f;
        public float MouseRatioY = -2.0f;
        protected int MouseIndex = 0;

        protected override void OnMouseMove(MouseEventArgs e)
        {
            MousePt = e.Location;
            MouseRatioX = Graphics.GetRatioX(e.X); // (e.X * 2.0f / Width) - 1.0f;
            MouseRatioY = Graphics.GetRatioY(e.Y); // 1.0f - (e.Y * 2.0f / Height);
            MouseActive = true;
            // Console.WriteLine("OnMouseMove: " + e.X + " | " + e.Y);

            AsyncUpdateUI = true;
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                MouseIndex++;
            }
            else
            {
                MouseIndex--;
            }

            // Console.WriteLine("OnMouseWheel: " + e.Delta);
            AsyncUpdateUI = true;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            MousePt = new Point(-1, -1);
            MouseActive = false;

            AsyncUpdateUI = true;
        }

        #endregion Mouse
    }
}
