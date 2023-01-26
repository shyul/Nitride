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
            lo.ReferenceMulti = TextBoxRMulti.Text.ToUInt32(1);
            lo.R_Div = TextBoxRDiv.Text.ToUInt32(1);
            lo.N_Div = TextBoxNDiv.Text.ToUInt32(1);
            lo.F_Num = TextBoxFNum.Text.ToUInt32(0);
            lo.F_Den = TextBoxFDen.Text.ToUInt32(1);

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

            if (ComboBoxVcoSelect.Text.ToUpper().Contains("AUTO"))
            {
                lo.VcoSel_Force = false;
            }
            else
            {
                lo.VcoSel_Force = true;
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

            if (lo.Reference.Enabled && !double.IsNaN(lo.Reference.Frequency))
            {
                lo.EnableRefMultiH = (lo.RefMultiplyOut > 100e6);
                // lo.PFD_DLY_SEL = 

                TextBoxVcoFreq.Text = lo.Frequency.ToString();
                TextBoxPfdFreq.Text = lo.PhaseDetectFreqency.ToString();

                UpdateUI();
            }

        }

        public void UpdateUI()
        {
            var lo = LMX2572;
            CheckBoxRefDoublerEnable.Checked = lo.EnableRefDoubler;
            TextBoxPreR.Text = lo.PreR.ToString();
            TextBoxRMulti.Text = lo.ReferenceMulti.ToString();
            TextBoxRDiv.Text = lo.R_Div.ToString();
            TextBoxNDiv.Text = lo.N_Div.ToString();
            TextBoxFNum.Text = lo.F_Num.ToString();
            TextBoxFDen.Text = lo.F_Den.ToString();

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
            TextBoxPfdDlySel.Text = lo.PFD_DLY_SEL.ToString();




            if (lo.VcoSel_Force)
            {
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
                ComboBoxVcoSelect.Text = "Auto";
            }

            if (!CheckBoxManualVcoSelect.Checked)
            {
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
            LabelVcoLock.Text = lo.IsLocked ? "Locked" : "Unlock";
            LabelVcoLock.ForeColor = lo.IsLocked ? Color.Green : Color.Red;




        }
    }
}
