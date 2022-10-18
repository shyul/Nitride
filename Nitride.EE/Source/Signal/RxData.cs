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
    public class SpectrumDatum
    {
        public SpectrumDatum(int maxLength)
        {
            FFT = new(maxLength);
            WaveForm = new(maxLength);
            FreqTrace = new(maxLength);
        }

        public WaveForm WaveForm { get; } 

        public FFT FFT { get; }

        public FreqTrace FreqTrace { get; }

        public void Transform() => FFT.Transform(WaveForm, FreqTrace.Data);
    }

    public class RxData
    {
        public RxData(int numOfCh, int maxLength)
        {
            NumOfCh = numOfCh;

            SpectrumDatums = new SpectrumDatum[numOfCh];

            for (int i = 0; i < numOfCh; i++)
            {
                SpectrumDatums[i] = new SpectrumDatum(maxLength);
            }

            WaveForms = SpectrumDatums.Select(n => n.WaveForm).ToArray();
            SampleBuffer = new SampleBuffer(numOfCh * maxLength * 8);
        }

        public bool IsBusy { get; set; } = false;

        private int NumOfCh { get; }

        public int Length { get; set; }

        public SampleBuffer SampleBuffer { get; private set; }

        public SpectrumDatum[] SpectrumDatums { get; }

        public WaveForm[] WaveForms { get; }

        public void CopyData(SampleFormat format, int offset = 0)
        {
            int i, j;
            int pt = 0;
            int num = NumOfCh;
            int count = (Length * NumOfCh) + offset; // R16 = 1, C32 = 2, C32 Dual = 4

            SampleBuffer source = SampleBuffer;

            switch (format)
            {
                default:
                case SampleFormat.R16:
                    for (i = offset; i < count; i += num)
                    {
                        for (j = 0; j < num; j++)
                        {
                            WaveForms[j].Data[pt] = new Complex(source.Sample_S16[i + j], 0);
                        }

                        pt++;
                    }
                    break;
                case SampleFormat.C16:
                    for (i = offset; i < count; i += num)
                    {
                        for (j = 0; j < num; j++)
                        {
                            WaveForms[j].Data[pt] = new Complex(source.Sample_D16[i + j].D1, source.Sample_D16[i + j].D2);
                        }
                        pt++;
                    }
                    break;
                case SampleFormat.R32:
                    for (i = offset; i < count; i += num)
                    {
                        for (j = 0; j < num; j++)
                        {
                            WaveForms[j].Data[pt] = new Complex(source.Sample_S32[i + j], 0);
                        }
                        pt++;
                    }
                    break;
                case SampleFormat.C32:
                    for (i = offset; i < count; i += num)
                    {
                        for (j = 0; j < num; j++)
                        {
                            double real = (double)source.Sample_D32[i + j].D1 / 1D;
                            double imag = (double)source.Sample_D32[i + j].D2 / 1D;
                            var c = WaveForms[j].Data[pt] = new Complex(real, imag);
                            //WaveForms[j].Data[pt] = new Complex(source.Sample_D32[i + j].D2, source.Sample_D32[i + j].D1);
                            // Console.WriteLine("c = " + c);
                        }
                        pt++;
                    }
                    break;
                case SampleFormat.R64:
                    for (i = offset; i < count; i += num)
                    {
                        for (j = 0; j < num; j++)
                        {
                            WaveForms[j].Data[pt] = new Complex(source.Sample_S64[i + j], 0);
                        }
                        pt++;
                    }
                    break;
                case SampleFormat.C64:
                    for (i = offset; i < count; i += num)
                    {
                        for (j = 0; j < num; j++)
                        {
                            WaveForms[j].Data[pt] = new Complex(source.Sample_D64[i + j].D1, source.Sample_D64[i + j].D2);
                        }
                        pt++;
                    }
                    break;
            }

            //Console.WriteLine("count = " + count + " | num = " + num + " | pt = " + pt);
            /*
            var test = WaveForms[0].Data.Take(32);

            foreach (var c in test)
            {
                int real = (int)c.Real;
                int image = (int)c.Imaginary;

                Console.Write("(0x" + real.ToString("X8") + " | 0x" + image.ToString("X8") + ") ");
            }
            Console.WriteLine();*/
        }
    }
}
