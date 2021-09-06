using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nitride.EE
{
    public interface IAnalogPin : IPort
    {
        Range<double> Range { get; }

        double Value { get; set; }
    }
}
