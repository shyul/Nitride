/// ***************************************************************************
/// Nitride Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Chrono Table
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Nitride.Chart;

namespace Nitride
{
    public abstract class DataTable : ITable, IDataProvider
    {
        ~DataTable() => Dispose();

        public void Dispose()
        {
            lock (DataConsumers)
            {
                foreach (IDataRenderer idr in DataConsumers)
                {
                    //RemoveDataConsumer(idr);
                    if (idr is ChartWidget bc)
                        bc.Close();
                    //else
                       // RemoveDataConsumer(idr);
                }

                DataConsumers.Clear();
            }

            Clear();
        }

        public object DataLockObject { get; } = new();

        //protected abstract IEnumerable<DataRow> Rows { get; } //= new();

        /// <summary>
        /// Returns the number of the Rows in the BarTable.
        /// </summary>
        public abstract int Count { get; } // => Rows.Count;

        public abstract void Clear();

        /// <summary>
        /// Returns if the BarTable is has no Bars.
        /// </summary>
        public virtual bool IsEmpty => Count < 1;

        /// <summary>
        /// Returns Last Row's Index
        /// </summary>
        protected int LastIndex => Count - 1;

        //public double this[int i, NumericColumn column] => i >= Count || i < 0 ? double.NaN : Rows[i][column];
        public abstract double this[int i, NumericColumn column] { get; }

        //public IDatum this[int i, DatumColumn column] => i >= Count || i < 0 ? null : Rows[i][column];
        //public abstract IDatum this[int i, DatumColumn column] { get; }

        /// <summary>
        /// Label for X Axis
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public abstract string GetXAxisLabel(int i);

        #region Data Consumers

        public void DataIsUpdated()
        {
            UpdateTime = DateTime.Now;
            DataConsumers.ForEach(n => n.DataIsUpdated(this));
        }

        private List<IDataConsumer> DataConsumers { get; } = new();

        private IEnumerable<IDataRenderer> DataRenderers => DataConsumers.Where(n => n is IDataRenderer).Select(n => n as IDataRenderer);

        public bool AddDataConsumer(IDataConsumer idk) => DataConsumers.CheckAdd(idk);

        public bool RemoveDataConsumer(IDataConsumer idk) => DataConsumers.CheckRemove(idk);

        public void ClearDataConsumers() => DataConsumers.Clear();

        public DateTime UpdateTime { get; set; } = TimeTool.MinInvalid;

        public bool ReadyToShow => Count > 0 && Status >= TableStatus.DataReady;

        public TableStatus Status
        {
            get => m_Status;

            set
            {
                m_Status = value;
                UpdateTime = DateTime.Now;

                lock (DataConsumers)
                {
                    if (ReadyToShow)
                    {
                        if (m_Status == TableStatus.CalculateFinished)
                        {
                            foreach (var idr in DataRenderers)
                                idr.PointerSnapToEnd();
                        }
                        else if (m_Status == TableStatus.TickingFinished)
                        {
                            foreach (var idr in DataRenderers)
                                idr.PointerSnapToNextTick();
                        }
                    }

                    DataConsumers.ForEach(n => n.DataIsUpdated(this));
                }
            }
        }

        private TableStatus m_Status = TableStatus.Default;


        #endregion Data Consumers

        public void ExportCSV(FileInfo file, IEnumerable<NumericColumn> columns, string format = "")
        {
            if (file.Directory.Exists)
            {
                if (file.Exists) file.Delete();

                //file.Create();
                using StreamWriter sw = file.CreateText();

                string header = "X_Axis,";

                foreach (var column in columns)
                {
                    header += column.Label + ",";
                }

                header = header.Trim().Trim(',').Trim();


                sw.WriteLine(header);

                for (int i = 0; i < Count; i++)
                {
                    header = GetXAxisLabel(i) + ",";

                    foreach (var column in columns)
                    {
                        header += (string.IsNullOrEmpty(format) ? this[i, column].ToString() : this[i, column].ToString(format)) + ",";
                    }

                    sw.WriteLine(header.TrimEnd(','));
                }
            }
        }
    }
}
