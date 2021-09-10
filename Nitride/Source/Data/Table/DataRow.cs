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

namespace Nitride
{
    public abstract class DataRow : IEquatable<DataRow>
    {
        public abstract double X { get; }

        #region Numeric Column

        /// <summary>
        /// BarAnalysis Data Line
        /// </summary>
        // set is not allowed// One column only has one data
        private Dictionary<NumericColumn, double> NumericColumnsLUT { get; } = new Dictionary<NumericColumn, double>();

        public double this[NumericColumn column]
        {
            get => column is NumericColumn ic && NumericColumnsLUT.ContainsKey(ic) ? NumericColumnsLUT[ic] : double.NaN;
            set
            {
                if (double.IsNaN(value) && NumericColumnsLUT.ContainsKey(column))
                    NumericColumnsLUT.Remove(column);
                else
                    NumericColumnsLUT[column] = value;
            }
        }

        public double this[ISingleData isd]
        {
            get => this[isd.Column_Result];
            set => this[isd.Column_Result] = value;
        }

        public (double high, double low) this[IDualData idd]
        {
            get => (this[idd.Column_High], this[idd.Column_Low]);
            set
            {
                this[idd.Column_High] = value.high;
                this[idd.Column_Low] = value.low;
            }
        }

        #endregion Numeric Column

        #region Datum Column

        private Dictionary<DatumColumn, IDatum> DatumColumnsLUT { get; } = new Dictionary<DatumColumn, IDatum>();

        public IDatum this[DatumColumn dc]
        {
            get => DatumColumnsLUT.ContainsKey(dc) ? DatumColumnsLUT[dc] : null;

            set
            {
                if (value is IDatum dat && value.GetType() == dc.DatumType)
                {
                    DatumColumnsLUT[dc] = dat;
                }
                else if (value is null && DatumColumnsLUT.ContainsKey(dc))
                {
                    DatumColumnsLUT.Remove(dc);
                }
                else
                {
                    throw new("Invalid data type assigned");
                }
            }
        }

        #endregion Datum Column

        #region Equality

        public static bool operator ==(DataRow s1, DataRow s2) => s1.Equals(s2);
        public static bool operator !=(DataRow s1, DataRow s2) => !s1.Equals(s2);

        public bool Equals(DataRow other) => other is DataRow dr && dr.X == X;
        public override bool Equals(object other) => other is DataRow dr && dr.X == X;
        public override int GetHashCode() => X.GetHashCode();

        #endregion Equality
    }
}
