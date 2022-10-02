using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using Nitride;
using Nitride.Chart;
using Nitride.Plot;

namespace Nitride.EE
{
    public class FreqTrace
    {
        public FreqTrace(IEnumerable<(double Freq, double Value)> traceData)
        {
            Data = traceData;
            //StartFreq = traceData.First().Freq;
            //StopFreq = traceData.Last().Freq;
            Count = Data.Count();
            //FreqStep = (StopFreq - StartFreq) / (Count - 1);
        }

        public virtual IEnumerable<(double Freq, double Value)> Data { get; }

        public double this[int i] => Data.ElementAt(i).Value;

        public virtual int Count { get; }

        /*
        public virtual double StartFreq { get; }

        public virtual double StopFreq { get; }

        public virtual double FreqStep { get; }*/

        public virtual void Evaluate(FreqTable pixTable, TraceFrame frame)
        {
            for (int i = 0; i < pixTable.Count; i++)
            {
                FreqRow prow = pixTable[i];
                prow[frame.TraceColumn] = prow[frame.HighValueColumn] = prow[frame.LowValueColumn] = Data.ElementAt(i).Value;// Peak detection!
            }
        }
    }


    public class FreqTracePeak : FreqTrace
    {
        public FreqTracePeak(IEnumerable<(double Freq, double Value)> traceData) : base(traceData) { }

        public override void Evaluate(FreqTable pixTable, TraceFrame frame)
        {
            double freq_min, freq_max, high, low, f, d;
            int j = 0;

            for (int i = 0; i < pixTable.Count; i++)
            {
                FreqRow prow = pixTable[i];

                freq_min = prow.Frequency - (pixTable.FreqStep / 2);
                freq_max = prow.Frequency + (pixTable.FreqStep / 2);

                high = double.MinValue;
                low = double.MaxValue;

                while (j < Count)
                {
                    var drow = Data.ElementAt(j);
                    f = drow.Freq;
                    d = drow.Value;

                    if (f >= freq_min && f < freq_max)
                    {
                        if (high < d) high = d;
                        if (low > d) low = d;
                    }
                    else
                    {
                        break;
                    }

                    j++;
                }

                //Console.WriteLine("h_value = " + high + " | l_value = " + low);

                prow[frame.TraceColumn] = prow[frame.HighValueColumn] = high;// Peak detection!
                prow[frame.LowValueColumn] = low;
            }
        }
    }

    public class FreqTraceNegativePeak : FreqTrace
    {
        public FreqTraceNegativePeak(IEnumerable<(double Freq, double Value)> traceData) : base(traceData) { }

        public override void Evaluate(FreqTable pixTable, TraceFrame frame)
        {
            double freq_min, freq_max, high, low, f, d;
            int j = 0;

            for (int i = 0; i < pixTable.Count; i++)
            {
                FreqRow prow = pixTable[i];

                freq_min = prow.Frequency - (pixTable.FreqStep / 2);
                freq_max = prow.Frequency + (pixTable.FreqStep / 2);

                high = double.MinValue;
                low = double.MaxValue;

                while (j < Count)
                {
                    var drow = Data.ElementAt(j);
                    f = drow.Freq;
                    d = drow.Value;

                    if (f >= freq_min && f < freq_max)
                    {
                        if (high < d) high = d;
                        if (low > d) low = d;
                    }
                    else
                    {
                        break;
                    }

                    j++;
                }

                prow[frame.HighValueColumn] = high;// Peak detection!
                prow[frame.TraceColumn] = prow[frame.LowValueColumn] = low;
            }
        }
    }

    public class FreqTraceAverage : FreqTrace
    {
        public FreqTraceAverage(IEnumerable<(double Freq, double Value)> traceData) : base(traceData) { }

        public override void Evaluate(FreqTable pixTable, TraceFrame frame)
        {
            double freq_min, freq_max, high, low, f, d;
            int j = 0;

            for (int i = 0; i < pixTable.Count; i++)
            {
                FreqRow prow = pixTable[i];

                freq_min = prow.Frequency - (pixTable.FreqStep / 2);
                freq_max = prow.Frequency + (pixTable.FreqStep / 2);

                high = double.MinValue;
                low = double.MaxValue;

                while (j < Count)
                {
                    var drow = Data.ElementAt(j);
                    f = drow.Freq;
                    d = drow.Value;

                    if (f >= freq_min && f < freq_max)
                    {
                        if (high < d) high = d;
                        if (low > d) low = d;
                    }
                    else
                    {
                        break;
                    }

                    j++;
                }

                prow[frame.HighValueColumn] = high;// Peak detection!
                prow[frame.LowValueColumn] = low;
                prow[frame.TraceColumn] = (high + low) / 2;
            }
        }
    }

