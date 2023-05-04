/// ***************************************************************************
/// Nitride Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;

namespace Nitride
{
    public abstract class Column : IEquatable<Column>
    {
        public virtual string Name { get; set; } = string.Empty;

        public virtual string Label { get; set; } = string.Empty;

        public object DataLockObject => this;

        #region Equality

        public override int GetHashCode() => Name.GetHashCode() ^ GetType().GetHashCode();

        public virtual bool Equals(Column other)
        {
            if (other is not null)
                if (DataLockObject == other.DataLockObject) 
                {
                    //Console.Write("#");
                    return true;
                }
                else
                {
                    //Console.Write("$");
                    return GetType() == other.GetType() && Name == other.Name;
                }
            else
                return false;
        }

        public override bool Equals(object other)
        {
            if (base.Equals(other))
            {
                //Console.Write("^");
                return true;
            }
            else if (other is Column dc)
                return Equals(dc);
            else
                return false;
        }

        public static bool operator !=(Column s1, Column s2) => !s1.Equals(s2);
        public static bool operator ==(Column s1, Column s2) => s1.Equals(s2);

        #endregion Equality
    }
}
