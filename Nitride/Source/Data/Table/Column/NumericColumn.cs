/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Dyson Project - me@xuli.us
/// 
/// ***************************************************************************

namespace Nitride
{
    public class NumericColumn : Column
    {
        public NumericColumn(string name) => Name = Label = name;

        public NumericColumn(string name, string label)
        {
            Name = name;
            Label = label;
        }

        public override int GetHashCode() => Name.GetHashCode() ^ GetType().GetHashCode();
    }
}
