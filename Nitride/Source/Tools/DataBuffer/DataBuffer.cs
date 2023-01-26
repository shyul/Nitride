using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Nitride
{
    public enum DataBufferFormat
    {
        U8,
        S8,
        U16,
        S16,
        DU16,
        DS16,
        U32,
        S32,
        DU32,
        DS32,
        U64,
        S64,
        DU64,
        DS64
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DualShort
    {
        public short D1 { get; set; }

        public short D2 { get; set; }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DualUShort
    {
        public ushort D1 { get; set; }

        public ushort D2 { get; set; }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DualInt
    {
        public int D1 { get; set; }

        public int D2 { get; set; }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DualUInt
    {
        public uint D1 { get; set; }

        public uint D2 { get; set; }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DualLong
    {
        public long D1 { get; set; }

        public long D2 { get; set; }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DualULong
    {
        public ulong D1 { get; set; }

        public ulong D2 { get; set; }
    }

    [StructLayout(LayoutKind.Explicit)]
    public class DataBuffer : IDisposable
    {
        public DataBuffer(int length)
        {
            Bytes = new byte[length];
        }

        public DataBuffer(byte[] bytes)
        {
            Bytes = bytes;
        }

        public uint Length => Convert.ToUInt32(Bytes.Length);

        [FieldOffset(0)]
        public byte[] Bytes;

        [FieldOffset(0)]
        public short[] S16;

        [FieldOffset(0)]
        public ushort[] U16;

        [FieldOffset(0)]
        public DualShort[] DS16; //  public (short D1, short D2)[] DS16;

        [FieldOffset(0)]
        public DualUShort[] DU16;

        [FieldOffset(0)]
        public int[] S32;

        [FieldOffset(0)]
        public uint[] U32;

        [FieldOffset(0)]
        public DualInt[] DS32; // public (int D1, int D2)[] DS32;

        [FieldOffset(0)]
        public DualUInt[] DU32; // public (int D1, int D2)[] DS32;

        [FieldOffset(0)]
        public long[] S64;

        [FieldOffset(0)]
        public ulong[] U64;

        [FieldOffset(0)]
        public DualLong[] DS64; // public (long D1, long D2)[] DS64;

        [FieldOffset(0)]
        public DualULong[] DU64; // public (long D1, long D2)[] DS64;

        public void Dispose()
        {
            //Buffer = new byte[0];
        }
    }
}
