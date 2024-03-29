﻿/// ***************************************************************************
/// Nitride Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using Nitride.GridView;

namespace Nitride
{
    public class SimpleDataProvider : IDataProvider
    {
        public void DataIsUpdated()
        {
            UpdateTime = DateTime.Now;
            DataConsumers.ForEach(n => n.DataIsUpdated(this));
        }

        public DateTime UpdateTime { get; set; } //= DateTime.MinValue;

        public List<IDataConsumer> DataConsumers { get; } //= new List<IDataConsumer>();

        public bool AddDataConsumer(IDataConsumer idk)
        {
            return DataConsumers.CheckAdd(idk);
        }

        public bool RemoveDataConsumer(IDataConsumer idk)
        {
            if (idk is DockForm df) df.ReadyToShow = false;
            return DataConsumers.CheckRemove(idk);
        }
        /*
        public void Updated()
        {
            UpdateTime = DateTime.Now;
            DataConsumers.ForEach(n => n.DataIsUpdated(this));
        }*/
    }
}
