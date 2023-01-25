using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nitride.EE
{
    public class PowerSupplyChannel : IInstrumentResource
    {
        public PowerSupplyChannel(string channelName, IPowerSupply powerSupply, Range<double> voltageRange, Range<double> currentRange)
        {
            Name = channelName;
            PowerSupply = powerSupply;
            VoltageRange = voltageRange;
            CurrentRange = currentRange;
        }

        public string Name { get; }

        public string Label { get; set; }

        public string Description { get; set; }

        public bool Enabled
        {
            get => PowerSupply.PowerSupply_Enabled(Name);

            set
            {
                if (value) 
                    PowerSupply.PowerSupply_ON(Name);
                else 
                    PowerSupply.PowerSupply_OFF(Name);
            }
        }

        public IPowerSupply PowerSupply { get; }

        public IInstrument Parent => PowerSupply;

        public string ResourceName => Parent.ResourceName + "_" + Name;

        public bool IsReady { get; set; }

        public void WriteSetting() => PowerSupply.PowerSupply_WriteSetting(Name);

        public void ReadSetting() => PowerSupply.PowerSupply_ReadSetting(Name);

        public (double voltage, double current) ReadOutput() => PowerSupply.PowerSupply_ReadOutput(Name);

        public PowerSupplyMode Mode { get; set; } = PowerSupplyMode.ConstantVoltage;

        public double Voltage
        {
            get => m_Voltage;

            set
            {
                double v = value;
                if (v > VoltageRange.Max) v = VoltageRange.Max;
                else if (v < VoltageRange.Min) v = VoltageRange.Min;
                m_Voltage = v;
            }
        }

        private double m_Voltage;

        public double Current
        {
            get => m_Current;

            set
            {
                double i = value;
                if (i > CurrentRange.Max) i = CurrentRange.Max;
                else if (i < CurrentRange.Min) i = CurrentRange.Min;
                m_Current = i;
            }
        }

        public double m_Current;

        public Range<double> VoltageRange { get; }

        public Range<double> CurrentRange { get; }


    }
}
