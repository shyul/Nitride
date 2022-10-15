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
        public void Configure(int count, double rate = 1, double startTime = 0)
        {
            Count = count;

            while (Count > Data.Count)
            {
                Data.Add(new Complex());
            }

            /*
            while (Count < Data.Count)
            {
                Data.RemoveAt(Data.Count - 1);
            }*/

            SampleRate = rate;
            StartTime = startTime;
            Duration = ((Count - 1) / SampleRate);
            StopTime = StartTime + Duration;
        }

        public int Count { get; private set; }

        public double SampleRate { get; private set; }

        public double StartTime { get; private set; }

        public double Duration { get; private set; }

        public double StopTime { get; private set; }

        public List<Complex> Data { get; } = new();

        public double Peak => Data.Select(x => x.Magnitude).Max();

        public double Rms => Math.Sqrt(Data.Select(x => Math.Pow(x.Magnitude, 2)).Sum() / Count);

        public void CopyData(Complex[] samples, int offset = 0)
        {
            int cnt = Math.Min(samples.Length - offset, Count);
            for (int i = 0; i < cnt; i++)
            {
                Data[i] = samples[i + offset];
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
