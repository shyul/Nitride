using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Nitride
{
    public class MatFile : IDisposable
    {
        public MatFile()
        {
            // Bytes 117 through 124 of the header contain an offset to subsystem-specific data in the MAT-file. All
            // zeros or all spaces in this field indicate that there is no subsystem - specific data stored in the file.
            Header[116] = 0x00;
            Header[117] = 0x00;
            Header[118] = 0x00;
            Header[119] = 0x00;
            Header[120] = 0x00;
            Header[121] = 0x00;
            Header[122] = 0x00;
            Header[123] = 0x00;

            // Version: When creating a MAT-file, set this field to 0x0100.
            Header[124] = 0x00;
            Header[125] = 0x01;

            // Endian Indicator: Contains the two characters, M and I, written to the MAT-file in this order, as a
            // 16 - bit value.If, when read from the MAT-file as a 16 - bit value, the characters
            // appear in reversed order(IM rather than MI), it indicates that the program
            // reading the MAT-file must perform byte-swapping to interpret the data in the
            // MAT - file correctly.
            Header[126] = 0x49;
            Header[127] = 0x4D;

            Description = "MATLAB 5.0 MAT-file, Platform: PCWIN64, Created on: Thu Nov 13 10:10:27 1997";

            Data = new() { 14, 0, 0, 0, 0, 0, 0, 0 };

            /*
            Data[0] = 14; // miMATRIX
            Data[1] = 0x0;
            Data[2] = 0x0;
            Data[3] = 0x0;

            Data[4] = 0x0; // Size of miMATRIX here
            Data[5] = 0x0;
            Data[6] = 0x0;
            Data[7] = 0x0;
            */



        }

        public void Dispose()
        {

        }

        ~MatFile() => Dispose();

        public string Description
        {
            get => Encoding.UTF8.GetString(Header.SubArray(0, 116));
            set
            {

                for (int i = 0; i < 116; i++)
                {
                    if (i < value.Length)
                    {
                        Header[i] = Convert.ToByte(value[i]);
                    }
                    else
                    {
                        Header[i] = 0x20;
                    }
                }
            }
        }

        public byte[] Header { get; } = new byte[128];

        public List<byte> Data { get; }

        private static void WriteFlags(BinaryWriter bw, byte clas, bool isComplex, bool isGlobal = false, bool isLogical = false)
        {
            bw.Write((uint)6);
            bw.Write((uint)8);

            byte[] flags = new byte[8];
            flags[2] = Convert.ToByte((isComplex ? (1 << 4) : 0) + (isGlobal ? (1 << 5) : 0) + (isLogical ? (1 << 6) : 0));
            flags[3] = clas;
            bw.Write(flags);
        }

        private static void WriteDimensionSize(BinaryWriter bw, int[] sizes)
        {
            bw.Write((uint)5);
            bw.Write((uint)(4 * sizes.Length));

            for (int i = 0; i < sizes.Length; i++)
            {
                bw.Write(sizes[i]);
            }

            for (int i = 0; i < sizes.Length % 2; i++)
            {
                bw.Write((int)0);
            }
        }

        private static void WriteArrayName(BinaryWriter bw, string name)
        {
            bw.Write((uint)1);

            bw.Write((uint)(8));

            for (int i = 0; i < 8; i++) 
            {
                bw.Write('F');
            }

            /*
            bw.Write((uint)(name.Length));

            for 

            bw.Write(name);

            for (int i = 0; i < name.Length % 8; i++) 
            {
                bw.Write((byte)0);
            }*/
        }
        /*
        public void AddDouble(double[] value)
        {
            using MemoryStream ms = new();
            using BinaryWriter bw = new(ms);
            bw.Write((uint)9);
            bw.Write((uint)(value.Length * 8));

            for (int i = 0; i < value.Length; i++)
            {
                bw.Write(value[i]);
            }

            Data.AddRange(ms.ToArray());
        }

        public void AddInt32(int[] value)
        {
            using MemoryStream ms = new();
            using BinaryWriter bw = new(ms);
            bw.Write((uint)5);
            bw.Write((uint)(value.Length * 4));

            for (int i = 0; i < value.Length; i++)
            {
                bw.Write(value[i]);
            }

            Data.AddRange(ms.ToArray());
        }
        */
        public void AddDouble(string name, double data)
        {
            using MemoryStream ms = new();
            using BinaryWriter bw = new(ms);

            WriteFlags(bw, 6, false);
            WriteDimensionSize(bw, new int[] { 1, 1 });
            WriteArrayName(bw, name);

            bw.Write((uint)9);
            bw.Write((uint)8);
            bw.Write(data);

            ms.Position = 0;
            Data.AddRange(ms.ToArray());
        }

        public void AddDouble(string name, double[] data)
        {
            using MemoryStream ms = new();
            using BinaryWriter bw = new(ms);

            WriteFlags(bw, 6, false);
            WriteDimensionSize(bw, new int[] { 1, data.Length });
            WriteArrayName(bw, name);

            bw.Write((uint)9);
            bw.Write((uint)8);

            for (int i = 0; i < data.Length; i++)
                bw.Write(data[i]);

            ms.Position = 0;
            Data.AddRange(ms.ToArray());
        }

        public void AddComplex(string name, Complex data)
        {
            using MemoryStream ms = new();
            using BinaryWriter bw = new(ms);
            WriteFlags(bw, 6, true);
            WriteDimensionSize(bw, new int[] { 1, 1 });
            WriteArrayName(bw, name);

            bw.Write((uint)9);
            bw.Write((uint)8);
            bw.Write(data.Real);

            bw.Write((uint)9);
            bw.Write((uint)8);
            bw.Write(data.Imaginary);

            ms.Position = 0;
            Data.AddRange(ms.ToArray());
        }

        public void Save(string fileName) 
        {
            
            uint length = Convert.ToUInt32(Data.Count - 8);
            Data[7] = Convert.ToByte((length >> 24) & 0xFF);
            Data[6] = Convert.ToByte((length >> 16) & 0xFF);
            Data[5] = Convert.ToByte((length >> 8) & 0xFF);
            Data[4] = Convert.ToByte((length >> 0) & 0xFF);
            
            using FileStream fs = File.Open(fileName, FileMode.Create);
            using BinaryWriter bw = new(fs, Encoding.UTF8, false);

            bw.Write(Header);
            bw.Write(Data.ToArray());
        }
    }
}
