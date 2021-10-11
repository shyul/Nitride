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
    public class ZParam : Parameter
    {
        public ZParam(string name, int portCount)
        {
            Name = name;
            PortCount = portCount;
        }

        private Dictionary<int, (ComplexColumn V, ComplexColumn I, ComplexColumn P)> VIColumnLUT { get; } = new();

        public (ComplexColumn V, ComplexColumn I, ComplexColumn P) this[int num]
        {
            get
            {
                if (!VIColumnLUT.ContainsKey(num))
                {
                    VIColumnLUT.Add(num, (
                        new ComplexColumn(GetType().Name + "_" + Name + "_V_" + num, "V" + num),
                        new ComplexColumn(GetType().Name + "_" + Name + "_I_" + num, "I" + num),
                        new ComplexColumn(GetType().Name + "_" + Name + "_P_" + num, "P" + num)
                    ));
                }

                return VIColumnLUT[num];
            }
        }

        public void GetZ1(FreqTable ft, ComplexColumn z2Column, ComplexColumn z1Column)
        {
            foreach (FreqRow row in ft.Rows)
            {
                Complex z11 = row[this[1, 1]];
                Complex z12 = row[this[1, 2]];
                Complex z21 = row[this[2, 1]];
                Complex z22 = row[this[2, 2]];
                Complex z2 = row[z2Column];
                row[z1Column] = z11 - (z12 * z21 / (z22 + z2));
            }
        }

        // Get Z2

        public void CalculatePort2VI(FreqTable ft)
        {
            var (V1_Column, I1_Column, P1_Column) = this[1];
            var (V2_Column, I2_Column, P2_Column) = this[2];

            foreach (var row in ft.Rows)
            {
                Complex z11 = row[this[1, 1]];
                Complex z12 = row[this[1, 2]];
                Complex z21 = row[this[2, 1]];
                Complex z22 = row[this[2, 2]];
                
                Complex v1 = row[V1_Column];
                Complex i1 = row[I1_Column];
                row[P1_Column] = v1 * i1;

                Complex i2 = row[I2_Column] = (v1 - (z11 * i1)) / z12;
                Complex v2 = row[V2_Column] = (z21 * i1) + (z22 * i2);
                row[P2_Column] = v2 * i2;
            }
        }

        public void CalculatePort1VI(FreqTable ft)
        {
            var (V1_Column, I1_Column, P1_Column) = this[1];
            var (V2_Column, I2_Column, P2_Column) = this[2];

            foreach (var row in ft.Rows)
            {
                Complex z11 = row[this[1, 1]];
                Complex z12 = row[this[1, 2]];
                Complex z21 = row[this[2, 1]];
                Complex z22 = row[this[2, 2]];

                Complex v2 = row[V2_Column];
                Complex i2 = row[I2_Column];
                row[P2_Column] = v2 * i2;

                Complex i1 = row[I1_Column] = (v2 - (z22 * i2)) / z21;
                Complex v1 = row[V1_Column] = (z11 * i1) + (z12 * i2);
                row[P1_Column] = v1 * i1;
            }
        }
    }
}
