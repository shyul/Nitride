using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using Nitride;
using System.Linq;

namespace Nitride.EE.FTDI
{
    /// <summary>
    /// Class wrapper for FTD2XX.DLL
    /// </summary>
    public partial class FTDI
    {
        public FT_DEVICE_INFO_NODE[] DeviceList { get; private set; }

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

        byte[] TxBuffer { get; } = new byte[65536];

        byte[] RxBuffer { get; } = new byte[65536];

        private uint Write_Buffer(int pt)
        {
            Console.WriteLine("SPI Total Bytes = " + pt);

            uint numBytes = 0;
            FT_STATUS status = Write(TxBuffer, pt, ref numBytes);

            if (status != FT_STATUS.FT_OK)
            {
                Console.WriteLine("Write Error! " + numBytes);
                return 0;
            }
            else
            {
                Console.WriteLine("Write Bytes Count: " + numBytes);// + " | Data = " + Convert.ToString(TxBuffer[1], 2));
            }

            return numBytes;
        }

        private uint Read_Buffer(int count)
        {
            uint numBytes = 0;

            while (numBytes < count)
            {
                Thread.Sleep(10);
                GetRxBytesAvailable(ref numBytes);
            }

            FT_STATUS status = Read(RxBuffer, numBytes, ref numBytes);

            if (status != FT_STATUS.FT_OK)
            {
                Console.WriteLine("Read Error! " + numBytes);
                return 0;
            }
            else
            {
                Console.WriteLine("Read Bytes Count: " + numBytes + " | Data = " + Convert.ToString(RxBuffer[0], 2) + " | " + Convert.ToString(RxBuffer[1], 2));
            }

            return numBytes;
        }

        public byte DBUS { get; set; } = 0;

        public byte DBUS_DIR { get; set; } = 0;

        public int DBUS_WR(int pt)
        {
            TxBuffer[pt] = 0x80;
            TxBuffer[pt + 1] = DBUS;
            TxBuffer[pt + 2] = DBUS_DIR;
            return pt + 3;
        }

        public int DBUS_RD(int pt)
        {
            TxBuffer[pt] = 0x81;
            return pt + 1;
        }

        public byte CBUS { get; set; } = 0;

        public byte CBUS_DIR { get; set; } = 0; // 1 = "OUTPUT"

        public int CBUS_WR(int pt)
        {
            TxBuffer[pt] = 0x82;
            TxBuffer[pt + 1] = CBUS;
            TxBuffer[pt + 2] = CBUS_DIR;
            return pt + 3;
        }

        public int CBUS_RD(int pt)
        {
            TxBuffer[pt] = 0x83;
            return pt + 1;
        }

        public void Write_GPIO() 
        {
            int pt = 0;
            pt = DBUS_WR(pt);
            pt = CBUS_WR(pt);
            Write_Buffer(pt);
        }


        private int SET_LE_LOW(int pt)
        {
            DBUS &= (~(1 << 3)) & 0xFF;
            DBUS_DIR |= (1 << 3) & 0xFF;
            return DBUS_WR(pt);

            /*
            TxBuffer[pt] = 0x80;
            TxBuffer[pt + 1] = 0x0;
            TxBuffer[pt + 2] = 0xB; // mask
            return pt + 3;*/
        }

        private int SET_LE_HIGH(int pt)
        {
            
            DBUS |= (1 << 3) & 0xFF;
            DBUS_DIR |= (1 << 3) & 0xFF;
            return DBUS_WR(pt);

            /*
            TxBuffer[pt] = 0x80;
            TxBuffer[pt + 1] = 0x8; // 8;
            TxBuffer[pt + 2] = 0xB; // mask
            return pt + 3;*/
        }

        private int SPI_WRITE_SINGLE(int pt, byte addr, ushort data) // data.Length
        {
            // 3 Bytes SPI Write Read
            TxBuffer[pt] = 0x11; // Data Out Only, Bytes, OUT Clock -VE
            TxBuffer[pt + 1] = 3 - 1; // Length L
            TxBuffer[pt + 2] = 0; // Length H
            TxBuffer[pt + 3] = Convert.ToByte(addr & 0x7F); // Addr and R/nW
            TxBuffer[pt + 4] = Convert.ToByte(data >> 8); // Data H
            TxBuffer[pt + 5] = Convert.ToByte(data & 0xFF); // Data L

            return pt + 6;
        }

