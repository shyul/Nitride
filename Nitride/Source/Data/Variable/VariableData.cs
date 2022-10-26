/// ***************************************************************************
/// Nitride Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2022 Xu Li - me@xuli.us
/// 
/// Text based data functions.
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace Nitride
{
    public abstract class VariableData : IDataProvider, IVariable, IDisposable
    {
        ~VariableData() => Dispose();

        public void Dispose()
        {
            DataConsumers.Clear();
        }

        public virtual string Name { get; protected set; }

        public virtual string Description { get; protected set; }

        public virtual string Unit { get; protected set; }

        public double Value { get; protected set; }


        public double MinimumStep { get; set; } = double.MinValue;

        public virtual void Update(string str) 
        {
            Value = str.ToDouble();
            DataIsUpdated();
        }

        public void Update(double val)
        {
            if (val < MinimumStep) val = MinimumStep;
            Value = val;
            DataIsUpdated();
        }

        public bool IsSubscribe { get; protected set; }

        public abstract bool Subscribe();

        public abstract bool Unsubscribe();

        public void DataIsUpdated()
        {
            UpdateTime = DateTime.Now;
            DataConsumers.ForEach(n => n.DataIsUpdated(this));
        }

        public bool AddDataConsumer(IDataConsumer idk) => DataConsumers.CheckAdd(idk);

        public bool RemoveDataConsumer(IDataConsumer idk) => DataConsumers.CheckRemove(idk);

        private List<IDataConsumer> DataConsumers { get; } = new ();

        public DateTime UpdateTime { get; private set; } = DateTime.MinValue;
    }
}
