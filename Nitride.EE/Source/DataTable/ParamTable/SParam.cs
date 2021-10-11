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
    public class SParam : Parameter
    {
        public SParam(string name, int portCount, double z0 = 50)
        {
            Name = name;
            PortCount = portCount;
            Z0 = z0;
        }

        public double Z0 { get; }

        public ZParam GetZTable(FreqTable ft)
        {
            if (PortCount == 2)
            {
                double z0 = Z0;
                ZParam zt = new(Name, 2);
                Console.WriteLine("Get Z Table:\n");
                int pt = 0;
                foreach (var row in ft.Rows)
                {
                    double freq = row.Frequency;
                    pt++;

                    Complex s11 = row[this[1, 1]];
                    Complex s12 = row[this[1, 2]];
                    Complex s21 = row[this[2, 1]];
                    Complex s22 = row[this[2, 2]];

                    Complex deltaS = ((1 - s11) * (1 - s22)) - (s12 * s21);

                    row[zt[1, 1]] = (((1 + s11) * (1 - s22)) + (s12 * s21)) * z0 / deltaS;
                    row[zt[1, 2]] = 2 * s12 * z0 / deltaS;
                    row[zt[2, 1]] = 2 * s21 * z0 / deltaS;
                    row[zt[2, 2]] = (((1 - s11) * (1 + s22)) + (s12 * s21)) * z0 / deltaS;

                    //if (pt < 5 || (freq >= 1.9e6 && freq <= 2.4e6))
                    if (freq >= 1.9e6 && freq <= 2.4e6)
                    {
                        Console.WriteLine("freq = " + freq + "; s11 = " + row[this[1, 1]] + "; s21 = " + row[this[2, 1]] + "; s12 = " + row[this[1, 2]] + "; s22 = " + row[this[2, 2]]);
                        Console.WriteLine("freq = " + freq + "; z11 = " + row[zt[1, 1]] + "; z21 = " + row[zt[2, 1]] + "; z12 = " + row[zt[1, 2]] + "; z22 = " + row[zt[2, 2]]);
                        Console.WriteLine("freq = " + freq + "; Mag(z11) = " + row[zt[1, 1]].Magnitude + "; Mag(z21) = " + row[zt[2, 1]].Magnitude + "; Mag(z12) = " + row[zt[1, 2]].Magnitude + "; Mag(z22) = " + row[zt[2, 2]].Magnitude);
                        Console.WriteLine();
                    }
                }

                return zt;
            }
            else
                throw new Exception("The function only supports 2 ports, S-Parameter table.");
        }
    }
}
