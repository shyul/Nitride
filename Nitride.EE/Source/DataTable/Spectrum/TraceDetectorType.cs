/// ***************************************************************************
/// Nitride Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// // when data length is longer than trace length.
/// 
/// ***************************************************************************
/// 
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nitride;
using Nitride.Chart;

namespace Nitride.EE
{
    public enum TraceDetectorType : int
    {
        Peak = 0,
        NegativePeak,
        Average, // Average of peak and low
        Mean,
        RMS, // Average of all bin members.
        Spline
    }
}
