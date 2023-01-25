using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nitride.EE
{
    public interface IInstrumentResource : IReady
    {
        IInstrument Parent { get; }

        string ResourceName { get; }
    }

    public interface IInstrument : IHardwareDevice
    {
        IEnumerable<IInstrumentResource> Resources { get; }
    }
}
