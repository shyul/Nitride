using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Nitride.EE.WinUSB
{
	[StructLayout(LayoutKind.Sequential)]
	public struct USB_CONFIGURATION_DESCRIPTOR
	{
		public byte bLength;
		public byte bDescriptorType;
		public ushort wTotalLength;
		public byte bNumInterfaces;
		public byte bConfigurationValue;
		public byte iConfiguration;
		public byte bmAttributes;
		public byte MaxPower;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct USB_INTERFACE_DESCRIPTOR
	{
		public byte bLength;
		public byte bDescriptorType;
		public byte bInterfaceNumber;
		public byte bAlternateSetting;
		public byte bNumEndpoints;
		public byte bInterfaceClass;
		public byte bInterfaceSubClass;
		public byte bInterfaceProtocol;
		public byte iInterface;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct WINUSB_PIPE_INFORMATION
	{
		public UsbdPipeType PipeType;
		public byte PipeId;
		public ushort MaximumPacketSize;
		public byte Interval;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct USBD_ISO_PACKET_DESCRIPTOR
	{
		public uint Offset;
		public uint Length;
		public uint Status;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct WINUSB_SETUP_PACKET
	{
		public byte RequestType;
		public byte Request;
		public ushort Value;
		public ushort Index;
		public ushort Length;
	}

	public struct SP_DEVICE_INTERFACE_DATA
	{
		public int cbSize;
		public Guid InterfaceClassGuid;
		public int Flags;
		public IntPtr Reserved;
	}

	public struct SP_DEVICE_INTERFACE_DETAIL_DATA
	{
		public int cbSize;
		public string DevicePath;
	}

	public struct SP_DEVINFO_DATA
	{
		public int cbSize;
		public Guid ClassGuid;
		public int DevInst;
		public int Reserved;
	}
}
