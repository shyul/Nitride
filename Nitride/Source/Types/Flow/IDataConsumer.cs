/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Dyson Project - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;

namespace Nitride
{
    public interface IDataConsumer : IDisposable
    {
        /// <summary>
        /// The IDataProvider notifies by calling this function
        /// </summary>
        void DataIsUpdated(IDataProvider provider); // void DataIsUpdated(IDataSource ids);
    }
}
