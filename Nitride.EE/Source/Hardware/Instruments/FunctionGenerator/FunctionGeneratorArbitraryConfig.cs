using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nitride.EE
{
    public class FunctionGeneratorArbitraryConfig : FunctionGeneratorConfig
    {
        public FunctionGeneratorArbitraryConfig(FunctionGeneratorChannel channel)
        {
            Channel = channel;
        }

        public FunctionGeneratorChannel Channel { get; }

        public double Gain { get; set; } = 1;

        public double SampleRate { get; set; } = 100e6;

        public Range<double> Range { get; } = new Range<double>(-5, 5);

        public List<double> Samples { get; set; }

        public double Amplitude
        {
            get => Range.Maximum - Range.Minimum;

            set
            {
                double offset = DcOffset;
                double halfRange = value / 2;
                Range.Set(offset - halfRange, offset + halfRange);
            }
        }

        public override double DcOffset
        {
            get => Range.Maximum - Range.Minimum;

            set
            {
                double halfRange = Amplitude / 2;
                Range.Set(value - halfRange, value + halfRange);
            }
        }

        #region DAC Interface

        public bool IsReady => false;

        public string Name => Channel.Name;

        public bool Enabled => Channel.Enabled && Channel.Config == this;

        public void DataIsUpdated(IDataProvider provider)
        {

        }

        public void Dispose()
        {

        }

        public void GetSingle()
        {

            Channel.Enabled = true;
        }

        #endregion DAC Interface
    }

}
