/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Dyson Project - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;

namespace Nitride
{
    public interface IDataProvider // : IDisposable
    {
        bool AddDataConsumer(IDataConsumer idk);

        bool RemoveDataConsumer(IDataConsumer idk);

        DateTime UpdateTime { get; }
    }
}
