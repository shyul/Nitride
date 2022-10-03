/// ***************************************************************************
/// Nitride Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Radix-2 Software FFT
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace Nitride.EE
{
    public class FFT
    {
        public FFT(int length = 65536, WindowsType type = WindowsType.FlatTop, double[] winF = null, int[] winParam = null)
        {
            if (length > 4 && length.IsPowerOf2())
            {
                Length = length;
            

                if (type == WindowsType.Custom)
                {
                    if (winF?.Length == length)
                        WinF = winF;
                    else
                        throw new ArgumentException("Custom Window's array length has to match FFT length");
                }
                else
                    WinF = WindowFunction.GetWindow(length, type);

                Wn = new Complex[Length];
                double ang = 2 * Math.PI / Length;
                Complex w = new Complex(Math.Cos(ang), -Math.Sin(ang));
                Wn[0] = new Complex(1.0, 0.0);

                int n = Length / 2;
                for (int i = 1; i < n; i++)
                    Wn[i] = Wn[i - 1] * w;

            }
            else
                throw new ArgumentException("Length must be greater than 4 and power of 2");
        }

        public int Length { get; } = 1024;
        public WindowsType WindowType { get; private set; }
        public Complex[] Wn { get; private set; }
        public double[] WinF { get; private set; }

        public void Transform(WaveForm td, IList<FreqPoint> fd, int startPt = 0)
        {
            Complex[] dsw = new Complex[Length];

            for (int i = 0; i < Length; i++)
            {
                dsw[i] = td.Data[i + startPt] * WinF[i];
            }

            int l2 = Length / 2;
            int m = 0;
            int w = 1;
            int d;

            // Transform Radix-2
            while (l2 >= 1)
            {
                for (int i = 0; i < w; i++)
                {
                    d = Length / w;
                    for (int j = 0; j < l2; j++)
                    {
                        Complex TmpA = dsw[i * d + j] + dsw[i * d + l2 + j];
                        Complex TmpB = (dsw[i * d + j] - dsw[i * d + l2 + j]) * Wn[w * j];
                        dsw[i * d + j] = TmpA;
                        dsw[i * d + l2 + j] = TmpB;
                    }
                }
                l2 /= 2;
                m += 1;
                w *= 2;
            }

            for (uint i = 0; i < Length; i++)
            {
                fd[(int)i].Value = dsw[i.EndianInverse(m)];
            }
        }
    }
}
