using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nitride.EE
{
    public class MultimeterChannel : IAnalogInput
    {
        public MultimeterChannel(int chNum, string channelName, IMultimeter dmm)
        {
            ChannelNumber = chNum;
            Name = channelName;
            Multimeter = dmm;
        }

        public int ChannelNumber { get; }

        public string Name { get; }

        public string Label { get; set; }

        public string Description { get; set; }

        public bool Enabled { get; set; } = true;

        public IMultimeter Multimeter { get; }

        public IInstrument Parent => Multimeter;

        public string ResourceName => Parent.ResourceName + "_" + Name;

        public bool IsReady { get; set; }

        public Range<double> Range => new(0, 10);

        public bool IsAutoRange { get; set; } = true;

        public MultimeterConfig Config { get; set; }

        public void WriteSetting() => Multimeter.Multimeter_WriteSetting(Name);

        public void ReadSetting() => Multimeter.Multimeter_ReadSetting(Name);

        public double Value
        {
            get => Multimeter.Multimeter_Read(Name);

            set { }
        }
    }
}
