using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using Nitride;

namespace Nitride.EE.FTDI
{
    /// <summary>
    /// Class wrapper for FTD2XX.DLL
    /// </summary>
    public partial class FTDI
    {
        public FT_DEVICE_INFO_NODE[] DeviceList { get; private set; }


        byte[] TxBuffer { get; } = new byte[1024];
        byte[] RxBuffer { get; } = new byte[1024];

        public FT_STATUS UpdateDeviceList()
        {
            uint devCount = 0;
            FT_STATUS status = GetNumberOfDevices(ref devCount);

            if (status == FT_STATUS.FT_OK)
            {
                DeviceList = new FT_DEVICE_INFO_NODE[devCount];
                status |= GetDeviceList(DeviceList);
            }
            return status;
        }

        public void SPI_Single_Write(int addr, ushort data)
        {
            // Set LE low
            TxBuffer[0] = 0x80; // Command
            TxBuffer[1] = 0x0; // Value
            TxBuffer[2] = 0xB; // mask

            // 3 Bytes SPI Write Read
            TxBuffer[3] = 0x11; // Write only /////// 0x31; // Bytes Read / Write
            TxBuffer[4] = 2; // Length L
            TxBuffer[5] = 0x0; // Length H
            TxBuffer[6] = Convert.ToByte(addr & 0x7F); // Addr and R/nW
            TxBuffer[7] = Convert.ToByte(data >> 8); // Data H
            TxBuffer[8] = Convert.ToByte(data & 0xFF); // Data L

            // Set LE high
            TxBuffer[9] = 0x80;
            TxBuffer[10] = 0x8;// 8;
            TxBuffer[11] = 0xB; // mask

            uint numBytes = 0;
            FT_STATUS status = Write(TxBuffer, 12, ref numBytes);

            if (status != FT_STATUS.FT_OK)
            {
                Console.WriteLine("Write Error! " + numBytes);
                return;
            }
            else
            {
                Console.WriteLine("Write Bytes Count: " + numBytes);// + " | Data = " + Convert.ToString(TxBuffer[1], 2));
            }
        }

        public ushort SPI_Single_Read(int addr)
        {
            // Set LE low
            TxBuffer[0] = 0x80;
            TxBuffer[1] = 0x0;
            TxBuffer[2] = 0xB; // mask

            // 3 Bytes SPI Write Read
            TxBuffer[3] = 0x31; // Write only /////// 0x31; // Bytes Read / Write
            TxBuffer[4] = 2; // Length L
            TxBuffer[5] = 0x0; // Length H
            TxBuffer[6] = Convert.ToByte((addr & 0x7F) + (1 << 7)); // Addr and R/nW
            TxBuffer[7] = 0x0; // Data H
            TxBuffer[8] = 0x0; // Data L

            // Set LE high
            TxBuffer[9] = 0x80;
            TxBuffer[10] = 0x8;// 8;
            TxBuffer[11] = 0xB; // mask

            uint numBytes = 0;
            FT_STATUS status = Write(TxBuffer, 12, ref numBytes);

            if (status != FT_STATUS.FT_OK)
            {
                Console.WriteLine("Write Error! " + numBytes);
                return 0x0;
            }
            else
            {
                Console.WriteLine("Write Bytes Count: " + numBytes);// + " | Data = " + Convert.ToString(TxBuffer[1], 2));
            }

            numBytes = 0;

            while (numBytes < 3)
            {
                Thread.Sleep(10);
                GetRxBytesAvailable(ref numBytes);
            }

            Read(RxBuffer, numBytes, ref numBytes);

            if (status != FT_STATUS.FT_OK)
            {
                Console.WriteLine("Read Error! " + numBytes);
                return 0x0;
            }
            else
            {
                Console.WriteLine("Read Bytes Count: " + numBytes + " | Data = " + Convert.ToString(RxBuffer[0], 2) + " | " + Convert.ToString(RxBuffer[1], 2));
            }

            return Convert.ToUInt16((RxBuffer[1] << 8) + RxBuffer[2]);
        }

    }
}
