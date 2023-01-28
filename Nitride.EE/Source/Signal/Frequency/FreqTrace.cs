/// ***************************************************************************
/// Nitride Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2023 Xu Li - me@xuli.us
/// 
/// Frequency
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Nitride.EE
{
    public class FreqTrace
    {
        public FreqTrace(int maxLength)
        {
            FFT = new(maxLength);
            Length = maxLength;
            Data = new List<FreqPoint>();

            for (int i = 0; i < maxLength; i++)
            {
                Data.Add(new FreqPoint());
            }
        }

        public void Configure(int length, double startFreq, double stopFreq, WindowsType winType)
        {
            Length = length;
            StartFreq = startFreq;
            StopFreq = stopFreq;
            FreqStep = (StopFreq - StartFreq) / (Length - 1D);

            for (int i = 0; i < Data.Count; i++)
            {
                Data[i].Frequency = StartFreq + (i * FreqStep);
            }

            FFT.UpdataConfiguration(length, winType);

            IsUpdated = false;
        }

        public bool IsUpdated { get; set; } = false;

        public List<FreqPoint> Data { get; }

        public FFT FFT { get; }

        public bool FlipSpectrum { get; set; } = false;

        public int Length { get; private set; }

        public double StartFreq { get; private set; }

        public double StopFreq { get; private set; }

        public double FreqStep { get; private set; }

        public void Transform(Complex[] data)
        {
            FFT.Transform(data);

            uint k, k1;
            uint N = FFT.Length;

            if (FlipSpectrum)
            {
                for (k = 0; k < N; k++)
                {
                    k1 = (k + (N / 2));

                    if (k1 >= N) k1 -= N;

                    Data[(int)k1].Value = FFT.Dsw[k.EndianInverse(FFT.M)];
                }
            }
            else
            {
                for (k = 0; k < N; k++)
                {
                    Data[(int)k].Value = FFT.Dsw[k.EndianInverse(FFT.M)];
                }
            }

            IsUpdated = true;
        }
    }
}
