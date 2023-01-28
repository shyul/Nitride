/// ***************************************************************************
/// Nitride Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2023 Xu Li - me@xuli.us
/// 
/// FFTStream
/// WaveFormReceiver -> WaveForm -> (FreqTrace) SpectrumFFT -> (Frame) SpectrumData -> SpectrumChart
/// 
/// ***************************************************************************

using Nitride.Chart;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace Nitride.EE
{
    // 1. generate sweep plan
    // 2. Send each point to SDR and trigger rx event
    // 3. Pull the data back

    public class SpectrumChannel
    {
        public SpectrumChannel(SpectrumControl spc, int i, int poolSize, int maxLength)
        {
            SpectrumControl = spc;

            ChronoChart sc = SpectrumControl.SampleChart;

            SpectrumData sd = SpectrumData = new();
            SpectrumFFT = new(sd, poolSize, maxLength);
            var i_col = Sample_I_Column = new("Ch" + i + " Real ", "FS");
            var q_col = Sample_Q_Column = new("Ch" + i + " Imag ", "FS");

            OscillatorArea sa = SampleChartArea = new(sc, "Ch" + i + " Area", 0.3f)
            {
                HasXAxisBar = true,
                Reference = 0,
                //UpperLimit = 1717986918,
                //LowerLimit = -1717986918,
                //UpperColor = Color.Green,
                //LowerColor = Color.DarkOrange,
                //FixedTickStep_Right = 10,
            };

            sa.AddSeries(new LineSeries(i_col)
            {
                Order = 0,
                Importance = Importance.Minor,
                Name = "Ch" + i + " Real",
                LegendName = "Ch" + i + " Real",
                Color = Color.SlateBlue,
                IsAntialiasing = true,
                Tension = 0,
                HasTailTag = false
            });

            sa.AddSeries(new LineSeries(q_col)
            {
                Order = 0,
                Importance = Importance.Minor,
                Name = "Ch" + i + " Imag",
                LegendName = "Ch" + i + " Imag",
                Color = Color.DarkGreen,
                IsAntialiasing = true,
                Tension = 0,
                HasTailTag = false
            });

            sc.AddArea(sa);
            SpectrumChart = new("Ch" + i + " Spectrum", sd);
        }

        private SpectrumControl SpectrumControl { get; }



        public NumericColumn Sample_I_Column { get; }

        public NumericColumn Sample_Q_Column { get; }

        public OscillatorArea SampleChartArea { get; }

        public SpectrumFFT SpectrumFFT { get; } // Has Task

        public SpectrumData SpectrumData { get; } // Has Task

        public SpectrumChart SpectrumChart { get; } // Has Task

        #region Properties

        public bool Enabled { get; set; } = true;

        public int Length => SpectrumControl.Length;

        public SweepMode SweepMode => SpectrumControl.SweepMode;

        public TraceDetectorType TraceDetectorType { get; set; } = TraceDetectorType.Peak;

        public double CenterFreq { get; set; } = 0; // 187.5e6;

        public double FreqSpan { get; set; }

        public double StartFreq => CenterFreq - (FreqSpan / 2);

        public double StopFreq => CenterFreq + (FreqSpan / 2);

        // public double 

        public WindowsType WindowsType { get; set; } = WindowsType.FlatTop;

        // private LocalOscillator LocalOscillator { get; }



        public void Configure()
        {
            // Fetch Desirable DSP_BW from SpectrumControl

            double dsp_bw = 0; // Receiver.Bandwidth; // ***********

            SpectrumData sd = SpectrumData;
            sd.Detector = TraceDetectorType;

            double center_freq = CenterFreq; // ***********
            double fft_startFreq = center_freq - (dsp_bw / 2);
            double fft_stopFreq = center_freq + (dsp_bw / 2);

            if (SweepMode == SweepMode.FFT)
            {
                SpectrumFFT.Configure(Length, fft_startFreq, fft_stopFreq, WindowsType);

            }
            else
            {

                sd.PersistEnable = false;

            }


        }

        #endregion Properties
    }
}
