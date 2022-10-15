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
    public class FreqPoint
    {
        public double Frequency { get; set; }

        public Complex Value { get; set; }

        public double Magnitude => Value.Magnitude;
    }

    public class FreqTrace
    {
        public void Configure(int count, double startFreq, double stopFreq)
        {
            Count = count;

            while (Count > Data.Count) 
            {
                Data.Add(new FreqPoint());
            }
            /*
            while (Count < Data.Count)
            {
                Data.RemoveAt(Data.Count - 1);
            }*/

            StartFreq = startFreq;
            StopFreq = stopFreq;
            FreqStep = (StopFreq - StartFreq) / (Count - 1D);

            for (int i = 0; i < Data.Count; i++)
            {
                Data[i].Frequency = StartFreq + (i * FreqStep);
            }
        }

        public int Count { get; private set; }

        public double StartFreq { get; private set; }

        public double StopFreq { get; private set; }

        public double FreqStep { get; private set; }

        public List<FreqPoint> Data { get; } = new();
    }

    public class SpectrumDatum
    {
        public void Configure(int count, double startFreq, double stopFreq, double rate = 1, double startTime = 0)
        {

            WaveForm.Configure(count, rate, startTime);
            FreqTrace.Configure(count, startFreq, stopFreq);
        }

        public WaveForm WaveForm { get; } = new();

        public FreqTrace FreqTrace { get; } = new();

    }

    public class RxData
    {
        public RxData(int numOfCh) 
        {
            NumOfCh = numOfCh;

            SpectrumDatums = new SpectrumDatum[numOfCh];

            for (int i = 0; i < numOfCh; i++)
            {
                SpectrumDatums[i] = new SpectrumDatum();
            }

            WaveForms = SpectrumDatums.Select(n => n.WaveForm).ToArray();
        }

        public void Configure(int lengthPerCh, double startFreq, double stopFreq, double rate = 1, double startTime = 0)
        {
            foreach (var d in SpectrumDatums)
            {
                d.Configure(lengthPerCh, startFreq, stopFreq, rate, startTime);
            }

            SampleCount = lengthPerCh;

            if (SampleBuffer is null || SampleBuffer.Length < NumOfCh * SampleCount * 2)
            {
                SampleBuffer = new SampleBuffer(NumOfCh * SampleCount * 8);
            }
        }

        public bool IsBusy { get; set; } = false;

        private int NumOfCh { get; }

        private int SampleCount { get; set; }

        public SampleBuffer SampleBuffer { get; private set; }

        public SpectrumDatum[] SpectrumDatums { get; }

        public WaveForm[] WaveForms { get; }

        public void CopyData(SampleFormat format, int offset = 0)
        {
            int i, j;
            int pt = 0;
            int num = NumOfCh;
            int count = (SampleCount * NumOfCh) + offset; // R16 = 1, C32 = 2, C32 Dual = 4

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
                            // WaveForms[j].Data[pt] = new Complex(source.Sample_D32[i + j].D2, source.Sample_D32[i + j].D1);
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
        }
    }


    public enum SweepMode
    {
        FFT, // FFT Frame Length
        DDC, // DDC FFT Detector Length x Points (A.k.a Multi FFT frame merged)
        Analog, // Points (dewelling time), FFT Detector Length
    }
}
