using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nitride.EE.WinUSB
{
    public abstract class UsbEndPoint
    {
        public WinUsbDevice Device => Interface.Device;

        public UsbInterface Interface { get; protected set; }

        public byte PipeId { get; protected set; }

        public ushort MaximumPacketSize { get; protected set; }

        public uint Timeout
        {
            get => m_Timeout; 
            set
            {
                m_Timeout = value;
                if (!SetPolicy(PolicyType.PIPE_TRANSFER_TIMEOUT, ref m_Timeout))
                    throw new Exception("Set Pipe Timeout fail!");
            }
        }

        protected uint m_Timeout;

        public bool IgnoreShortPackets
        {
            get => m_IgnoreShortPackets;
            set
            {
                byte val = Convert.ToByte(value);
                if (!SetPolicy(PolicyType.IGNORE_SHORT_PACKETS, ref val))
                    throw new Exception("Set Pipe policy fail!");
                m_IgnoreShortPackets = Convert.ToBoolean(val);
            }
        }

        protected bool m_IgnoreShortPackets;

        public bool SetPolicy(uint policyType, ref byte val) => NativeMethods.WinUsb_SetPipePolicy(Device.Handle, PipeId, policyType, 1, ref val);

        public bool SetPolicy(uint policyType, ref uint val) => NativeMethods.WinUsb_SetPipePolicyUint(Device.Handle, PipeId, policyType, 4, ref val);
    }
}
