/// ***************************************************************************
/// Nitride Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2023 Xu Li - me@xuli.us
/// 
/// ***************************************************************************


namespace Nitride
{
    public interface IReady : IEnabled
    {
        bool IsReady { get; }
    }
}
