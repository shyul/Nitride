/// ***************************************************************************
/// Nitride Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Radix-2 Software FFT
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
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
                Length = (uint)length;

                if (type == WindowsType.Custom)
                {
                    if (winF?.Length == length)
                        WinF = winF;
                    else
                        throw new ArgumentException("Custom Window's array length has to match FFT length");
                }
                else
                    WinF = WindowFunction.GetWindow(length, type);

                uint n = Length / 2;
                Wn = new Complex[n];
                double ang = 2 * Math.PI / Length;
                Complex w = new(Math.Cos(ang), -Math.Sin(ang));
                Wn[0] = new(1.0, 0.0);

                for (uint i = 1; i < n; i++)
                    Wn[i] = Wn[i - 1] * w;

                Dsw = new Complex[Length];
            }
            else
                throw new ArgumentException("Length must be greater than 4 and power of 2");
        }

        public uint Length { get; } = 1024;
        public WindowsType WindowType { get; private set; }
        public double[] WinF { get; private set; }
        public Complex[] Wn { get; private set; }
        public Complex[] Dsw { get; private set; }

        //public bool FileSaved = false;

        public void Transform(WaveForm td, List<FreqPoint> fd, int startPt = 0)
        {
            uint l1 = Length;
            uint l2 = (l1 >> 1); //Length / 2;
            uint m = 0;
            uint w = 1;
            uint i, j, k, d, ia, ib, iwn;
            int k1;
            Complex a, b, wn;

            for (i = 0; i < Length; i++)
            {
                Dsw[i] = td.Data[(int)(i + startPt)] * WinF[i];
            }

            //uint maxiwn = 0;
            //StreamWriter sw = null;

            //if (!FileSaved)
             //   sw = File.CreateText("B:\\fft.txt");  //null;
            /*
            if (!File.Exists("B:\\fft.txt")) 
            {
                //File.Create("B:\\fft.txt");
                sw = File.CreateText("B:\\fft.txt");
            }*/

            // Transform Radix-2
            while (l2 >= 1)
            {
                //if (!FileSaved) sw.WriteLine("\n *** l2 = " + l2 + " | m = " + m + " | w = " + w + " | d = " + d);
                for (i = 0; i < w; i++)
                {
                    //if (!FileSaved) sw.WriteLine("\n ** i = " + i + "\n");
                    d = i * l1;
                    iwn = 0;
                    for (j = 0; j < l2; j++)
                    {
                        ia = d + j;
                        ib = ia + l2;
            

                        //maxiwn = Math.Max(maxiwn, iwn);

                        wn = Wn[iwn];
                        a = Dsw[ia];
                        b = Dsw[ib];
               
                        Dsw[ia] = (a + b);
                        Dsw[ib] = (a - b) * wn;

                        iwn += w; // iwn = w * j;
                        //if (!FileSaved) sw.WriteLine("* j = " + j + " | ia = " + ia + "| ib = " + ib + " | wn = " + (w * j).ToString());
                    }
                }

                l1 >>= 1; // l1 = (l1 >> 1);
                l2 = (l1 >> 1);
                m++;
                w <<= 1; // w = (w << 1);
                /*
                l2 /= 2;
                m += 1;
                w *= 2;*/
            }

            //FileSaved = true;
            //Console.WriteLine("maxiwn = " + maxiwn);

            for (k = 0; k < Length; k++)
            {
                k1 = (int)(k + (Length / 2));

                if (k1 >= Length) k1 -= (int)Length;

                fd[k1].Value = Dsw[k.EndianInverse(m)];
            }
        }
    }
}
