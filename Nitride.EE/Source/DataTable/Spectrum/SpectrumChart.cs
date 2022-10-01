/// ***************************************************************************
/// Nitride Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// SpectrumChart
/// 
/// ***************************************************************************
/// 
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nitride;
using Nitride.Chart;

namespace Nitride.EE
{
    public sealed class SpectrumChart : ChartWidget
    {
        public SpectrumChart(string name, SpectrumData sd) : base(name)
        {
            Data = sd;

            DataUpdateTask = new(() => DataUpdateWorker());
            DataUpdateTask.Start();
        }

        protected override void Dispose(bool disposing) 
        {
            DataUpdateCancellationTokenSource.Cancel();
            base.Dispose(disposing);
        }

        private SpectrumData Data { get; }

        public Area MainArea { get; }
 
        public Area HistoArea { get; }

        public override int RightBlankAreaWidth => 0;

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

        private Task DataUpdateTask { get; }

        private CancellationTokenSource DataUpdateCancellationTokenSource { get; } = new();

        public void DataUpdateWorker() 
        {
            while (true)
            {
                if (DataUpdateCancellationTokenSource.IsCancellationRequested)
                    return;

                if (Data.FrameBuffer.Count > 0 && m_AsyncUpdateUI == false) // && Graphics is not busy!!
                {
                    FreqFrame = Data.FrameBuffer.Dequeue();




                    m_AsyncUpdateUI = true;
                }
                else
                {
                    Thread.Sleep(10);
                }
            }
        }

        public TraceFrame FreqFrame { get; private set; }

        public override void DataIsUpdated(IDataProvider _) => m_AsyncUpdateUI = true;
    }
}
