using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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

        public bool Abort() => WinUsb_AbortPipe(Device.Handle, PipeId);

        public bool Flush() => WinUsb_FlushPipe(Device.Handle, PipeId);

        public bool SetPolicy(uint policyType, ref byte val) => WinUsb_SetPipePolicy(Device.Handle, PipeId, policyType, 1, ref val);

        public bool SetPolicy(uint policyType, ref uint val) => WinUsb_SetPipePolicy(Device.Handle, PipeId, policyType, 4, ref val);

        public override string ToString() => GetType().Name + ": 0x" + PipeId.ToString("X") + " Timeout = " + Timeout + "ms";

        //  Two declarations for WinUsb_SetPipePolicy.
        //  Use this one when the returned Value is a Byte (all except PIPE_TRANSFER_TIMEOUT):

        [DllImport("winusb.dll", SetLastError = true)]
        private static extern bool WinUsb_SetPipePolicy(IntPtr InterfaceHandle, Byte PipeID, UInt32 PolicyType, UInt32 ValueLength, ref byte Value);

        [DllImport("winusb.dll", SetLastError = true)]
        private static extern bool WinUsb_GetPipePolicy(IntPtr InterfaceHandle, Byte PipeID, UInt32 PolicyType, ref UInt32 ValueLength, out byte Value);

        //  Use this alias when the returned Value is a UInt32 (PIPE_TRANSFER_TIMEOUT only):

        [DllImport("winusb.dll", SetLastError = true)]
        private static extern bool WinUsb_SetPipePolicy(IntPtr InterfaceHandle, Byte PipeID, UInt32 PolicyType, UInt32 ValueLength, ref UInt32 Value);

        [DllImport("winusb.dll", SetLastError = true)]
        private static extern bool WinUsb_GetPipePolicy(IntPtr InterfaceHandle, Byte PipeID, UInt32 PolicyType, ref UInt32 ValueLength, out UInt32 Value);

        [DllImport("winusb.dll", SetLastError = true)]
        private static extern bool WinUsb_AbortPipe(IntPtr InterfaceHandle, byte PipeID);

        [DllImport("winusb.dll", SetLastError = true)]
        private static extern bool WinUsb_FlushPipe(IntPtr InterfaceHandle, byte PipeID);

        [DllImport("winusb.dll", SetLastError = true)]
        protected static extern bool WinUsb_RegisterIsochBuffer(IntPtr InterfaceHandle, byte PipeID, byte[] Buffer, uint BufferLength, out IntPtr BufferHandle);

        [DllImport("winusb.dll", SetLastError = true)]
        protected static extern bool WinUsb_UnregisterIsochBuffer(IntPtr BufferHandle);
    }
}
