using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nitride.EE
{
    public partial class SpectrumControlPanel : UserControl
    {
        public SpectrumControlPanel(SpectrumControl sc)
        {
            SpectrumControl = sc;

            SpectrumChannelPanels = new SpectrumChannelPanel[sc.NumOfCh];

            for (int i = 0; i < sc.NumOfCh; i++)
            {
                SpectrumChannelPanel scf = SpectrumChannelPanels[i] = new SpectrumChannelPanel(sc.SpectrumChannel[i]);
                scf.Text = "Ch" + i + " Spectrum Settings";
                scf.Show();
            }

            InitializeComponent();
            Location = new Point(0, 0);
            Dock = DockStyle.Fill;

            ComboBoxSweepMode.Items.Add<SweepMode>();
            ComboBoxSweepMode.Text = SweepMode.FFT.ToString();

            for (int i = 4096; i <= sc.Receiver.MaxSampleLength; i *= 2)
            {
                ComboBoxSampleLength.Items.Add(i.ToString());
            }

            UpdateControls();
        }

        public SpectrumControl SpectrumControl { get; }

        public SpectrumChannelPanel[] SpectrumChannelPanels { get; }

        public Action ConfigIsUpdated;

        public void UpdateControls()
        {
            SpectrumControl sc = SpectrumControl;
            ComboBoxSweepMode.Text = sc.SweepMode.ToString();
            ComboBoxSampleLength.Text = sc.SampleLength.ToString();
            TextBoxDecmRate.Text = sc.Receiver.DecimationRate.ToString();
            TextBoxSampleRate.Text = sc.Receiver.SampleRate.ToString();
            TextBoxBandwidth.Text = sc.Receiver.Bandwidth.ToString();
            CheckBoxEnableTimeDomain.Checked = sc.EnableTimeDomain;
            TextBoxSampleTime.Text = (sc.Receiver.SampleTime * 1000).ToString("0.###") + " ms";

            //SpectrumControl.ApplyConfig_Spectrum();
            foreach (var ch in SpectrumChannelPanels)
            {
                ch.UpdateControls();
            }
        }

        public void UpdateConfiguration()
        {
            SpectrumControl sc = SpectrumControl;
            sc.SweepMode = ComboBoxSweepMode.Text.ParseEnum<SweepMode>();
            sc.SampleLength = ComboBoxSampleLength.Text.ToInt32();
            sc.Receiver.DecimationRate = TextBoxDecmRate.Text.ToUInt32();
            sc.EnableTimeDomain = CheckBoxEnableTimeDomain.Checked;

            UpdateControls();

            if (ConfigIsUpdated is not null) ConfigIsUpdated();
        }

        private void BtnApplyConfiguration_Click(object sender, EventArgs e)
        {
            UpdateConfiguration();
            SpectrumControl.ApplyConfig();
            UpdateControls();
        }

        private void BtnTrigContinous_Click(object sender, EventArgs e)
        {
            // CheckBoxPause.Checked = false;
            // SpectrumControl.Pause = false;
            SpectrumControl.StartStream();
        }

        private void BtnTrigSingle_Click(object sender, EventArgs e)
        {
            SpectrumControl.GetSingle();
        }

        private void BtnAdcDataTrigStop_Click(object sender, EventArgs e)
        {
            SpectrumControl.StopStream();
        }

        private void CheckBoxPause_CheckedChanged(object sender, EventArgs e)
        {
            SpectrumControl.Pause = CheckBoxPause.Checked;
        }
    }
}
