/// ***************************************************************************
/// Nitride Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2023 Xu Li - me@xuli.us
/// 
/// FFTStream
/// WaveFormReceiver (Buffer -> WaveForm) -> SpectrumFFT (FreqTrace) -> (Frame) SpectrumData -> SpectrumChart
/// 
/// ***************************************************************************

using Nitride.Business;
using Nitride.Chart;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace Nitride.EE
{
    public class SpectrumChannel
    {
        public SpectrumChannel(SpectrumControl spc, int i, int poolSize, int maxLength)
        {
            SpectrumControl = spc;
            ChronoChart sc = SpectrumControl.SampleChart;
            SpectrumData sd = SpectrumData = new();
            sd.ConfigureDepth(128, 32, 800); // HistoDepth, Persist Depth, Vertical Bins


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

        public SpectrumFFT SpectrumFFT { get; } // Has Task // Configured

        public SpectrumData SpectrumData { get; } // Has Task. // Configured

        public SpectrumChart SpectrumChart { get; } // Has Task

        #region Properties

        public bool Enabled { get; set; } = true;

        public int SampleLength => SpectrumControl.SampleLength;

        public SweepMode SweepMode => SpectrumControl.SweepMode;

        public WindowsType WindowsType { get; set; } = WindowsType.FlatTop;

        public bool PersistEnable { get; set; } = true;

        public bool EnableHisto { get; set; } = true;

        public TraceDetectorType TraceDetectorType
        {
            get => SpectrumData.Detector;
            set => SpectrumData.Detector = value;
        }

        public int TracePoint { get; set; } = 800;

        #endregion Properties


        #region X Axis

        public double CenterFreq { get; set; } = 187.5e6;

        // In general, it should be always smaller than the DSP BW in FFT mode.
        public double FreqSpan { get; set; } = 100e6;

        public double StartFreq => CenterFreq - (FreqSpan / 2);

        public double StopFreq => CenterFreq + (FreqSpan / 2);

        public double VBW => SpectrumData.FreqStep;

        public double RBW { get; private set; }


        #endregion X Axis

        #region Y Axis
        public double Reference { get; set; } = 0;

        public double Range { get; set; } = 130;

        public double TickStep { get; set; } = 10;

        #endregion Y Axis

        #region Configuration

        public double DSP_Gain { get; set; } = 1;

        public void Configure()
        {
            SpectrumControl.Pause = true;

            double dsp_bw = SpectrumControl.Receiver.Bandwidth; // ***********

            SpectrumData sd = SpectrumData;
            sd.EnableHisto = EnableHisto;
            sd.ConfigureLevel(Reference, Range);
            sd.ConfigureFreqRange(CenterFreq, FreqSpan, TracePoint);

            double center_freq = CenterFreq; // ***********
            double fft_startFreq = center_freq - (dsp_bw / 2);
            double fft_stopFreq = center_freq + (dsp_bw / 2);

            if (SweepMode == SweepMode.FFT)
            {
                sd.PersistEnable = PersistEnable;
                double gain = 20 * Math.Log10(SpectrumFFT.Gain * DSP_Gain);
                sd.ConfigureCorrection(-gain, 20);
                SpectrumFFT.Configure(SampleLength, fft_startFreq, fft_stopFreq, WindowsType);
                RBW = dsp_bw / SpectrumFFT.Length;
            }
            else
            {

                sd.PersistEnable = false;

            }

            SpectrumChart.UpdateConfiguration(TickStep);
            SpectrumChart.ShowAll();

            SpectrumControl.Pause = false;
        }

        #endregion Configuration

        // 1. generate sweep plan
        // 2. Send each point to SDR and trigger rx event
        // 3. Pull the data back

    }
}
