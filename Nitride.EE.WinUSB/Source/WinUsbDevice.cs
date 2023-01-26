using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
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
                Console.Write(".");
                
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
                IndexL = 0,
                IndexH = 0, //x2125,
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
                IndexL = 0,
                IndexH = 0, //x2125,
                Length = dataStageLength,
                Value = value
            };

            return WinUsb_ControlTransfer(Handle, setupPacket, dataStage, dataStageLength, ref bytesReturned, IntPtr.Zero);
        }

        public bool ControlWrite(byte request, ushort value = 0)
        {
            uint bytesReturned = 0;
            WINUSB_SETUP_PACKET setupPacket = new()
            {
                RequestType = 0x41,
                Request = request,
                IndexL = 0,
                IndexH = 0, //x2125,
                Length = 0,
                Value = value
            };

            return WinUsb_ControlTransfer(Handle, setupPacket, null, 0, ref bytesReturned, IntPtr.Zero);
        }

        // private byte[] ControlReadBuffer = new byte[1024];

        public T ControlRead<T>(byte request, ushort value, byte index = 0) // where T : notnull
        {
            ushort dataStageLength = Convert.ToUInt16(Marshal.SizeOf(typeof(T)));
            byte[] buffer = new byte[dataStageLength];
            uint bytesReturned = 0;

            WINUSB_SETUP_PACKET setupPacket = new()
            {
                RequestType = 0xC1,
                Request = request,
                IndexL = 0,
                IndexH = index,
                Length = dataStageLength,
                Value = value
            };

            if (WinUsb_ControlTransfer(Handle, setupPacket, buffer, dataStageLength, ref bytesReturned, IntPtr.Zero))
            {
                if (bytesReturned == dataStageLength)
                    return buffer.DeserializeBytes<T>(); // data.DeserializeBytes(ControlReadBuffer) == bytesReturned;
                else
                    return default(T);
            }

            return default(T);
        }

        private byte[] ControlWriteBuffer = new byte[1024];

        public bool ControlWrite<T>(byte request, ushort value, T data, byte index = 0)
        {
            ushort dataStageLength = Convert.ToUInt16(data.SerializeBytes(ControlWriteBuffer));
            uint bytesReturned = 0;

            Console.Write("ControlWrite Data: " + dataStageLength + " | ");

            for (int i = 0; i < dataStageLength; i++) 
            {
                Console.Write(ControlWriteBuffer[i].ToString("X") + " ");
            }

            Console.Write("\n\r");
            
            WINUSB_SETUP_PACKET setupPacket = new()
            {
                RequestType = 0x41,
                Request = request,
                IndexL = 0,
                IndexH = index,
                Length = dataStageLength,
                Value = value
            };

            return WinUsb_ControlTransfer(Handle, setupPacket, ControlWriteBuffer, dataStageLength, ref bytesReturned, IntPtr.Zero) && (bytesReturned == dataStageLength);
        }

        public bool ControlTransfer(WINUSB_SETUP_PACKET setupPacket, byte[] buffer)
        {
            uint bytesReturned = 0;
            return WinUsb_ControlTransfer(Handle, setupPacket, buffer, setupPacket.Length, ref bytesReturned, IntPtr.Zero) && (bytesReturned == setupPacket.Length);
        }

        public void PrintInfo()
        {


            foreach (var iface in Interfaces)
            {
                Console.WriteLine("Interface: " + iface.InterfaceClass);
                iface.PrintInfo();
                
            }
        }

        public void TestStringDesc() 
        {
            byte[] buffer = new byte[64];
            WinUsb_GetDescriptor(Handle, 3, 1, 0x0409, buffer, (uint)buffer.Length, out UInt32 LengthTransfered);
            string s = System.Text.Encoding.UTF8.GetString(buffer, 0, (int)LengthTransfered);
            Console.WriteLine("USB: " + s);
        }
    }
}
