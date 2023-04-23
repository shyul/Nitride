using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace Nitride.EE
{
    public class PowerDetectorChannel
    {
        public PowerDetectorChannel(int index, PowerDetector det)
        {
            Detector = det;
            ResultColumn = new ComplexColumn("Result Ch " + index);
            // FreqChart = new FreqChart("Detector Ch " + index, det.FreqTable);

            FreqTrace = new FreqTrace(Detector.MaxSampleLength);
        }

        public PowerDetector Detector { get; }

        public FreqTrace FreqTrace { get; }

        public double Gain => FreqTrace.FFT.Gain;

        public FreqTable FreqTable { get; } = new();

        public ComplexColumn ResultColumn { get; }

        public int SampleLength => Detector.SampleLength;

        public double CenterFreq { get; set; }

        public WindowType WindowType { get; set; } = WindowType.Blackman;

        public double Peak()
        {
            int start = (SampleLength / 2) - 10;
            int end = start + 20;

            var rows = FreqTable.Rows.Where(n => n.Index >= start && n.Index <= end);

            if (rows.Count() > 0)
            {
                return rows.Select(n => n[ResultColumn].Magnitude).Max();
            }
            else
                return double.NaN;
        }

        public double Peak(int centerPt, int span) 
        {
            int start = centerPt - (span / 2);
            int end = centerPt + (span / 2);

            var rows = FreqTable.Rows.Where(n=>n.Index >= start  && n.Index <= end);

            if (rows.Count() > 0) 
            {
                return rows.Select(n => n[ResultColumn].Magnitude).Max();
            }
            else
                return double.NaN;
        }

        public double Peak(double centerFreq, double span)
        {
            return 0;
        }
    }
}
