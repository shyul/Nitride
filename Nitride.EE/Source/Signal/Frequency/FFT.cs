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
        public FFT(int length = 262144)
        {
            Length = (uint)length;
            WinF = new double[length];
            Wn = new Complex[length];
            Dsw = new Complex[length];
        }

        public void UpdataConfiguration(int length, WindowsType type) //, bool flip)
        {
            if (length > 4 && length <= WinF.Length && length.IsPowerOf2())
            {
                Length = (uint)length;
                IsFFT = true;
                WindowFunction.GetWindow(length, type, ref WinF);
                Gain = WinF.Take((int)Length).Sum(); // / Length;
                InitWn();
            }
            else
                throw new ArgumentException("Length must be greater than 4 and power of 2");
        }

        public void UpdataConfiguration(int length, double[] windowFunction) //, bool flip)
        {
            if (length > 4 && length == windowFunction.Length && length <= WinF.Length && length.IsPowerOf2())
            {
                Length = (uint)length;
                IsFFT = true;
                WinF = windowFunction;
                Gain = WinF.Take((int)Length).Sum(); // / Length;
                InitWn();
            }
            else
                throw new ArgumentException("Length must be greater than 4 and power of 2");
        }

        public void UpdataConfiguration(int length, bool isFFT) //, bool flip)
        {
            if (length > 4 && length <= WinF.Length && length.IsPowerOf2())
            {
                Length = (uint)length;
                IsFFT = isFFT;

                for (int i = 0; i < WinF.Length; i++) 
                {
                    WinF[i] = 1;
                }

                Gain = 1; // / Length;
                InitWn();
            }
            else
                throw new ArgumentException("Length must be greater than 4 and power of 2");
        }

        public bool IsFFT { get; private set; }

        private void InitWn() 
        {
            uint n = Length / 2;

            double ang = 2 * Math.PI / Length;
            Complex w = IsFFT ? new(Math.Cos(ang), -Math.Sin(ang)) : new(Math.Cos(ang), Math.Sin(ang));
            Wn[0] = new(1.0, 0.0);

            for (uint i = 1; i < n; i++)
                Wn[i] = Wn[i - 1] * w;
        }

        /*
        public void UpdataConfiguration(int length = 65536, WindowsType type = WindowsType.FlatTop, bool flip = true, double[] winF = null, bool isFft = true)
        {
            if (length > 4 && length <= WinF.Length && length.IsPowerOf2())
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
                    WindowFunction.GetWindow(length, type, ref WinF);

                uint n = Length / 2;

                double ang = 2 * Math.PI / Length;
                Complex w = isFft ? new(Math.Cos(ang), -Math.Sin(ang)) : new(Math.Cos(ang), Math.Sin(ang));
                Wn[0] = new(1.0, 0.0);

                for (uint i = 1; i < n; i++)
                    Wn[i] = Wn[i - 1] * w;

                FlipSpectrum = flip;

                Gain = WinF.Take((int)Length).Sum(); // / Length;
            }
            else
                throw new ArgumentException("Length must be greater than 4 and power of 2");

            // Console.WriteLine("FFT Win = " + type.ToString() + " | Length = " + length + " | Gain = " + Gain);

        }*/

        public uint Length { get; private set; } = 1024;
        public double Gain { get; private set; }

        public double[] WinF;// { get; private set; }

        public Complex[] Wn;// { get; private set; }

        public Complex[] Dsw;// { get; private set; }.

        public uint M { get; private set; }

        //public bool FileSaved = false;

        public void Transform(Complex[] td, int startPt = 0) // List<FreqPoint> fd, 
        {
            uint l1 = Length;
            uint l2 = (l1 >> 1); //Length / 2;
            uint m = 0;
            uint w = 1;
            uint i, j, d, ia, ib, iwn;
            //int k1;
            // uint k;
            Complex a, b, wn;

            for (i = 0; i < Length; i++)
            {
                Dsw[i] = td[(int)(i + startPt)] * WinF[i];
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
            /*
            foreach (var pt in Dsw.Last(32))
            {
                Console.Write("(" + pt + ") ");
            }
            */

            M = m;

            if (!IsFFT) 
            {
                for (i = 0; i < Length; i++)
                {
                    Dsw[i] = Dsw[i] / Length;
                }
            }

            // return Dsw;

            /*
            if (FlipSpectrum)
            {
                for (k = 0; k < Length; k++)
                {
                    k1 = (int)(k + (Length / 2));

                    if (k1 >= Length) k1 -= (int)Length;

                    fd[k1].Value = Dsw[k.EndianInverse(m)];
                }
            }
            else
            {
                for (k = 0; k < Length; k++)
                {
                    fd[(int)k].Value = Dsw[k.EndianInverse(m)];
                }
            }
            */


            /*
            foreach (var pt in fd.Take(32))
            {
                Console.Write("(freq = " + pt.Frequency + " | val = " + pt.Value + ") ");
            }*/
        }
    }
}
