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
using System.Text;

namespace Nitride.EE
{
    public class FreqTable : DataTable
    {
        public FreqTable(double startFreq, double stopFreq, int numOfPts)
        {
            for (int i = 0; i < numOfPts; i++)
            {
                double freq = startFreq + (i * (stopFreq - startFreq) / (numOfPts - 1D));
                Rows.Add(new FreqRow(freq, this));
            }
        }
        public double Start { get; protected set; }

        public double Stop { get; protected set; }

        private List<FreqRow> Rows { get; } = new();

        public override int Count => Rows.Count;


        public override double this[int i, NumericColumn column] => i >= Count || i < 0 ? double.NaN : Rows[i][column];

        public override IDatum this[int i, DatumColumn column] => i >= Count || i < 0 ? null : Rows[i][column];


    }


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
    }
}
