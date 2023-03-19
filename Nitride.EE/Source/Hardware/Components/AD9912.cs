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
    public class AD9912 : IDerivedClock
    {
        public AD9912(IClock reference)
        {
            Reference = reference;
            Regs[0x0] = 0x99;
            Regs[0x5] = 0x1; // io_update
            Regs[0x10] = 0x50; // init
            Regs[0x12] = 0x1; // reset
            Regs[0x13] = 0x0; // init

            Regs[0x104] = 0x0; // init
            Regs[0x105] = 0x0; // init
            Regs[0x106] = 0x0; // init

            Regs[0x1A6] = 0x0; // FTW 7-0
            Regs[0x1A7] = 0x0; // FTW 15-8
            Regs[0x1A8] = 0x0; // FTW 23-16
            Regs[0x1A9] = 0x0; // FTW 31-24
            Regs[0x1AA] = 0x0; // FTW 39-32
            Regs[0x1AB] = 0x40; // FTW 47-40

            Regs[0x1AC] = 0x0; // PTW
            Regs[0x1AD] = 0x0; // PTW

            Regs[0x200] = 0x5; // HSTL Output driver!
            Regs[0x201] = 0x0; // CMOS Output driver!

            Regs[0x40B] = 0xFF; // DAC Full-scale
            Regs[0x40C] = 0x3; // DAC Full-scale

            FTW_Base = Math.Pow(2, 48);
        }

        public readonly static ushort[] InitAddr =
            new ushort[] { 0x12, 0x5, 0x10, 0x13, 0x104, 0x105, 0x106,
                0x1A6, 0x1A7, 0x1A8, 0x1A9, 0x1AA, 0x1AB,
                0x1AC, 0x1AD, 0x200, 0x201, 0x40B, 0x40C, 0x5 };

        public Reg16 Regs { get; } = new(56); // 16-bits x 56 structure

        public IClock Reference { get; }

        public double DivRatio { get; set; }

        // public double Frequency => Reference.Frequency * DivRatio;

        public bool Enabled { get; set; }

        public double Frequency { get => FTW * 1e9 / FTW_Base; set => FTW = Convert.ToUInt64(FTW_Base * value / 1e9); }

        public double FTW_Base { get; }

        public ulong FTW
        {
            get => ((ulong)Regs[0x1AB] << 40) + ((ulong)Regs[0x1AA] << 32) + ((ulong)Regs[0x1A9] << 24) + ((ulong)Regs[0x1A8] << 16) + ((ulong)Regs[0x1A7] << 8) + ((ulong)Regs[0x1A6]);

            set 
            {
                Regs[0x1AB] = Convert.ToByte((value >> 40) & 0xFF);
                Regs[0x1AA] = Convert.ToByte((value >> 32) & 0xFF);
                Regs[0x1A9] = Convert.ToByte((value >> 24) & 0xFF);
                Regs[0x1A8] = Convert.ToByte((value >> 16) & 0xFF);
                Regs[0x1A7] = Convert.ToByte((value >> 8) & 0xFF);
                Regs[0x1AB6] = Convert.ToByte(value & 0xFF);
            }
        }

        public ulong PTW { get; set; }


    }
}
