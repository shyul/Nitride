/// ***************************************************************************
/// Nitride Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2023 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;

namespace Nitride.EE
{
    public class LT6952 : PLL
    {
        public LT6952(IClock reference)
        {
            Reference = reference;
            Channels = new OutputChannel[11];
            
            for (int i = 0; i < 11; i++) 
            {
                Channels[i] = new OutputChannel(i, this);
            }

            //R_Div = 1;
            //N_Div = 40;
            Regs[0x00] = 0x15;
            Regs[0x01] = 0xaa;
            Regs[0x02] = 0x0a;
            
            Regs[0x03] = 0xf0;
            Regs[0x04] = 0xcf;
            Regs[0x05] = 0x03;
            
            //Regs[0x03] = 0xff;
            //Regs[0x04] = 0xff;
            //Regs[0x05] = 0x3f;

            Regs[0x06] = 0x0c;
            Regs[0x07] = 0x01;
            Regs[0x08] = 0x00;
            Regs[0x09] = 0x28;
            Regs[0x0a] = 0x13;
            Regs[0x0b] = 0x06;
            Regs[0x0c] = 0x08;
            Regs[0x0d] = 0x00;
            Regs[0x0e] = 0x00;
            Regs[0x0f] = 0x00;
            Regs[0x10] = 0x08;
            Regs[0x11] = 0x00;
            Regs[0x12] = 0x00;
            Regs[0x13] = 0x00;
            Regs[0x14] = 0x00;
            Regs[0x15] = 0x00;
            Regs[0x16] = 0x00;
            Regs[0x17] = 0x00;
            Regs[0x18] = 0x9b;
            Regs[0x19] = 0x00;
            Regs[0x1a] = 0x00;
            Regs[0x1b] = 0x02;
            Regs[0x1c] = 0x9c;
            Regs[0x1d] = 0x00;
            Regs[0x1e] = 0x00;
            Regs[0x1f] = 0x00;
            Regs[0x20] = 0xf8;
            Regs[0x21] = 0x00;
            Regs[0x22] = 0x20;
            Regs[0x23] = 0x00;
            Regs[0x24] = 0x18;
            Regs[0x25] = 0x80;
            Regs[0x26] = 0x00;
            Regs[0x27] = 0x00;
            Regs[0x28] = 0x9c;
            Regs[0x29] = 0x60;
            Regs[0x2a] = 0x1f;
            Regs[0x2b] = 0x00;
            Regs[0x2c] = 0x00;
            Regs[0x2d] = 0x00;
            Regs[0x2e] = 0x20;
            Regs[0x2f] = 0x00;
            Regs[0x30] = 0xf8;
            Regs[0x31] = 0x00;
            Regs[0x32] = 0x00;
            Regs[0x33] = 0x00;
            Regs[0x34] = 0xf8;
            Regs[0x35] = 0x00;
            Regs[0x36] = 0x00;
            Regs[0x37] = 0x00;
            Regs[0x38] = 0x12;



        }

        public Reg16 Regs { get; } = new(56); // 8-bits x 56 structure

        public override double R_Ratio => R_Div; // 1 - 1023;

        public int R_Div
        {
            get => (Regs[0x7] >> 0) & 0xFF;
            set => Regs[0x7] = (ushort)(value & 0xFF);
        }

        public override uint N_Div
        {
            get => Convert.ToUInt32(((Regs[0x8] & 0xFF) << 8) + (Regs[0x9] & 0xFF));
            set 
            {
                Regs[0x8] = (ushort)((value >> 8) & 0xFF);
                Regs[0x9] = (ushort)(value & 0xFF);
            }
        }

        public override uint F_Num => 0;

        public override uint F_Den => 1;

        public int RevCode => (Regs[0x38] >> 4) & 0xF;

        public int PartCode => Regs[0x38] & 0xF;

        public override bool IsLocked => ((Regs[0x00] >> 4) & 0x7) == 0x1;

        public bool VcoValid => ((Regs[0x00] >> 2) & 0x3) == 0x1;

        public bool RefValid => ((Regs[0x00] >> 0) & 0x3) == 0x1;

