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
        public SpectrumControl(WaveFormReceiver recv)
        {
            Receiver = recv;
            SampleLength = Receiver.MaxSampleLength;

            SpectrumChannel = new SpectrumChannel[NumOfCh];

            SampleTable = new(Receiver.MaxSampleLength);
            SampleChart = new ChronoChart("Sample Chart", SampleTable)
            {
                IndexCount = 500,
                ReadyToShow = true,
                StopPt = 500, // (fftPts > 1500) ? 1500 : fftPts - 1,
                Location = new Point(0, 0),
                Dock = DockStyle.Fill
            };

            for (int i = 0; i < NumOfCh; i++)
            {
                SpectrumChannel sch = SpectrumChannel[i] = new(this, "SCh" + i.ToString(), Receiver.PoolSize, Receiver.MaxSampleLength);
                sch.ApplyConfig();
            }

            EnableTimeDomain = true;
            Receiver.WaveFormEnqueue += WaveFormEnqueue;
        }

        public WaveFormReceiver Receiver { get; }

        #region SampleChart

        public ChronoTable SampleTable { get; }

        public ChronoChart SampleChart { get; private set; }

        #endregion SampleChart

        #region Spectrum

        public SpectrumChannel[] SpectrumChannel { get; }

        #endregion Spectrum

        #region Properties

        public int NumOfCh => Receiver.NumOfCh;

        public bool EnableTimeDomain { get; set; } = true;

        public int SampleLength { get; set; } = 8192;

        public double SampleRate => Receiver.SampleRate;

        public SweepMode SweepMode { get; set; } = SweepMode.FFT;

        public bool Pause
        {
            get => m_Pause;

            set
            {
                Receiver.IsPause = value;

                for (int i = 0; i < NumOfCh; i++)
                {
                    SpectrumChannel sch = SpectrumChannel[i];
                    sch.SpectrumData.PauseUpdate = value;
                }

                m_Pause = value;
            }
        }

        private bool m_Pause = true;

        // #1 Configure the Sample Length must be done with everything stopped.
        public void ApplyConfig()
        {
            if (Receiver.IsPause)
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
                SampleTable.ConfigureNumberOfPoints(SampleLength);

                ApplyConfig_Receiver();
                ApplyConfig_Spectrum();
            }
            else
            {
                Console.WriteLine("Receiver should be stopped before changing the sample length");
            }
        }

        // #2 Can be configured anytime, must update the spectrum settings.
        public void ApplyConfig_Receiver()
        {
            // Write DDC configuration...
            Receiver.ApplyConfig();
            SampleTable.SampleRate = Receiver.SampleRate;
            // SampleTable.ConfigureNumberOfPoints(SampleLength);
            ApplyConfig_Spectrum();
        }

        // #3 Can be configured anytime
        public void ApplyConfig_Spectrum()
        {
            for (int i = 0; i < NumOfCh; i++)
            {
                SpectrumChannel sch = SpectrumChannel[i];
                sch.DSP_Gain = Receiver.DSP_Gain[i];
                sch.ApplyConfig();
            }
        }

        #endregion Properties

        #region Methods

        public bool GetSingle() => Receiver.TrigSingle();

        public bool StartStream()
        {
            // if ()

            Pause = false;

            if (!Receiver.TrigContinous()) return false;
            /*
            foreach (var sch in SpectrumChannel)
            {
               sch.
            }*/


            return true;
        }

        public bool StopStream()
        {
            Receiver.TrigStop();

            Pause = true;
            return false;
        }

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
                    // wf.IsUpdated = false;  // .
                    sch.SpectrumFFT.WaveFormEnqueue(wf);
                }
                else
                {

                }
            }

            if (EnableTimeDomain)
            {
                ct.DataIsUpdated();
                // Console.WriteLine("ct.DataIsUpdated()");
            }

        }
    }
}
