/// ***************************************************************************
/// Nitride Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// SampleBuffer
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Nitride.EE
{
    public class WaveForm 
    {
        public WaveForm(int count, double rate = 1, double start = 0)
        {
            Data = new Complex[count];
            SampleRate = rate;
            Start = start;
            IsBusy = false;
        }

        public bool IsBusy { get; set; }

        public double SampleRate { get; set; }

        public double Start { get; set; }

        public double Duration => ((Count - 1) / SampleRate);

        public double Stop => Start + Duration;

        public int Count => Data.Length;

        public Complex[] Data { get; }

        public double Peak => Data.Select(x => x.Magnitude).Max();

        public double Rms => Math.Sqrt(Data.Select(x => Math.Pow(x.Magnitude, 2)).Sum() / Count);

        public static void CopyData(SampleBuffer source, WaveForm[] dest, SampleFormat format, int offset = 0)
        {
            int i;
            int pt = 0;
            int num = dest.Length;
            int len = (dest.Select(n => n.Count).Min() * num) + offset;

            switch (format)
            {
                default:
                case SampleFormat.R16:
                    for (i = offset; i < len; i += num)
                    {
                        dest[i % num].Data[pt] = new Complex(source.Sample_S16[i], 0);
                        pt++;
                    }
                    break;
                case SampleFormat.C16:
                    for (i = offset; i < len; i += num)
                    {
                        dest[i % num].Data[pt] = new Complex(source.Sample_D16[i].D1, source.Sample_D16[i].D2);
                        pt++;
                    }
                    break;
                case SampleFormat.R32:
                    for (i = offset; i < len; i += num)
                    {
                        dest[i % num].Data[pt] = new Complex(source.Sample_S32[i], 0);
                        pt++;
                    }
                    break;
                case SampleFormat.C32:
                    for (i = offset; i < len; i += num)
                    {
                        dest[i % num].Data[pt] = new Complex(source.Sample_D32[i].D1, source.Sample_D32[i].D2);
                        pt++;
                    }
                    break;
                case SampleFormat.R64:
                    for (i = offset; i < len; i += num)
                    {
                        dest[i % num].Data[pt] = new Complex(source.Sample_S64[i], 0);
                        pt++;
                    }
                    break;
                case SampleFormat.C64:
                    for (i = offset; i < len; i += num)
                    {
                        dest[i % num].Data[pt] = new Complex(source.Sample_D64[i].D1, source.Sample_D64[i].D2);
                        pt++;
                    }
                    break;
            }
        }
    }
}
