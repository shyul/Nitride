/// ***************************************************************************
/// Nitride Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************


namespace Nitride
{
    public interface IEnabled : IObject
    {
        /// <summary>
        /// For Sorting and Task
        /// </summary>
        bool Enabled { get; }
    }
}
