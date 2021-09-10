/// ***************************************************************************
/// Nitride Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Chrono Table
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nitride.EE
{
    public class FreqRow : DataRow
    {
        public FreqRow(double freq, FreqTable ft)
        {
            Table = ft;
            Frequency = freq;
        }

        public FreqTable Table { get; }

        public double Frequency { get; }

        public override double X => Frequency;
    }
}
