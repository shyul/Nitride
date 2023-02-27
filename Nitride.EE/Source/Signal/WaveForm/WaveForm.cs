/// ***************************************************************************
/// Nitride Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2023 Xu Li - me@xuli.us
/// 
/// WaveForm
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Nitride.EE
{
    public class WaveForm 
    {
        public WaveForm(int maxLength)
        {
            Length = Count = maxLength;
            Data = new Complex[Count];
        }

        public void Configure(int length, double rate = 1, double startTime = 0)
        {
            if (length > Count) throw new Exception("WaveForm length: " + length + " > Count: " + Count);

            Length = length;

            SampleRate = rate;
            StartTime = startTime;
            Duration = ((Length - 1) / SampleRate);
            StopTime = StartTime + Duration;
        }

        public bool IsUpdated { get; set; } = false;

        public Complex this[int i] => Data[i];

        public int Count { get; }

        public double SampleRate { get; private set; }

        public double StartTime { get; private set; }

        public double Duration { get; private set; }

        public double StopTime { get; private set; }

        public int Length { get; private set; }

        public Complex[] Data { get; } 

        public double Max => Data.Take(Length).Select(x => x.Magnitude).Max();
        
        public double Min => Data.Take(Length).Select(x => x.Magnitude).Min();

        public double Rms => Math.Sqrt(Data.Take(Length).Select(x => Math.Pow(x.Magnitude, 2)).Sum() / Length);

        public double Mean => Data.Take(Length).Select(x => x.Magnitude).Average(); 

        public void CopyData(Complex[] samples, int offset = 0)
        {
            int cnt = Math.Min(samples.Length - offset, Count);
            for (int i = 0; i < cnt; i++)
            {
                Data[i] = samples[i + offset];
            }
        }

        public void GetSineWave(double fullScale = 65535, double normFreq = 1)
        {
            double ang = 2 * normFreq * Math.PI; // normFreq * Math.PI;// / numPt;
            Complex w = new(Math.Cos(ang), Math.Sin(ang));
            Data[0] = new Complex(fullScale, 0.0);

            for (int i = 1; i < Length; i++)
            {
                Data[i] = Data[i - 1] * w;
            }
        }

        public void GetChirp(double fullScale, double[] normFreq)
        {
            Data[0] = fullScale * Complex.One;

            for (int i = 1; i < Length; i++)
            {
                double ang = 2 * normFreq[i] * Math.PI; // normFreq * Math.PI;// / numPt;
                Complex w = new(Math.Cos(ang), Math.Sin(ang));
                Data[i] =  Data[i - 1] * w;
            }
            
            /*
            for (int i = 0; i < Count; i++)
            {
                Data[i] = Complex.Exp(Complex.ImaginaryOne * Math.PI * normFreq[i] * normFreq[i]);
            }*/

        }

        public void GetPN(uint real, uint imag, uint mask, double min, double max, int p1 = 9, int p2 = 5) // or 23 / 18
        {
            List<double> rl = new(Length);
            List<double> il = new(Length);
            /*
            for (int i = 0; i < 800; i++)
            {
                real = ((real & 0x7FFFFFFF) << 1) + ((((real >> (p1 - 1)) & 0x1) ^ ((real >> (p2 - 1)) & 0x1)) & 0x1);
                imag = ((imag & 0x7FFFFFFF) << 1) + ((((imag >> (p1 - 1)) & 0x1) ^ ((imag >> (p2 - 1)) & 0x1)) & 0x1);
            }*/

            for (int i = 0; i < Length; i++)
            {
                real = ((real & 0x7FFFFFFF) << 1) + ((((real >> (p1 - 1)) & 0x1) ^ ((real >> (p2 - 1)) & 0x1)) & 0x1);
                imag = ((imag & 0x7FFFFFFF) << 1) + ((((imag >> (p1 - 1)) & 0x1) ^ ((imag >> (p2 - 1)) & 0x1)) & 0x1);

                rl.Add((real & mask));
                il.Add((imag & mask));
            }

            double l_avg = (rl.Average() + il.Average()) / 2;
            double avg = (max - min) / 2;
            double offset = (l_avg - avg);

            for (int i = 0; i < Length; i++)
            {
                rl[i] -= offset;
                il[i] -= offset;
            }

            double l_min = Math.Min(rl.Min(), il.Min());
            double l_max = Math.Max(rl.Max(), il.Max());


            double scale = Math.Max(l_max / max, l_min / min);

            // Console.WriteLine("offset = " + offset + " | scale = " + scale);

            for (int i = 0; i < Length / 2; i++)
            {
                double r = (rl[i + (Length / 2)]) / scale;
                double q = (il[i + (Length / 2)]) / scale;

               // if (r > max) r = max; else if (r < min) r = min;
               // if (q > max) q = max; else if (q < min) q = min;
                // Console.WriteLine("r = " + r + " | q = " + q);

                Data[Length - 1 - i] = Data[i] = new Complex(r, q);
                // new Complex(r, q);
            }
        }

        public void GetGold(uint real, uint imag, uint mask, double min, double max) // or 23 / 18
        {
            List<double> rl = new(Length);
            List<double> il = new(Length);

            int p1 = 6;
            int p2 = 5;
            int p3 = 2;
            int p4 = 1;

            for (int i = 0; i < 800; i++)
            {
                real = ((real & 0x7FFFFFFF) << 1) + ((((real >> (p1 - 1)) & 0x1) ^ ((real >> (p2 - 1)) & 0x1) ^ ((real >> (p3 - 1)) & 0x1) ^ ((real >> (p4 - 1)) & 0x1)) & 0x1);
                imag = ((imag & 0x7FFFFFFF) << 1) + ((((imag >> (p1 - 1)) & 0x1) ^ ((imag >> (p2 - 1)) & 0x1) ^ ((real >> (p3 - 1)) & 0x1) ^ ((real >> (p4 - 1)) & 0x1)) & 0x1);
            }

            for (int i = 0; i < Length; i++)
            {
                real = ((real & 0x7FFFFFFF) << 1) + ((((real >> (p1 - 1)) & 0x1) ^ ((real >> (p2 - 1)) & 0x1) ^ ((real >> (p3 - 1)) & 0x1) ^ ((real >> (p4 - 1)) & 0x1)) & 0x1);
                imag = ((imag & 0x7FFFFFFF) << 1) + ((((imag >> (p1 - 1)) & 0x1) ^ ((imag >> (p2 - 1)) & 0x1) ^ ((real >> (p3 - 1)) & 0x1) ^ ((real >> (p4 - 1)) & 0x1)) & 0x1);

                rl.Add((real & mask));
                il.Add((imag & mask));
            }

            double l_avg = (rl.Average() + il.Average()) / 2;
            double avg = (max - min) / 2;
            double offset = (l_avg - avg);

            for (int i = 0; i < Length; i++)
            {
                rl[i] -= offset;
                il[i] -= offset;
            }

            double l_min = Math.Min(rl.Min(), il.Min());
            double l_max = Math.Max(rl.Max(), il.Max());


            double scale = Math.Max(l_max / max, l_min / min);

            Console.WriteLine("offset = " + offset + " | scale = " + scale);

            for (int i = 0; i < Length; i++)
            {
                double r = (rl[i]) / scale;
                double q = (il[i]) / scale;

                // if (r > max) r = max; else if (r < min) r = min;
                // if (q > max) q = max; else if (q < min) q = min;
                Console.WriteLine("r = " + r + " | q = " + q);

                Data[i] = new Complex(r, q);


            }
        }

        /*
        public void GetPN(uint real, uint imag, uint mask, int p1 = 9, int p2 = 5)
        {
            int a, b;

            for (int i = 0; i < 1000; i++)
            {
                real = ((real & 0x7FFFFFFF) << 1) + ((((real >> (p1 - 1)) & 0x1) ^ ((real >> (p2 - 1)) & 0x1)) & 0x1);
                imag = ((imag & 0x7FFFFFFF) << 1) + ((((imag >> (p1 - 1)) & 0x1) ^ ((imag >> (p2 - 1)) & 0x1)) & 0x1);
            }

            for (int i = 0; i < Count; i++)
            {
                real = ((real & 0x7FFFFFFF) << 1) + ((((real >> (p1 - 1)) & 0x1) ^ ((real >> (p2 - 1)) & 0x1)) & 0x1);
                imag = ((imag & 0x7FFFFFFF) << 1) + ((((imag >> (p1 - 1)) & 0x1) ^ ((imag >> (p2 - 1)) & 0x1)) & 0x1);

                a = (int)( real & mask);
                b = (int)(imag & mask);


                Data[i] = new Complex(r, q);
            }
        

        }*/

    }
}
