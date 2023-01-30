using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nitride.EE
{
    public class Tunner : IPll, IInstrumentResource
    {
        public IInstrument Parent { get; set; }

        public string ResourceName { get; set; }

        public bool IsReady { get; set; }

        public bool Enabled { get; set; }

        public string Name { get; set; }

        public string Label { get; set; }

        public string Description { get; set; }


        public bool IsLocked => throw new NotImplementedException();

        public IClock Reference => throw new NotImplementedException();

        public double DivRatio => throw new NotImplementedException();

        public double Frequency => throw new NotImplementedException();


    }
}
