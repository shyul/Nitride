using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nitride.EE.WinUSB
{
    public class UsbInterface
    {
        public UsbInterface(WinUsbDevice device, USB_INTERFACE_DESCRIPTOR desc)
        {
            Device = device;
            // Length = desc.bLength;
            // DescriptorType = desc.bDescriptorType;
            InterfaceNumber = desc.bInterfaceNumber;
            AlternateSetting = desc.bAlternateSetting;
            NumEndpoints = desc.bNumEndpoints;
            InterfaceClass = desc.bInterfaceClass;
            InterfaceSubClass = desc.bInterfaceSubClass;
            InterfaceProtocol = desc.bInterfaceProtocol;
            Interface = desc.iInterface;
        }

        public WinUsbDevice Device { get; }

        // public byte Length { get; set; }

        // public byte DescriptorType { get; set; }

        public byte InterfaceNumber { get; set; }

        public byte AlternateSetting { get; set; }

        public byte NumEndpoints { get; set; }

        public byte InterfaceClass { get; set; }

        public byte InterfaceSubClass { get; set; }

        public byte InterfaceProtocol { get; set; }

        public byte Interface { get; set; }

        public List<UsbEndPoint> EndPoints { get; } = new();

        public void PrintInfo()
        {
            foreach (var ep in EndPoints)
            {
                Console.WriteLine("Interface EP: " + ep.ToString());
            }
        }
    }
}
