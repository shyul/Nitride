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
    public partial class LMX2572_Control : UserControl
    {
        public LMX2572_Control(LMX2572 lmx)
        {
            LMX2572 = lmx;
            InitializeComponent();
            UpdateUI();
        }

        public LMX2572 LMX2572 { get; }

        public void Commit() 
        {
            var lo = LMX2572;

            lo.EnableRefDoubler = CheckBoxRefDoublerEnable.Checked;
            lo.PreR = TextBoxPreR.Text.ToUInt32(1);

            lo.ReferenceMulti = TextBoxRMulti.Text.Trim() switch
            {
                "3" => 3,
                "4" => 4,
                "5" => 5,
                "6" => 6,
                "7" => 7,
                _ => 1,
            };

            lo.R_Div = TextBoxRDiv.Text.ToUInt32(1);
            lo.N_Div = TextBoxNDiv.Text.ToUInt32(1);
            lo.F_Num = TextBoxFNum.Text.ToUInt32(0);
            lo.F_Den = TextBoxFDen.Text.ToUInt32(1);

            lo.CPG = ComboBoxChargePumpCurrent.Text.Trim() switch
            {
                "0.625 mA" => 1,
                "1.25 mA" => 2,
                "1.875 mA" => 3,
                "2.5 mA" => 4,
                "3.125 mA" => 5,
                "3.75 mA" => 6,
                "4.375 mA" => 7,
                "5 mA" => 12,
                "5.625 mA" => 13,
                "6.25 mA" => 14,
                "6.875 mA" => 15,
                _ => 0 // Tri-state
            };

            lo.Mash_Order = ComboBoxMashOrder.Text switch
            {
                "1" => 1,
                "2" => 2,
                "3" => 3,
                "4" => 4,
                _ => 0,
            };

            lo.Mash_Seed = TextBoxMashSeed.Text.ToUInt32(0);
            lo.Mash_Reset_Counter = TextBoxMashResetCounter.Text.ToUInt32(50000);
            lo.Mash_Seed_Enable = CheckBoxMashSeedEnable.Checked;
            lo.Mash_Reset_N = CheckBoxMashResetN.Checked;

            lo.VcoSel_Force = CheckBoxManualVcoSelect.Checked;

            if (lo.VcoSel_Force)
            {
                lo.VcoSel = ComboBoxVcoSelect.Text switch
                {
                    "VCO1" => 1,
                    "VCO2" => 2,
                    "VCO3" => 3,
                    "VCO4" => 4,
                    "VCO5" => 5,
                    "VCO6" => 6,
                    _ => 1,
                };
            }

            lo.Ch_Div = ComboBoxChDivider.Text switch
            {
                "2" => 0,
                "4" => 1,
                "8" => 3,
                "16" => 5,
                "32" => 7,
                "64" => 9,
                "128" => 12,
                "256" => 14,
                _ => 0
            };

            lo.RFOutA_Mux = ComboBoxRfoutAMux.Text switch
            {
                "Divider" => 0,
                "VCO" => 1,
                _ => 3,
            };

            lo.RFOutB_Mux = ComboBoxRfoutBMux.Text switch
            {
                "Divider" => 0,
                "VCO" => 1,
                "SysRef" => 2,
                _ => 3,
            };

            lo.RFOutA_Enable = CheckBoxRfoutAEnable.Checked;
            lo.RFOutB_Enable = CheckBoxRfoutBEnable.Checked;
            lo.RFOutA_Level = TextBoxRfoutAPower.Text.ToUInt32(0);
            lo.RFOutB_Level = TextBoxRfoutBPower.Text.ToUInt32(0);

            lo.FCAL_EN = CheckBox_FCAL_EN.Checked;
            lo.QUICK_RECAL_EN = CheckBox_QUICK_RECAL_EN.Checked;

            lo.VCO_CAPCTRL_FORCE = CheckBox_VCO_CAPCTRL_FORCE.Checked;

            if (lo.VCO_CAPCTRL_FORCE)
            {
                lo.VCO_CAPCTRL = TextBox_VCO_CAPCTRL.Text.ToUInt32(0xBF);
            }

            lo.Phase_Sync_Enable = CheckBoxSyncEnable.Checked;
            lo.PSYNC_Pin_Enable = CheckBoxPsyncPin.Checked;

            if (lo.Reference.Enabled && !double.IsNaN(lo.Reference.Frequency))
            {
                lo.Commit();
                UpdateUI();
            }
        }

        public void UpdateUI()
        {
            var lo = LMX2572;

            if (lo.IsLocked)
            {
                LabelVcoLock.Text = "Locked";
                LabelVcoLock.ForeColor = Color.Green;
            }
            else
            {
                LabelVcoLock.Text = "Unlocked";
                LabelVcoLock.ForeColor = Color.Red;
            }

            if (lo.PowerDown)
            {
                BtnPowerDown.Text = "Power Up";
            }
            else
            {
                BtnPowerDown.Text = "Power Down";
            }

            // LabelVcoLock.Text = lo.IsLocked ? "Locked" : "Unlock";
            // LabelVcoLock.ForeColor = lo.IsLocked ? Color.Green : Color.Red;

            var (ref_num, ref_pre) = lo.Reference.Frequency.ToUnitPrefixNumberString();
            TextBoxReferenceFreq.Text = (lo.Reference.Frequency * ref_num) + " " + ref_pre + "Hz";

            CheckBoxRefDoublerEnable.Checked = lo.EnableRefDoubler;
            TextBoxPreR.Text = lo.PreR.ToString();
            TextBoxRMulti.Text = lo.ReferenceMulti.ToString();
            TextBoxRDiv.Text = lo.R_Div.ToString();
            TextBoxNDiv.Text = lo.N_Div.ToString();
            TextBoxFNum.Text = lo.F_Num.ToString();
            TextBoxFDen.Text = lo.F_Den.ToString();

            // TextBoxPfdFreq.Text = lo.PhaseDetectFreqency.ToString();
            var (pdf_num, pdf_pre) = lo.PhaseDetectFreqency.ToUnitPrefixNumberString();
            TextBoxPfdFreq.Text = (lo.PhaseDetectFreqency * pdf_num) + " " + pdf_pre + "Hz";

            // TextBoxVcoFreq.Text = lo.Frequency.ToString();
            var (vco_num, vco_pre) = lo.VcoFrequency.ToUnitPrefixNumberString();
            TextBoxVcoFreq.Text = (lo.VcoFrequency * vco_num) + " " + vco_pre + "Hz";


            var (cha_num, cha_pre) = lo.Ch_A_Frequency.ToUnitPrefixNumberString();
            TextBoxRfoutAFreq.Text = (lo.Ch_A_Frequency * cha_num) + " " + cha_pre + "Hz";

            var (chb_num, chb_pre) = lo.Ch_B_Frequency.ToUnitPrefixNumberString();
            TextBoxRfoutBFreq.Text = (lo.Ch_B_Frequency * chb_num) + " " + chb_pre + "Hz";

            ComboBoxMashOrder.Text = lo.Mash_Order switch
            {
                1 => "1",
                2 => "2",
                3 => "3",
                4 => "4",
                _ => "INT"
            };
            TextBoxMashSeed.Text = lo.Mash_Seed.ToString();
            TextBoxMashResetCounter.Text = lo.Mash_Reset_Counter.ToString();
            CheckBoxMashSeedEnable.Checked = lo.Mash_Seed_Enable;
            CheckBoxMashResetN.Checked = lo.Mash_Reset_N;

            ComboBoxChargePumpCurrent.Text = lo.CPG switch
            {
                0 => "Tri-State",
                1 => "0.625 mA",
                2 => "1.25 mA",
                3 => "1.875 mA",
                4 => "2.5 mA",
                5 => "3.125 mA",
                6 => "3.75 mA",
                7 => "4.375 mA",
                8 => "2.5 mA",
                9 => "3.125 mA",
                10 => "3.75 mA",
                11 => "4.375 mA",
                12 => "5 mA",
                13 => "5.625 mA",
                14 => "6.25 mA",
                15 => "6.875 mA",
                _ => "Invalid"
            };

            CheckBoxManualVcoSelect.Checked = lo.VcoSel_Force;

            if (lo.VcoSel_Force)
            {
                ComboBoxVcoSelect.Enabled = true;
                ComboBoxVcoSelect.Text = lo.VcoSel switch
                {
                    1 => "VCO1",
                    2 => "VCO2",
                    3 => "VCO3",
                    4 => "VCO4",
                    5 => "VCO5",
                    6 => "VCO6",
                    _ => "XXX"
                };
            }
            else
            {
                ComboBoxVcoSelect.Enabled = false;
                ComboBoxVcoSelect.Text = "VCO" + lo.ActualVcoSel.ToString();
            }

            ComboBoxChDivider.Text = lo.Ch_Div switch
            {
                0 => "2",
                1 => "4",
                3 => "8",
                5 => "16",
                7 => "32",
                9 => "64",
                12 => "128",
                14 => "256",
                _ => "XXX"
            };

            ComboBoxRfoutAMux.Text = lo.RFOutA_Mux switch
            {
                0 => "Divider",
                1 => "VCO",
                2 => "Not-Used",
                3 => "High-Z",
                _ => "High-Z"
            };

            ComboBoxRfoutBMux.Text = lo.RFOutB_Mux switch
            {
                0 => "Divider",
                1 => "VCO",
                2 => "SysRef",
                3 => "High-Z",
                _ => "High-Z"
            };

            CheckBoxRfoutAEnable.Checked = lo.RFOutA_Enable;
            CheckBoxRfoutBEnable.Checked = lo.RFOutB_Enable;
            TextBoxRfoutAPower.Text = lo.RFOutA_Level.ToString();
            TextBoxRfoutBPower.Text = lo.RFOutB_Level.ToString();


            TextBoxPfdDlySel.Text = lo.PFD_DLY_SEL.ToString();
            CheckBox_FCAL_EN.Checked = lo.FCAL_EN;

            CheckBox_QUICK_RECAL_EN.Checked = lo.QUICK_RECAL_EN;

            //TextBox_VCO_CAPCTRL.Text = "0x" + lo.VCO_CAPCTRL.ToString("X");
            CheckBox_VCO_CAPCTRL_FORCE.Checked = lo.VCO_CAPCTRL_FORCE;

            if (lo.VCO_CAPCTRL_FORCE)
            {
                TextBox_VCO_CAPCTRL.Text = lo.VCO_CAPCTRL.ToString();
                TextBox_VCO_CAPCTRL.ReadOnly = false;
            }
            else
            {
                TextBox_VCO_CAPCTRL.Text = lo.VCO_CAPCTRL_RD.ToString();
                TextBox_VCO_CAPCTRL.ReadOnly = true;
            }

            TextBox_FCAL_LPFD_ADJ.Text = lo.FCAL_LPFD_ADJ switch
            {
                0 => "Fpd ≥ 10 MHz",
                1 => "10 MHz > Fpd ≥ 5 MHz",
                2 => "5 MHz > Fpd ≥ 2.5 MHz",
                _ => "Fpd < 2.5 MHz"
            };

            TextBox_FCAL_HPFD_ADJ.Text = lo.FCAL_LPFD_ADJ switch
            {
                0 => "Fpd ≤ 37.5 MHz",
                1 => "37.5 MHz < Fpd ≤ 75 MHz",
                2 => "75 MHz < Fpd ≤ 100 MHz",
                _ => "Fpd > 100 MHz"
            };

            TextBox_CAL_CLK_DIV.Text = lo.CAL_CLK_DIV switch
            {
                0 => "Fosc ≤ 200 MHz",
                1 => "Fosc > 200 MHz",
                _ => "Other"
            };

            CheckBoxSyncEnable.Checked = lo.Phase_Sync_Enable;
            CheckBoxPsyncPin.Checked = lo.PSYNC_Pin_Enable;

            TextBoxPhaseShift.Text = lo.Phase_Shift.ToString("0.#####") + " °";
            TextBoxRfoutAPhaseShift.Text = lo.Ch_A_Phase.ToString("0.#####") + " °";
            TextBoxRfoutBPhaseShift.Text = lo.Ch_B_Phase.ToString("0.#####") + " °";
        }

        private void CheckBoxManualVcoSelect_CheckedChanged(object sender, EventArgs e)
        {
            var lo = LMX2572;
            lo.VcoSel_Force = CheckBoxManualVcoSelect.Checked;

            if (lo.VcoSel_Force)
            {
                ComboBoxVcoSelect.Enabled = true;
                ComboBoxVcoSelect.Text = "VCO" + lo.VcoSel.ToString();
            }
            else
            {
                ComboBoxVcoSelect.Enabled = false;
                ComboBoxVcoSelect.Text = "VCO" + lo.ActualVcoSel.ToString();
            }
        }

        private void BtnCommit_Click(object sender, EventArgs e)
        {
            Commit();

            if (LMX2572.WriteAll is not null)
                LMX2572.WriteAll();
        }

        private void BtnUpdateUI_Click(object sender, EventArgs e)
        {
            LMX2572.ReadStatus();
            UpdateUI();
        }

        private void BtnLoadTICS_Click(object sender, EventArgs e)
        {
            OpenFileDialog.Filter = "TICS Pro Register Values (*.txt)|*.txt|All Files|*.*";
            if (OpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                var lo = LMX2572;
                string reg_text = File.ReadAllText(OpenFileDialog.FileName);

                lo.LoadTICSFile(reg_text);

                for (ushort i = 0; i < lo.Regs.Length; i++)
                {
                    Console.WriteLine("Regs[" + i.ToString() + "] = 0x" + lo.Regs[i].ToString("X") + ";");
                }
            }

            UpdateUI();
        }

        private void CheckBox_VCO_CAPCTRL_FORCE_CheckedChanged(object sender, EventArgs e)
        {
            var lo = LMX2572;
            if (CheckBox_VCO_CAPCTRL_FORCE.Checked)
            {
                TextBox_VCO_CAPCTRL.Text = lo.VCO_CAPCTRL.ToString();
                TextBox_VCO_CAPCTRL.ReadOnly = false;
                lo.VCO_CAPCTRL_FORCE = true;
            }
            else
            {
                TextBox_VCO_CAPCTRL.ReadOnly = true;
                lo.VCO_CAPCTRL_FORCE = false;
            }
        }

        private void BtnCalVco_Click(object sender, EventArgs e)
        {
            Commit();
            LMX2572.VcoCalibration();
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            LMX2572.Reset = true; // Self Clearing
            LMX2572.BulkWrite(new ushort[] { 0 }, LMX2572.Regs);
        }

        private void BtnPowerDown_Click(object sender, EventArgs e)
        {
            if (LMX2572.PowerDown)
            {
                LMX2572.PowerDown = false;
                BtnPowerDown.Text = "Power Down";
            }
            else
            {
                LMX2572.PowerDown = true;
                BtnPowerDown.Text = "Power Up";
            }

            LMX2572.BulkWrite(new ushort[] { 0 }, LMX2572.Regs);
        }
    }
}
