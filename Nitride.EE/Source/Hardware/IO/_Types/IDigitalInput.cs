using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nitride.EE
{
    public interface IDigitalInput
    {
        bool Value { get; set; }

        double Threshold { get; }
    }
}
