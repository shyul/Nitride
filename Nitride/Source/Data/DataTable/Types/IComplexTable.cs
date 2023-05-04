/// ***************************************************************************
/// Nitride Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************
using System.Numerics;

namespace Nitride
{
    public interface IComplexTable : ITable
    {
        Complex this[int i, ComplexColumn column] { get; }
    }
}
