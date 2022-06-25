using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nitride.EE.Visa
{
    public class PowerSensor : ViClient
    {
        public PowerSensor(string resourceName) : base(resourceName)
        {


        }

        ~PowerSensor()
        {
            Dispose();
        }

    }
}
