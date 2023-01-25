using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nitride.EE
{
    public interface IPowerMeter : IInstrument
    {
        Dictionary<string, PowerMeterChannel> PowerMeterChannels { get; }

        /*
        public IEnumerable<IInstrumentResource> Resources => PowerMeterChannels.Select(n => n.Value);

        public bool IsConnected { get; set; }

        public string ResourceName { get; set; }

        public abstract bool Open();

        public abstract void Close();

        public abstract void Dispose();
        */
    }
}
