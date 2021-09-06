/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Dyson Project - me@xuli.us
/// 
/// ***************************************************************************

namespace Nitride
{
    public interface ISingleData : IDependable
    {
        //Color Color { get; }

        NumericColumn Column_Result { get; }
    }
}