    public class FreqTraceMean : FreqTrace
    {
        public FreqTraceMean(IEnumerable<(double Freq, double Value)> traceData) : base(traceData) { }

        public override void Evaluate(FreqTable pixTable, TraceFrame frame)
        {
            double freq_min, freq_max, high, low, f, d, cnt, sum;
            int j = 0;

            for (int i = 0; i < pixTable.Count; i++)
            {
                FreqRow prow = pixTable[i];

                freq_min = prow.Frequency - (pixTable.FreqStep / 2);
                freq_max = prow.Frequency + (pixTable.FreqStep / 2);

                high = double.MinValue;
                low = double.MaxValue;

                cnt = 0;
                sum = 0;

                while (j < Count)
                {
                    var drow = Data.ElementAt(j);
                    f = drow.Freq;
                    d = drow.Value;
                    cnt++;

                    if (f >= freq_min && f < freq_max)
                    {
                        if (high < d) high = d;
                        if (low > d) low = d;
                        cnt++;
                        sum += d;
                    }
                    else
                    {
                        break;
                    }

                    j++;
                }

                prow[frame.HighValueColumn] = high;// Peak detection!
                prow[frame.LowValueColumn] = low;
                prow[frame.TraceColumn] = sum / cnt;
            }
        }
    }

    public class FreqTraceRms : FreqTrace
    {
        public FreqTraceRms(IEnumerable<(double Freq, double Value)> traceData) : base(traceData) { }

        public override void Evaluate(FreqTable pixTable, TraceFrame frame)
        {
            double freq_min, freq_max, high, low, f, d, cnt, sum;
            int j = 0;

            for (int i = 0; i < pixTable.Count; i++)
            {
                FreqRow prow = pixTable[i];

                freq_min = prow.Frequency - (pixTable.FreqStep / 2);
                freq_max = prow.Frequency + (pixTable.FreqStep / 2);

                high = double.MinValue;
                low = double.MaxValue;

                cnt = 0;
                sum = 0;

                while (j < Count)
                {
                    var drow = Data.ElementAt(j);
                    f = drow.Freq;
                    d = drow.Value;  // d = (drow.Value * 1) + 0;
                    cnt++;

                    if (f >= freq_min && f < freq_max)
                    {
                        if (high < d) high = d;
                        if (low > d) low = d;
                        cnt++;
                        sum += d * d;
                    }
                    else
                    {
                        break;
                    }

                    j++;
                }

                prow[frame.HighValueColumn] = high;// Peak detection!
                prow[frame.LowValueColumn] = low;
                prow[frame.TraceColumn] = Math.Sqrt(sum / cnt);
            }
        }
    }

    public class FreqTraceSpline : FreqTrace
    {
        public FreqTraceSpline(IEnumerable<(double Freq, double Value)> traceData, double startSlope = double.NaN, double endSlope = double.NaN) : base(traceData)
        {
            if (Count > 1)
            {
                N = Count;

                double[] R = new double[N];
                X = traceData.Select(n => n.Freq).ToArray();
                Y = traceData.Select(n => n.Value).ToArray();

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

        }

        public int N { get; }

        public double[] X { get; }

        public double[] Y { get; }

        public double[] A { get; }

        public double[] B { get; }

        public override void Evaluate(FreqTable pixTable, TraceFrame frame)
        {
            int pt = 0;
            double y;
            for (int i = 0; i < pixTable.Count; i++)
            {
                FreqRow prow = pixTable[i];
                double x = prow.Frequency;
                y = double.NaN;

                if (x < X[pt])
                {
                    throw new ArgumentException("The X values to evaluate must be sorted.");
                }
                else if (x >= X[0] && x <= X[N - 1])
                {
                    while ((pt < X.Length - 2) && (x > X[pt + 1]))
                    {
                        pt++;
                    }

                    double dx = X[pt + 1] - X[pt];
                    double t = (x - X[pt]) / dx;
                    y = (1 - t) * Y[pt] + t * Y[pt + 1] + t * (1 - t) * (A[pt] * (1 - t) + B[pt] * t);
                }

                prow[frame.TraceColumn] = prow[frame.LowValueColumn] = prow[frame.HighValueColumn] = y;
            }
        }
    }
}
