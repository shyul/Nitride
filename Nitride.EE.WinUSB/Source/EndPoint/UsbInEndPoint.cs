using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Nitride.EE.WinUSB
{
    public abstract class UsbInEndPoint : UsbEndPoint
    {
        public virtual bool Read(byte[] buffer)
        {
            return Read(buffer, 0, (uint)buffer.Length, out _);
        }

        public bool Read(byte[] buffer, int offset, uint bytesToRead, out uint bytesRead)
        {
            unsafe
            {
                fixed (byte* pBuffer = buffer)
                {
                    return WinUsb_ReadPipe(Device.Handle, PipeId, pBuffer + offset, bytesToRead, out bytesRead, IntPtr.Zero);
                }
            }
        }

        [DllImport("winusb.dll", SetLastError = true)]
        private static unsafe extern bool WinUsb_ReadPipe(IntPtr InterfaceHandle, byte PipeID, byte* pBuffer, uint BufferLength, out uint LengthTransferred, IntPtr Overlapped);

        [DllImport("winusb.dll", SetLastError = true)]
        private static unsafe extern bool WinUsb_ReadPipe(IntPtr InterfaceHandle, byte PipeID, byte* pBuffer, uint BufferLength, out uint LengthTransferred, NativeOverlapped* pOverlapped);
    }
}
