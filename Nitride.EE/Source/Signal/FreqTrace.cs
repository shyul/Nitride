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
        public FreqTrace(int maxLength)
        {
            Length = maxLength;
            Data = new List<FreqPoint>();

            for (int i = 0; i < maxLength; i++) 
            {
                Data.Add(new FreqPoint());
            }
        }

        public void Configure(int length, double startFreq, double stopFreq)
        {
            Length = length;
            StartFreq = startFreq;
            StopFreq = stopFreq;
            FreqStep = (StopFreq - StartFreq) / (Length - 1D);

            for (int i = 0; i < Data.Count; i++)
            {
                Data[i].Frequency = StartFreq + (i * FreqStep);
            }
        }

        public int Length { get; private set; }

        public double StartFreq { get; private set; }

        public double StopFreq { get; private set; }

        public double FreqStep { get; private set; }

        public List<FreqPoint> Data { get; }
    }

    public enum SweepMode
    {
        FFT, // FFT Frame Length
        DDC, // DDC FFT Detector Length x Points (A.k.a Multi FFT frame merged)
        Analog, // Points (dewelling time), FFT Detector Length
    }
}
