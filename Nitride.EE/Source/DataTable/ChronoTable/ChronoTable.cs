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
        public ChronoTable() 
        {
        
        }

        public ChronoTable(int numOfPts)
        {
            ConfigureNumberOfPoints(numOfPts);
        }

        ~ChronoTable() => Dispose();

        public double StartTime { get; set; } = 0;

        public double SampleRate
        {
            get => m_SampleRate; set
            {
                m_SampleRate = value;
                SampleTimeStep = 1 / m_SampleRate;
            }
        }

        public double m_SampleRate;

        public double SampleTimeStep { get; private set; }

        public double Start => Count > 0 ? TimeRows.First().X : double.NaN;

        public double Stop => Count > 0 ? TimeRows.Last().X : double.NaN;

        private List<ChronoRow> TimeRows { get; } = new();

        public IEnumerable<ChronoRow> Rows => TimeRows.OrderBy(n => n.Index);

        public override int Count => m_Count; // TimeRows.Count;

        private int m_Count = 0;

        public override void Clear()
        {
            lock (TimeRows) 
            {
                m_Count = 0;
                TimeRows.Clear(); 
            }
        }

        public void ConfigureNumberOfPoints(int numOfPts)
        {
            lock (TimeRows)
            {
                while (TimeRows.Count < numOfPts)
                {
                    TimeRows.Add(new ChronoRow(this));
                }

                m_Count = numOfPts;
        
                for (int i = 0; i < m_Count; i++)
                {
                    TimeRows[i].Index = i;
                    TimeRows[i].Clear();
                }
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

        public Complex this[int i, ComplexColumn column] => i >= Count || i < 0 ? double.NaN : TimeRows[i][column];

        public override string GetXAxisLabel(int i)
        {
            //return (this[i].Frequency / 1e6).ToString("0.######") + "MHz";
            return this[i].Index.ToString();
        }
    }
}
