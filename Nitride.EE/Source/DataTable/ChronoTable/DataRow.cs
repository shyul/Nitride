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
    public abstract class DataRow
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
            get
            {
                return column switch
                {
                    NumericColumn ic when NumericColumnsLUT.ContainsKey(ic) => NumericColumnsLUT[ic],
                    _ => double.NaN,
                };
            }
            set
            {
                if (double.IsNaN(value) && NumericColumnsLUT.ContainsKey(column))
                    NumericColumnsLUT.Remove(column);
                else
                    switch (column)
                    {
                        default:
                            if (!NumericColumnsLUT.ContainsKey(column))
                                NumericColumnsLUT.Add(column, value);
                            else
                                NumericColumnsLUT[column] = value;
                            break;
                    }
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
    }
}
