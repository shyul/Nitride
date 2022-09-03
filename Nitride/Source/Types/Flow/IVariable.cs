/// ***************************************************************************
/// Nitride Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2022 Xu Li - me@xuli.us
/// 
/// Variable is a sigular varying data point
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;

namespace Nitride
{
    public interface IVariable
    {
        string Name { get; }

        DateTime UpdateTime { get; }

        void Update(string str);

        bool IsSubscribe { get; }

        bool Subscribe();

        bool Unsubscribe();


    }
}
