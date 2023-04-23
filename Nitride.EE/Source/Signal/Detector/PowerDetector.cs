using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nitride.EE
{
    public class PowerDetector
    {
        public PowerDetector(int numOfCh, int maxSampleLength)
        {
            NumOfCh = numOfCh;
            MaxSampleLength = maxSampleLength;
            WaveFormGroup = new WaveFormGroup(NumOfCh, MaxSampleLength);
           

            Channels = new PowerDetectorChannel[NumOfCh];
            for (int i = 0; i < NumOfCh; i++)
            {
                Channels[i] = new PowerDetectorChannel(i, this);
            }
        }

        public int NumOfCh { get; }

        public int MaxSampleLength { get; }

        public int SampleLength { get; private set; }

        public double RBW { get; private set; }

        public WaveFormGroup WaveFormGroup { get; }

        public PowerDetectorChannel[] Channels { get; }

        public void Configure(double bandwidth, int length)
        {
            SampleLength = length;
            RBW = bandwidth / length;

            foreach (var ch in Channels)
            {
                double startFreq = ch.CenterFreq - (bandwidth / 2);
                double stopFreq = ch.CenterFreq + (bandwidth / 2);

                FreqTrace tr = ch.FreqTrace;
                tr.Configure(SampleLength, startFreq, stopFreq, ch.WindowType, true);

                FreqTable ft = ch.FreqTable;
                ft.Configure(startFreq, stopFreq, SampleLength);
            }

            // also update freqTable
        }

        public void Evaluate() 
        {
            for (int i = 0; i < NumOfCh; i++)
            {
                PowerDetectorChannel ch = Channels[i];

                ch.FreqTrace.Transform(WaveFormGroup[i].Data);

                ComplexColumn col_res = ch.ResultColumn;

                for (int j = 0; j < SampleLength; j++) 
                {
                    ch.FreqTable[j][col_res] = ch.FreqTrace.Data[j].Value / ch.Gain;
                }

                double p0 = Math.Log10(ch.Peak()) * 20;
                Console.WriteLine("Ch" + i + " = " + p0);

                ch.FreqTable.DataIsUpdated();
            }
        }
    }
}
