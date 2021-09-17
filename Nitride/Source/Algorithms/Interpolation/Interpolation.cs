/// ***************************************************************************
/// Nitride Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Math and numeric related basic functions.
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;

namespace Nitride
{
    public static partial class Algorithms
    {
        public static double Linear(double x, double x1, double y1, double x2, double y2)
        {
            return y1 + ((x - x1) * (y2 - y1) / (x2 - x1));
        }


        public static List<(double x, double y)> Linear(List<double> xin, List<(double x, double y)> table) 
        {
            List<(double x, double y)> list = new();

            int pt = 0;
            foreach (double x in xin)
            {
                if (x <= table.First().x)
                {
                    list.Add(table.First());
                }
                else if (x >= table.Last().x)
                {
                    list.Add(table.Last());
                }
                else
                {
                    while (pt < table.Count - 1)
                    {
                        if (x >= table[pt].x)
                            break;
                        else
                            pt++;
                    }

                    if (pt + 1 < table.Count)
                    {
                        var (x1, y1) = table[pt];
                        var (x2, y2) = table[pt + 1];

                        double y = Linear(x, x1, y1, x2, y2);

                        list.Add((x, y));
                    }
                }

            }

            return list;
        }
    }
}
