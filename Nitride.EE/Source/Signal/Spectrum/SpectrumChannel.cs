/// ***************************************************************************
/// Nitride Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2023 Xu Li - me@xuli.us
/// 
/// FFTStream
/// WaveFormReceiver (Buffer -> WaveForm) -> SpectrumFFT (FreqTrace) -> (Frame) SpectrumData -> SpectrumChart
/// 
/// ***************************************************************************

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using Nitride.Chart;

namespace Nitride.EE
{
    public class SpectrumChannel
    {
        public SpectrumChannel(SpectrumControl spc, string channel_name, int poolSize, int maxLength)
        {
            SpectrumControl = spc;
            ChronoChart sc = SpectrumControl.SampleChart;
            SpectrumData sd = SpectrumData = new();

            SpectrumFFT = new(sd, poolSize, maxLength);

            var i_col = Sample_I_Column = new(channel_name + " Real ", " FS");
            var q_col = Sample_Q_Column = new(channel_name + " Imag ", " FS");

            OscillatorArea sa = SampleChartArea = new(sc, channel_name + " Area", 0.3f)
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
                Name = channel_name + " Real",
                LegendName = channel_name + " Real",
                Color = Color.SlateBlue,
                IsAntialiasing = true,
                Tension = 0,
                HasTailTag = false
            });

            sa.AddSeries(new LineSeries(q_col)
            {
                Order = 0,
                Importance = Importance.Minor,
                Name = channel_name + " Imag",
                LegendName = channel_name + " Imag",
                Color = Color.DarkGreen,
                IsAntialiasing = true,
                Tension = 0,
                HasTailTag = false
            });

            sc.AddArea(sa);
            SpectrumChart = new(channel_name + " Spectrum", sd);
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

        public WindowType WindowsType { get; set; } = WindowType.FlatTop;

        public bool EnableHisto { get; set; } = true;

        public bool PersistEnable { get; set; } = true;

        public TraceDetectorType TraceDetectorType
        {
            get => SpectrumData.Detector;
            set => SpectrumData.Detector = value;
        }

        public int TracePoint { get; set; } = 800;

        public double VBW => SpectrumData.FreqStep;

        public int HistoDepth { get; set; } = 128;

        public int PersistDepth { get;  set; } = 32;

        public int PersistHeight { get; set; } = 800;

        #endregion Properties

        #region X Axis

        // Purely for display purpose
        public double CenterFreq { get; set; } = 187.5e6;

        // In general, it should be always smaller than the DSP BW in FFT mode.
        public double FreqSpan { get; set; } = 50e6;

        public double StartFreq => CenterFreq - (FreqSpan / 2);

        public double StopFreq => CenterFreq + (FreqSpan / 2);

        public double RBW { get; private set; }

        #endregion X Axis

        #region Y Axis
        public double Reference { get; set; } = 0;

        public double Range { get; set; } = 130;

        public double TickStep { get; set; } = 10;

        #endregion Y Axis

        #region Configuration

        public double DSP_Gain { get; set; } = 1;

        public void ApplyConfig()
        {
            // SpectrumControl.Pause = true;
            SpectrumData.PauseUpdate = true;

            double dsp_bw = SpectrumControl.Receiver.Bandwidth;
            double center_freq = CenterFreq;
            double dsp_startFreq = center_freq - (dsp_bw / 2);
            double dsp_stopFreq = center_freq + (dsp_bw / 2);

            SpectrumData sd = SpectrumData;
            sd.EnableHisto = EnableHisto;
            sd.ConfigureHistoDepth(HistoDepth, TracePoint, PersistHeight);
            sd.ConfigurePersist(PersistDepth);
            sd.ConfigureLevel(Reference, Range);
            sd.ConfigureFreqRange(CenterFreq, FreqSpan);

            if (SweepMode == SweepMode.FFT)
            {
                SpectrumFFT.Configure(SampleLength, dsp_startFreq, dsp_stopFreq, WindowsType);
                sd.EnablePersist = PersistEnable & EnableHisto;
                double gain = 20 * Math.Log10(SpectrumFFT.Gain * DSP_Gain);
                sd.ConfigureCorrection(-gain, 20);
                RBW = dsp_bw / SpectrumFFT.Length;
            }
            else
            {


                sd.EnablePersist = false;
            }

            SpectrumChart.UpdateConfiguration(TickStep);
            SpectrumChart.ShowAll();

            // SpectrumControl.Pause = false;
            SpectrumData.PauseUpdate = false;
        }

        #endregion Configuration

        // 1. generate sweep plan
        // 2. Send each point to SDR and trigger rx event
        // 3. Pull the data back

    }
}
