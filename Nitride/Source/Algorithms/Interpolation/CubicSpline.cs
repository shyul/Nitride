/// ***************************************************************************
/// Nitride Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Math and numeric related basic functions.
/// https://en.wikipedia.org/wiki/Spline_interpolation
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nitride
{
    public class CubicSpline
    {
        public CubicSpline(IEnumerable<double> x, IEnumerable<double> y, double startSlope = double.NaN, double endSlope = double.NaN)
        {
            N = x.Count();
            if (N == y.Count() && N > 1) 
            {
                X = x.ToArray();
                Y = y.ToArray();
              

                double[] R = new double[N];

                TridiagonalMatrix tdm = new(N);
                double dx1, dy1, dx2, dy2;

                if (double.IsNaN(startSlope)) 
                {
                    dx1 = X[1] - X[0];
                    tdm.C[0] = 1D / dx1;
                    tdm.B[0] = 2D * tdm.C[0];
                    R[0] = 3D * (Y[1] - Y[0]) / (dx1 * dx1);
                }
                else
                {
                    tdm.B[0] = 1;
                    R[0] = startSlope;
                }

                for (int i = 1; i < N - 1; i++)
                {
                    dx1 = X[i] - X[i - 1];
                    dx2 = X[i + 1] - X[i];

                    tdm.A[i] = 1D / dx1;
                    tdm.C[i] = 1D / dx2;
                    tdm.B[i] = 2D * (tdm.A[i] + tdm.C[i]);

                    dy1 = Y[i] - Y[i - 1];
                    dy2 = Y[i + 1] - Y[i];
                    R[i] = 3 * (dy1 / (dx1 * dx1) + dy2 / (dx2 * dx2));
                }

                if (double.IsNaN(endSlope))
                {
                    dx1 = X[N - 1] - X[N - 2];
                    dy1 = Y[N - 1] - Y[N - 2];
                    tdm.A[N - 1] = 1.0f / dx1;
                    tdm.B[N - 1] = 2.0f * tdm.A[N - 1];
                    R[N - 1] = 3 * (dy1 / (dx1 * dx1));
                }
                else
                {
                    tdm.B[N - 1] = 1;
                    R[N - 1] = endSlope;
                }

                double[] K = tdm.Solve(R);

                //Console.WriteLine(tdm.ToString());

                //Console.WriteLine("R = " + R.ToStringWithIndex());
                //Console.WriteLine("K = " + K.ToStringWithIndex());

                //Console.WriteLine("R: {0}", ArrayUtilToString(R));
                //Console.WriteLine("K: {0}", ArrayUtilToString(K));

                A = new double[N - 1];
                B = new double[N - 1];

                for (int i = 1; i < N; i++)
                {
                    dx1 = X[i] - X[i - 1];
                    dy1 = Y[i] - Y[i - 1];
                    A[i - 1] = K[i - 1] * dx1 - dy1;    // Equation 10
                    B[i - 1] = -K[i] * dx1 + dy1;       // Equation 11
                }

                //Console.WriteLine("A = " + A.ToStringWithIndex());
                //Console.WriteLine("B = " + B.ToStringWithIndex());

                //Console.WriteLine("A: {0}", ArrayUtilToString(A));
                //Console.WriteLine("B: {0}", ArrayUtilToString(B));
            }
            else
            {
                throw new Exception("x array and y array size should be the same, the size should be greater than 1.");
            }    
        }

        public static string ArrayUtilToString<T>(T[] array, string format = "")
        {
            var s = new StringBuilder();
            string formatString = "{0" + format + "}";

            for (int i = 0; i < array.Length; i++)
            {
                if (i < array.Length - 1)
                {
                    s.AppendFormat(formatString + ", ", array[i]);
                }
                else
                {
                    s.AppendFormat(formatString, array[i]);
                }
            }

            return s.ToString();
        }

        public double[] Evaluate(IEnumerable<double> xin) 
        {
            int n = xin.Count();
            double[] y = new double[n];

            int pt = 0, i = 0;
            foreach (double x in xin) 
            {
                if (x < X[0] || x > X[N - 1])
                    y[i] = double.NaN;

                if (x < X[pt])
                {
                    throw new ArgumentException("The X values to evaluate must be sorted.");
                }

                while ((pt < X.Length - 2) && (x > X[pt + 1]))
                {
                    pt++;
                }
                /*
                while (pt < N - 1)
                {
                    if (x >= X[pt])
                        break;
                    else
                        pt++;
                }*/

                double dx = X[pt + 1] - X[pt];
                double t = (x - X[pt]) / dx;
                y[i] = (1 - t) * Y[pt] + t * Y[pt + 1] + t * (1 - t) * (A[pt] * (1 - t) + B[pt] * t);

                //Console.WriteLine("xs = {0}, j = {1}, t = {2}", x, pt, t);

                i++;
            }

            return y.ToArray();
        }

        public double Evaluate(double x)
        {
            if (x < X[0] || x > X[N - 1]) 
                return double.NaN;

            int pt = 0;
            while (pt < N - 1)
            {
                if (x >= X[pt])
                    break;
                else
                    pt++;
            }

            double dx = X[pt + 1] - X[pt];
            double t = (x - X[pt]) / dx;
            return (1 - t) * Y[pt] + t * Y[pt + 1] + t * (1 - t) * (A[pt] * (1 - t) + B[pt] * t);
        }

        public int N { get; }

        public double[] X { get; }

        public double[] Y { get; }

        public double[] A { get; }

        public double[] B { get; }


        public static void Test() 
        {
            int n = 6;

            // Create the data to be fitted
            double[] x = new double[n];
            double[] y = new double[n];
            Random rand = new(1);

            for (int i = 0; i < n; i++)
            {
                x[i] = i;
                y[i] = rand.NextDouble() * 10;
            }

            // Compute the x values at which we will evaluate the spline.
            // Upsample the original data by a const factor.
            int upsampleFactor = 10;
            int nInterpolated = n * upsampleFactor;
            double[] xs = new double[nInterpolated];

            for (int i = 0; i < nInterpolated; i++)
            {
                xs[i] = i * (n - 1D) / (nInterpolated - 1D);
            }

            CubicSpline spline = new(x, y, 0.0D, double.NaN);

            var result = spline.Evaluate(xs);

            //double[] ys = CubicSpline.Compute(x, y, xs, 0.0f, Single.NaN, true);

            Console.WriteLine(result.ToStringWithIndex());
        }
    }
}
