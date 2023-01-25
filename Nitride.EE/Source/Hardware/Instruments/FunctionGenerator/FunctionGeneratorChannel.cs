using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nitride.EE
{
    public class FunctionGeneratorChannel : IInstrumentResource
    {
        public FunctionGeneratorChannel(string channelName, IFunctionGenerator fgen)
        {
            Name = channelName;
            FunctionGenerator = fgen;
        }

        public string Name { get; }

        public string Label { get; set; }

        public string Description { get; set; }

        public bool Enabled
        {
            get => m_Enabled;

            set
            {
                m_Enabled = value;

                if (m_Enabled)
                    FunctionGenerator.FunctionGenerator_ON(Name);
                else
                    FunctionGenerator.FunctionGenerator_OFF(Name);
            }
        }

        private bool m_Enabled = false;

        public IFunctionGenerator FunctionGenerator { get; }

        public FunctionGeneratorConfig Config { get; set; }

        public IInstrument Parent => FunctionGenerator;

        public string ResourceName => Parent.ResourceName + "_" + Name;

        public bool IsReady { get; set; }
    }
}
