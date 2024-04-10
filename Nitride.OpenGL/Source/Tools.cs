using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Drawing;

namespace Nitride.OpenGL
{
    public static partial class GLTools
    {
        public static int CreateProgram(string vShader, string fShader)
        {
            int vShaderHanle = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vShaderHanle, vShader);
            GL.CompileShader(vShaderHanle);
            Console.WriteLine("VertexShader Log = " + GL.GetShaderInfoLog(vShaderHanle));

            int fShaderHanle = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fShaderHanle, fShader);
            GL.CompileShader(fShaderHanle);
            Console.WriteLine("FragmentShader Log = " + GL.GetShaderInfoLog(fShaderHanle));

            int handle = GL.CreateProgram();
            GL.AttachShader(handle, vShaderHanle);
            GL.AttachShader(handle, fShaderHanle);
            GL.LinkProgram(handle);
            Console.WriteLine("ShaderProgram Log = " + GL.GetShaderInfoLog(handle));

            GL.DetachShader(handle, vShaderHanle);
            GL.DetachShader(handle, fShaderHanle);

            GL.DeleteShader(vShaderHanle);
            GL.DeleteShader(fShaderHanle);

            return handle;
        }

        public static (int bufferHandle, int arrayHandle) CreateBuffer<T>(T[] buffer, int length, BufferUsageHint usage = BufferUsageHint.StreamDraw) where T : struct
        {
            Type type = typeof(T);
            int typeSize = Marshal.SizeOf(type);

            int bufferHandle = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, bufferHandle);
            GL.BufferData(BufferTarget.ArrayBuffer, length * typeSize, buffer, usage);

            int arrayHandle = GL.GenVertexArray();
            GL.BindVertexArray(arrayHandle);
            //GL.BindBuffer(BufferTarget.ArrayBuffer, bufferHandle);

            int i = 0, offset = 0;
            
            // Console.WriteLine("Fields:");
            foreach (FieldInfo field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                int size = Marshal.SizeOf(field.FieldType);
                int sizeFloat = size / sizeof(float);
                GL.VertexAttribPointer(i, sizeFloat, VertexAttribPointerType.Float, false, typeSize, offset);
                GL.EnableVertexAttribArray(i);
                // Console.WriteLine($"Name: {field.Name}, Type: {field.FieldType}, Size = " + size + ", Step = " + sizeFloat);
                i++;
                offset += size;
            }

            /*
            Console.WriteLine("\nProperties:");
            foreach (PropertyInfo property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                Console.WriteLine($"Name: {property.Name}, Type: {property.PropertyType}");
            }*/

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);

            return (bufferHandle, arrayHandle);
        }

        public static void UpdateBuffer<T>(int bufferHandle, int arrayHandle, T[] buffer, int length) where T : struct
        {
            Type type = typeof(T);
            int typeSize = Marshal.SizeOf(type);

            GL.BindBuffer(BufferTarget.ArrayBuffer, bufferHandle);
            GL.BindVertexArray(arrayHandle);
            GL.BufferData(BufferTarget.ArrayBuffer, length * typeSize, buffer, BufferUsageHint.StreamDraw);
        }

        public static int CreateTexture(Bitmap bmp)
        {
            int textureId = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, textureId);

            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp.Width, bmp.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            bmp.UnlockBits(data);

            // GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
            //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            // https://opentk.net/learn/chapter1/5-textures.html?tabs=load-texture-opentk4

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear); // Nearest);

            GL.BindTexture(TextureTarget.Texture2D, 0);

            return textureId;
        }





    }

    /// <summary>
    /// P/Invoke functions and declarations for Microsoft Windows (32-bit and 64-bit).
    /// </summary>
    internal static class Win32
    {
        #region Enums

        public enum WindowLongs : int
        {
            GWL_EXSTYLE = -20,
            GWLP_HINSTANCE = -6,
            GWLP_HWNDPARENT = -8,
            GWL_ID = -12,
            GWL_STYLE = -16,
            GWL_USERDATA = -21,
            GWL_WNDPROC = -4,
            DWLP_DLGPROC = 4,
            DWLP_MSGRESULT = 0,
            DWLP_USER = 8,
        }

        [Flags]
        public enum WindowStyles : uint
        {
            WS_BORDER = 0x800000,
            WS_CAPTION = 0xc00000,
            WS_CHILD = 0x40000000,
            WS_CLIPCHILDREN = 0x2000000,
            WS_CLIPSIBLINGS = 0x4000000,
            WS_DISABLED = 0x8000000,
            WS_DLGFRAME = 0x400000,
            WS_GROUP = 0x20000,
            WS_HSCROLL = 0x100000,
            WS_MAXIMIZE = 0x1000000,
            WS_MAXIMIZEBOX = 0x10000,
            WS_MINIMIZE = 0x20000000,
            WS_MINIMIZEBOX = 0x20000,
            WS_OVERLAPPED = 0x0,
            WS_OVERLAPPEDWINDOW = WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_SIZEFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX,
            WS_POPUP = 0x80000000u,
            WS_POPUPWINDOW = WS_POPUP | WS_BORDER | WS_SYSMENU,
            WS_SIZEFRAME = 0x40000,
            WS_SYSMENU = 0x80000,
            WS_TABSTOP = 0x10000,
            WS_VISIBLE = 0x10000000,
            WS_VSCROLL = 0x200000,
        }

        [Flags]
        public enum WindowStylesEx : uint
        {
            WS_EX_ACCEPTFILES = 0x00000010,
            WS_EX_APPWINDOW = 0x00040000,
            WS_EX_CLIENTEDGE = 0x00000200,
            WS_EX_COMPOSITED = 0x02000000,
            WS_EX_CONTEXTHELP = 0x00000400,
            WS_EX_CONTROLPARENT = 0x00010000,
            WS_EX_DLGMODALFRAME = 0x00000001,
            WS_EX_LAYERED = 0x00080000,
            WS_EX_LAYOUTRTL = 0x00400000,
            WS_EX_LEFT = 0x00000000,
            WS_EX_LEFTSCROLLBAR = 0x00004000,
            WS_EX_LTRREADING = 0x00000000,
            WS_EX_MDICHILD = 0x00000040,
            WS_EX_NOACTIVATE = 0x08000000,
            WS_EX_NOINHERITLAYOUT = 0x00100000,
            WS_EX_NOPARENTNOTIFY = 0x00000004,
            WS_EX_NOREDIRECTIONBITMAP = 0x00200000,
            WS_EX_OVERLAPPEDWINDOW = WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE,
            WS_EX_PALETTEWINDOW = WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST,
            WS_EX_RIGHT = 0x00001000,
            WS_EX_RIGHTSCROLLBAR = 0x00000000,
            WS_EX_RTLREADING = 0x00002000,
            WS_EX_STATICEDGE = 0x00020000,
            WS_EX_TOOLWINDOW = 0x00000080,
            WS_EX_TOPMOST = 0x00000008,
            WS_EX_TRANSPARENT = 0x00000020,
            WS_EX_WINDOWEDGE = 0x00000100,
        }

        #endregion

        #region Miscellaneous User32 stuff

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        #endregion

        #region Miscellaneous Kernel32 stuff

        public static int GetLastError()
            => Marshal.GetLastWin32Error();     // This alias isn't strictly needed, but it reads better.

        #endregion

        #region GetWindowLong/SetWindowLong and friends

        public static IntPtr GetWindowLongPtr(IntPtr hWnd, WindowLongs nIndex)
            => GetWindowLongPtr(hWnd, (int)nIndex);

        public static IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex)
        {
            if (IntPtr.Size == 8)
                return GetWindowLongPtr64(hWnd, nIndex);
            else
                return GetWindowLongPtr32(hWnd, nIndex);
        }

        [DllImport("user32.dll", EntryPoint = "GetWindowLong", SetLastError = true)]
        private static extern IntPtr GetWindowLongPtr32(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr", SetLastError = true)]
        private static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);

        public static IntPtr SetWindowLongPtr(IntPtr hWnd, WindowLongs nIndex, IntPtr dwNewLong)
            => SetWindowLongPtr(hWnd, (int)nIndex, dwNewLong);

        public static IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
        {
            if (IntPtr.Size == 8)
                return SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
            else
                return new IntPtr(SetWindowLong32(hWnd, nIndex, dwNewLong.ToInt32()));
        }

        [DllImport("user32.dll", EntryPoint = "SetWindowLong", SetLastError = true)]
        private static extern int SetWindowLong32(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", SetLastError = true)]
        private static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        #endregion
    }
}
