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
        }

        public double SampleRate { get; set; }

        public double Start { get; set; }

        public double Duration => ((Count - 1) / SampleRate);

        public double Stop => Start + Duration;

        public int Count => Data.Length;

        public Complex[] Data { get; }

        public double Peak => Data.Select(x => x.Magnitude).Max();

        public double Rms => Math.Sqrt(Data.Select(x => Math.Pow(x.Magnitude, 2)).Sum() / Count);

        public void CopyData(Complex[] samples)
        {
            for (int i = 0; i < samples.Length; i++)
            {
                Data[i] = samples[i];
            }
        }

        public void GetSineWave(double fullScale, double normFreq)
        {
            double ang = 2 * normFreq * Math.PI; // normFreq * Math.PI;// / numPt;
            Complex w = new(Math.Cos(ang), Math.Sin(ang));
            Data[0] = new Complex(fullScale, 0.0);

            for (int i = 1; i < Count; i++)
            {
                Data[i] = Data[i - 1] * w;
            }
        }
    }
}
