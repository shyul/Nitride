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
    public class ChronoRow : DataRow
    {
        public ChronoRow(long timeStamp, ChronoTable ct)
        {
            Table = ct;
            TimeStamp = timeStamp;
        }

        public ChronoTable Table { get; }

        public long TimeStamp { get; }

        public override double X => TimeStamp;
    }
}
