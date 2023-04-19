using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Nitride.EE
{
    public interface IReceiver : IInstrumentResource
    {
        int NumOfCh { get; }





        uint DecimationRate { get; set; }

        double SampleRate { get; }

        double Bandwidth { get; }

        int MaxSampleLength { get; }

        int SampleLength { get; set; }

        double ReceiveTime { get; }


        bool ApplyReceiverConfig();

        bool IsFetchRunning { get; }

        bool IsFetchPause { get; }

        bool IsFetchPaused { get; }

        bool RunSingleFetch();

        bool RunFetch();

        void PauseFetch();

        void ResumeFetch();

        void StopFetch();
    }
}
