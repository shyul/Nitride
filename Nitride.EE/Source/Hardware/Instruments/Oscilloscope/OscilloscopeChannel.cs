using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nitride.EE
{
    public abstract class OscilloscopeChannel : ITriggerSource, IDataProvider
    {
        protected OscilloscopeChannel(int chNum, string channelName, IOscilloscope device)
        {
            ChannelNumber = chNum;
            Name = channelName;
            Oscilloscope = device;
        }

        public int ChannelNumber { get; }

        public string Name { get; }
 
        public string Label { get; set; }

        public string Description { get; set; }

        public bool Enabled { get; set; } = true;

        public IOscilloscope Oscilloscope { get; }

        public IInstrument Parent => Oscilloscope;

        public string ResourceName => Parent.ResourceName + "_" + Name;

        public abstract void WriteSetting();

        public TriggerEdge TriggerEdge { get; set; } = TriggerEdge.Rising;

        public void DataIsUpdated()
        {
            UpdateTime = DateTime.Now;
            DataConsumers.ForEach(n => n.DataIsUpdated(this));
        }

        public DateTime UpdateTime { get; private set; } = DateTime.MinValue;


        private List<IDataConsumer> DataConsumers { get; set; } = new List<IDataConsumer>();

        public bool AddDataConsumer(IDataConsumer idk)
        {
            if (!DataConsumers.Contains(idk))
            {
                DataConsumers.Add(idk);
                return true;
            }
            return false;
        }

        public bool RemoveDataConsumer(IDataConsumer idk)
        {
            if (DataConsumers.Contains(idk))
            {
                DataConsumers.RemoveAll(n => n == idk);
                return true;
            }
            return false;
        }
    }
}
