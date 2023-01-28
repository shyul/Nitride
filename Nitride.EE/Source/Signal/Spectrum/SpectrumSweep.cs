/// ***************************************************************************
/// Nitride Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2023 Xu Li - me@xuli.us
/// 
/// SpectrumSweep
/// WaveFormReceiver -> WaveForm -> (FreqTrace) SpectrumFFT -> (Frame) SpectrumData -> SpectrumChart
/// 
/// ***************************************************************************

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace Nitride.EE
{
    public class SpectrumSweep
    {
        public void WaveFormEnqueue(WaveForm wf, double centerFreq)
        {
            double bw = wf.SampleRate;


        }
    }
}
