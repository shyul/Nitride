using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nitride;

namespace Nitride.EE
{
    public abstract class DigitalReceiver : Receiver , IDataAcquisition
    {
        protected List<ReceiverData> RxDataBufferPool { get; set; } = new();

        protected Queue<ReceiverData> RxDataBufferQueue { get; } = new();

        public virtual IClock SampleClock { get; set; }

        public virtual double DecimationRate { get; set; }

        public virtual double SampleRate => SampleClock.Frequency / DecimationRate;

        //public override double Bandwidth { get; set; }

        //public override double CenterFrequency { get; set; }

        public virtual bool IsBusy { get; protected set; }

        public abstract void GetSingle();

        public abstract void StartStream();

        public abstract void StopStream();
    }


    public abstract class Receiver : IInstrumentResource 
    {
        public IInstrument Parent { get; set; }

        public string ResourceName { get; set; }

        public bool IsReady { get; set; }

        public bool Enabled { get; set; }

        public string Name { get; set; }

        public string Label { get; set; }

        public string Description { get; set; }

        public virtual double Bandwidth { get; set; }

        public virtual double CenterFrequency { get; set; }

    }
}
