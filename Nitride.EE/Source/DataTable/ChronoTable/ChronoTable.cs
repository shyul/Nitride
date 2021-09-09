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
    public class ChronoTable : DataTable
    {




        private List<ChronoRow> Rows { get; } = new();

        public override int Count => Rows.Count;


        public override double this[int i, NumericColumn column] => i >= Count || i < 0 ? double.NaN : Rows[i][column];

        public override IDatum this[int i, DatumColumn column] => i >= Count || i < 0 ? null : Rows[i][column];

    }


    public class ChronoRow : DataRow
    {

        public FreqTable Table { get; }

        public long TimeStamp { get; }

        public override double X => TimeStamp;
    }
}
