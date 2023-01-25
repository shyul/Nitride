using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Nitride.EE
{
    /*
    [StructLayout(LayoutKind.Explicit)]
    public class Stack : IDisposable
    {
        public Stack(int length)
        {
            Data = new byte[length];
        }

        public uint Length => Convert.ToUInt32(Data.Length);

        [FieldOffset(0)]
        public byte[] Data;

        [FieldOffset(0)]
        public short[] S16;

        [FieldOffset(0)]
        public ushort[] U16;

        [FieldOffset(0)]
        public (short D1, short D2)[] DS16;

        [FieldOffset(0)]
        public int[] S32;

        [FieldOffset(0)]
        public uint[] U32;

        [FieldOffset(0)]
        public (int D1, int D2)[] Sample_D32;

        [FieldOffset(0)]
        public long[] S64;

        [FieldOffset(0)]
        public (long D1, long D2)[] Sample_D64;

        public void Dispose()
        {
            //Buffer = new byte[0];
        }
    }*/

    [StructLayout(LayoutKind.Explicit)]
    public class Stack : IDisposable
    {
        public Stack(int length)
        {
            Data = new byte[length];
        }

        public uint Length => Convert.ToUInt32(Data.Length);

        [FieldOffset(0)]
        public byte[] Data;

        [FieldOffset(0)]
        public short[] S16;

        [FieldOffset(0)]
        public Dual16BitDatum[] DS16;

        [FieldOffset(0)]
        public int[] S32;

        [FieldOffset(0)]
        public uint[] U32;

        [FieldOffset(0)]
        public Dual32BitDatum[] DS32;

        [FieldOffset(0)]
        public long[] S64;

        [FieldOffset(0)]
        public Dual64BitDatum[] DS64;

        public void Dispose()
        {
            //Buffer = new byte[0];
        }
    }


}
