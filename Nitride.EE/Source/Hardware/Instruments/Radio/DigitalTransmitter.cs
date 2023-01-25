using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nitride.EE
{
    public class DigitalTransmitter : IInstrumentResource
    {
        public IInstrument Parent { get; set; }

        public string ResourceName { get; set; }

        public bool IsReady { get; set; }

        public bool Enabled { get; set; }

        public string Name { get; set; }

        public string Label { get; set; }

        public string Description { get; set; }

        public IClock SampleClock { get; set; }

        public double InterpolateRate { get; set; }

        public double SampleRate => SampleClock.Frequency / InterpolateRate;
    }
}
