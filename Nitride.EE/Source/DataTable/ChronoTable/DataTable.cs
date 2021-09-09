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
    public abstract class DataTable : IDatumTable, IDataProvider
    {
        public object DataLockObject { get; } = new();

        //protected abstract IEnumerable<DataRow> Rows { get; } //= new();

        /// <summary>
        /// Returns the number of the Rows in the BarTable.
        /// </summary>
        public abstract int Count { get; } // => Rows.Count;

        /// <summary>
        /// Returns if the BarTable is has no Bars.
        /// </summary>
        public bool IsEmpty => Count < 1;

        /// <summary>
        /// Returns Last Row's Index
        /// </summary>
        protected int LastIndex => Count - 1;

        //public double this[int i, NumericColumn column] => i >= Count || i < 0 ? double.NaN : Rows[i][column];
        public abstract double this[int i, NumericColumn column] { get; }

        //public IDatum this[int i, DatumColumn column] => i >= Count || i < 0 ? null : Rows[i][column];
        public abstract IDatum this[int i, DatumColumn column] { get; }

        #region Data Consumers

        private List<IDataConsumer> DataConsumers { get; } = new();

        private IEnumerable<IDataRenderer> DataRenderers => DataConsumers.Where(n => n is IDataRenderer).Select(n => n as IDataRenderer);

        public bool AddDataConsumer(IDataConsumer idk) => DataConsumers.CheckAdd(idk);

        public bool RemoveDataConsumer(IDataConsumer idk) => DataConsumers.CheckRemove(idk);

        public DateTime UpdateTime { get; private set; } = TimeTool.MinInvalid;

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
    }
}
