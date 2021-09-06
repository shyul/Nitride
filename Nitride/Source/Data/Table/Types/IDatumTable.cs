/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Dyson Project - me@xuli.us
/// 
/// ***************************************************************************


namespace Nitride
{
    public interface IDatumTable : ITable
    {
        IDatum this[int i, DatumColumn column] { get; }
    }
}
