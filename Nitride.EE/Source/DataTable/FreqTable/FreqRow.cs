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
        public FreqRow(FreqTable ft)
        {
            FreqTable = ft;
        }

        public FreqRow(double freq, int index, FreqTable ft)
        {
            FreqTable = ft;
            Index = index;
            Frequency = freq;
            /*
            double stepBy2 = ft.FreqStep / 2;
            FreqRange = new (Frequency - stepBy2, Frequency + stepBy2);*/
        }

        public FreqTable FreqTable { get; }

        public int Index { get; set; }

        public double Frequency
        {
            get => m_Frequency;

            set
            {
                m_Frequency = value;
                double stepBy2 = FreqTable.FreqStep / 2;
                FreqRange.Set(m_Frequency - stepBy2, m_Frequency + stepBy2);
            }
        }

        private double m_Frequency;

        public Range<double> FreqRange { get; } = new();

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
