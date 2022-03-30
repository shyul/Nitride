/// ***************************************************************************
/// Nitride Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Chrono Table
/// 
/// ***************************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.IO;
using Nitride.Chart;
using System.Drawing;

namespace Nitride.EE
{
    public class Z_Parameter : Parameter
    {
        public Z_Parameter(string name, int portCount)
        {
            Name = name;
            PortCount = portCount;
        }

        public S_Parameter GetSTable(FreqTable ft, double z0)
        {
            if (PortCount == 2)
            {
                S_Parameter st = new(Name, 2, z0);

                int pt = 0;
                foreach (var row in ft.Rows)
                {
                    double freq = row.Frequency;
                    pt++;

                    Complex z11 = row[this[1, 1]];
                    Complex z12 = row[this[1, 2]];
                    Complex z21 = row[this[2, 1]];
                    Complex z22 = row[this[2, 2]];

                    Complex delta = ((z11 + z0) * (z22 + z0)) - (z12 * z21);

                    row[st[1, 1]] = (((z11 - z0) * (z22 + z0)) - (z12 * z21)) / delta;
                    row[st[1, 2]] = 2 * z12 * z0 / delta;
                    row[st[2, 1]] = 2 * z21 * z0 / delta;
                    row[st[2, 2]] = (((z11 + z0) * (z22 - z0)) - (z12 * z21)) / delta;
                }

                return st;
            }
            else if (PortCount == 1)
            {
                S_Parameter st = new(Name, 1, z0);
                //Console.WriteLine("Get Z Table:\n");
                int pt = 0;
                foreach (var row in ft.Rows)
                {
                    double freq = row.Frequency;
                    pt++;

                    Complex z11 = row[this[1, 1]];
                    row[st[1, 1]] = (z11 - z0) / (z11 + z0);
                }

                return st;
            }
            else
                throw new Exception("The function only supports 1 or 2 ports, Z-Parameter table.");
        }

        // https://en.wikipedia.org/wiki/Impedance_parameters
        public void GetZinWithZload(FreqTable ft, ComplexColumn zinColumn, ComplexColumn zloadColumn)
        {
            foreach (FreqRow row in ft.Rows)
            {
                Complex z11 = row[this[1, 1]];
                Complex z12 = row[this[1, 2]];
                Complex z21 = row[this[2, 1]];
                Complex z22 = row[this[2, 2]];
                Complex zload = row[zloadColumn];
                row[zinColumn] = z11 - (z12 * z21 / (z22 + zload));
            }
        }

        // Get Zout
        public void GetZoutWithZsource(FreqTable ft, ComplexColumn zoutColumn, ComplexColumn zsourceColumn)
        {
            foreach (FreqRow row in ft.Rows)
            {
                Complex z11 = row[this[1, 1]];
                Complex z12 = row[this[1, 2]];
                Complex z21 = row[this[2, 1]];
                Complex z22 = row[this[2, 2]];
                Complex zsource = row[zsourceColumn];
                row[zoutColumn] = z22 - (z12 * z21 / (z11 + zsource));
            }
        }

        public void CalculatePort2VI(FreqTable ft, ComplexColumn V1, ComplexColumn I1, ComplexColumn V2, ComplexColumn I2)
        {
            foreach (var row in ft.Rows)
            {
                Complex z11 = row[this[1, 1]];
                Complex z12 = row[this[1, 2]];
                Complex z21 = row[this[2, 1]];
                Complex z22 = row[this[2, 2]];

                Complex v1 = row[V1];
                Complex i1 = row[I1];

                Complex i2 = row[I2] = (v1 - (z11 * i1)) / z12;
                row[V2] = (z21 * i1) + (z22 * i2);
            }
        }

        public void CalculatePort2VI_ReverseP2Current(FreqTable ft, ComplexColumn V1, ComplexColumn I1, ComplexColumn V2, ComplexColumn I2)
        {
            foreach (var row in ft.Rows)
            {
                Complex z11 = row[this[1, 1]];
                Complex z12 = row[this[1, 2]];
                Complex z21 = row[this[2, 1]];
                Complex z22 = row[this[2, 2]];

                Complex v1 = row[V1];
                Complex i1 = row[I1];

                Complex i2 = (v1 - (z11 * i1)) / z12;
                row[V2] = (z21 * i1) + (z22 * i2);
                row[I2] = 0 - i2;
            }
        }

        public void CalculatePort1VI(FreqTable ft, ComplexColumn V1, ComplexColumn I1, ComplexColumn V2, ComplexColumn I2)
        {
            foreach (var row in ft.Rows)
            {
                Complex z11 = row[this[1, 1]];
                Complex z12 = row[this[1, 2]];
                Complex z21 = row[this[2, 1]];
                Complex z22 = row[this[2, 2]];

                Complex v2 = row[V2];
                Complex i2 = row[I2];

                Complex i1 = row[I1] = (v2 - (z22 * i2)) / z21;
                row[V1] = (z11 * i1) + (z12 * i2);
            }
        }
        public void CalculatePort1VI_ReverseP2Current(FreqTable ft, ComplexColumn V1, ComplexColumn I1, ComplexColumn V2, ComplexColumn I2)
        {
            foreach (var row in ft.Rows)
            {
                Complex z11 = row[this[1, 1]];
                Complex z12 = row[this[1, 2]];
                Complex z21 = row[this[2, 1]];
                Complex z22 = row[this[2, 2]];

                Complex v2 = row[V2];
                Complex i2 = 0 - row[I2];

                Complex i1 = row[I1] = (v2 - (z22 * i2)) / z21;
                row[V1] = (z11 * i1) + (z12 * i2);
            }
        }
    }
}
