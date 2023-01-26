/// ***************************************************************************
/// Nitride Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2023 Xu Li - me@xuli.us
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
    public class ReceiverData
    {
        public ReceiverData(int numOfCh, int maxLength)
        {
            NumOfCh = numOfCh;

            SpectrumDatums = new FFT_Data[numOfCh];

            for (int i = 0; i < numOfCh; i++)
            {
                SpectrumDatums[i] = new FFT_Data(maxLength);
            }

            WaveForms = SpectrumDatums.Select(n => n.WaveForm).ToArray();
            SampleBuffer = new DataBuffer(numOfCh * maxLength * 8);
        }

        public bool IsBusy { get; set; } = false;

        private int NumOfCh { get; }

        public int Length { get; set; }

        public DataBuffer SampleBuffer { get; private set; }

        public FFT_Data[] SpectrumDatums { get; }

        public WaveForm[] WaveForms { get; }

        public void CopyData(DataBufferFormat format, int offset = 0)
        {
            int i, j;
            int pt = 0;
            int num = NumOfCh;
            int count = (Length * NumOfCh) + offset; // R16 = 1, C32 = 2, C32 Dual = 4

            DataBuffer source = SampleBuffer;

            switch (format)
            {
                default:
                case DataBufferFormat.S16:
                    for (i = offset; i < count; i += num)
                    {
                        for (j = 0; j < num; j++)
                        {
                            WaveForms[j].Data[pt] = new Complex(source.S16[i + j], 0);
                        }

                        pt++;
                    }
                    break;
                case DataBufferFormat.DS16:
                    for (i = offset; i < count; i += num)
                    {
                        for (j = 0; j < num; j++)
                        {
                            WaveForms[j].Data[pt] = new Complex(source.DS16[i + j].D1, source.DS16[i + j].D2);
                        }
                        pt++;
                    }
                    break;
                case DataBufferFormat.S32:
                    for (i = offset; i < count; i += num)
                    {
                        for (j = 0; j < num; j++)
                        {
                            WaveForms[j].Data[pt] = new Complex(source.S32[i + j], 0);
                        }
                        pt++;
                    }
                    break;
                case DataBufferFormat.DS32:
                    for (i = offset; i < count; i += num)
                    {
                        for (j = 0; j < num; j++)
                        {
                            double real = (double)source.DS32[i + j].D1 / 1D;
                            double imag = (double)source.DS32[i + j].D2 / 1D;
                            var c = WaveForms[j].Data[pt] = new Complex(real, imag);
                            //WaveForms[j].Data[pt] = new Complex(source.Sample_D32[i + j].D2, source.Sample_D32[i + j].D1);
                            // Console.WriteLine("c = " + c);
                        }
                        pt++;
                    }
                    break;
                case DataBufferFormat.S64:
                    for (i = offset; i < count; i += num)
                    {
                        for (j = 0; j < num; j++)
                        {
                            WaveForms[j].Data[pt] = new Complex(source.S64[i + j], 0);
                        }
                        pt++;
                    }
                    break;
                case DataBufferFormat.DS64:
                    for (i = offset; i < count; i += num)
                    {
                        for (j = 0; j < num; j++)
                        {
                            WaveForms[j].Data[pt] = new Complex(source.DS64[i + j].D1, source.DS64[i + j].D2);
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


    public class FFTDatum
    {
        public FFTDatum(int maxLength)
        {
            FFT = new(maxLength);
            WaveForm = new(maxLength);
            FreqTrace = new(maxLength);
        }

        public WaveForm WaveForm { get; }

        public FFT FFT { get; }

        public FreqTrace FreqTrace { get; }

        public bool FlipSpectrum { get; set; } = false;

        public void Transform()
        {
            FFT.Transform(WaveForm.Data);

            List<FreqPoint> fd = FreqTrace.Data;

            uint k, k1;
            uint N = FFT.Length;

            if (FlipSpectrum)
            {
                for (k = 0; k < N; k++)
                {
                    k1 = (k + (N / 2));

                    if (k1 >= N) k1 -= N;

                    fd[(int)k1].Value = FFT.Dsw[k.EndianInverse(FFT.M)];
                }
            }
            else
            {
                for (k = 0; k < N; k++)
                {
                    fd[(int)k].Value = FFT.Dsw[k.EndianInverse(FFT.M)];
                }
            }
        }
    }



    public class RxData
    {
        public RxData(int numOfCh, int maxLength)
        {
            NumOfCh = numOfCh;

            SpectrumDatums = new FFTDatum[numOfCh];

            for (int i = 0; i < numOfCh; i++)
            {
                SpectrumDatums[i] = new FFTDatum(maxLength);
            }

            WaveForms = SpectrumDatums.Select(n => n.WaveForm).ToArray();
            SampleBuffer = new DataBuffer(numOfCh * maxLength * 8);
        }

        public bool IsBusy { get; set; } = false;

        private int NumOfCh { get; }

        public int Length { get; set; }

        public DataBuffer SampleBuffer { get; private set; }

        public FFTDatum[] SpectrumDatums { get; }

        public WaveForm[] WaveForms { get; }

        public void CopyData(DataBufferFormat format, int offset = 0)
        {
            int i, j;
            int pt = 0;
            int num = NumOfCh;
            int count = (Length * NumOfCh) + offset; // R16 = 1, C32 = 2, C32 Dual = 4

            DataBuffer source = SampleBuffer;

            switch (format)
            {
                default:
                case DataBufferFormat.S16:
                    for (i = offset; i < count; i += num)
                    {
                        for (j = 0; j < num; j++)
                        {
                            WaveForms[j].Data[pt] = new Complex(source.S16[i + j], 0);
                        }

                        pt++;
                    }
                    break;
                case DataBufferFormat.DS16:
                    for (i = offset; i < count; i += num)
                    {
                        for (j = 0; j < num; j++)
                        {
                            WaveForms[j].Data[pt] = new Complex(source.DS16[i + j].D1, source.DS16[i + j].D2);
                        }
                        pt++;
                    }
                    break;
                case DataBufferFormat.S32:
                    for (i = offset; i < count; i += num)
                    {
                        for (j = 0; j < num; j++)
                        {
                            WaveForms[j].Data[pt] = new Complex(source.S32[i + j], 0);
                        }
                        pt++;
                    }
                    break;
                case DataBufferFormat.DS32:
                    for (i = offset; i < count; i += num)
                    {
                        for (j = 0; j < num; j++)
                        {
                            double real = (double)source.DS32[i + j].D1 / 1D;
                            double imag = (double)source.DS32[i + j].D2 / 1D;
                            var c = WaveForms[j].Data[pt] = new Complex(real, imag);
                            //WaveForms[j].Data[pt] = new Complex(source.Sample_D32[i + j].D2, source.Sample_D32[i + j].D1);
                            // Console.WriteLine("c = " + c);
                        }
                        pt++;
                    }
                    break;
                case DataBufferFormat.S64:
                    for (i = offset; i < count; i += num)
                    {
                        for (j = 0; j < num; j++)
                        {
                            WaveForms[j].Data[pt] = new Complex(source.S64[i + j], 0);
                        }
                        pt++;
                    }
                    break;
                case DataBufferFormat.DS64:
                    for (i = offset; i < count; i += num)
                    {
                        for (j = 0; j < num; j++)
                        {
                            WaveForms[j].Data[pt] = new Complex(source.DS64[i + j].D1, source.DS64[i + j].D2);
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
