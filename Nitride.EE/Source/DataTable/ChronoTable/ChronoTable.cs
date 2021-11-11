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
    public class ChronoTable : DataTable, IComplexTable
    {
        public ChronoTable(int numOfPts)
        {
            Clear(numOfPts);
            /*
            for (int i = 0; i < numOfPts; i++)
            {
                TimeRows.Add(new ChronoRow(i, this));
            }*/
        }

        ~ChronoTable() => Dispose();

        public double StartTime { get; set; }

        public double SampleRate { get; set; }

        public double Start => Count > 0 ? TimeRows.First().X : double.NaN;

        public double Stop => Count > 0 ? TimeRows.Last().X : double.NaN;

        private List<ChronoRow> TimeRows { get; } = new();

        public IEnumerable<ChronoRow> Rows => TimeRows.OrderBy(n => n.TimeStamp);

        public override int Count => TimeRows.Count;

        public override void Clear()
        {
            lock (TimeRows)
                TimeRows.Clear();
        }

        public void Clear(int numOfPts)
        {
            TimeRows.Clear();
            for (int i = 0; i < numOfPts; i++)
            {
                TimeRows.Add(new ChronoRow(i, this));
            }
        }

        public ChronoRow this[int i]
        {
            get
            {
                lock (TimeRows)
                    if (i >= Count || i < 0)
                        return null;
                    else
                        return TimeRows[i];
            }
        }

        public override double this[int i, NumericColumn column] => i >= Count || i < 0 ? double.NaN : TimeRows[i][column];

        public Complex this[int i, ComplexColumn column] => i >= Count || i < 0 ? Complex.NaN : TimeRows[i][column];

        //public override IDatum this[int i, DatumColumn column] => i >= Count || i < 0 ? null : Rows[i][column];
    }
}
