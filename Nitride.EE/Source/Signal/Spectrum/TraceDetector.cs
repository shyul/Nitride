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
    public class TraceDetector
    {
        public TraceDetector(FreqTrace trace)
        {
            //Trace = trace;
            Count = trace.Length;
            Data = trace.Data.Take(Count).ToList();
        }

        //public virtual FreqTrace Trace { get; }

        public List<FreqPoint> Data { get; }

        public double this[int i] => Data[i].Magnitude;

        public virtual int Count { get; }

        public virtual void Evaluate(SpectrumData sd, TraceFrame frame)
        {
            FreqTable pixTable = sd.FreqTable;
            for (int i = 0; i < pixTable.Count; i++)
            {
                FreqRow prow = pixTable[i];
                prow[frame.MagnitudeColumn] = prow[frame.MagnitudeHighColumn] = prow[frame.MagnitudeLowColumn] = Data[i].Magnitude;// Peak detection!
            }
        }
    }

    public class PeakTraceDetector : TraceDetector
    {
        public PeakTraceDetector(FreqTrace trace) : base(trace) { }

        public override void Evaluate(SpectrumData sd, TraceFrame frame)
        {
            FreqTable pixTable = sd.FreqTable;
            bool isLog = sd.IsLog;
            double high, low, f, d, multi, offset;
            int j = 0;

            //double a_h = double.MinValue;
            //double a_l = double.MaxValue;

            for (int i = 0; i < pixTable.Count; i++)
            {
                FreqRow prow = pixTable[i];
                multi = prow[SpectrumData.MultiCorrColumn];
                offset = prow[SpectrumData.OffsetCorrColumn];

                high = double.MinValue;
                low = double.MaxValue;

                while (j < Count)
                {
                    var drow = Data[j];
                    f = drow.Frequency;
                    d = drow.Magnitude;

                    if (prow.FreqRange.ContainsNoMax(f))
                    {
                        if (high < d) high = d;
                        if (low > d) low = d;
                    }
                    else if (prow.FreqRange.Max < f)
                    {
                        break;
                    }

                    j++;
                }

                // Console.WriteLine("h_value = " + high + " | l_value = " + low);

                prow[frame.MagnitudeColumn] = prow[frame.MagnitudeHighColumn] = multi * (isLog ? Math.Log10(high) : high) + offset; // Peak detection!
                double res = prow[frame.MagnitudeLowColumn] = multi * (isLog ? Math.Log10(low) : low) + offset;

               // if (res > a_h) a_h = res;
               // if (res < a_l) a_l = res;
            }

            //Console.WriteLine("a_h = " + a_h + " | a_l = " + a_l);


        }
    }

    public class NegativePeakTraceDetector : TraceDetector
    {
        public NegativePeakTraceDetector(FreqTrace trace) : base(trace) { }

        public override void Evaluate(SpectrumData sd, TraceFrame frame)
        {
            FreqTable pixTable = sd.FreqTable;
            bool isLog = sd.IsLog;
            double high, low, f, d, multi, offset;
            int j = 0;

            for (int i = 0; i < pixTable.Count; i++)
            {
                FreqRow prow = pixTable[i];
                multi = prow[SpectrumData.MultiCorrColumn];
                offset = prow[SpectrumData.OffsetCorrColumn];

                high = double.MinValue;
                low = double.MaxValue;

                while (j < Count)
                {
                    var drow = Data[j];
                    f = drow.Frequency;
                    d = drow.Magnitude; // prow[SpectrumData.MultiCorrColumn] * (isLog ? Math.Log10(drow.Magnitude) : drow.Magnitude) + prow[SpectrumData.OffsetCorrColumn];

                    if (prow.FreqRange.ContainsNoMax(f))
                    {
                        if (high < d) high = d;
                        if (low > d) low = d;
                    }
                    else if (prow.FreqRange.Max < f)
                    {
                        break;
                    }

                    j++;
                }

                prow[frame.MagnitudeHighColumn] = multi * (isLog ? Math.Log10(high) : high) + offset; // high;// Peak detection!
                prow[frame.MagnitudeColumn] = prow[frame.MagnitudeLowColumn] = multi * (isLog ? Math.Log10(low) : low) + offset;
            }
        }
    }

    public class AverageTraceDetector : TraceDetector
    {
        public AverageTraceDetector(FreqTrace trace) : base(trace) { }

        public override void Evaluate(SpectrumData sd, TraceFrame frame)
        {
            FreqTable pixTable = sd.FreqTable;
            bool isLog = sd.IsLog;
            double high, low, f, d, multi, offset;
            int j = 0;

            for (int i = 0; i < pixTable.Count; i++)
            {
                FreqRow prow = pixTable[i];
                multi = prow[SpectrumData.MultiCorrColumn];
                offset = prow[SpectrumData.OffsetCorrColumn];

                high = double.MinValue;
                low = double.MaxValue;

                while (j < Count)
                {
                    var drow = Data[j];
                    f = drow.Frequency;
                    d = drow.Magnitude;

                    if (prow.FreqRange.ContainsNoMax(f))
                    {
                        if (high < d) high = d;
                        if (low > d) low = d;
                    }
                    else if (prow.FreqRange.Max < f)
                    {
                        break;
                    }

                    j++;
                }

                high = prow[frame.MagnitudeHighColumn] = multi * (isLog ? Math.Log10(high) : high) + offset; // high;// Peak detection!
                low = prow[frame.MagnitudeLowColumn] = multi * (isLog ? Math.Log10(low) : low) + offset;
                prow[frame.MagnitudeColumn] = (high + low) / 2;
            }
        }
    }

    public class MeanTraceDetector : TraceDetector
    {
        public MeanTraceDetector(FreqTrace trace) : base(trace) { }

        public override void Evaluate(SpectrumData sd, TraceFrame frame)
        {
            FreqTable pixTable = sd.FreqTable;
            bool isLog = sd.IsLog;
            double high, low, f, d, cnt, sum, multi, offset;
            int j = 0;

            for (int i = 0; i < pixTable.Count; i++)
            {
                FreqRow prow = pixTable[i];
                multi = prow[SpectrumData.MultiCorrColumn];
                offset = prow[SpectrumData.OffsetCorrColumn];

                high = double.MinValue;
                low = double.MaxValue;

                cnt = 0;
                sum = 0;

                while (j < Count)
                {
                    var drow = Data[j];
                    f = drow.Frequency;
                    d = drow.Magnitude;

                    if (prow.FreqRange.ContainsNoMax(f))
                    {
                        if (high < d) high = d;
                        if (low > d) low = d;
                        cnt++;
                        sum += d;
                    }
                    else if (prow.FreqRange.Max < f)
                    {
                        break;
                    }

                    j++;
                }

                prow[frame.MagnitudeHighColumn] = multi * (isLog ? Math.Log10(high) : high) + offset; // Peak detection!
                prow[frame.MagnitudeLowColumn] = multi * (isLog ? Math.Log10(low) : low) + offset;

                double mean = sum / cnt;
                prow[frame.MagnitudeColumn] = multi * (isLog ? Math.Log10(mean) : mean) + offset;
            }
        }
    }

    public class RmsTraceDetector : TraceDetector
    {
        public RmsTraceDetector(FreqTrace trace, double corOffset = 1000) : base(trace) 
        {
            CompensateOffset = corOffset;
        }

        double CompensateOffset { get; }

        public override void Evaluate(SpectrumData sd, TraceFrame frame)
        {
            FreqTable pixTable = sd.FreqTable;
            bool isLog = sd.IsLog;
            double high, low, f, d, cnt, sum, multi, offset; //, avg;
            int j = 0;

            for (int i = 0; i < pixTable.Count; i++)
            {
                FreqRow prow = pixTable[i];
                multi = prow[SpectrumData.MultiCorrColumn];
                offset = prow[SpectrumData.OffsetCorrColumn];

                high = double.MinValue;
                low = double.MaxValue;

                cnt = 0;
                sum = 0;

                while (j < Count)
                {
                    var drow = Data[j];
                    f = drow.Frequency;
                    d = drow.Magnitude;

                    if (prow.FreqRange.ContainsNoMax(f))
                    {
                        if (high < d) high = d;
                        if (low > d) low = d;
                        cnt++;
                        d += CompensateOffset;
                        sum += d * d;
                    }
                    else if (prow.FreqRange.Max < f)
                    {
                        break;
                    }

                    j++;
                }

                prow[frame.MagnitudeHighColumn] = multi * (isLog ? Math.Log10(high) : high) + offset; // Peak detection!
                prow[frame.MagnitudeLowColumn] = multi * (isLog ? Math.Log10(low) : low) + offset;
                // avg = (high + low) / 2; 

                double mean = Math.Sqrt(sum / cnt) - CompensateOffset;
                double rms = prow[frame.MagnitudeColumn] = multi * (isLog ? Math.Log10(mean) : mean) + offset;
                // Console.WriteLine("high = " + high + " | low = " + low + " | rms = " + rms);
            }
        }
    }

    public class SplineTraceDetector : TraceDetector
    {
        public SplineTraceDetector(FreqTrace trace, double startSlope = double.NaN, double endSlope = double.NaN) : base(trace)
        {
            if (Count > 1)
            {
                N = Count;

                double[] R = new double[N];

                var data = Data.Take(Count);

                X = data.Select(n => n.Frequency).ToArray();
                Y = data.Select(n => n.Magnitude).ToArray();

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

        public override void Evaluate(SpectrumData sd, TraceFrame frame)
        {
            FreqTable pixTable = sd.FreqTable;
            bool isLog = sd.IsLog;
            int pt = 0;
            double y, multi, offset;
            for (int i = 0; i < pixTable.Count; i++)
            {
                FreqRow prow = pixTable[i];
                multi = prow[SpectrumData.MultiCorrColumn];
                offset = prow[SpectrumData.OffsetCorrColumn];

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

                prow[frame.MagnitudeColumn] = prow[frame.MagnitudeLowColumn] = prow[frame.MagnitudeHighColumn] = multi * (isLog ? Math.Log10(y) : y) + offset;
            }
        }
    }
}