        public bool InvertStatPin
        {
            get => ((Regs[0x1] >> 7) & 0x1) == 0x1;

            set
            {
                if (value)
                {
                    Regs[0x1] |= ((0x1) << 7) & 0xFF;
                }
                else
                {
                    Regs[0x1] &= (~((0x1) << 7)) & 0xFF;
                }
            }
        }

        public int StatPinMask
        {
            get => ((Regs[0x1] >> 0) & 0x7F);
            set
            {
                Regs[0x1] &= 0x80;
                Regs[0x1] |= (ushort)(value & 0x7F);
            }
        }

        public bool PowerDown
        {
            get => ((Regs[0x2] >> 7) & 0x1) == 0x1;

            set
            {
                if (value)
                {
                    Regs[0x2] |= ((0x1) << 7) & 0xFF;
                }
                else
                {
                    Regs[0x2] &= (~((0x1) << 7)) & 0xFF;
                }
            }
        }

        public bool PowerDownPLL
        {
            get => ((Regs[0x2] >> 6) & 0x1) == 0x1;

            set
            {
                if (value)
                {
                    Regs[0x2] |= ((0x1) << 6) & 0xFF;
                }
                else
                {
                    Regs[0x2] &= (~((0x1) << 6)) & 0xFF;
                }
            }
        }

        public bool PowerDownVcoInputDetector
        {
            get => ((Regs[0x2] >> 5) & 0x1) == 0x1;

            set
            {
                if (value)
                {
                    Regs[0x2] |= ((0x1) << 5) & 0xFF;
                }
                else
                {
                    Regs[0x2] &= (~((0x1) << 5)) & 0xFF;
                }
            }
        }

        public bool PowerDownReferenceInputDetector
        {
            get => ((Regs[0x2] >> 4) & 0x1) == 0x1;

            set
            {
                if (value)
                {
                    Regs[0x2] |= ((0x1) << 4) & 0xFF;
                }
                else
                {
                    Regs[0x2] &= (~((0x1) << 4)) & 0xFF;
                }
            }
        }

        public bool ReferenceBoostCurrent
        {
            get => ((Regs[0x2] >> 3) & 0x1) == 0x1;

            set
            {
                if (value)
                {
                    Regs[0x2] |= ((0x1) << 3) & 0xFF;
                }
                else
                {
                    Regs[0x2] &= (~((0x1) << 3)) & 0xFF;
                }
            }
        }

        public bool ReferenceInputFilter
        {
            get => ((Regs[0x2] >> 2) & 0x1) == 0x1;

            set
            {
                if (value)
                {
                    Regs[0x2] |= ((0x1) << 2) & 0xFF;
                }
                else
                {
                    Regs[0x2] &= (~((0x1) << 2)) & 0xFF;
                }
            }
        }

        public bool VcoInputFilter
        {
            get => ((Regs[0x2] >> 1) & 0x1) == 0x1;

            set
            {
                if (value)
                {
                    Regs[0x2] |= ((0x1) << 1) & 0xFF;
                }
                else
                {
                    Regs[0x2] &= (~((0x1) << 1)) & 0xFF;
                }
            }
        }


        public bool PowerOnReset
        {
            get => ((Regs[0x2] >> 0) & 0x1) == 0x1;

            set
            {
                if (value)
                {
                    Regs[0x2] |= ((0x1) << 0) & 0xFF;
                }
                else
                {
                    Regs[0x2] &= (~((0x1) << 0)) & 0xFF;
                }
            }
        }



        public bool TempO
        {
            get => ((Regs[0x5] >> 7) & 0x1) == 0x1;

            set
            {
                if (value)
                {
                    Regs[0x5] |= ((0x1) << 7) & 0xFF;
                }
                else
                {
                    Regs[0x5] &= (~((0x1) << 7)) & 0xFF;
                }
            }
        }

        public bool ChargePumpReset
        {
            get => ((Regs[0xA] >> 7) & 0x1) == 0x1;

            set
            {
                if (value)
                {
                    Regs[0xA] |= ((0x1) << 7) & 0xFF;
                }
                else
                {
                    Regs[0xA] &= (~((0x1) << 7)) & 0xFF;
                }
            }
        }

