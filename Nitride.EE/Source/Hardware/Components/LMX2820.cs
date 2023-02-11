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
    public class LMX2820 : PLL
    {
        public LMX2820(IClock reference)
        {
            Reference = reference;

            // Initialize Default Registers
            LoadDefault();
            Regs[74] = 0;
        }

        public Reg16 Regs { get; } = new(123);

        public Func<int, ushort> SPI_RD;

        public Action<int, ushort> SPI_WR;

        private void REG_RD(byte addr)
        {
            if (SPI_RD is not null)
            {
                Regs[addr] = SPI_RD(addr);
            }
        }

        private void REG_WR(byte addr)
        {
            if (SPI_WR is not null)
            {
                SPI_WR(addr, Regs[addr]);
            }
        }

        public void Commit() 
        {
            // Auto settings...
        
        }

        public void InstantCalibration()
        {
            REG_WR(45);
            REG_WR(44);
            REG_WR(43); // PLL NUM
            REG_WR(42);
            REG_WR(39); // PLL DEN
            REG_WR(38);
            REG_WR(36); // PLL N
            REG_WR(32); // OUT DIV
            REG_WR(18); // LD DLY
            REG_WR(17); // LD TYPE
            REG_WR(6); // ACAL_DELAY
            REG_WR(2);
            REG_WR(1);
            REG_WR(0);
        }

        public void VcoCalibration()
        {
            REG_WR(43); // PLL NUM
            REG_WR(42);
            REG_WR(39); // PLL DEN
            REG_WR(38);
            REG_WR(36); // PLL N
            REG_WR(32); // OUT DIV
            REG_WR(23); // VCO SEL FORCE
            REG_WR(22); // VCO SEL 
            REG_WR(20); // VCO DACISET
            REG_WR(10); // VCO DAC FORCE / CAPCTRL FORCE
            REG_WR(6); // ACAL_DELAY
            REG_WR(2);
            REG_WR(1);
            REG_WR(0);
        }

        public void ReadStatus()
        {
            REG_RD(74);
        }

        public void ReadAll()
        {

        }

        public void WriteAll()
        {

        }

        // public bool LOOPBACK_EN // R34


        public override double R_Ratio => PreR * R_Div / (ReferenceMulti * (EnableRefDoubler ? 2 : 1));
        public double RefMultiplyOut => Reference.Frequency * (EnableRefDoubler ? 2 : 1) * ReferenceMulti / PreR;

        // public double DivRatio => (EnableRefDoubler ? 2 : 1) / PreR / R_Div * (N_Div + ((double)F_Num / (double)F_Den));
        // public double Frequency => PFD_Frequency * (N_Div + ((double)F_Num / (double)F_Den)); // VCO: 3.2 GHz ~ 6.4 GHz;

        public override bool IsLocked
        {
            get => ((Regs[74] >> 14) & 0x3) == 0x2;
            set { }
        }

        public int ActualVcoSel
        {
            get => ((Regs[74] >> 2) & 0x7);
        }

        public bool EnableRefDoubler
        {
            get => ((Regs[11] >> 4) & 0x1) == 0x1;

            set
            {
                if (value)
                {
                    Regs[11] |= ((0x1) << 4) & 0xFFFF;
                }
                else
                {
                    Regs[11] &= (~((0x1) << 4)) & 0xFFFF;
                }
            }
        }

        public uint PreR
        {
            get => (uint)(Regs[14]) & 0xFFF;

            set
            {
                // Regs[14] &= 0xF000;
                Regs[14] = 0x3 << 12;
                Regs[14] |= (ushort)(value & 0xFFF);
            }
        }

        public uint ReferenceMulti // 3 bit
        {
            get => (uint)(Regs[12] >> 10) & 0x7;

            set
            {
                // Regs[12] &= (~((0x7) << 10)) & 0xFFFF;
                Regs[12] = 0x8;
                Regs[12] |= (ushort)((value & 0x7) << 10);
            }
        }

        public uint R_Div
        {
            get => (uint)(Regs[13] >> 5) & 0xFF;

            set
            {
                // Regs[13] &= (~((0xFF) << 5)) & 0xFFFF;
                Regs[13] = 0x18;
                Regs[13] |= (ushort)((value & 0xFF) << 5);
            }
        }

        public uint CPG
        {
            get => (uint)(Regs[16] >> 1) & 0xF;

            set
            {
                // Regs[16] &= (~((0xF) << 1)) & 0xFFFF;
                Regs[16] = 0xB8 << 5; // 0x1700; // 
                Regs[16] |= (ushort)((value & 0xF) << 1);
            }
        }

        public bool LD_Continuous
        {
            get => ((Regs[17] >> 6) & 0x1) == 0x1;

            set
            {
                if (value)
                {
                    Regs[17] |= ((0x1) << 6) & 0xFFFF;
                }
                else
                {
                    Regs[17] &= (~((0x1) << 6)) & 0xFFFF;
                }
            }
        }

        public ushort LD_Delay
        {
            get => Regs[18];
            set => Regs[18] = value;
        }

        public bool TempSense_Enable
        {
            get => ((Regs[19] >> 3) & 0x3) == 0x3;
            set => Regs[19] = (ushort)((0x109 << 5) + (value ? (0x3 << 3) : 0x0));
        }

        public override uint N_Div
        {
            get => (ushort)(Regs[36] & 0x7FFF);
            set => Regs[36] = (ushort)(value & 0x7FFF);
        }

        public override uint F_Num
        {
            get => ((uint)Regs[42] << 16) + Regs[43];

            set
            {
                Regs[43] = (ushort)(value & 0xFFFF);
                Regs[42] = (ushort)((value >> 16) & 0xFFFF);
                // if (value == 0) Mash_Order = 0; // Disable Mash when F_Num = 0; 
            }
        }

        public override uint F_Den
        {
            get => ((uint)Regs[38] << 16) + Regs[39];

            set
            {
                Regs[39] = (ushort)(value & 0xFFFF);
                Regs[38] = (ushort)((value >> 16) & 0xFFFF);
            }
        }

        // 0 => INT Mode, 1, 2, 3
        public uint Mash_Order
        {
            get => (uint)((Regs[35] >> 7) & 0x3);

            set
            {
                Regs[35] &= (~((0x3) << 7)) & 0xFFFF;
                Regs[35] |= (ushort)((value & 0x7) << 7);
            }
        }

        public uint Mash_Seed
        {
            get => ((uint)Regs[40] << 16) + Regs[41];

            set
            {
                Regs[41] = (ushort)(value & 0xFFFF);
                Regs[40] = (ushort)((value >> 16) & 0xFFFF);
            }
        }

        public bool Mash_Seed_Enable
        {
            get => ((Regs[35] >> 6) & 0x1) == 0x1;

            set
            {
                if (value)
                {
                    Regs[35] |= ((0x1) << 6) & 0xFFFF;
                }
                else
                {
                    Regs[35] &= (~((0x1) << 6)) & 0xFFFF;
                }
            }
        }

        public uint Mash_Reset_Counter
        {
            get => ((uint)Regs[62] << 16) + Regs[63];

            set
            {
                Regs[63] = (ushort)(value & 0xFFFF);
                Regs[62] = (ushort)((value >> 16) & 0xFFFF);
            }
        }

        public bool Mash_Reset_N
        {
            get => ((Regs[35] >> 12) & 0x1) == 0x0;

            set
            {
                if (value)
                {
                    Regs[35] &= (~((0x1) << 12)) & 0xFFFF;
                }
                else
                {
                    Regs[35] |= ((0x1) << 12) & 0xFFFF;
                }
            }
        }








        public uint VcoSel
        {
            get => (uint)(Regs[22] >> 13) & 0x7;

            set
            {
                Regs[22] &= (~((0x7) << 13)) & 0xFFFF;
                Regs[22] |= (ushort)((value & 0x7) << 13);
            }
        }



        public bool VcoSel_Force
        {
            get => ((Regs[23] >> 0) & 0x1) == 0x1;

            set
            {
                if (value)
                {
                    Regs[23] |= ((0x1) << 0) & 0xFFFF;
                }
                else
                {
                    Regs[23] &= (~((0x1) << 0)) & 0xFFFF;
                }
            }
        }


        public bool PSYNC_Pin_Enable
        {
            get => ((Regs[68] >> 5) & 0x1) == 0x0;

            set
            {
                if (value)
                {
                    Regs[35] &= (~((0x1) << 5)) & 0xFFFF;
                }
                else
                {
                    Regs[35] |= ((0x1) << 5) & 0xFFFF;
                }
            }
        }


        public double TempSense
        {
            get => (0.85 * (uint)((Regs[76]) & 0x7F)) - 415;
        }

        public double Ch_A_Frequency
        {
            get
            {
                return RFOutA_Mux switch
                {
                    0 => VcoFrequency / Math.Pow(2, (Ch_Div_A + 1)),
                    1 => VcoFrequency,
                    2 => VcoFrequency * 2,
                    _ => 0
                };
            }
        }

        public double Ch_B_Frequency
        {
            get
            {
                return RFOutB_Mux switch
                {
                    0 => VcoFrequency / Math.Pow(2, (Ch_Div_B + 1)),
                    1 => VcoFrequency,
                    2 => VcoFrequency * 2,
                    _ => 0
                };
            }
        }


        public uint Ch_Div_A
        {
            get => (uint)(Regs[32] >> 6) & 0x7;  // 3-bits

            set
            {
                Regs[32] &= (~((0x7) << 6)) & 0xFFFF;
                Regs[32] |= (ushort)((value & 0x7) << 6);
            }
        }

        public uint Ch_Div_B
        {
            get => (uint)(Regs[32] >> 9) & 0x7;  // 3-bits

            set
            {
                Regs[32] &= (~((0x7) << 9)) & 0xFFFF;
                Regs[32] |= (ushort)((value & 0x7) << 9);
            }
        }

        public bool RFOutA_Enable
        {
            get => ((Regs[78] >> 4) & 0x1) != 0x1;

            set
            {
                if (value)
                {
                    Regs[78] &= (~((0x1) << 4)) & 0xFFFF;
                }
                else
                {
                    Regs[78] |= ((0x1) << 4) & 0xFFFF;
                }
            }
        }

        public bool RFOutB_Enable
        {
            get => ((Regs[79] >> 8) & 0x1) != 0x1;

            set
            {
                if (value)
                {
                    Regs[79] &= (~((0x1) << 8)) & 0xFFFF;
                }
                else
                {
                    Regs[79] |= ((0x1) << 8) & 0xFFFF;
                }
            }
        }

        public uint RFOutA_Mux
        {
            get => (uint)(Regs[78] >> 0) & 0x3;

            set
            {
                Regs[78] &= (~((0x3) << 0)) & 0xFFFF;
                Regs[78] |= (ushort)((value & 0x3) << 0);
            }
        }

        public uint RFOutB_Mux
        {
            get => (uint)(Regs[79] >> 4) & 0x3;

            set
            {
                Regs[79] &= (~((0x3) << 4)) & 0xFFFF;
                Regs[79] |= (ushort)((value & 0x3) << 4);
            }
        }

        public uint RFOutA_Level
        {
            get => (uint)(Regs[79] >> 1) & 0x7;

            set
            {
                Regs[79] &= (~((0x7) << 1)) & 0xFFFF;
                Regs[79] |= (ushort)((value & 0x7) << 1);
            }
        }

        public uint RFOutB_Level
        {
            get => (uint)(Regs[80] >> 6) & 0x7;

            set
            {
                Regs[80] &= (~((0x7) << 6)) & 0xFFFF;
                Regs[80] |= (ushort)((value & 0x7) << 6);
            }
        }

        public uint PFD_DLY
        {
            get => (uint)(Regs[37] >> 9) & 0x3F;

            set
            {
                Regs[37] = 0x100;
                Regs[37] |= (ushort)((value & 0x3F) << 9);
            }
        }

        public bool PFD_DLY_MANUAL
        {
            get => ((Regs[10] >> 12) & 0x1) == 0x1;

            set
            {
                if (value)
                {
                    Regs[10] |= ((0x1) << 12) & 0xFFFF;
                }
                else
                {
                    Regs[10] &= (~((0x1) << 12)) & 0xFFFF;
                }
            }
        }

        public bool QUICK_RECAL_EN
        {
            get => ((Regs[2] >> 0) & 0x1) == 0x1;

            set
            {
                if (value)
                {
                    Regs[2] |= ((0x1) << 0) & 0xFFFF;
                }
                else
                {
                    Regs[2] &= (~((0x1) << 0)) & 0xFFFF;
                }
            }
        }

        public bool INSTCAL_EN
        {
            get => ((Regs[1] >> 0) & 0x1) == 0x1;

            set
            {
                if (value)
                {
                    Regs[1] |= ((0x1) << 0) & 0xFFFF;
                }
                else
                {
                    Regs[1] &= (~((0x1) << 0)) & 0xFFFF;
                }
            }
        }

        public bool INSTCAL_DBLR_EN
        {
            get => ((Regs[1] >> 1) & 0x1) == 0x1;

            set
            {
                if (value)
                {
                    Regs[1] |= ((0x1) << 1) & 0xFFFF;
                }
                else
                {
                    Regs[1] &= (~((0x1) << 1)) & 0xFFFF;
                }
            }
        }

        public uint INSTCAL_DLY
        {
            get => (uint)(Regs[2] >> 1) & 0x7FF;

            set
            {
                Regs[2] &= (~((0x7FF) << 1)) & 0xFFFF;
                Regs[0] |= (ushort)((value & 0x7FF) << 1);
            }
        }



        public uint FCAL_LPFD_ADJ
        {
            get => (uint)(Regs[0] >> 7) & 0x3;

            set
            {
                Regs[0] &= (~((0x3) << 7)) & 0xFFFF;
                Regs[0] |= (ushort)((value & 0x3) << 7);
            }
        }

        public uint FCAL_HPFD_ADJ
        {
            get => (uint)(Regs[0] >> 9) & 0x3;

            set
            {
                Regs[0] &= (~((0x3) << 9)) & 0xFFFF;
                Regs[0] |= (ushort)((value & 0x3) << 9);
            }
        }

        public uint CAL_CLK_DIV
        {
            get => (uint)(Regs[2] >> 12) & 0x7;

            set
            {
                Regs[2] &= (~((0x7) << 12)) & 0xFFFF;
                Regs[2] |= (ushort)((value & 0x7) << 12);
            }
        }


        public uint INSTCAL_PLL_NUM
        {
            get => ((uint)Regs[44] << 16) + Regs[45];

            set
            {
                Regs[45] = (ushort)(value & 0xFFFF);
                Regs[44] = (ushort)((value >> 16) & 0xFFFF);
            }
        }

        public bool VCO_CAPCTRL_FORCE
        {
            get => ((Regs[10] >> 7) & 0x1) == 0x1;

            set
            {
                if (value)
                {
                    Regs[10] |= ((0x1) << 7) & 0xFFFF;
                }
                else
                {
                    Regs[10] &= (~((0x1) << 7)) & 0xFFFF;
                }
            }
        }

        public uint VCO_CAPCTRL
        {
            get => (uint)(Regs[22]) & 0xFF;

            set
            {
                Regs[22] &= (~((0xFF) << 0)) & 0xFFFF;
                Regs[22] |= (ushort)((value & 0xFF) << 0);
            }
        }

        public uint VCO_CAPCTRL_RD => (uint)(Regs[74] >> 5) & 0xFF;

        public bool INSTCAL_SKIP_ACAL
        {
            get => ((Regs[0] >> 13) & 0x1) == 0x1;

            set
            {
                if (value)
                {
                    Regs[0] |= ((0x1) << 13) & 0xFFFF;
                }
                else
                {
                    Regs[0] &= (~((0x1) << 13)) & 0xFFFF;
                }
            }
        }

        public uint ACAL_CMP_DLY
        {
            get => (uint)(Regs[6] >> 8) & 0xFF;

            set
            {
                Regs[6] = 0x43;
                Regs[6] |= (ushort)((value & 0xFF) << 8);
            }
        }

        public bool DBLR_CAL_EN
        {
            get => ((Regs[0] >> 6) & 0x1) == 0x1;

            set
            {
                if (value)
                {
                    Regs[0] |= ((0x1) << 6) & 0xFFFF;
                }
                else
                {
                    Regs[0] &= (~((0x1) << 6)) & 0xFFFF;
                }
            }
        }

        public bool FCAL_EN
        {
            get => ((Regs[0] >> 4) & 0x1) == 0x1;

            set
            {
                if (value)
                {
                    Regs[0] |= ((0x1) << 4) & 0xFFFF;
                }
                else
                {
                    Regs[0] &= (~((0x1) << 4)) & 0xFFFF;
                }
            }
        }

        public void LoadTICSFile(string reg_text)
        {
            //   string reg_text = File.ReadAllText(@"reg.txt");

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
            Regs[0] = 0x6070;
            Regs[1] = 0x57A0;
            Regs[2] = 0x81F4;
            Regs[3] = 0x41;
            Regs[4] = 0x4204;
            Regs[5] = 0x32;
            Regs[6] = 0xA43;
            Regs[7] = 0x0;
            Regs[8] = 0xC802;
            Regs[9] = 0x5;
            Regs[10] = 0x0;
            Regs[11] = 0x602;
            Regs[12] = 0x408;
            Regs[13] = 0x38;
            Regs[14] = 0x3001;
            Regs[15] = 0x2001;
            
            Regs[16] = 0x171E;
            //Regs[16] = 0x172A;

            Regs[17] = 0x15C0;
            Regs[18] = 0x0;
            Regs[19] = 0x2120;
            Regs[20] = 0x272C;
            Regs[21] = 0x1C64;
            Regs[22] = 0xE2BF;
            Regs[23] = 0x1102;
            Regs[24] = 0xE34;
            Regs[25] = 0x624;
            Regs[26] = 0xDB0;
            Regs[27] = 0x8001;
            Regs[28] = 0x639;
            Regs[29] = 0x318C;
            Regs[30] = 0xB18C;
            Regs[31] = 0x401;
            Regs[32] = 0x1001;
            Regs[33] = 0x0;
            Regs[34] = 0x10;
            Regs[35] = 0x3000;
            Regs[36] = 0x50;
            Regs[37] = 0x300;
            Regs[38] = 0x0;
            Regs[39] = 0x1;
            Regs[40] = 0x0;
            Regs[41] = 0x0;
            Regs[42] = 0x0;
            Regs[43] = 0x0;
            Regs[44] = 0x0;
            Regs[45] = 0x0;
            Regs[46] = 0x300;
            Regs[47] = 0x300;
            Regs[48] = 0x4180;
            Regs[49] = 0x0;
            Regs[50] = 0x80;
            Regs[51] = 0x203F;
            Regs[52] = 0x0;
            Regs[53] = 0x0;
            Regs[54] = 0x0;
            Regs[55] = 0x2;
            Regs[56] = 0x1;
            Regs[57] = 0x1;
            Regs[58] = 0x0;
            Regs[59] = 0x1388;
            Regs[60] = 0x1F4;
            Regs[61] = 0x3E8;
            Regs[62] = 0x0;
            Regs[63] = 0xC350;
            Regs[64] = 0x80;
            Regs[65] = 0x0;
            Regs[66] = 0x3F;
            Regs[67] = 0x1000;
            Regs[68] = 0x0;
            Regs[69] = 0x11;
            Regs[70] = 0xE;
            Regs[71] = 0x0;
            Regs[72] = 0x0;
            Regs[73] = 0x0;
            Regs[74] = 0x0;
            Regs[75] = 0x0;
            Regs[76] = 0x0;
            Regs[77] = 0x608;
            Regs[78] = 0x1;
            Regs[79] = 0x1E;
            Regs[80] = 0x1C0;
            Regs[81] = 0x0;
            Regs[82] = 0x0;
            Regs[83] = 0xF00;
            Regs[84] = 0x40;
            Regs[85] = 0x0;
            Regs[86] = 0x40;
            Regs[87] = 0xFF00;
            Regs[88] = 0x3FF;
            Regs[89] = 0x0;
            Regs[90] = 0x0;
            Regs[91] = 0x0;
            Regs[92] = 0x0;
            Regs[93] = 0x1000;
            Regs[94] = 0x0;
            Regs[95] = 0x0;
            Regs[96] = 0x17F8;
            Regs[97] = 0x0;
            Regs[98] = 0x1C80;
            Regs[99] = 0x19B9;
            Regs[100] = 0x533;
            Regs[101] = 0x3E8;
            Regs[102] = 0x28;
            Regs[103] = 0x14;
            Regs[104] = 0x14;
            Regs[105] = 0xA;
            Regs[106] = 0x0;
            Regs[107] = 0x0;
            Regs[108] = 0x0;
            Regs[109] = 0x0;
            Regs[110] = 0x1F;
            Regs[111] = 0x0;
            Regs[112] = 0xFFFF;
            Regs[113] = 0x0;
            Regs[114] = 0x0;
            Regs[115] = 0x0;
            Regs[116] = 0x0;
            Regs[117] = 0x0;
            Regs[118] = 0x0;
            Regs[119] = 0x0;
            Regs[120] = 0x0;
            Regs[121] = 0x0;
            Regs[122] = 0x0;
        }
    }
}
