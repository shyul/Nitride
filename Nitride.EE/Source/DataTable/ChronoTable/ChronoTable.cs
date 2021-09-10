﻿/// ***************************************************************************
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
    public class ChronoTable : DataTable
    {
        public ChronoTable(int numOfPts) 
        {
            for (int i = 0; i < numOfPts; i++)
            {
                Rows.Add(new ChronoRow(i, this));
            }
        }

        ~ChronoTable() => Dispose();

        public double StartTime { get; set; }

        public double SampleRate { get; set; }

        public double Start => Count > 0 ? Rows.First().X : double.NaN;

        public double Stop => Count > 0 ? Rows.Last().X : double.NaN;

        private List<ChronoRow> Rows { get; } = new();

        public override int Count => Rows.Count;

        public override void Clear()
        {
            lock (Rows)
                Rows.Clear();
        }

        public ChronoRow this[int i]
        {
            get
            {
                lock (Rows)
                    if (i >= Count || i < 0)
                        return null;
                    else
                        return Rows[i];
            }
        }

        public override double this[int i, NumericColumn column] => i >= Count || i < 0 ? double.NaN : Rows[i][column];

        public override IDatum this[int i, DatumColumn column] => i >= Count || i < 0 ? null : Rows[i][column];

    }
}
