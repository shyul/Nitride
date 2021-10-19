using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Nitride.EE.WinUSB
{
	public static class NativeMethods
	{
		#region winusb.dll

		internal const uint DEVICE_SPEED = 1;
		internal const byte USB_ENDPOINT_DIRECTION_MASK = 0X80;

		[DllImport("winusb.dll", SetLastError = true)]
		internal static extern bool WinUsb_ControlTransfer(WinUsbHandle usbHandle, WINUSB_SETUP_PACKET SetupPacket, byte[] Buffer, uint BufferLength, ref uint LengthTransferred, IntPtr Overlapped);

		[DllImport("winusb.dll", SetLastError = true)]
		internal static extern bool WinUsb_Free(IntPtr InterfaceHandle);

		[DllImport("winusb.dll", SetLastError = true)]
		internal static extern bool WinUsb_Initialize(SafeFileHandle DeviceHandle, ref WinUsbHandle usbHandle);

		/// <summary>
		/// Use this declaration to retrieve DEVICE_SPEED (the only currently defined InformationType).
		/// </summary>
		/// <param name="InterfaceHandle"></param>
		/// <param name="InformationType"></param>
		/// <param name="BufferLength"></param>
		/// <param name="Buffer"></param>
		/// <returns></returns>
		[DllImport("winusb.dll", SetLastError = true)]
		internal static extern bool WinUsb_QueryDeviceInformation(WinUsbHandle usbHandle, uint InformationType, ref uint BufferLength, ref byte Buffer);

		[DllImport("winusb.dll", SetLastError = true)]
		internal static extern bool WinUsb_QueryInterfaceSettings(WinUsbHandle usbHandle, byte AlternateInterfaceNumber, ref USB_INTERFACE_DESCRIPTOR UsbAltInterfaceDescriptor);

		[DllImport("winusb.dll", SetLastError = true)]
		internal static extern bool WinUsb_QueryPipe(WinUsbHandle usbHandle, byte AlternateInterfaceNumber, byte PipeIndex, ref WINUSB_PIPE_INFORMATION PipeInformation);

		[DllImport("winusb.dll", SetLastError = true)]
		internal static extern bool WinUsb_ReadIsochPipeAsap(IntPtr BufferHandle, uint Offset, uint Length, bool ContinueStream, uint NumberOfPackets, ref USBD_ISO_PACKET_DESCRIPTOR IsoPacketDescriptors, IntPtr Overlapped);

		[DllImport("winusb.dll", SetLastError = true)]
		internal static extern bool WinUsb_ReadPipe(WinUsbHandle usbHandle, byte PipeID, byte[] Buffer, uint BufferLength, ref uint LengthTransferred, IntPtr Overlapped);

		[DllImport("winusb.dll", SetLastError = true)]
		internal static extern bool WinUsb_RegisterIsochBuffer(WinUsbHandle usbHandle, byte PipeID, byte[] Buffer, uint BufferLength, out IntPtr BufferHandle);

		[DllImport("winusb.dll", SetLastError = true)]
		internal static extern bool WinUsb_SetCurrentAlternateSetting(WinUsbHandle usbHandle, byte AlternateSetting);

		/// <summary>
		/// Two declarations for WinUsb_SetPipePolicy. 
		/// Use this one when the returned Value is a byte (all except PIPE_TRANSFER_TIMEOUT):
		/// </summary>
		/// <param name="usbHandle"></param>
		/// <param name="PipeID"></param>
		/// <param name="PolicyType"></param>
		/// <param name="ValueLength"></param>
		/// <param name="Value"></param>
		/// <returns></returns>
		[DllImport("winusb.dll", SetLastError = true)]
		internal static extern bool WinUsb_SetPipePolicy(WinUsbHandle usbHandle, byte PipeID, uint PolicyType, uint ValueLength, ref byte Value);

		/// <summary>
		/// Use this alias when the returned Value is a uint (PIPE_TRANSFER_TIMEOUT only):
		/// </summary>
		/// <param name="usbHandle"></param>
		/// <param name="PipeID"></param>
		/// <param name="PolicyType"></param>
		/// <param name="ValueLength"></param>
		/// <param name="Value"></param>
		/// <returns></returns>
		[DllImport("winusb.dll", SetLastError = true, EntryPoint = "WinUsb_SetPipePolicy")]
		internal static extern bool WinUsb_SetPipePolicyUint(WinUsbHandle usbHandle, byte PipeID, uint PolicyType, uint ValueLength, ref uint Value);

		[DllImport("winusb.dll", SetLastError = true)]
		internal static extern bool WinUsb_UnregisterIsochBuffer(IntPtr BufferHandle);

		[DllImport("winusb.dll", SetLastError = true)]
		internal static extern bool WinUsb_WriteIsochPipeAsap(IntPtr BufferHandle, uint Offset, uint Length, bool ContinueStream, IntPtr Overlapped);

		[DllImport("winusb.dll", SetLastError = true)]
		internal static extern bool WinUsb_WritePipe(WinUsbHandle usbHandle, byte PipeID, byte[] Buffer, uint BufferLength, ref uint LengthTransferred, IntPtr Overlapped);

		/// <summary>
		/// Device path name returned by SetupDiGetDeviceInterfaceDetail
		///  Type of access requested (read/write).
		///  FILE_SHARE attributes to allow other processes to access the device while this handle is open.
		///  Security structure. Using Null for this may cause problems under Windows XP.
		///  Creation disposition value. Use OPEN_EXISTING for devices.
		///  Flags and attributes for files. The winsub driver requires FILE_FLAG_OVERLAPPED.
		///  Handle to a template file. Not used.
		/// </summary>
		/// <param name="devicePathName"></param>
		/// <returns>A handle or INVALID_HANDLE_VALUE.</returns>
		public static SafeFileHandle GetDeviceFileHandle(string devicePathName) => CreateFile
					(devicePathName,
					 (GENERIC_WRITE | GENERIC_READ),
					 FILE_SHARE_READ | FILE_SHARE_WRITE,
					 IntPtr.Zero,
					 OPEN_EXISTING,
					 FILE_ATTRIBUTE_NORMAL | FILE_FLAG_OVERLAPPED,
					 IntPtr.Zero);

		#endregion winusb.dll

		#region setupapi.dll

		internal const int DIGCF_PRESENT = 2;
		internal const int DIGCF_DEVICEINTERFACE = 0X10;

		[DllImport("setupapi.dll", SetLastError = true)]
		internal static extern IntPtr SetupDiCreateDeviceInfoList(ref Guid ClassGuid, IntPtr hwndParent);

		[DllImport("setupapi.dll", SetLastError = true)]
		internal static extern int SetupDiDestroyDeviceInfoList(IntPtr DeviceInfoSet);

		[DllImport("setupapi.dll", SetLastError = true)]
		internal static extern bool SetupDiEnumDeviceInterfaces(IntPtr DeviceInfoSet, IntPtr DeviceInfoData, ref Guid InterfaceClassGuid, int MemberIndex, ref SP_DEVICE_INTERFACE_DATA DeviceInterfaceData);

		[DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
		internal static extern IntPtr SetupDiGetClassDevs(ref Guid ClassGuid, IntPtr Enumerator, IntPtr hwndParent, int Flags);

		[DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
		internal static extern bool SetupDiGetDeviceInterfaceDetail(IntPtr DeviceInfoSet, ref SP_DEVICE_INTERFACE_DATA DeviceInterfaceData, IntPtr DeviceInterfaceDetailData, int DeviceInterfaceDetailDataSize, ref int RequiredSize, IntPtr DeviceInfoData);

		public static string FindDevicePathName(Guid guid)
		{
			int bufferSize = 0;
			bool lastDevice = false;
			var myDeviceInterfaceData = new SP_DEVICE_INTERFACE_DATA();
			myDeviceInterfaceData.cbSize = Marshal.SizeOf(myDeviceInterfaceData);
			var deviceInfoSet = SetupDiGetClassDevs(ref guid, IntPtr.Zero, IntPtr.Zero, DIGCF_PRESENT | DIGCF_DEVICEINTERFACE);
			int memberIndex = 0;
			string devicePathName = null;

			do
			{
				if (SetupDiEnumDeviceInterfaces(deviceInfoSet, IntPtr.Zero, ref guid, memberIndex, ref myDeviceInterfaceData))
				{
					SetupDiGetDeviceInterfaceDetail(deviceInfoSet, ref myDeviceInterfaceData, IntPtr.Zero, 0, ref bufferSize, IntPtr.Zero);
					IntPtr detailDataBuffer = Marshal.AllocHGlobal(bufferSize);
					Marshal.WriteInt32(detailDataBuffer, (IntPtr.Size == 4) ? (4 + Marshal.SystemDefaultCharSize) : 8);
					SetupDiGetDeviceInterfaceDetail(deviceInfoSet, ref myDeviceInterfaceData, detailDataBuffer, bufferSize, ref bufferSize, IntPtr.Zero);
					var pDevicePathName = new IntPtr(detailDataBuffer.ToInt64() + 4);
					devicePathName = Marshal.PtrToStringAuto(pDevicePathName);
				}
				else
				{
					lastDevice = true;
				}

			} while (lastDevice != true);

			return devicePathName;
		}

		#endregion setupapi.dll

		internal const int FILE_ATTRIBUTE_NORMAL = 0X80;
		internal const int FILE_FLAG_OVERLAPPED = 0X40000000;
		internal const int FILE_SHARE_READ = 1;
		internal const int FILE_SHARE_WRITE = 2;
		internal const uint GENERIC_READ = 0X80000000;
		internal const uint GENERIC_WRITE = 0X40000000;
		internal const int INVALID_HANDLE_VALUE = -1;
		internal const int OPEN_EXISTING = 3;

		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		internal static extern SafeFileHandle CreateFile(String lpFileName, uint dwDesiredAccess, int dwShareMode, IntPtr lpSecurityAttributes, int dwCreationDisposition, int dwFlagsAndAttributes, IntPtr hTemplateFile);
	}
}
