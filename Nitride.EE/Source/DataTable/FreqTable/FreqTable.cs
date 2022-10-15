/// ***************************************************************************
/// Nitride Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// FreqTable
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
                FreqStep = (stopFreq - startFreq) / (numOfPts - 1D);
                StartFreq = startFreq;

                while (Count < numOfPts)
                {
                    FreqRows.Add(new FreqRow(this));
                }
                
                while (Count > numOfPts)
                {
                    FreqRows.RemoveAt(Count - 1);
                }

                // Console.WriteLine("startFreq = " + startFreq + " | stopFreq = " + stopFreq + " | numOfPts = " + numOfPts + " | Step = " + FreqStep);

                int pt = 0;
                for (int i = 0; i < numOfPts; i++)
                {
                    double freq = startFreq + (i * FreqStep);
                    var row = FreqRows[i];
                    row.Frequency = freq;
                    row.Index = i;

                    //FreqRows.Add(new FreqRow(freq, pt, this));
                    StopFreq = freq;
                    pt++;
                }
            }
        }

        // To be removed!
        public void Sort()
        {
            lock (DataLockObject)
            {
                var rows = FreqRows.OrderBy(n => n.Frequency).ToList();

                FreqRows.Clear();

                int pt = 0;
                foreach (var row in rows)
                {
                    if (!Contains(row.Frequency))
                    {
                        row.Index = pt;
                        FreqRows.Add(row);
                        FreqToIndex.Add(row.Frequency, row.Index);
                        pt++;
                    }
                }
            }
        }

        /*
        public void Configure(double startFreq, double stopFreq, double freqStep)
        {
            lock (DataLockObject)
            {
                FreqRows.Clear();
                StartFreq = startFreq;
                FreqStep = freqStep;

                int pt = 0;
                for (double freq = startFreq; freq <= stopFreq; freq += freqStep)
                {
                    FreqRows.Add(new FreqRow(freq, pt, this));
                    StopFreq = freq;
                    pt++;
                }
            }
        }
        */
        public double StartFreq { get; protected set; } = double.MaxValue; // => Count > 0 ? Rows.First().X : double.NaN;

        public double StopFreq { get; protected set; } = double.MinValue; // => Count > 0 ? Rows.Last().X : double.NaN;

        public double FreqStep { get; protected set; } = double.MaxValue;

        public List<FreqRow> FreqRows { get; } = new();

        protected Dictionary<double, int> FreqToIndex { get; } = new();

        public bool Contains(double freq) => FreqToIndex.ContainsKey(freq);




        public override int Count => FreqRows.Count;

        public override bool IsEmpty => !FreqRows.Any();

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

        public Complex this[int i, ComplexColumn column] => i >= Count || i < 0 ? double.NaN : FreqRows[i][column];

        public override string GetXAxisLabel(int i)
        {
            //return (this[i].Frequency / 1e6).ToString("0.######") + "MHz";
            return this[i].Frequency.ToString();
        }

        public Range<double> GetRange(NumericColumn column, double startFreq, double stopFreq)
        {
            var rows = Rows.Where(n => n.Frequency <= stopFreq && n.Frequency >= startFreq).Select(n => n[column]);
            return rows.Any() ? new Range<double>(rows.Min(), rows.Max()) : null;
        }

        public Range<double> GetRange(NumericColumn column)
        {
            var rows = Rows.Select(n => n[column]);
            return rows.Any() ? new Range<double>(rows.Min(), rows.Max()) : null;
        }
    }
}
