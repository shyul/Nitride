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
using System.Numerics;

namespace Nitride.EE
{
    public class FreqRow : DataRow, IComparable<FreqRow>
    {
        public FreqRow(double freq, int index, FreqTable ft)
        {
            FreqTable = ft;
            Index = index;
            Frequency = freq;
        }

        public FreqTable FreqTable { get; }

        public int Index { get; set; }

        public double Frequency { get; }

        public override double X => Frequency;

        private Dictionary<ComplexColumn, Complex> ComplexColumnsLUT { get; } = new Dictionary<ComplexColumn, Complex>();

        public Complex this[ComplexColumn column]
        {
            get => column is ComplexColumn ic && ComplexColumnsLUT.ContainsKey(ic) ? ComplexColumnsLUT[ic] : double.NaN;
            set
            {
                if (value == double.NaN && ComplexColumnsLUT.ContainsKey(column))
                    ComplexColumnsLUT.Remove(column);
                else
                    ComplexColumnsLUT[column] = value;
            }
        }

        public int CompareTo(FreqRow other)
        {
            if (Frequency > other.Frequency) return 1;
            else if (Frequency < other.Frequency) return -1;
            else return 0;
        }
    }
}
