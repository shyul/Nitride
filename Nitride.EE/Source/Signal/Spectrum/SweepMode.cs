/// ***************************************************************************
/// Nitride Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2023 Xu Li - me@xuli.us
/// 
/// Frequency
/// 
/// ***************************************************************************

using System;

namespace Nitride.EE
{
    public enum SweepMode
    {
        FFT, // FFT Frame Length
        Segment, // Points (dewelling time), FFT Detector Length
        Detector,
    }
}
