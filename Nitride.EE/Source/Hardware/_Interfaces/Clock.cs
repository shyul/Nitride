using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nitride.EE
{
    public interface IClock
    {
        double Frequency { get; }

        bool IsEnabled { get; }
    }

    public interface IDerivedClock : IClock
    {
        IClock Source { get; }

        double DivRatio { get; }
    }

    public interface IPllClock : IDerivedClock
    {
        bool IsLocked { get; }
    }
}
