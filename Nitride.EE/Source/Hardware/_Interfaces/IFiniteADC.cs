using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nitride.EE
{
    public interface IFiniteADC : IDataAcquisition, IDataProvider
    {
        double SampleRate { get; set; }

        List<double> Samples { get; set; }
    }
}
