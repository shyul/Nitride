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
    public partial class LMX2820_Control : UserControl
    {
        public LMX2820_Control(LMX2820 lmx)
        {
            LMX2820 = lmx;
            InitializeComponent();
            UpdateUI();
        }

        public LMX2820 LMX2820 { get; }

        public void Commit()
        {
            var lo = LMX2820;

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
                "1.4 mA" => 0x1,
                "5.6 mA" => 0x4,
                "7 mA" => 0x5,
                "11.2 mA" => 0x6,
                "12.6 mA" => 0x7,
                "2.8 mA" => 0x8,
                "4.2 mA" => 0x9,
                "8.4 mA" => 12,
                "9.8 mA" => 13,
                "14 mA" => 14,
                "15.4 mA" => 15,
                _ => 0x0
            };

            lo.Mash_Order = ComboBoxMashOrder.Text switch
            {
                "1" => 1,
                "2" => 2,
                "3" => 3,
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
                    "VCO7" => 7,
                    _ => 1,
                };
            }

            lo.Ch_Div_A = ComboBoxChADivider.Text switch
            {
                "2" => 0,
                "4" => 1,
                "8" => 2,
                "16" => 3,
                "32" => 4,
                "64" => 5,
                "128" => 6,
                _ => 0
            };

            lo.Ch_Div_B = ComboBoxChBDivider.Text switch
            {
                "2" => 0,
                "4" => 1,
                "8" => 2,
                "16" => 3,
                "32" => 4,
                "64" => 5,
                "128" => 6,
                _ => 0
            };

            lo.RFOutA_Mux = ComboBoxRfoutAMux.Text.Trim() switch
            {
                "Divider" => 0,
                "VCO" => 1,
                "Doubler" => 2,
                _ => 1,
            };

            lo.RFOutB_Mux = ComboBoxRfoutBMux.Text.Trim() switch
            {
                "Divider" => 0,
                "VCO" => 1,
                "Doubler" => 2,
                _ => 1,
            };

            lo.RFOutA_Enable = CheckBoxRfoutAEnable.Checked;
            lo.RFOutB_Enable = CheckBoxRfoutBEnable.Checked;
            lo.RFOutA_Level = TextBoxRfoutAPower.Text.ToUInt32(0);
            lo.RFOutB_Level = TextBoxRfoutBPower.Text.ToUInt32(0);

            lo.INSTCAL_EN = CheckBox_INSTCAL_EN.Checked;
            lo.INSTCAL_DBLR_EN = CheckBox_INSTCAL_DBLR_EN.Checked;
            lo.INSTCAL_SKIP_ACAL = CheckBox_INSTCAL_SKIP_ACAL.Checked;
            lo.DBLR_CAL_EN = CheckBox_DBLR_CAL_EN.Checked;
            lo.FCAL_EN = CheckBox_FCAL_EN.Checked;
            lo.PFD_DLY_MANUAL = CheckBox_PFD_DLY_MANUAL.Checked;

            if (lo.PFD_DLY_MANUAL)
            {
                lo.PFD_DLY = TextBoxPfdDlySel.Text.ToUInt32(2);
            }

            lo.INSTCAL_PLL_NUM = TextBox_INSTCAL_PLL_NUM.Text.ToUInt32(0);
            lo.ACAL_CMP_DLY = TextBox_ACAL_CMP_DLY.Text.ToUInt32(10);

            //lo.INSTCAL_DLY = TextBox_INSTCAL_DLY.Text.ToUInt32(0x1F4);
            lo.QUICK_RECAL_EN = CheckBox_QUICK_RECAL_EN.Checked;

            lo.VCO_CAPCTRL_FORCE = CheckBox_VCO_CAPCTRL_FORCE.Checked;

            if (lo.VCO_CAPCTRL_FORCE)
            {
                lo.VCO_CAPCTRL = TextBox_VCO_CAPCTRL.Text.ToUInt32(0xBF);
            }

            lo.Phase_Sync_Enable = CheckBoxSyncEnable.Checked;
            lo.PSYNC_Pin_Enable = CheckBoxPsyncPin.Checked;

            lo.TempSense_Enable = CheckBoxEnableTemp.Checked;

            if (lo.Reference.Enabled && !double.IsNaN(lo.Reference.Frequency))
            {
                lo.Commit();
                UpdateUI();
            }
        }

        public void UpdateUI()
        {
            var lo = LMX2820;

            if (lo.IsLocked)
            {
                LabelVcoLock.Text = "Locked";
                LabelVcoLock.ForeColor = Color.Green;
            }
            else
            {
                LabelVcoLock.Text = "Unlock";
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
                _ => "INT"
            };
            TextBoxMashSeed.Text = lo.Mash_Seed.ToString();
            TextBoxMashResetCounter.Text = lo.Mash_Reset_Counter.ToString();
            CheckBoxMashSeedEnable.Checked = lo.Mash_Seed_Enable;
            CheckBoxMashResetN.Checked = lo.Mash_Reset_N;

            ComboBoxChargePumpCurrent.Text = lo.CPG switch
            {
                0x0 => "Tri-State",
                0x1 => "1.4 mA",
                0x4 => "5.6 mA",
                0x5 => "7 mA",
                0x6 => "11.2 mA",
                0x7 => "12.6 mA",
                0x8 => "2.8 mA",
                0x9 => "4.2 mA",
                12 => "8.4 mA",
                13 => "9.8 mA",
                14 => "14 mA",
                15 => "15.4 mA",
                _ => "Invalid"
            };

            CheckBoxManualVcoSelect.Checked = lo.VcoSel_Force;

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

            ComboBoxChADivider.Text = lo.Ch_Div_A switch
            {
                0 => "2",
                1 => "4",
                2 => "8",
                3 => "16",
                4 => "32",
                5 => "64",
                6 => "128",
                _ => "XXX"
            };

            ComboBoxChBDivider.Text = lo.Ch_Div_B switch
            {
                0 => "2",
                1 => "4",
                2 => "8",
                3 => "16",
                4 => "32",
                5 => "64",
                6 => "128",
                _ => "XXX"
            };

            ComboBoxRfoutAMux.Text = lo.RFOutA_Mux switch
            {
                0 => "Divider",
                1 => "VCO",
                2 => "Doubler",
                _ => "Invalid"
            };

            ComboBoxRfoutBMux.Text = lo.RFOutB_Mux switch
            {
                0 => "Divider",
                1 => "VCO",
                2 => "Doubler",
                _ => "Invalid"
            };

            CheckBoxRfoutAEnable.Checked = lo.RFOutA_Enable;
            CheckBoxRfoutBEnable.Checked = lo.RFOutB_Enable;
            TextBoxRfoutAPower.Text = lo.RFOutA_Level.ToString();
            TextBoxRfoutBPower.Text = lo.RFOutB_Level.ToString();




            CheckBox_INSTCAL_EN.Checked = lo.INSTCAL_EN;
            CheckBox_INSTCAL_DBLR_EN.Checked = lo.INSTCAL_DBLR_EN;
            CheckBox_INSTCAL_SKIP_ACAL.Checked = lo.INSTCAL_SKIP_ACAL;
            CheckBox_DBLR_CAL_EN.Checked = lo.DBLR_CAL_EN;
            CheckBox_FCAL_EN.Checked = lo.FCAL_EN;
            CheckBox_PFD_DLY_MANUAL.Checked = lo.PFD_DLY_MANUAL;

            if (lo.PFD_DLY_MANUAL)
            {
                TextBoxPfdDlySel.ReadOnly = false;
                TextBoxPfdDlySel.Text = lo.PFD_DLY.ToString();
            }
            else
            {
                TextBoxPfdDlySel.ReadOnly = true;
            }

            TextBox_INSTCAL_DLY.Text = lo.INSTCAL_DLY.ToString();
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

            if (lo.INSTCAL_EN)
            {
                TextBox_INSTCAL_PLL_NUM.Enabled = true;
         
            }
            else
            {
                TextBox_INSTCAL_PLL_NUM.Enabled = false;
                // TextBox_INSTCAL_PLL_NUM.Text =
            }

            TextBox_INSTCAL_PLL_NUM.Text = lo.INSTCAL_PLL_NUM.ToString();


            TextBox_ACAL_CMP_DLY.Text = lo.ACAL_CMP_DLY.ToString();

            TextBox_FCAL_LPFD_ADJ.Text = lo.FCAL_LPFD_ADJ switch
            {
                0 => "Fpd ≥ 10 MHz",
                1 => "10 MHz > Fpd ≥ 5 MHz",
                2 => "5 MHz > Fpd ≥ 2.5 MHz",
                _ => "Fpd < 2.5 MHz"
            };

            TextBox_FCAL_HPFD_ADJ.Text = lo.FCAL_LPFD_ADJ switch
            {
                0 => "Fpd ≤ 100 MHz",
                1 => "100 MHz < Fpd ≤ 150 MHz",
                2 => "150 MHz < Fpd ≤ 200 MHz",
                _ => "Fpd > 200 MHz"
            };

            TextBox_CAL_CLK_DIV.Text = lo.CAL_CLK_DIV switch
            {
                0 => "Fosc ≤ 200 MHz",
                1 => "Fosc ≤ 400 MHz",
                2 => "Fosc ≤ 800 MHz",
                _ => "Other"
            };

            CheckBoxSyncEnable.Checked = lo.Phase_Sync_Enable;
            CheckBoxPsyncPin.Checked = lo.PSYNC_Pin_Enable;


            TextBoxPhaseShift.Text = lo.Phase_Shift.ToString("0.#####") + " °";
            TextBoxRfoutAPhaseShift.Text = lo.Ch_A_Phase.ToString("0.#####") + " °";
            TextBoxRfoutBPhaseShift.Text = lo.Ch_B_Phase.ToString("0.#####") + " °";

            CheckBoxEnableTemp.Checked = lo.TempSense_Enable;
            TextBoxTemperature.Text = lo.TempSense.ToString("0.##") + " °C";
        }

        private void CheckBoxManualVcoSelect_CheckedChanged(object sender, EventArgs e)
        {
            var lo = LMX2820;
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

            if (LMX2820.WriteAll is not null)
                LMX2820.WriteAll();
        }

        private void BtnUpdateUI_Click(object sender, EventArgs e)
        {
            LMX2820.ReadStatus();
            UpdateUI();
        }

        private void BtnLoadTICS_Click(object sender, EventArgs e)
        {
            OpenFileDialog.Filter = "TICS Pro Register Values (*.txt)|*.txt|All Files|*.*";
            if (OpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                string reg_text = File.ReadAllText(OpenFileDialog.FileName);

                LMX2820.LoadTICSFile(reg_text);
                
                for (ushort i = 0; i < LMX2820.Regs.Length; i++)
                {
                    Console.WriteLine("Regs[" + i.ToString() + "] = 0x" + LMX2820.Regs[i].ToString("X") + ";");
                }
            }

            UpdateUI();

            /*
            using StringReader sr = new(reg_text);

            while (sr.ReadLine() is string rline)
            {
                string[] reg_fields = rline.Split('\t');
                uint reg_value = Convert.ToUInt32(reg_fields[1], 16);
                byte addr = Convert.ToByte((reg_value >> 16) & 0xFF);
                ushort data = Convert.ToUInt16(reg_value & 0xFFFF);
                Regs[addr] = data;
            }

            for (int i = 0; i < Regs.Length; i++)
            {
                Console.WriteLine(i.ToString("X") + " = " + Regs[i].ToString("X"));
            }*/
        }

        private void CheckBox_INSTCAL_EN_CheckedChanged(object sender, EventArgs e)
        {
            var lo = LMX2820;
            if (CheckBox_INSTCAL_EN.Checked)
            {
                lo.F_Num = TextBoxFNum.Text.ToUInt32(0);
                lo.F_Den = TextBoxFDen.Text.ToUInt32(1);
                LMX2820.INSTCAL_PLL_NUM = Convert.ToUInt32(Math.Pow(2, 32) * lo.F_Num / lo.F_Den);
                TextBox_INSTCAL_PLL_NUM.Text = LMX2820.INSTCAL_PLL_NUM.ToString();
                TextBox_INSTCAL_PLL_NUM.Enabled = true;
                lo.INSTCAL_EN = true;
            }
            else
            {
                TextBox_INSTCAL_PLL_NUM.Enabled = false;
                lo.INSTCAL_EN = false;
            }
        }

        private void CheckBox_PFD_DLY_MANUAL_CheckedChanged(object sender, EventArgs e)
        {
            var lo = LMX2820;
            if (CheckBox_PFD_DLY_MANUAL.Checked)
            {
                TextBoxPfdDlySel.Text = LMX2820.PFD_DLY.ToString();
                TextBoxPfdDlySel.ReadOnly = false;
                lo.PFD_DLY_MANUAL = true;
            }
            else
            {
                TextBoxPfdDlySel.ReadOnly = true;
                lo.PFD_DLY_MANUAL = false;
            }
        }

        private void CheckBox_VCO_CAPCTRL_FORCE_CheckedChanged(object sender, EventArgs e)
        {
            var lo = LMX2820;
            if (CheckBox_VCO_CAPCTRL_FORCE.Checked)
            {
                TextBox_VCO_CAPCTRL.Text = LMX2820.VCO_CAPCTRL.ToString();
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
            LMX2820.VcoCalibration();
        }

        private void ButtonInstantCalibration_Click(object sender, EventArgs e)
        {
            Commit();
            LMX2820.InstantCalibration();
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            LMX2820.Reset = true; // Self Clearing
            LMX2820.BulkWrite(new ushort[] { 0 }, LMX2820.Regs);
        }

        private void BtnPowerDown_Click(object sender, EventArgs e)
        {
            if (LMX2820.PowerDown) 
            {
                LMX2820.PowerDown = false;
                BtnPowerDown.Text = "Power Down";
            }
            else
            {
                LMX2820.PowerDown = true;
                BtnPowerDown.Text = "Power Up";
            }

            LMX2820.BulkWrite(new ushort[] { 0 }, LMX2820.Regs);
        }
    }
}
