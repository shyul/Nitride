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
    public class ChronoRow : DataRow
    {
        public ChronoRow(ChronoTable ct)
        {
            Table = ct;
        }

        public ChronoTable Table { get; }

        public int Index { get; set; }

        public override double X => Table.StartTime + (Table.SampleTimeStep * Index);

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
    }
}
