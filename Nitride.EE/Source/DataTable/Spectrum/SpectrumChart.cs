using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Nitride;
using Nitride.Chart;

namespace Nitride.EE
{
    public sealed class SpectrumChart : ChartWidget
    {
        public SpectrumChart(string name, SpectrumData sd, NumericColumn mainCol) : base(name)
        {
            Data = sd;
        }

        private SpectrumData Data { get; }


        public Area MainArea { get; }


        public Area HistoArea { get; }

        public override string this[int i]
        {
            get
            {
                if (Data.FreqTable[i] is FreqRow sp && sp.Frequency is double d)
                    return (d / 1e6).ToString("0.######") + "MHz";
                else
                    return string.Empty;
            }
        }

        public override ITable Table => Data.FreqTable;

        public override bool ReadyToShow { get => m_ReadyToShow; set { m_ReadyToShow = value; } }

        public override void CoordinateOverlay()
        {

        }

        public void DataUpdateWorker() 
        {
            while (true)
            {
                if (Data.FrameBuffer.Count > 0)
                {
                    FreqFrame = Data.FrameBuffer.Dequeue();

                    m_AsyncUpdateUI = true;
                }
            }
        }

        public FreqFrame FreqFrame { get; private set; }

        public override void DataIsUpdated(IDataProvider _) => m_AsyncUpdateUI = true;
    }
}
