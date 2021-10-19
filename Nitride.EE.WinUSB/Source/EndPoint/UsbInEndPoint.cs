using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nitride.EE.WinUSB
{
    public abstract class UsbInEndPoint : UsbEndPoint
    {
        public virtual bool ReceiveData(ref uint byteCount, ref byte[] buffer)
        {
            uint byteCountIn = byteCount;
            return NativeMethods.WinUsb_ReadPipe(Device.Handle, PipeId, buffer, byteCountIn, ref byteCount, IntPtr.Zero);
        }
    }
}
