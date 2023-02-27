/// ***************************************************************************
/// Nitride Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2023 Xu Li - me@xuli.us
/// 
/// WaveForm
/// 
/// ***************************************************************************

using Nitride.Chart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Nitride.EE
{
    public class Pulse
    {
        public Pulse(int maxLength)
        {
            Length = maxLength;
            FFT = new(maxLength);
            PWS = new Complex[maxLength];
        }

        public int Length { get; private set; }

        public Complex[] PWS { get; }

        public FFT FFT { get; }

        public void Configure(int length)
        {
            if (length <= PWS.Length)
            {
                Length = length;
                FFT.UpdataConfiguration(length, WindowsType.Rectangle);
            }
        }

        public void GetPws(WaveForm wf, int offset = 0)
        {
            FFT.Transform(wf.Data, offset);
            for (int i = 0; i < Length; i++)
            {
                PWS[i] = Complex.Conjugate(FFT.Dsw[i]);
            }
        }

        public Complex Match(WaveForm wf, int offset)
        {
            Complex r = Complex.Zero;
            FFT.Transform(wf.Data, offset);
            for (int i = 0; i < Length; i++)
            {
                r += FFT.Dsw[i] * PWS[i];
            }
            return r;
        }
    }


    public class Radar
    {
        public Radar(int maxPulseLength = 1024, int maxReturnLength = 8192, int maxCorrelation = 8)
        {
            for (int i = 0; i < maxCorrelation; i++)
            {
            
            }
        }

        // Configure Sample Rate
        // Configure Time / Distance


        public int HomeIndex { get; private set; }


        public int Correlation { get; private set; }

        public WaveForm[] PulseWaveForm { get; }

        public Pulse[] InitialPulse { get; }

        public Pulse[] ReferencePulse { get; }

        public WaveForm[] ReturnReference { get; }

        public WaveForm[] ReturnEcho { get; }


        // public ChronoTable 

        public ChronoTable ReturnTable { get; }
 
        public int PulseLength { get; }

        public void GenerateExampleWaveForm() 
        {
        
        
        }

        public void EvaluateInitialPulse(ChronoTable ct)
        {
            Pulse p = InitialPulse[0];

            int pt = 0;
            for (int i = 1 - PulseLength; i < PulseLength; i++)
            {
                Complex r = p.Match(PulseWaveForm[0], i);
                double m = 10 * Math.Log10(r.Magnitude);

                pt++;
            }

        }

        // Get Home Index 
        public void EvaluateReference(int maxIndex) 
        {
            double maxMatch = double.MinValue;
            int maxMatchIndex = 0;

            for (int i = 0; i < maxIndex; i++) 
            {
            
            
            }
        
        }

        // 1. Go over Refer
    }

}