        public bool ForceChargePumpUp
        {
            get => ((Regs[0xA] >> 6) & 0x1) == 0x1;

            set
            {
                if (value)
                {
                    Regs[0xA] |= ((0x1) << 6) & 0xFF;
                }
                else
                {
                    Regs[0xA] &= (~((0x1) << 6)) & 0xFF;
                }
            }
        }

        public bool ForceChargePumpDown
        {
            get => ((Regs[0xA] >> 5) & 0x1) == 0x1;

            set
            {
                if (value)
                {
                    Regs[0xA] |= ((0x1) << 5) & 0xFF;
                }
                else
                {
                    Regs[0xA] &= (~((0x1) << 5)) & 0xFF;
                }
            }
        }

        public int ChargePumpCurrent
        {
            get => ((Regs[0xA] >> 0) & 0x1F);
            set
            {
                Regs[0x1] &= (ushort)((~0x1F) & 0xFFFF);
                Regs[0x1] |= (ushort)(value & 0x1F);
            }
        }

        public OutputChannel[] Channels { get; }

        public void Commit()
        {
            Console.WriteLine("LT6952: R_Div = " + R_Div + " | R_Ratio = " + R_Ratio + " | PhaseDetectFreqency = " + PhaseDetectFreqency + " | N_Div = " + N_Div + " | Frequency = " + Frequency);
            Channels.RunEach(n => { n.UpdateRegister(); });
        }

        public enum PowerDownMode : uint
        {
            Normal = 0,
            Mute = 1,
            Driver = 2,
            All = 3
        }

        public class OutputChannel : IDerivedClock
        {
            public OutputChannel(int index, LT6952 dev) 
            {
                Index = index;
                LT6952 = dev;
            }

            public int Index { get; set; }

            public PowerDownMode PowerDownMode { get; set; } = PowerDownMode.All;

            public int Prescaler { get; set; } = 0; // MP // 5 bits 0 ~ 32;

            public int Divider { get; set; } = 0; // 3 - bits 0 ~ 7;

            public double DivRatio // 1 ~ 4096
            {
                get => (Prescaler + 1) * Math.Pow(2, Divider);
                set
                {
                    if (value <= 32)
                    {
                        Divider = 0;
                        Prescaler = Convert.ToInt32(value - 1);
                    }
                    else
                    {
                        for (int i = 1; i < 8; i++)
                        {
                            double p = value / Math.Pow(2, i);
                            if (p <= 32)
                            {
                                Divider = i;
                                Prescaler = Convert.ToInt32(Math.Round(p - 1, MidpointRounding.AwayFromZero));
                                return;
                            }
                        }

                        Console.WriteLine("Invalid DivRatio Number: " + value);
                    }
                }
            }

            public bool Inverted { get; set; } = false;

            public int DigitalDelay { get; set; } = 0; // in half VCO cycle!

            public int AnalogDelay { get; set; } = 0;

            public void UpdateRegister() 
            {
                ushort base_addr = Convert.ToUInt16(0xC + (Index * 4));
                Regs[base_addr] = Convert.ToUInt16(((Prescaler & 0x1F) << 3) + ((Divider & 0x7) << 0));

                base_addr++;
                Regs[base_addr] = Convert.ToUInt16(((Inverted ? 1 : 0) << 4) + (((DigitalDelay >> 8) & 0xF) << 0));

                base_addr++;
                Regs[base_addr] = Convert.ToUInt16(DigitalDelay & 0xFF);

                base_addr++;
                Regs[base_addr] = Convert.ToUInt16(AnalogDelay & 0x3F);

                base_addr = Convert.ToUInt16(0x3 + (Index >> 2)); // (Index % 4)
                Regs[base_addr] &= Convert.ToUInt16((~((0x3) << (2 * (Index % 4)))) & 0xFF);
                Regs[base_addr] |= Convert.ToUInt16((((uint)PowerDownMode & 0x3) << (2 * (Index % 4))) & 0xFF);

                Console.WriteLine("Channel " + Index + ": Frequency = " + Frequency);
            }

            public double Frequency => Reference.Frequency / DivRatio;

            public bool Enabled => PowerDownMode == PowerDownMode.Normal;

            public IClock Reference => LT6952;

            public LT6952 LT6952 { get; }

            public Reg16 Regs => LT6952.Regs;
        }
    }
}
