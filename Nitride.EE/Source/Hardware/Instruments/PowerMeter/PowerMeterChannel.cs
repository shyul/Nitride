using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nitride.EE
{
    public class PowerMeterChannel : IDataAcquisition
    {
        public string Name { get; }

        public string Label { get; set; }

        public string Description { get; set; }

        public bool Enabled { get; set; } = true;

        public IPowerMeter PowerMeter { get; }

        public IInstrument Parent => PowerMeter;

        public string ResourceName => Parent.ResourceName + "_" + Name;

        public double Value { get; set; }

        public bool IsReady { get; set; }

        public bool IsBusy { get; set; }

        public void GetSingle()
        {

        }

        public void StartStream()
        {

        }

        public void StopStream()
        {

        }

        // Other useful properties
    }
}
