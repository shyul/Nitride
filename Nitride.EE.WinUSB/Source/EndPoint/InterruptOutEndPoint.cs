using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nitride.EE.WinUSB
{
    /// <summary>
    /// https://www.beyondlogic.org/usbnutshell/usb4.shtml
    /// </summary>
    public class InterruptOutEndPoint : UsbOutEndPoint
    {
        public InterruptOutEndPoint(UsbInterface iface, WINUSB_PIPE_INFORMATION pipeInfo)
        {
            Interface = iface;
            PipeId = pipeInfo.PipeId;
            MaximumPacketSize = pipeInfo.MaximumPacketSize;
            Timeout = 2000;
            //IgnoreShortPackets = false;
            Interval = pipeInfo.Interval;
        }

        // Added Task to refresh the transfer all the time.

        public byte Interval { get; }
    }
}
