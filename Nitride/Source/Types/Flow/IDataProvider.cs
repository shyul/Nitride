/// ***************************************************************************
/// Nitride Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2022 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;

namespace Nitride
{
    public interface IDataProvider // : IDisposable
    {
        void DataIsUpdated();/*
        {
            UpdateTime = DateTime.Now;
            DataConsumers.ForEach(n => n.DataIsUpdated(this));
        }*/

        //List<IDataConsumer> DataConsumers { get; }

        bool AddDataConsumer(IDataConsumer idk);// => DataConsumers.CheckAdd(idk);

        bool RemoveDataConsumer(IDataConsumer idk);// => DataConsumers.CheckRemove(idk);

        DateTime UpdateTime { get; } //set; }
    }
}
