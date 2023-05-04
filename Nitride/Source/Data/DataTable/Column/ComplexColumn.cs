/// ***************************************************************************
/// Nitride Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

namespace Nitride
{
    public class ComplexColumn : Column
    {
        public ComplexColumn(string name) => Name = Label = name;

        public ComplexColumn(string name, string label)
        {
            Name = name;
            Label = label;
        }

        public override int GetHashCode() => Name.GetHashCode() ^ typeof(ComplexColumn).GetHashCode();
    }
}
