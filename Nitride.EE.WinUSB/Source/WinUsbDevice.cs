using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Nitride.EE.WinUSB
{
    public class WinUsbDevice
    {
        public WinUsbDevice(Guid guid) : this(NativeMethods.FindDevicePathName(guid)) { }

        public WinUsbDevice(string pathName)
        {
            WinUsbHandle handle = new();

            if (pathName is not null &&
                NativeMethods.GetDeviceFileHandle(pathName) is var fileHandle &&
                !fileHandle.IsInvalid &&
                NativeMethods.WinUsb_Initialize(fileHandle, ref handle))
            {
                Handle = handle;
                byte interfaceIndex = 0;
                while (true)
                {
                    USB_INTERFACE_DESCRIPTOR ifaceDescriptor = new();
                    if (NativeMethods.WinUsb_QueryInterfaceSettings(handle, interfaceIndex, ref ifaceDescriptor))
                    {
                        var iface = new UsbInterface(this, ifaceDescriptor);
                        List<UsbEndPoint> endpoints = new();

                        for (int i = 0; i < ifaceDescriptor.bNumEndpoints; i++)
                        {
                            WINUSB_PIPE_INFORMATION pipeInfo = new();
                            NativeMethods.WinUsb_QueryPipe(handle, interfaceIndex, Convert.ToByte(i), ref pipeInfo);
                            bool IsDirectionIn = (pipeInfo.PipeId & 0x80) == 0x80; // false == out;

                            switch ((pipeInfo.PipeType, IsDirectionIn))
                            {
                                /*
                                case (UsbdPipeType.Control, true): // Control In
                                    endpoints.Add(new ControlInEndPoint(iface, pipeInfo));
                                    break;

                                case (UsbdPipeType.Control, false): // Control Out
                                    endpoints.Add(new ControlOutEndPoint(iface, pipeInfo));
                                    break;*/

                                case (UsbdPipeType.Bulk, true): // Bulk In
                                    endpoints.Add(new BulkInEndPoint(iface, pipeInfo));
                                    break;

                                case (UsbdPipeType.Bulk, false): // Bulk Out
                                    endpoints.Add(new BulkOutEndPoint(iface, pipeInfo));
                                    break;

                                case (UsbdPipeType.Interrupt, true): // Interrupt In
                                    endpoints.Add(new InterruptInEndPoint(iface, pipeInfo));
                                    break;

                                case (UsbdPipeType.Interrupt, false): // Interrupt Out
                                    endpoints.Add(new InterruptOutEndPoint(iface, pipeInfo));
                                    break;

                                case (UsbdPipeType.Isochronous, true): // Isochronous In
                                    endpoints.Add(new IsochronousInEndPoint(iface, pipeInfo));
                                    break;

                                case (UsbdPipeType.Isochronous, false): // Isochronous Out
                                    endpoints.Add(new IsochronousOutEndPoint(iface, pipeInfo));
                                    break;

                                default: break;
                            }
                        }

                        iface.EndPoints = endpoints.ToDictionary(n => n.PipeId, n => n);
                        Interfaces.Add(iface);
                    }
                    else
                        break;

                    interfaceIndex++;
                }
            }
            else
                throw new Exception("Device Handle is invalid or Device not found.");


        }

        ~WinUsbDevice()
        {
            Handle.Release();
            Handle.Dispose();
        }

        public WinUsbHandle Handle { get; }

        public List<UsbInterface> Interfaces { get; } = new();

        public uint Speed
        {
            get
            {
                uint length = 1;
                var speed = new byte[1];

                if (NativeMethods.WinUsb_QueryDeviceInformation(Handle, NativeMethods.DEVICE_SPEED, ref length, ref speed[0]))
                    return Convert.ToUInt32(speed[0]);
                else
                    throw new Exception("Failed to check device speed.");
            }
        }

        public bool ControlRead(ref byte[] dataStage, ref uint bytesReturned)
        {
            ushort dataStageLength = Convert.ToUInt16(dataStage.Length);

            WINUSB_SETUP_PACKET setupPacket = new()
            {
                RequestType = 0xC1,
                Request = 2,
                Index = 0,
                Length = dataStageLength,
                Value = 0
            };

            return NativeMethods.WinUsb_ControlTransfer(Handle, setupPacket, dataStage, dataStageLength, ref bytesReturned, IntPtr.Zero);
        }

        public bool ControlWrite(ref byte[] dataStage, ref uint bytesReturned, ushort value = 0)
        {
            ushort dataStageLength = Convert.ToUInt16(dataStage.Length);

            WINUSB_SETUP_PACKET setupPacket = new()
            {
                RequestType = 0x41,
                Request = 1,
                Index = 0,
                Length = dataStageLength,
                Value = value
            };

            return NativeMethods.WinUsb_ControlTransfer(Handle, setupPacket, dataStage, dataStageLength, ref bytesReturned, IntPtr.Zero);
        }
    }
}
