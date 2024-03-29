﻿/// ***************************************************************************
/// Nitride Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Chrono Table
/// 
/// ***************************************************************************

using System;
using System.Collections.Concurrent;
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

        public override void Clear() 
        {
            base.Clear();
            ComplexColumnsLUT.Clear();
        }

        private ConcurrentDictionary<ComplexColumn, Complex> ComplexColumnsLUT { get; } = new ConcurrentDictionary<ComplexColumn, Complex>();

        public Complex this[ComplexColumn column]
        {
            get => column is ComplexColumn ic && ComplexColumnsLUT.ContainsKey(ic) ? ComplexColumnsLUT[ic] : double.NaN;
            set
            {
                if (value == double.NaN && ComplexColumnsLUT.ContainsKey(column))
                    ComplexColumnsLUT.TryRemove(column, out _);
                else
                    ComplexColumnsLUT[column] = value;
            }
        }
    }
}
