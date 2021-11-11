using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nitride.EE.WinUSB
{
    public class InterruptInEndPoint : UsbInEndPoint
    {
        public InterruptInEndPoint(UsbInterface iface, WINUSB_PIPE_INFORMATION pipeInfo)
        {
            Interface = iface;
            PipeId = pipeInfo.PipeId;
            MaximumPacketSize = pipeInfo.MaximumPacketSize;
            Timeout = 2000;
            //IgnoreShortPackets = false;
            Interval = pipeInfo.Interval;
        }

        // Delegate Function for responding to interrupts

        public byte Interval { get; }
    }
}
