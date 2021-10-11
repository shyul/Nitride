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
        ~FreqTable() => Dispose();

        public void Configure(double startFreq, double stopFreq, int numOfPts)
        {
            lock (DataLockObject)
            {
                FreqRows.Clear();
                double step = (stopFreq - startFreq) / (numOfPts - 1D);
                Start = startFreq;

                int pt = 0;
                for (int i = 0; i < numOfPts; i++)
                {
                    double freq = startFreq + (i * step);
                    FreqRows.Add(new FreqRow(freq, pt, this));
                    Stop = freq;
                    pt++;
                }
            }
        }

        public void Configure(double startFreq, double stopFreq, double stepFreq)
        {
            lock (DataLockObject)
            {
                FreqRows.Clear();
                Start = startFreq;

                int pt = 0;
                for (double freq = startFreq; freq < stopFreq; freq += stepFreq)
                {
                    FreqRows.Add(new FreqRow(freq, pt, this));
                    Stop = freq;
                    pt++;
                }
            }
        }

        public double Start { get; protected set; } = double.MaxValue; // => Count > 0 ? Rows.First().X : double.NaN;

        public double Stop { get; protected set; } = double.MinValue; // => Count > 0 ? Rows.Last().X : double.NaN;

        public List<FreqRow> FreqRows { get; } = new();

        protected Dictionary<double, int> FreqToIndex { get; } = new();

        public void ImportRow(FreqRow row)
        {



        }

        public void ImportRows(IEnumerable<FreqRow> rows)
        {



        }

        public void Sort() 
        {
        
        
        }


        public override int Count => FreqRows.Count;

        public override void Clear()
        {
            lock (DataLockObject)
                FreqRows.Clear();
        }

        public IEnumerable<double> FreqList => FreqRows.Select(n => n.Frequency).OrderBy(n => n);

        public IEnumerable<FreqRow> Rows => FreqRows.OrderBy(n => n.Frequency);

        public FreqRow this[int i]
        {
            get
            {
                lock (DataLockObject)
                    if (i >= Count || i < 0)
                        return null;
                    else
                        return FreqRows[i];// as ;
            }
        }

        public override double this[int i, NumericColumn column] => i >= Count || i < 0 ? double.NaN : FreqRows[i][column];

        public Complex this[int i, ComplexColumn column] => i >= Count || i < 0 ? Complex.NaN : FreqRows[i][column];
    }
}
