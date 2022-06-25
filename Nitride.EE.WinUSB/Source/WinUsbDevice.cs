using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nitride.EE.WinUSB
{
    public partial class WinUsbDevice : IDisposable
    {
        public WinUsbDevice(Guid guid) : this(FindDevicePathList(guid).Last()) { }

        public WinUsbDevice(string pathName)
        {
            IntPtr handle = IntPtr.Zero;

            if (pathName is not null &&
                GetDeviceFileHandle(pathName) is var devHandle &&
                !devHandle.IsInvalid &&
                WinUsb_Initialize(devHandle, ref handle))
            {
                DeviceHandle = devHandle;
                Handle = handle;
                byte interfaceIndex = 0;
                while (true)
                {
                    if (WinUsb_QueryInterfaceSettings(handle, interfaceIndex, out USB_INTERFACE_DESCRIPTOR ifaceDescriptor))
                    {
                        var iface = new UsbInterface(this, ifaceDescriptor);

                        for (int i = 0; i < ifaceDescriptor.bNumEndpoints; i++)
                        {
                            WinUsb_QueryPipe(handle, interfaceIndex, Convert.ToByte(i), out WINUSB_PIPE_INFORMATION pipeInfo);
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
                                    iface.EndPoints.Add(new BulkInEndPoint(iface, pipeInfo));
                                    break;

                                case (UsbdPipeType.Bulk, false): // Bulk Out
                                    iface.EndPoints.Add(new BulkOutEndPoint(iface, pipeInfo));
                                    break;

                                case (UsbdPipeType.Interrupt, true): // Interrupt In
                                    iface.EndPoints.Add(new InterruptInEndPoint(iface, pipeInfo));
                                    break;

                                case (UsbdPipeType.Interrupt, false): // Interrupt Out
                                    iface.EndPoints.Add(new InterruptOutEndPoint(iface, pipeInfo));
                                    break;

                                case (UsbdPipeType.Isochronous, true): // Isochronous In
                                    iface.EndPoints.Add(new IsochronousInEndPoint(iface, pipeInfo));
                                    break;

                                case (UsbdPipeType.Isochronous, false): // Isochronous Out
                                    iface.EndPoints.Add(new IsochronousOutEndPoint(iface, pipeInfo));
                                    break;

                                default: break;
                            }
                        }

                        Interfaces.Add(iface);
                    }
                    else
                        break;

                    interfaceIndex++;
                }
            }
            else
                Console.WriteLine("Device Not Found");
                
               // throw new Exception("Device Handle is invalid or Device not found.");


        }

        ~WinUsbDevice()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public bool IsDisposed { get; private set; } = false;

        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed)
                return;

            if (disposing)
            {
                // Dispose managed resources
                if (DeviceHandle is not null && !DeviceHandle.IsInvalid)
                    DeviceHandle.Dispose();
                DeviceHandle = null;
            }

            // Dispose unmanaged resources
            FreeWinUSB();
            IsDisposed = true;
        }

        private void FreeWinUSB()
        {
            if (Handle != IntPtr.Zero)
                WinUsb_Free(Handle);
            Handle = IntPtr.Zero;
        }

        private SafeFileHandle DeviceHandle { get; set; }

        public IntPtr Handle { get; set; }

        public List<UsbInterface> Interfaces { get; } = new();

        public uint Speed
        {
            get
            {
                uint length = 1;

                if (WinUsb_QueryDeviceInformation(Handle, DEVICE_SPEED, ref length, out byte speed))
                    return Convert.ToUInt32(speed);
                else
                    throw new Exception("Failed to check device speed.");
            }
        }

        public bool ControlRead(byte request, ushort value, ref byte[] dataStage, ref uint bytesReturned)
        {
            ushort dataStageLength = Convert.ToUInt16(dataStage.Length);

            WINUSB_SETUP_PACKET setupPacket = new()
            {
                RequestType = 0xC1,
                Request = request,
                Index = 0,
                Length = dataStageLength,
                Value = value
            };

            return WinUsb_ControlTransfer(Handle, setupPacket, dataStage, dataStageLength, ref bytesReturned, IntPtr.Zero);
        }

        public bool ControlWrite(byte request, ushort value, ref byte[] dataStage, ref uint bytesReturned)
        {
            ushort dataStageLength = Convert.ToUInt16(dataStage.Length);

            WINUSB_SETUP_PACKET setupPacket = new()
            {
                RequestType = 0x41,
                Request = request,
                Index = 0, //x2125,
                Length = dataStageLength,
                Value = value
            };

            return WinUsb_ControlTransfer(Handle, setupPacket, dataStage, dataStageLength, ref bytesReturned, IntPtr.Zero);
        }

        private readonly byte[] EmptyBuffer = new byte[] { 0, 0, 0, 0 }; //Array.Empty<byte>();

        public bool ControlWrite(byte request, ushort value)
        {
            uint length_returned = 0;
            ushort dataStageLength = Convert.ToUInt16(EmptyBuffer.Length);

            WINUSB_SETUP_PACKET setupPacket = new()
            {
                RequestType = 0x41,
                Request = request,
                Index = 0, //x2125,
                Length = dataStageLength,
                Value = value
            };

            return WinUsb_ControlTransfer(Handle, setupPacket, EmptyBuffer, dataStageLength, ref length_returned, IntPtr.Zero);
        }

        public void PrintInfo()
        {
            foreach (var iface in Interfaces)
            {
                Console.WriteLine("Interface: " + iface.InterfaceClass);
                iface.PrintInfo();
                
            }
        }
    }
}
