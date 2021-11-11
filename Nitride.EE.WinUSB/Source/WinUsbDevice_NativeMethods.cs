using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Nitride.EE.WinUSB
{
	partial class WinUsbDevice
	{
		#region winusb.dll

		private const uint DEVICE_SPEED = 1;
		private const byte USB_ENDPOINT_DIRECTION_MASK = 0X80;

		private const int USB_DEVICE_DESCRIPTOR_TYPE = 0x01;
		private const int USB_CONFIGURATION_DESCRIPTOR_TYPE = 0x02;
		private const int USB_STRING_DESCRIPTOR_TYPE = 0x03;

		private const int ERROR_NO_MORE_ITEMS = 259;

		[DllImport("winusb.dll", SetLastError = true)]
		private static extern bool WinUsb_ControlTransfer(IntPtr InterfaceHandle, WINUSB_SETUP_PACKET SetupPacket, Byte[] Buffer, UInt32 BufferLength, ref UInt32 LengthTransferred, IntPtr Overlapped);

		[DllImport("winusb.dll", SetLastError = true)]
		private static unsafe extern bool WinUsb_ControlTransfer(IntPtr InterfaceHandle, WINUSB_SETUP_PACKET SetupPacket, Byte[] Buffer, UInt32 BufferLength, ref UInt32 LengthTransferred, NativeOverlapped* pOverlapped);

		[DllImport("winusb.dll", SetLastError = true)]
		private static extern bool WinUsb_Free(IntPtr InterfaceHandle);

		[DllImport("winusb.dll", SetLastError = true)]
		private static extern bool WinUsb_Initialize(SafeFileHandle DeviceHandle, ref IntPtr InterfaceHandle);

		[DllImport("winusb.dll", SetLastError = true)]
		private static extern bool WinUsb_QueryDeviceInformation(IntPtr InterfaceHandle, UInt32 InformationType, ref UInt32 BufferLength, out byte Buffer);

		[DllImport("winusb.dll", SetLastError = true)]
		private static extern bool WinUsb_QueryInterfaceSettings(IntPtr InterfaceHandle, Byte AlternateInterfaceNumber, out USB_INTERFACE_DESCRIPTOR UsbAltInterfaceDescriptor);

		[DllImport("winusb.dll", SetLastError = true)]
		private static extern bool WinUsb_QueryPipe(IntPtr InterfaceHandle, Byte AlternateInterfaceNumber, Byte PipeIndex, out WINUSB_PIPE_INFORMATION PipeInformation);

		[DllImport("kernel32.dll", SetLastError = true)]
		private static unsafe extern bool CancelIo(IntPtr hFile);

		[DllImport("kernel32.dll", SetLastError = true)]
		private static unsafe extern bool CancelIoEx(IntPtr hFile, NativeOverlapped* pOverlapped);

		[DllImport("winusb.dll", SetLastError = true)]
		private static extern bool WinUsb_GetDescriptor(IntPtr InterfaceHandle, byte DescriptorType,
						byte Index, UInt16 LanguageID, byte[] Buffer, UInt32 BufferLength, out UInt32 LengthTransfered);

		[DllImport("winusb.dll", SetLastError = true)]
		private static extern bool WinUsb_GetDescriptor(IntPtr InterfaceHandle, byte DescriptorType,
						byte Index, UInt16 LanguageID, out USB_DEVICE_DESCRIPTOR deviceDesc, UInt32 BufferLength, out UInt32 LengthTransfered);

		[DllImport("winusb.dll", SetLastError = true)]
		private static extern bool WinUsb_GetDescriptor(IntPtr InterfaceHandle, byte DescriptorType,
						byte Index, UInt16 LanguageID, out USB_CONFIGURATION_DESCRIPTOR deviceDesc, UInt32 BufferLength, out UInt32 LengthTransfered);

		[DllImport("winusb.dll", SetLastError = true)]
		private static extern bool WinUsb_GetAssociatedInterface(IntPtr InterfaceHandle, byte AssociatedInterfaceIndex, out IntPtr AssociatedInterfaceHandle);

		[DllImport("winusb.dll", SetLastError = true)]
		private static extern bool WinUsb_SetCurrentAlternateSetting(IntPtr InterfaceHandle, byte AlternateSetting);

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
		private static extern bool WinUsb_SetPipePolicyUint(IntPtr InterfaceHandle, byte PipeID, uint PolicyType, uint ValueLength, ref uint Value);





		[DllImport("winusb.dll", SetLastError = true)]
		private static extern bool WinUsb_WritePipe(IntPtr InterfaceHandle, byte PipeID, byte[] Buffer, uint BufferLength, ref uint LengthTransferred, IntPtr Overlapped);

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
		private static SafeFileHandle GetDeviceFileHandle(string devicePathName) => CreateFile
					(devicePathName,
					 (GENERIC_WRITE | GENERIC_READ),
					 FILE_SHARE_READ | FILE_SHARE_WRITE,
					 IntPtr.Zero,
					 OPEN_EXISTING,
					 FILE_ATTRIBUTE_NORMAL | FILE_FLAG_OVERLAPPED,
					 IntPtr.Zero);



		#endregion winusb.dll

		#region setupapi.dll

		private const int DIGCF_PRESENT = 2;
		private const int DIGCF_DEVICEINTERFACE = 0X10;

		[DllImport("setupapi.dll", SetLastError = true)]
		private static extern IntPtr SetupDiCreateDeviceInfoList(ref Guid ClassGuid, IntPtr hwndParent);

		[DllImport("setupapi.dll", SetLastError = true)]
		private static extern int SetupDiDestroyDeviceInfoList(IntPtr DeviceInfoSet);

		[DllImport("setupapi.dll", SetLastError = true)]
		private static extern bool SetupDiEnumDeviceInterfaces(IntPtr DeviceInfoSet, IntPtr DeviceInfoData, ref Guid InterfaceClassGuid, int MemberIndex, ref SP_DEVICE_INTERFACE_DATA DeviceInterfaceData);

		[DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
		private static extern IntPtr SetupDiGetClassDevs(ref Guid ClassGuid, IntPtr Enumerator, IntPtr hwndParent, int Flags);

		[DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
		private static extern bool SetupDiGetDeviceInterfaceDetail(IntPtr DeviceInfoSet, ref SP_DEVICE_INTERFACE_DATA DeviceInterfaceData, IntPtr DeviceInterfaceDetailData, int DeviceInterfaceDetailDataSize, ref int RequiredSize, IntPtr DeviceInfoData);

		public static string[] FindDevicePathList(Guid guid)
		{
			//Console.WriteLine("Start 0...");
			int bufferSize = 0;
			var myDeviceInterfaceData = new SP_DEVICE_INTERFACE_DATA();
			myDeviceInterfaceData.cbSize = Marshal.SizeOf(myDeviceInterfaceData);
			//Console.WriteLine("Start...");

			var deviceInfoSet = SetupDiGetClassDevs(ref guid, IntPtr.Zero, IntPtr.Zero, DIGCF_PRESENT | DIGCF_DEVICEINTERFACE);
			int memberIndex = 0;
			List<string> deviceList = new();

			Console.WriteLine("Start searching for devices...");
			int i = 0;
			while (SetupDiEnumDeviceInterfaces(deviceInfoSet, IntPtr.Zero, ref guid, memberIndex, ref myDeviceInterfaceData) && i < 5)
			{
				SetupDiGetDeviceInterfaceDetail(deviceInfoSet, ref myDeviceInterfaceData, IntPtr.Zero, 0, ref bufferSize, IntPtr.Zero);
				IntPtr detailDataBuffer = Marshal.AllocHGlobal(bufferSize);
				Marshal.WriteInt32(detailDataBuffer, (IntPtr.Size == 4) ? (4 + Marshal.SystemDefaultCharSize) : 8);
				SetupDiGetDeviceInterfaceDetail(deviceInfoSet, ref myDeviceInterfaceData, detailDataBuffer, bufferSize, ref bufferSize, IntPtr.Zero);
				var pDevicePathName = new IntPtr(detailDataBuffer.ToInt64() + 4);
				Console.WriteLine("Found..." + pDevicePathName + " / " + Marshal.PtrToStringAuto(pDevicePathName));
				deviceList.Add(Marshal.PtrToStringAuto(pDevicePathName));
				i++;

			}

			return deviceList.ToArray();
		}

		#endregion setupapi.dll

		private const int FILE_ATTRIBUTE_NORMAL = 0X80;
		private const int FILE_FLAG_OVERLAPPED = 0X40000000;
		private const int FILE_SHARE_READ = 1;
		private const int FILE_SHARE_WRITE = 2;
		private const uint GENERIC_READ = 0X80000000;
		private const uint GENERIC_WRITE = 0X40000000;
		private const int INVALID_HANDLE_VALUE = -1;
		private const int OPEN_EXISTING = 3;

		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		private static extern SafeFileHandle CreateFile(String lpFileName, uint dwDesiredAccess, int dwShareMode, IntPtr lpSecurityAttributes, int dwCreationDisposition, int dwFlagsAndAttributes, IntPtr hTemplateFile);
	}
}
