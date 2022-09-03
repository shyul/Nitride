using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Nitride.EE.WinUSB
{
    public abstract class UsbOutEndPoint : UsbEndPoint
    {
        public bool Write(byte[] buffer)
        {
            return Write(buffer, 0, (uint)buffer.Length, out _);
        }

        public bool Write(byte[] buffer, uint offset, uint bytesToWrite, out uint bytesWritten)
        {
            bool success;
            unsafe
            {
                fixed (byte* pBuffer = buffer)
                {
                    success = WinUsb_WritePipe(Device.Handle, PipeId, pBuffer + offset, bytesToWrite, out bytesWritten, IntPtr.Zero);
                }
            }

            return success && (bytesWritten == bytesToWrite);
        }

        [DllImport("winusb.dll", SetLastError = true)]
        private static unsafe extern bool WinUsb_WritePipe(IntPtr InterfaceHandle, byte PipeID, byte* pBuffer, uint BufferLength, out uint LengthTransferred, IntPtr Overlapped);

        [DllImport("winusb.dll", SetLastError = true)]
        private static unsafe extern bool WinUsb_WritePipe(IntPtr InterfaceHandle, byte PipeID, byte* pBuffer, uint BufferLength, out uint LengthTransferred, NativeOverlapped* pOverlapped);
    }
}
