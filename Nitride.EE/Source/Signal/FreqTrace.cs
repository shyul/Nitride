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
        public FreqTrace(int count, double startFreq, double stopFreq)
        {
            Count = count;
            for (int i=0; i<count; i++) 
            {
                Data.Add(new FreqPoint());
            }

            Configure(startFreq, stopFreq);
        }

        public void Configure(double startFreq, double stopFreq)
        {
            StartFreq = startFreq;
            StopFreq = stopFreq;
            FreqStep = (StopFreq - StartFreq) / (Count - 1D);

            for (int i = 0; i < Count; i++)
            {
                Data[i].Frequency = StartFreq + (i * FreqStep);
            }
        }

        public int Count { get; }

        public double StartFreq { get; set; }

        public double StopFreq { get; set; }

        public double FreqStep { get; set; }

        public List<FreqPoint> Data { get; } = new();
    }

    public class SpectrumDatum
    {
        public SpectrumDatum(int count, double startFreq, double stopFreq, double rate = 1, double startTime = 0)
        {
            WaveForm = new WaveForm(count, rate, startTime);
            FreqTrace = new FreqTrace(count, startFreq, stopFreq);
        }

        /*
        public double StartFreq { get; set; }

        public double StopFreq { get; set; }

        public double FreqStep { get; set; }
        */

        public void Configure(double startFreq, double stopFreq, double rate = 1, double startTime = 0)
        {
            WaveForm.SampleRate = rate;
            WaveForm.Start = startTime;
            FreqTrace.Configure(startFreq, stopFreq);
        }

        public WaveForm WaveForm { get; }

        public FreqTrace FreqTrace { get; }

    }

    public class RxData
    {
        public RxData(int numOfCh, int lengthPerCh, double startFreq, double stopFreq, double rate = 1, double startTime = 0) 
        {
            NumOfCh = numOfCh;
            SampleCount = lengthPerCh;

            SampleBuffer = new SampleBuffer(NumOfCh * SampleCount * 8);
            SpectrumDatums = new SpectrumDatum[numOfCh];

            for (int i = 0; i < numOfCh; i++)
            {
                SpectrumDatums[i] = new SpectrumDatum(lengthPerCh, startFreq, stopFreq, rate, startTime);
            }

            WaveForms = SpectrumDatums.Select(n => n.WaveForm).ToArray();
        }

        public bool IsBusy { get; set; } = false;

        private int NumOfCh { get; }

        private int SampleCount { get; }

        public SampleBuffer SampleBuffer { get; }

        public SpectrumDatum[] SpectrumDatums { get; }

        public WaveForm[] WaveForms { get; }

        public void CopyData(SampleFormat format, int offset = 0)
        {
            int i, j;
            int pt = 0;
            int num = NumOfCh;
            int count = (SampleCount * NumOfCh) + offset;

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
                            WaveForms[j].Data[pt] = new Complex(source.Sample_D32[i + j].D1, source.Sample_D32[i + j].D2);
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
