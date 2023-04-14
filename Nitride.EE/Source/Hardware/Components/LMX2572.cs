/// ***************************************************************************
/// Nitride Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2023 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Nitride.EE
{
    public class LMX2572 : PLL
    {
        public LMX2572(IClock reference)
        {
            Reference = reference;

            // Initialize Default Registers
            LoadDefault();
        }

        public Reg16 Regs { get; } = new(126);

        public Action WriteAll { get; set; }

        public Action ReadAll { get; set; }

        public Action<ushort[], Reg16> BulkWrite { get; set; }

        public Action<ushort[], Reg16> BulkRead { get; set; }

        public readonly static ushort[] VcoCalibrationAddr = new ushort[] { 43, 42, 39, 38, 36, 32, 23, 22, 20, 10, 6, 2, 1, 0 };

        public void VcoCalibration()
        {
            Commit();
            if (BulkWrite is not null)
            {
                BulkWrite(VcoCalibrationAddr, Regs);
            }
        }

        public readonly static ushort[] StatusAddr = new ushort[] { 110, 111, 112 };

        public void ReadStatus()
        {
            if (BulkRead is not null)
            {
                BulkRead(StatusAddr, Regs);
            }
        }

        public double Fosc => Reference.Frequency;

        public double Fsm => Fosc / Math.Pow(2, CAL_CLK_DIV);

        public void Commit()
        {
            Console.WriteLine("PreR = " + PreR + " | R_Div = " + R_Div + " | ReferenceMulti = " + ReferenceMulti + " | EnableRefDoubler = " + (EnableRefDoubler ? 2 : 1) + " | R_Ratio = " + R_Ratio + " | PhaseDetectFreqency = " + PhaseDetectFreqency);

            EnableRefMultiH = (RefMultiplyOut > 100e6);

            if (Fosc > 200e6)
            {
                CAL_CLK_DIV = 1;
            }
            else
            {
                CAL_CLK_DIV = 0;
            }

            if (PhaseDetectFreqency <= 37.5e6)
            {
                FCAL_HPFD_ADJ = 0;
            }
            else if (PhaseDetectFreqency > 37.5e6 && PhaseDetectFreqency <= 75e6)
            {
                FCAL_HPFD_ADJ = 1;
            }
            else if (PhaseDetectFreqency > 75e6 && PhaseDetectFreqency <= 100e6)
            {
                FCAL_HPFD_ADJ = 2;
            }
            else
            {
                FCAL_HPFD_ADJ = 3;
            }

            if (PhaseDetectFreqency >= 10e6)
            {
                FCAL_LPFD_ADJ = 0;
            }
            else if (PhaseDetectFreqency >= 5e6 && PhaseDetectFreqency < 10e6)
            {
                FCAL_LPFD_ADJ = 1;
            }
            else if (PhaseDetectFreqency >= 2.5e6 && PhaseDetectFreqency < 5e6)
            {
                FCAL_LPFD_ADJ = 2;
            }
            else
            {
                FCAL_LPFD_ADJ = 3;
            }

            if (VcoFrequency < 4e9)
            {
                PFD_DLY_SEL = Mash_Order switch
                {
                    0 when N_Div >= 20 => 0,
                    1 when N_Div >= 25 => 1,
                    2 when N_Div >= 26 => 1,
                    3 when N_Div >= 32 => 2,
                    4 when N_Div >= 44 => 4,
                    _ => throw new Exception("N_Div is too low: " + N_Div + "VCO Frequency: " + VcoFrequency),
                };
            }
            else if (VcoFrequency >= 4e9 && VcoFrequency < 4.9e9)
            {
                PFD_DLY_SEL = Mash_Order switch
                {
                    0 when N_Div >= 24 => 1,
                    1 when N_Div >= 29 => 2,
                    2 when N_Div >= 30 => 2,
                    3 when N_Div >= 32 => 2,
                    4 when N_Div >= 44 => 4,
                    _ => throw new Exception("N_Div is too low: " + N_Div + "VCO Frequency: " + VcoFrequency),
                };
            }
            else if (VcoFrequency >= 4.9e9 && VcoFrequency <= 6.4e9)
            {
                PFD_DLY_SEL = Mash_Order switch
                {
                    0 when N_Div >= 24 => 1,
                    1 when N_Div >= 29 => 2,
                    2 when N_Div >= 30 => 2,
                    3 when N_Div >= 36 => 3,
                    4 when N_Div >= 48 => 5,
                    _ => throw new Exception("N_Div is too low: " + N_Div + "VCO Frequency: " + VcoFrequency),
                };
            }
            else
                throw new Exception("VCO Frequency is too high: " + VcoFrequency + " Reference.Frequency = " + Reference.Frequency);
        }

        public bool PowerDown
        {
            get => ((Regs[0] >> 0) & 0x1) == 0x1;

            set
            {
                if (value)
                {
                    Regs[0] |= ((0x1) << 0) & 0xFFFF;
                }
                else
                {
                    Regs[0] &= (~((0x1) << 0)) & 0xFFFF;
                }
            }
        }

        public bool Reset
        {
            get => ((Regs[0] >> 1) & 0x1) == 0x1;

            set
            {
                if (value)
                {
                    Regs[0] |= ((0x1) << 1) & 0xFFFF;
                }
                else
                {
                    Regs[0] &= (~((0x1) << 1)) & 0xFFFF;
                }
            }
        }

        public override double R_Ratio => 1.0D * PreR * R_Div / (ReferenceMulti * (EnableRefDoubler ? 2 : 1));

        public double RefMultiplyOut => 1.0D * Reference.Frequency * (EnableRefDoubler ? 2 : 1) * ReferenceMulti / PreR;

        // public double DivRatio => (EnableRefDoubler ? 2 : 1) / PreR / R_Div * (N_Div + ((double)F_Num / (double)F_Den));
        // public double Frequency => PFD_Frequency * (N_Div + ((double)F_Num / (double)F_Den)); // VCO: 3.2 GHz ~ 6.4 GHz;

        public override bool IsLocked
        {
            get => ((Regs[0x6E] >> 9) & 0x3) == 0x2;
            set { }
        }

        public int ActualVcoSel
        {
            get => ((Regs[0x6E] >> 5) & 0x7);
        }

        /// <summary>
        /// RefMultiplyOut > 100 MHz set to 1'b1, <= 100 MHz set to 1'b0;
        /// </summary>
        public bool EnableRefMultiH
        {
            get => ((Regs[9] >> 14) & 0x1) == 0x1;

            set
            {
                if (value)
                {
                    Regs[9] |= ((0x1) << 14) & 0xFFFF;
                }
                else
                {
                    Regs[9] &= (~((0x1) << 14)) & 0xFFFF;
                }
            }
        }

        public bool EnableRefDoubler
        {
            get => ((Regs[9] >> 12) & 0x1) == 0x1;

            set
            {
                if (value)
                {
                    Regs[9] |= ((0x1) << 12) & 0xFFFF;
                }
                else
                {
                    Regs[9] &= (~((0x1) << 12)) & 0xFFFF;
                }
            }
        }

        public uint PreR
        {
            get => (uint)(Regs[12]) & 0xFFF;

            set
            {
                Regs[12] &= 0xF000;
                Regs[12] |= (ushort)(value & 0xFFF);
            }
        }

        public uint ReferenceMulti
        {
            get => (uint)(Regs[10] >> 7) & 0x1F;

            set
            {
                Regs[10] &= (~((0x1F) << 7)) & 0xFFFF;
                Regs[10] |= (ushort)((value & 0x1F) << 7);
            }
        }

        public uint R_Div
        {
            get => (uint)(Regs[11] >> 4) & 0xFF;

            set
            {
                Regs[11] &= (~((0xFF) << 4)) & 0xFFFF;
                Regs[11] |= (ushort)((value & 0xFF) << 4);
            }
        }

        public uint CPG
        {
            get => (uint)(Regs[14] >> 3) & 0xF;

            set
            {
                Regs[14] = 0x30 << 7;
                Regs[14] |= (ushort)((value & 0xF) << 3);
            }
        }

        public override uint N_Div
        {
            get
            {
                return Regs[36] + (((uint)Regs[34] & 0x7) << 16);
            }
            set
            {
                Regs[36] = (ushort)(value & 0xFFFF);
                Regs[34] = (ushort)(((value >> 16) & 0x7) + (1 << 4));
            }
        }

        // Disable Mash when F_Num = 0; 
        public override uint F_Num
        {
            get => ((uint)Regs[42] << 16) + Regs[43];

            set
            {
                Regs[43] &= (ushort)(value & 0xFFFF);
                Regs[42] |= (ushort)((value >> 16) & 0xFFFF);
            }
        }

        public override uint F_Den
        {
            get => ((uint)Regs[38] << 16) + Regs[39];

            set
            {
                Regs[39] &= (ushort)(value & 0xFFFF);
                Regs[38] |= (ushort)((value >> 16) & 0xFFFF);
            }
        }

        public uint Mash_Order
        {
            get => (Regs[0x2C]) & (uint)0x7;

            set
            {
                Regs[0x2C] &= (~0x7 & 0xFFFF);
                Regs[0x2C] |= (ushort)(value & 0x7);
            }
        }

        public uint Mash_Seed
        {
            get => ((uint)Regs[40] << 16) + Regs[41];

            set
            {
                Regs[41] &= (ushort)(value & 0xFFFF);
                Regs[40] |= (ushort)((value >> 16) & 0xFFFF);
            }
        }

        public bool Mash_Seed_Enable
        {
            get => ((Regs[0x25] >> 15) & 0x1) == 0x1;

            set
            {
                if (value)
                {
                    Regs[0x25] |= ((0x1) << 15) & 0xFFFF;
                }
                else
                {
                    Regs[0x25] &= (~((0x1) << 15)) & 0xFFFF;
                }
            }
        }

        public uint Mash_Reset_Counter
        {
            get => ((uint)Regs[69] << 16) + Regs[70];

            set
            {
                Regs[70] &= (ushort)(value & 0xFFFF);
                Regs[69] |= (ushort)((value >> 16) & 0xFFFF);
            }
        }

        public bool Mash_Reset_N
        {
            get => ((Regs[0x2C] >> 5) & 0x1) == 0x0;

            set
            {
                if (value)
                {
                    Regs[0x2C] &= (~((0x1) << 5)) & 0xFFFF;
                }
                else
                {
                    Regs[0x2C] |= ((0x1) << 5) & 0xFFFF;
                }
            }
        }



        public uint VcoSel
        {
            get => (uint)(Regs[20] >> 11) & 0x7;

            set
            {
                Regs[20] &= (~((0x7) << 11)) & 0xFFFF;
                Regs[20] |= (ushort)((value & 0x7) << 11);
            }
        }

        public bool VcoSel_Force
        {
            get => ((Regs[20] >> 10) & 0x1) == 0x1;

            set
            {
                if (value)
                {
                    Regs[20] |= ((0x1) << 10) & 0xFFFF;
                }
                else
                {
                    Regs[20] &= (~((0x1) << 10)) & 0xFFFF;
                }
            }
        }

        public uint Ch_Div
        {
            get => (uint)(Regs[75] >> 6) & 0x1F;

            set
            {
                Regs[75] = (1 << 11);
                // Regs[75] &= (~((0x1F) << 6)) & 0xFFFF;
                Regs[75] |= (ushort)((value & 0x1F) << 6);
            }
        }

        public double Ch_Div_Val => Math.Pow(2.0, (Ch_Div + 1.0));

        public bool RFOutA_Enable
        {
            get => ((Regs[0x2C] >> 6) & 0x1) != 0x1;

            set
            {
                if (value)
                {
                    Regs[0x2C] &= (~((0x1) << 6)) & 0xFFFF;
                }
                else
                {
                    Regs[0x2C] |= ((0x1) << 6) & 0xFFFF;
                }
            }
        }

        public bool RFOutB_Enable
        {
            get => ((Regs[0x2C] >> 7) & 0x1) != 0x1;

            set
            {
                if (value)
                {
                    Regs[0x2C] &= (~((0x1) << 7)) & 0xFFFF;
                }
                else
                {
                    Regs[0x2C] |= ((0x1) << 7) & 0xFFFF;
                }
            }
        }

        public uint RFOutA_Mux
        {
            get => (uint)(Regs[45] >> 11) & 0x3;

            set
            {
                Regs[45] &= (~((0x3) << 11)) & 0xFFFF;
                Regs[45] |= (ushort)((value & 0x3) << 11);
            }
        }

        public uint RFOutB_Mux
        {
            get => (uint)(Regs[46]) & 0x3;

            set
            {
                Regs[46] &= (~(0x3)) & 0xFFFF;
                Regs[46] |= (ushort)(value & 0x3);
            }
        }

        public uint RFOutA_Level
        {
            get => (uint)(Regs[0x2C] >> 8) & 0x3F;

            set
            {
                Regs[0x2C] &= (~((0x3F) << 8)) & 0xFFFF;
                Regs[0x2C] |= (ushort)((value & 0x3F) << 8);
            }
        }

        public uint RFOutB_Level
        {
            get => (uint)Regs[0x2D] & 0x3F;

            set
            {
                Regs[0x2D] &= (~0x3F) & 0xFFFF;
                Regs[0x2D] |= (ushort)(value & 0x3F);
            }
        }

        public bool PSYNC_Pin_Enable
        {
            get => ((Regs[58] >> 15) & 0x1) == 0x0;

            set
            {
                if (value)
                {
                    Regs[58] &= (~((0x1) << 15)) & 0xFFFF;
                }
                else
                {
                    Regs[58] |= ((0x1) << 15) & 0xFFFF;
                }
            }
        }

        public bool Phase_Sync_Enable
        {
            get => ((Regs[0] >> 14) & 0x1) == 0x1;

            set
            {
                if (value)
                {
                    Regs[0] |= ((0x1) << 14) & 0xFFFF;
                }
                else
                {
                    Regs[0] &= (~((0x1) << 14)) & 0xFFFF;
                }
            }
        }

        public double Phase_Shift => Mash_Seed_Enable ? 360 * Mash_Seed / F_Den : 0;

        public double Ch_A_Phase
        {
            get
            {
                return RFOutA_Mux switch
                {
                    0 => Phase_Shift / Ch_Div_Val,
                    1 => Phase_Shift,
                    _ => double.NaN,
                };
            }
        }

        public double Ch_B_Phase
        {
            get
            {
                return RFOutB_Mux switch
                {
                    0 => Phase_Shift / Ch_Div_Val,
                    1 => Phase_Shift,
                    _ => double.NaN,
                };
            }
        }

        public double Ch_A_Frequency
        {
            get
            {
                // Console.WriteLine("VcoFrequency = " + VcoFrequency + " | Ch_Div = " + Ch_Div + " | Div = " + Math.Pow(2, (Ch_Div + 1)));
                return RFOutA_Mux switch
                {
                    0 => VcoFrequency / Ch_Div_Val, //Math.Pow(2, (Ch_Div + 1)),
                    1 => VcoFrequency,
                    _ => double.NaN,
                };
            }
        }

        public double Ch_B_Frequency
        {
            get
            {
                // Console.WriteLine("VcoFrequency = " + VcoFrequency + " | Ch_Div = " + Ch_Div + " | Div = " + Math.Pow(2, (Ch_Div + 1)));
                return RFOutB_Mux switch
                {
                    0 => VcoFrequency / Ch_Div_Val, //Math.Pow(2, (Ch_Div + 1)),
                    1 => VcoFrequency,
                    _ => double.NaN,
                };
            }
        }

        public bool FCAL_EN
        {
            get => ((Regs[0] >> 3) & 0x1) == 0x1;

            set
            {
                if (value)
                {
                    Regs[0] |= ((0x1) << 3) & 0xFFFF;
                }
                else
                {
                    Regs[0] &= (~((0x1) << 3)) & 0xFFFF;
                }
            }
        }

        public bool QUICK_RECAL_EN
        {
            get => ((Regs[78] >> 9) & 0x1) == 0x1;

            set
            {
                if (value)
                {
                    Regs[78] |= ((0x1) << 9) & 0xFFFF;
                }
                else
                {
                    Regs[78] &= (~((0x1) << 9)) & 0xFFFF;
                }
            }
        }

        public bool VCO_CAPCTRL_FORCE
        {
            get => ((Regs[8] >> 11) & 0x1) == 0x1;

            set
            {
                if (value)
                {
                    Regs[8] |= ((0x1) << 11) & 0xFFFF;
                }
                else
                {
                    Regs[8] &= (~((0x1) << 11)) & 0xFFFF;
                }
            }
        }

        public uint VCO_CAPCTRL
        {
            get => (uint)(Regs[19]) & 0xFF;

            set
            {
                Regs[19] = 0x27 << 8;
                Regs[19] |= (ushort)((value & 0xFF) << 0);
            }
        }

        public uint VCO_CAPCTRL_RD => (uint)(Regs[111] >> 0) & 0xFF;

        public uint PFD_DLY_SEL
        {
            get => (uint)(Regs[0x25] >> 8) & 0x3F;

            set
            {
                Regs[0x25] &= (~((0x3F) << 8)) & 0xFFFF;
                Regs[0x25] |= (ushort)((value & 0x3F) << 8);
            }
        }

        public uint FCAL_LPFD_ADJ
        {
            get => (uint)(Regs[0] >> 5) & 0x3;

            set
            {
                Regs[0] &= (~((0x3) << 5)) & 0xFFFF;
                Regs[0] |= (ushort)((value & 0x3) << 5);
            }
        }

        public uint FCAL_HPFD_ADJ
        {
            get => (uint)(Regs[0] >> 7) & 0x3;

            set
            {
                Regs[0] &= (~((0x3) << 7)) & 0xFFFF;
                Regs[0] |= (ushort)((value & 0x3) << 7);
            }
        }

        public uint CAL_CLK_DIV
        {
            get => (uint)(Regs[1] >> 0) & 0x7;

            set
            {
                Regs[1] = 0x101 << 3;
                Regs[1] |= (ushort)((value & 0x7) << 0);
            }
        }

        public void LoadTICSFile(string reg_text)
        {
            using StringReader sr = new(reg_text);

            while (sr.ReadLine() is string rline)
            {
                string[] reg_fields = rline.Split('\t');
                uint reg_value = Convert.ToUInt32(reg_fields[1], 16);
                byte addr = Convert.ToByte((reg_value >> 16) & 0xFF);
                ushort data = Convert.ToUInt16(reg_value & 0xFFFF);
                Regs[addr] = data;
            }
        }

        public void LoadDefault()
        {
            Regs[125] = 0x2288;
            Regs[124] = 0x0000;
            Regs[123] = 0x0000;
            Regs[122] = 0x0000;
            Regs[121] = 0x0000;
            Regs[120] = 0x0000;
            Regs[119] = 0x0000;
            Regs[118] = 0x0000;
            Regs[117] = 0x0000;
            Regs[116] = 0x0000;
            Regs[115] = 0x0000;
            Regs[114] = 0x7802;
            Regs[113] = 0x0000;
            Regs[112] = 0x0000;
            Regs[111] = 0x0000;
            Regs[110] = 0x0000;
            Regs[109] = 0x0000;
            Regs[108] = 0x0000;
            Regs[107] = 0x0000;
            Regs[106] = 0x0007;
            Regs[105] = 0x4440;
            Regs[104] = 0x2710;
            Regs[103] = 0x0000;
            Regs[102] = 0x0000;
            Regs[101] = 0x0000;
            Regs[100] = 0x2710;
            Regs[99] = 0x0000;
            Regs[98] = 0x0000;
            Regs[97] = 0x0000;
            Regs[96] = 0x0000;
            Regs[95] = 0x0000;
            Regs[94] = 0x0000;
            Regs[93] = 0x0000;
            Regs[92] = 0x0000;
            Regs[91] = 0x0000;
            Regs[90] = 0x0000;
            Regs[89] = 0x0000;
            Regs[88] = 0x0000;
            Regs[87] = 0x0000;
            Regs[86] = 0x0000;
            Regs[85] = 0xD800;
            Regs[84] = 0x0001;
            Regs[83] = 0x0000;
            Regs[82] = 0x2800;
            Regs[81] = 0x0000;
            Regs[80] = 0xCCCC;
            Regs[79] = 0x004C;
            Regs[78] = 0x0001;
            Regs[77] = 0x0000;
            Regs[76] = 0x000C;
            Regs[75] = 0x0800;
            Regs[74] = 0x0000;
            Regs[73] = 0x003F;
            Regs[72] = 0x0001;
            Regs[71] = 0x0081;
            Regs[70] = 0xC350;
            Regs[69] = 0x0000;
            Regs[68] = 0x03E8;
            Regs[67] = 0x0000;
            Regs[66] = 0x01F4;
            Regs[65] = 0x0000;
            Regs[64] = 0x1388;
            Regs[63] = 0x0000;
            Regs[62] = 0x00AF;
            Regs[61] = 0x00A8;
            Regs[60] = 0x03E8;
            Regs[59] = 0x0001;
            Regs[58] = 0x9001;
            Regs[57] = 0x0020;
            Regs[56] = 0x0000;
            Regs[55] = 0x0000;
            Regs[54] = 0x0000;
            Regs[53] = 0x0000;
            Regs[52] = 0x0421;
            Regs[51] = 0x0080;
            Regs[50] = 0x0080;
            Regs[49] = 0x4180;
            Regs[48] = 0x03E0;
            Regs[47] = 0x0300;
            Regs[46] = 0x07F0;
            Regs[45] = 0xC622;
            Regs[44] = 0x1D20;
            Regs[43] = 0x0000;
            Regs[42] = 0x0000;
            Regs[41] = 0x0000;
            Regs[40] = 0x0000;
            Regs[39] = 0x0001;
            Regs[38] = 0x0000;
            Regs[37] = 0x0105;
            Regs[36] = 0x0030;
            Regs[35] = 0x0004; // Constant
            Regs[34] = 0x0010;
            Regs[33] = 0x1E01;
            Regs[32] = 0x05BF;
            Regs[31] = 0xC3E6;
            Regs[30] = 0x18A6;
            Regs[29] = 0x0000;
            Regs[28] = 0x0488;
            Regs[27] = 0x0002;
            Regs[26] = 0x0808;
            Regs[25] = 0x0624;
            Regs[24] = 0x071A;
            Regs[23] = 0x007C;
            Regs[22] = 0x0001;
            Regs[21] = 0x0409;
            Regs[20] = 0x4848;
            Regs[19] = 0x27B7;
            Regs[18] = 0x0064;
            Regs[17] = 0x0096;
            Regs[16] = 0x0080;
            Regs[15] = 0x060E;
            Regs[14] = 0x1878;
            Regs[13] = 0x4000;
            Regs[12] = 0x5001;
            Regs[11] = 0xB018;
            Regs[10] = 0x10F8;
            Regs[9] = 0x0004;
            Regs[8] = 0x2000;
            Regs[7] = 0x00B2;
            Regs[6] = 0xC802;
            Regs[5] = 0x28C8;
            Regs[4] = 0x0A43;
            Regs[3] = 0x0782;
            Regs[2] = 0x0500;
            Regs[1] = 0x0808;
            Regs[0] = 0x2198;
        }
    }
}
