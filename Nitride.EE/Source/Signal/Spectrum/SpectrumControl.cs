/// ***************************************************************************
/// Nitride Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2023 Xu Li - me@xuli.us
/// 
/// Spectrum
/// BufferData -> WaveForm -> (FreqTrace) SpectrumFFT -> (Frame) SpectrumData -> SpectrumChart
/// 
/// 1. Contains the components for a SpecAn
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Net.Security;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nitride.Chart;

namespace Nitride.EE
{
    public class SpectrumControl
    {
        public SpectrumControl(IReceiver recv, int poolSize)
        {
            Receiver = recv;
            SampleLength = Receiver.MaxSampleLength;

            for (int i = 0; i < poolSize; i++)
            {
                FetchWaveFormPool.Add(new WaveFormGroup(NumOfCh, SampleLength));
            }

            FetchWaveFormPool.ForEach(n => n.HasUpdatedItem = false);

            SampleTable = new(SampleLength);
            SampleChart = new ChronoChart("Sample Chart", SampleTable)
            {
                IndexCount = 500,
                ReadyToShow = true,
                StopPt = 500, // (fftPts > 1500) ? 1500 : fftPts - 1,
                Location = new Point(0, 0),
                Dock = DockStyle.Fill
            };

            SpectrumChannel = new SpectrumChannel[NumOfCh];
            for (int i = 0; i < NumOfCh; i++)
            {
                SpectrumChannel sch = SpectrumChannel[i] = new(this, "SCh" + i.ToString(), PoolSize, Receiver.MaxSampleLength);
                sch.ApplyConfig();
            }

            EnableTimeDomain = true;
            // Receiver.WaveFormEnqueue += WaveFormEnqueue;
        }

        ~SpectrumControl() 
        {
            FetchWaveFormPool.ForEach(n => n.HasUpdatedItem = false);
        }

        public IReceiver Receiver { get; }

        public int NumOfCh => Receiver.NumOfCh;

        public SweepMode SweepMode { get; set; } = SweepMode.FFT;

        #region SampleChart

        public bool EnableTimeDomain { get; set; } = true;

        public int SampleLength { get => Receiver.SampleLength; set => Receiver.SampleLength = value; }

        public double SampleRate => Receiver.SampleRate;

        public ChronoTable SampleTable { get; }

        public ChronoChart SampleChart { get; private set; }

        #endregion SampleChart

        #region Spectrum

        public SpectrumChannel[] SpectrumChannel { get; }

        #endregion Spectrum

        #region Properties

        private List<WaveFormGroup> FetchWaveFormPool { get; } = new();

        public int PoolSize => FetchWaveFormPool.Count;

        public WaveFormGroup CurrentFetchWaveFormGroup => FetchWaveFormPool.Where(n => !n.HasUpdatedItem).FirstOrDefault();

        public bool Pause
        {
            get => Receiver.IsFetchPause;

            set
            {
                bool setPause = true;

                if (value)
                {
                    Receiver.PauseFetch();
                }
                else if (Receiver.IsFetchRunning)
                {
                    Receiver.ResumeFetch();
                    setPause = false;
                }

                for (int i = 0; i < NumOfCh; i++)
                {
                    SpectrumChannel sch = SpectrumChannel[i];
                    sch.SpectrumData.PauseUpdate = setPause;
                }
            }
        }

        // #1 Configure the Sample Length must be done with everything stopped.
        public void ApplyConfig()
        {
            if (Receiver.IsFetchPaused)
            {
                // Set the WaveFormGroup
                if (SampleLength <= Receiver.MaxSampleLength && SampleLength.IsPowerOf2())
                {
                    Receiver.SampleLength = SampleLength;
                }
                else
                {
                    throw new Exception("Invalid Sample Length: " + SampleLength);
                }

                FetchWaveFormPool.ForEach(n => n.Configure(SampleLength, SampleRate, 0));

                Receiver.ApplyReceiverConfig();

                SampleTable.ConfigureNumberOfPoints(SampleLength);
                SampleTable.SampleRate = Receiver.SampleRate;

                ApplyConfig_Spectrum();
            }
            else
            {
                Console.WriteLine("Receiver should be stopped before changing the sample length");
            }
        }

        // #3 Can be configured anytime
        public void ApplyConfig_Spectrum()
        {
            for (int i = 0; i < NumOfCh; i++)
            {
                SpectrumChannel sch = SpectrumChannel[i];
                // sch.DSP_Gain = Receiver.DSP_Gain[i];
                sch.ApplyConfig();
            }
        }

        #endregion Properties

        #region Methods

        public void RunSingleFetch() => Receiver.RunSingleFetch();

        public bool RunFetch() => Receiver.RunFetch();

        public void StopFetch() => Receiver.StopFetch();

        #endregion Methods

        /// <summary>
        /// Called by HandleCopy() in the WaveFormReceiver
        /// </summary>
        /// <param name="wfg">Incoming WaveFormGroup</param>
        public void WaveFormEnqueue(WaveFormGroup wfg)
        {
            // Console.WriteLine("WaveFormEnqueue");
            ChronoTable ct = SampleTable;

            for (int i = 0; i < NumOfCh; i++)
            {
                WaveForm wf = wfg.WaveForms[i];
                SpectrumChannel sch = SpectrumChannel[i];
                // double dsp_gain = sch.DSP_Gain;

                if (!sch.Enabled) // If the channel is disabled
                {
                    wf.IsUpdated = false; // Clear the new data flag, so the WFG can be recycled.
                    continue; // Skip to the next channel.
                }

                if (EnableTimeDomain)
                {
                    for (int j = 0; j < ct.Count; j++)
                    {
                        Complex c = wf.Data[j];
                        ChronoRow cr = ct[j];
                        (cr[sch.Sample_I_Column], cr[sch.Sample_Q_Column]) = (c.Real, c.Imaginary);

                        // Console.WriteLine(c.ToString());
                    }
                }

                if (SweepMode == SweepMode.FFT)
                {
                    sch.SpectrumFFT.WaveFormEnqueue(wf);
                }
                else
                {

                }
            }

            if (EnableTimeDomain)
            {
                ct.DataIsUpdated();
            }

        }
    }
}
