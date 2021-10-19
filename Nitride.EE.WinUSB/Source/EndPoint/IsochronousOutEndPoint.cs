using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nitride.EE.WinUSB
{
    public class IsochronousOutEndPoint : UsbEndPoint
    {
        public IsochronousOutEndPoint(UsbInterface iface, WINUSB_PIPE_INFORMATION pipeInfo)
        {
            Interface = iface;
            PipeId = pipeInfo.PipeId;
            MaximumPacketSize = pipeInfo.MaximumPacketSize;
            Timeout = 2000;
            IgnoreShortPackets = false;

			if (pipeInfo.Interval != 1)
			{
				throw new Exception("Isochronous EndPoint Interval must be 1: " + pipeInfo.Interval);
			}
		}

		public virtual bool SendData(ref byte[] buffer)
		{
			uint bufferLength = (uint)buffer.Length;

			var success = NativeMethods.WinUsb_RegisterIsochBuffer(Device.Handle, PipeId, buffer, bufferLength, out IntPtr bufferHandle);
			if (success)
			{
				success = NativeMethods.WinUsb_WriteIsochPipeAsap(bufferHandle, 0, bufferLength, false, IntPtr.Zero);
				if (success)
				{
					success = NativeMethods.WinUsb_UnregisterIsochBuffer(bufferHandle);
				}
			}
			return success;
		}
	}
}
