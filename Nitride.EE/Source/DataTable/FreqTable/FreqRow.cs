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

        private Dictionary<ComplexColumn, Complex> ComplexColumnsLUT { get; } = new Dictionary<ComplexColumn, Complex>();

        public Complex this[ComplexColumn column]
        {
            get => column is ComplexColumn ic && ComplexColumnsLUT.ContainsKey(ic) ? ComplexColumnsLUT[ic] : double.NaN;
            set
            {
                if (Complex.IsNaN(value) && ComplexColumnsLUT.ContainsKey(column))
                    ComplexColumnsLUT.Remove(column);
                else
                    ComplexColumnsLUT[column] = value;
            }
        }
    }
}
