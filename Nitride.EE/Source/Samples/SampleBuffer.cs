/// ***************************************************************************
/// Nitride Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// SampleBuffer
/// 
/// ***************************************************************************

using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Nitride.EE
{
    [StructLayout(LayoutKind.Explicit)]
    public class SampleBuffer : IDisposable
    {
        public SampleBuffer(int length)
        {
            Buffer = new byte[length];
        }

        public uint Length => Convert.ToUInt32(Buffer.Length);

        [FieldOffset(0)]
        public byte[] Buffer;

        [FieldOffset(0)]
        public short[] Sample_S16;

        [FieldOffset(0)]
        public Dual16BitDatum[] Sample_D16;

        [FieldOffset(0)]
        public int[] Sample_S32;

        [FieldOffset(0)]
        public uint[] Sample_U32;

        [FieldOffset(0)]
        public Dual32BitDatum[] Sample_D32;

        [FieldOffset(0)]
        public long[] Sample_S64;

        [FieldOffset(0)]
        public Dual64BitDatum[] Sample_D64;

        public void Dispose()
        {
            //Buffer = new byte[0];
        }
    }
}
