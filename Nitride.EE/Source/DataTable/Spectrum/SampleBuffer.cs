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
    [StructLayout(LayoutKind.Sequential)]
    public struct Dual16BitDatum
    {
        public short D1 { get; set; } //= 0;

        public short D2 { get; set; } //= 0;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Dual32BitDatum
    {
        public int D1 { get; set; } //= 0;

        public int D2 { get; set; } //= 0;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Dual64BitDatum
    {
        public long D1 { get; set; } //= 0;

        public long D2 { get; set; } //= 0;
    }

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

    public enum SampleFormat 
    {
        R16,
        C16,
        R32,
        C32,
        R64,
        C64
    }

    public class FftBuffer
    {
        public FftBuffer(int length, SampleFormat format, double rate, int nyquist = 0)
        {
            Format = format;
            Length = length;
            switch (Format)
            {
                case SampleFormat.R16:
                    SampleBuffer = new(Length * 2);
                    break;
                case SampleFormat.C16:
                case SampleFormat.R32:
                    SampleBuffer = new(Length * 4);
                    break;
                case SampleFormat.C32:
                case SampleFormat.R64:
                    SampleBuffer = new(Length * 8);
                    break;
                case SampleFormat.C64:
                default:
                    SampleBuffer = new(Length * 16);
                    break;
            }

            Td = new Complex[Length];
            Fd = new Complex[Length];
            DbMag = new (double Freq, double Value)[Length];

            Configure(format, length, rate, nyquist);
        }

        public FftBuffer(SampleBuffer buffer, int length, SampleFormat format, double rate, int nyquist = 0)
        {
            Format = format;
            Length = length;
            SampleBuffer = buffer;

            Td = new Complex[Length];
            Fd = new Complex[Length];
            DbMag = new (double Freq, double Value)[Length];

            Configure(format, length, rate, nyquist);
        }

        public void Configure(SampleFormat format, int length, double rate, int nyquist = 0)
        {
            IsBusy = true;
            Format = format;
            StartFreq = rate * (nyquist / 2);
            StopFreq = StartFreq + rate;
            FreqStep = rate / (Length - 1D);

            for (int i = 0; i < Length; i++)
            {
                DbMag[i].Freq = StartFreq + (i * FreqStep);
            }

            IsBusy = false;
        }

        public double StartFreq { get; protected set; } = double.MaxValue; // => Count > 0 ? Rows.First().X : double.NaN;

        public double StopFreq { get; protected set; } = double.MinValue; // => Count > 0 ? Rows.Last().X : double.NaN;

        public double FreqStep { get; protected set; } = double.MaxValue;

        public void CopyData(Complex[] samples) 
        {
            for (int i = 0; i < samples.Length; i++) 
            {
                Td[i] = samples[i];
            }
        }

        public void CopyData()
        {
            int i;
            switch (Format)
            {
                default:
                case SampleFormat.R16:
                    for (i = 0; i < Length; i++)
                    {
                        Td[i] = new Complex(SampleBuffer.Sample_S16[i], 0);
                    }
                    break;
                case SampleFormat.C16:
                    for (i = 0; i < Length; i++)
                    {
                        Td[i] = new Complex(SampleBuffer.Sample_D16[i].D1, SampleBuffer.Sample_D16[i].D2);
                    }
                    break;
                case SampleFormat.R32:
                    for (i = 0; i < Length; i++)
                    {
                        Td[i] = new Complex(SampleBuffer.Sample_S32[i], 0);
                    }
                    break;
                case SampleFormat.C32:
                    for (i = 0; i < Length; i++)
                    {
                        Td[i] = new Complex(SampleBuffer.Sample_D32[i].D1, SampleBuffer.Sample_D32[i].D2);
                    }
                    break;
                case SampleFormat.R64:
                    for (i = 0; i < Length; i++)
                    {
                        Td[i] = new Complex(SampleBuffer.Sample_S64[i], 0);
                    }
                    break;
                case SampleFormat.C64:
                    for (i = 0; i < Length; i++)
                    {
                        Td[i] = new Complex(SampleBuffer.Sample_D64[i].D1, SampleBuffer.Sample_D64[i].D2);
                    }
                    break;
            }
        }


        public SampleBuffer SampleBuffer { get; }

        public int Length { get; private set; }

        public SampleFormat Format { get; set; }

        public Complex[] Td { get; }

        public Complex[] Fd { get; }

        public (double Freq, double Value)[] DbMag { get; }

        public bool IsBusy { get; set; }
    }
}
