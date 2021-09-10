/// ***************************************************************************
/// Nitride Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Chrono Table
/// 
/// ***************************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Nitride.EE
{
    public class FreqTable : DataTable, IComplexTable
    {
        public FreqTable(double startFreq, double stopFreq, int numOfPts)
        {
            double deltaFreq = (stopFreq - startFreq) / (numOfPts - 1D);
            for (int i = 0; i < numOfPts; i++)
            {
                double freq = startFreq + (i * deltaFreq);
                Rows.Add(new FreqRow(freq, this));
            }
        }

        ~FreqTable() => Dispose();

        public double Start => Count > 0 ? Rows.First().X : double.NaN;

        public double Stop => Count > 0 ? Rows.Last().X : double.NaN;

        private List<FreqRow> Rows { get; } = new();

        public override int Count => Rows.Count;

        public override void Clear()
        {
            lock (Rows)
                Rows.Clear();
        }

        public IEnumerable<double> FreqList => Rows.Select(n => n.Frequency).OrderBy(n => n);

        public IEnumerable<FreqRow> RowList => Rows.OrderBy(n => n.Frequency);

        public FreqRow this[int i]
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

        //public override IDatum this[int i, DatumColumn column] => i >= Count || i < 0 ? null : Rows[i][column];

        public Complex this[int i, ComplexColumn column] => i >= Count || i < 0 ? Complex.NaN : Rows[i][column];
    }
}
