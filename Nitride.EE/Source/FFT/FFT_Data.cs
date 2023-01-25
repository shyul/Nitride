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
    public class FFT_Data
    {
        public FFT_Data(int maxLength)
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
}
