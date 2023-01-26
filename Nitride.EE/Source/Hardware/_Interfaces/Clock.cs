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

        bool Enabled { get; }
    }

    public interface IDerivedClock : IClock
    {
        IClock Reference { get; }

        double DivRatio { get; }
    }
}
