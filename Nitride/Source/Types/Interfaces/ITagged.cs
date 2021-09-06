/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Dyson Project - me@xuli.us
/// 
/// ***************************************************************************

using System.Collections.Generic;

namespace Nitride.Chart
{
    public interface ITagged
    {
        string Name { get; }

        IEnumerable<string> Tags { get; }
    }
}
