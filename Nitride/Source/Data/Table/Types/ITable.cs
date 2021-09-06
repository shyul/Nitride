/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Dyson Project - me@xuli.us
/// 
/// ***************************************************************************


namespace Nitride
{
    public interface ITable
    {
        int Count { get; }

        double this[int i, NumericColumn column] { get; }

        TableStatus Status { get; set; }

        object DataLockObject { get; }
    }
}
