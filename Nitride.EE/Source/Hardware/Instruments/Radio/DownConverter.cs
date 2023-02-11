using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nitride.EE
{
    // Fixed IF Stage

    // Tunable RF Stage

    public class TunerStage : IRfSignalSink, IRfSignalSource
    {
    
    }


    public class IntermediateFrequencyStage : IRfSignalSink, IRfSignalSource
    {
        // Calibration Methods and Data

        // 



        public Tunner LocalOscillator { get; set; }

        public WaveFormReceiver Receiver { get; set; }



        public IDownConverterState ConverterState { get; set; }

        public double Gain { get; set; }


        public Dictionary<double, Dictionary<double, double>> CalibrationTable { get; }


        public IRfSignalSink SignalSink { get; set; }

        public IRfSignalSource SignalSource { get; set; }

    }

    public interface IDownConverterState 
    {
        public double CenterFrequency { get; set; }

        public double Bandwidth { get; set; }

        public double LoFrequency { get; set; }

        public bool IsLoUpSide { get; set; }




    }

    public interface IRfSignalSink 
    {
    
    }

    public interface IRfSignalSource
    {

    }


}

