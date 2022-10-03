/// ***************************************************************************
/// Nitride Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// 
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Nitride.EE
{
    public static class SignalTool
    {


        public static Complex[] GetSineWave(int numPt, double fullScale, double normFreq)
        {
            Complex[] res = new Complex[numPt];
            double ang = 2 * normFreq * Math.PI; // normFreq * Math.PI;// / numPt;
            Complex w = new(Math.Cos(ang), Math.Sin(ang));
            res[0] = new Complex(fullScale, 0.0);

            for (int i = 1; i < numPt; i++)
            {
                res[i] = res[i - 1] * w;
            }

            return res;
        }

        public static double[] Sine(int N = 65536, double cycle = 1)
        {
            double ang = Math.PI * 2 * cycle / (N - 1);

            double[] data = new double[N];
            for (int i = 0; i < N; i++)
                data[i] = Math.Sin(i * ang);

            return data;
        }

        public static Complex[] ComplexSine(int N = 65536, double cycle = 1)
        {
            double ang = Math.PI * 2 * cycle / (N - 1);

            Complex[] data = new Complex[N];
            for (int i = 0; i < N; i++)
                data[i] = new Complex(Math.Cos(i * ang), Math.Sin(i * ang));

            return data;
        }

        public static double Amplitude(IEnumerable<double> signal)
            => signal.Count() > 0 ? (signal.Max() - signal.Min()) : double.NaN;

        public static double PhaseDifference(IEnumerable<double> signal1, IEnumerable<double> signal2)
        {

            double ref1 = signal1.Average();
            double ref2 = signal2.Average();


            double count = Math.Min(signal1.Count(), signal2.Count());

            List<(int index, double s1, double s2)> list = new();

            for (int i = 0; i < count; i++)
            {
                list.Add((i, signal1.ElementAt(i), signal2.ElementAt(2)));
            }

            for (int i = 1; i < count - 1; i++)
            {
                var (index_prev, s1_prev, s2_prev) = list[i - 1];
                var (index, s1, s2) = list[i];
                var (index_next, s1_next, s2_next) = list[i + 1];

                if (s1_prev < ref1 && s1 >= ref1 && s1_next > ref1)
                {
                    // detected rising edige, add the point to list
                }

            }

            return 0;
        }

        public static void Frequency(TriggerEdge triggerEdge, IEnumerable<double> signal)
        {


        }
    }
}
