using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nitride.EE.WinUSB
{
    public abstract class UsbOutEndPoint : UsbEndPoint
    {
        public virtual bool SendData(ref uint byteCount, ref byte[] buffer)
        {
            uint byteCountIn = byteCount;
            return NativeMethods.WinUsb_WritePipe(Device.Handle, PipeId, buffer, byteCountIn, ref byteCount, IntPtr.Zero);
        }

    }
}