        public void SPI_WRITE_REG(ushort[] addr, Reg16 reg) // data.Length
        {
            int pt = 0;

            foreach (var a in addr)
            {
                pt = SET_LE_HIGH(pt);
                pt = SET_LE_LOW(pt);
                pt = SPI_WRITE_SINGLE(pt, Convert.ToByte(a & 0xFF), reg[a]);
                pt = SET_LE_HIGH(pt);
                pt = SET_LE_HIGH(pt);
            }

            Write_Buffer(pt);
        }

        public void SPI_WRITE_ALL(Reg16 reg)
        {
            int pt = 0;
            pt = SET_LE_HIGH(pt);
            pt = SET_LE_LOW(pt);

            ushort length_1 = Convert.ToUInt16((reg.Count * 2) & 0xFFFF); // Length - 1 (+1) initial addr byte
            byte inital_addr = Convert.ToByte(((reg.OrderByDescending(n => n.Key).First().Key) & 0x7F) + (0 << 7));

            TxBuffer[pt] = 0x11; // Data Out Only, Bytes, OUT Clock -VE
            TxBuffer[pt + 1] = Convert.ToByte(length_1 & 0xFF); // Length L
            TxBuffer[pt + 2] = Convert.ToByte((length_1 >> 8) & 0xFF); // Length H
            TxBuffer[pt + 3] = inital_addr;

            pt += 4;

            foreach (var pair in reg.OrderByDescending(n => n.Key))
            {
                ushort data = pair.Value;
                TxBuffer[pt] = Convert.ToByte(data >> 8); // Data H
                TxBuffer[pt + 1] = Convert.ToByte(data & 0xFF); // Data L
                pt += 2;
            }

            pt = SET_LE_HIGH(pt);

            Write_Buffer(pt);
        }

        private int SPI_READ_SINGLE(int pt, byte addr) // data.Length
        {
            // 3 Bytes SPI Write Read
            TxBuffer[pt] = 0x31;
            TxBuffer[pt + 1] = 3 - 1; // Length L
            TxBuffer[pt + 2] = 0; // Length H
            TxBuffer[pt + 3] = Convert.ToByte((addr & 0x7F) + (1 << 7)); // Addr and R/nW
            TxBuffer[pt + 4] = 0x0;
            TxBuffer[pt + 5] = 0x0;

            return pt + 6;
        }

        public void SPI_READ_REG(ushort[] addr, Reg16 reg) // data.Length
        {
            int pt = 0;

            foreach (var a in addr)
            {
                pt = SET_LE_HIGH(pt);
                pt = SET_LE_LOW(pt);
                pt = SPI_READ_SINGLE(pt, Convert.ToByte(a & 0xFF));
                pt = SET_LE_HIGH(pt);
                pt = SET_LE_HIGH(pt);
            }

            Write_Buffer(pt);
            Read_Buffer(addr.Length * 3);

            pt = 0;

            foreach (var a in addr)
            {
                reg[a] = Convert.ToUInt16((RxBuffer[pt + 1] << 8) + RxBuffer[pt + 2]);
                pt += 3;
            }
        }

        public void SPI_READ_ALL(Reg16 reg)
        {
            int pt = 0;
            pt = SET_LE_HIGH(pt);
            pt = SET_LE_LOW(pt);

            ushort length_1 = Convert.ToUInt16((reg.Count * 2) & 0xFFFF); // Length - 1 (+1) initial addr byte
            byte inital_addr = Convert.ToByte(((reg.OrderByDescending(n => n.Key).First().Key) & 0x7F) + (1 << 7));

            TxBuffer[pt] = 0x31;
            TxBuffer[pt + 1] = Convert.ToByte(length_1 & 0xFF); // Length L
            TxBuffer[pt + 2] = Convert.ToByte((length_1 >> 8) & 0xFF); // Length H
            TxBuffer[pt + 3] = inital_addr;

            pt += 4;

            foreach (var pair in reg.OrderByDescending(n => n.Key))
            {
                // ushort data = pair.Value;
                TxBuffer[pt] = 0x0; // Data H
                TxBuffer[pt + 1] = 0x0; // Data L
                pt += 2;
            }

            pt = SET_LE_HIGH(pt);

            Write_Buffer(pt);
            Read_Buffer(length_1 + 1);

            pt = 1;
            foreach (var pair in reg.OrderByDescending(n => n.Key))
            {
                reg[pair.Key] = Convert.ToUInt16((RxBuffer[pt] << 8) + RxBuffer[pt + 1]);
                pt += 2;
            }
        }


        /*
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
        */
    }
}
