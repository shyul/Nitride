using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nitride.EE
{ 
    public partial class SpectrumChannelForm : Form
    {
        public SpectrumChannelForm(SpectrumChannel sch)
        {
            SpectrumChannel = sch;
            InitializeComponent();

            ComboBoxFftWindow.Items.Add<WindowsType>();
            ComboBoxFftWindow.SelectedIndex = 9;
            ComboBoxDetectorType.Items.Add<TraceDetectorType>();
            ComboBoxDetectorType.SelectedIndex = 0;

            UpdateControls();
        }

        SpectrumChannel SpectrumChannel { get; }

        public void UpdateControls()
        {
            SpectrumChannel sch = SpectrumChannel;

            TextBoxCenterFreq.Text = sch.CenterFreq.ToString();
            TextBoxFreqSpan.Text = sch.FreqSpan.ToString();
            TextBoxStartFreq.Text = sch.StartFreq.ToString();
            TextBoxStopFreq.Text = sch.StopFreq.ToString();
            TextBoxRBW.Text = sch.RBW.ToString();
            TextBoxVBW.Text = sch.VBW.ToString();

            TextBoxReferenceLevel.Text = sch.Reference.ToString();
            TextBoxRange.Text = sch.Range.ToString();
            TextBoxTickStep.Text = sch.TickStep.ToString();

            CheckBoxEnable.Checked = sch.Enabled;
            CheckBoxEnableHisto.Checked = sch.EnableHisto;
            CheckBoxEnablePersist.Checked = sch.PersistEnable;

            TextBoxSweepType.Text = sch.SweepMode.ToString();
            ComboBoxFftWindow.Text = sch.WindowsType.ToString();
            ComboBoxDetectorType.Text = sch.TraceDetectorType.ToString();
            TextBoxTracePoints.Text = sch.TracePoint.ToString();

            TextBoxDspGain.Text = sch.DSP_Gain.ToString();
            TextBoxFftGain.Text = sch.SpectrumFFT.Gain.ToString();
        }

        public void UpdateConfiguration()
        {
            SpectrumChannel sch = SpectrumChannel;
            sch.CenterFreq = TextBoxCenterFreq.Text.ToDouble();
            sch.FreqSpan = TextBoxFreqSpan.Text.ToDouble();
            sch.Reference = TextBoxReferenceLevel.Text.ToDouble();
            sch.Range = TextBoxRange.Text.ToDouble();
            sch.TickStep = TextBoxTickStep.Text.ToDouble();

            sch.Enabled = CheckBoxEnable.Checked;
            sch.EnableHisto = CheckBoxEnableHisto.Checked;
            sch.PersistEnable = CheckBoxEnablePersist.Checked;

            sch.WindowsType = ComboBoxFftWindow.Text.ParseEnum<WindowsType>();
            sch.TraceDetectorType = ComboBoxDetectorType.Text.ParseEnum<TraceDetectorType>();

            sch.TracePoint = TextBoxTracePoints.Text.ToInt32();

            UpdateControls();
        }

        private void BtnApplyConfiguration_Click(object sender, EventArgs e)
        {
            UpdateConfiguration();
            SpectrumChannel.ApplyConfig();
        }

        private void CheckBoxPause_CheckedChanged(object sender, EventArgs e)
        {
            SpectrumChannel.SpectrumData.PauseUpdate = CheckBoxPause.Checked;
        }
    }
}
