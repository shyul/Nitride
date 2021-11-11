using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Nitride.EE.WinUSB
{
	public class IsochronousInEndPoint : UsbEndPoint
	{
		public IsochronousInEndPoint(UsbInterface iface, WINUSB_PIPE_INFORMATION pipeInfo)
		{
			Interface = iface;
			PipeId = pipeInfo.PipeId;
			MaximumPacketSize = pipeInfo.MaximumPacketSize;
			Timeout = 2000;
			//IgnoreShortPackets = false;

			if(pipeInfo.Interval != 1) 
			{
				throw new Exception("Isochronous EndPoint Interval must be 1: " + pipeInfo.Interval);
			}
		}

		public virtual bool ReceiveData(uint numOfPackets, ref byte[] buffer)
		{
			uint bufferLength = (uint)buffer.Length;
			var isoPacketDescriptors = new USBD_ISO_PACKET_DESCRIPTOR[numOfPackets];
			var success = WinUsb_RegisterIsochBuffer(Device.Handle, PipeId, buffer, bufferLength, out IntPtr bufferHandle);
			if (success)
			{
				success = WinUsb_ReadIsochPipeAsap(bufferHandle, 0, bufferLength, false, numOfPackets, ref isoPacketDescriptors[0], IntPtr.Zero);
				Console.WriteLine(Marshal.GetLastWin32Error());

				if (success)
				{
					success = WinUsb_UnregisterIsochBuffer(bufferHandle);

					for (var i = 0; i <= numOfPackets - 1; i++)
					{
						Console.WriteLine("packet offset = " + isoPacketDescriptors[i].Offset);
						Console.WriteLine("packet length = " + isoPacketDescriptors[i].Length);
						Console.WriteLine("packet status = " + isoPacketDescriptors[i].Status);
						for (var j = 0; j < isoPacketDescriptors[i].Length; j++)
						{
							Console.WriteLine(buffer[j]);
						}
					}
				}
			}
			return success;
		}

		[DllImport("winusb.dll", SetLastError = true)]
		private static extern bool WinUsb_ReadIsochPipeAsap(IntPtr BufferHandle, uint Offset, uint Length, bool ContinueStream, uint NumberOfPackets, ref USBD_ISO_PACKET_DESCRIPTOR IsoPacketDescriptors, IntPtr Overlapped);
	}
}
