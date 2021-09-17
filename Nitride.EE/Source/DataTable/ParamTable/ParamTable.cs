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
    public class ParamTable : DataTable
    {
        public ParamTable(ParamType type, int portCount, double z0 = 50)
        {
            Type = type;
            PortCount = portCount;
            Z0 = z0;
        }

        public ParamTable(IEnumerable<double> freqList, ParamType type, int portCount, double z0 = 50)
        {
            Type = type;
            PortCount = portCount;
            Z0 = z0;

            int pt = 0;
            foreach (double freq in freqList)
            {
                ParamRows.Add(new ParamRow(freq, pt, this));
                pt++;
            }
        }

        public ParamTable(double startFreq, double stopFreq, int numOfPts, ParamType type, int portCount, double z0 = 50)
        {
            Type = type;
            PortCount = portCount;
            Z0 = z0;
            Configure(startFreq, stopFreq, numOfPts);
        }

        public ParamTable(double startFreq, double stopFreq, double stepFreq, ParamType type, int portCount, double z0 = 50)
        {
            Type = type;
            PortCount = portCount;
            Z0 = z0;
            Configure(startFreq, stopFreq, stepFreq);
        }

        public ParamTable(string fileName)
        {
            string suffix = fileName.Split('.').Last();

            //int portCount = -1;
            int validFieldCount = -1;
            //double normalized = -1;
            double freqUnit = 0;
            TouchstoneFormat format = TouchstoneFormat.Invalid;

            if (suffix.StartsWith('s') && suffix.EndsWith('p'))
            {
                PortCount = suffix.TrimStart('s').TrimEnd('p').ToInt32(-1);
            }

            if (PortCount > 0)
            {
                using var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using StreamReader sr = new(fs);

                bool validHeader = false;
                int linePt = 0;

                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine().Split('!').First().Trim();
                    if (line.Length > 0)// && (!line.StartsWith('!')))
                    {
                        if (line.StartsWith('#'))
                        {
                            var fields = line.Split(' ');
                            if (fields.Length == 6)
                            {
                                freqUnit = fields[1].ToUpper() switch
                                {
                                    "HZ" => 1,
                                    "KHZ" => 1e3,
                                    "MHZ" => 1e6,
                                    "GHZ" => 1e9,
                                    _ => throw new Exception("Invalid SnP file format.")
                                };

                                format = fields[3].ToUpper() switch
                                {
                                    "RI" => TouchstoneFormat.RealImaginary,
                                    "DB" => TouchstoneFormat.dBAngle,
                                    "MA" => TouchstoneFormat.MagnitudeAngle,
                                    _ => throw new Exception("Invalid SnP file format.")
                                };

                                Type = fields[2].ToUpper() switch
                                {
                                    "S" => ParamType.S,
                                    "Z" => ParamType.Z,
                                    "Y" => ParamType.Y,
                                    "G" => ParamType.G,
                                    _ => throw new Exception("Invalid SnP file format.")
                                };

                                Z0 = fields[5].ToDouble();

                                if (Z0 > 0)
                                {
                                    Console.WriteLine("PortCount = " + PortCount + "; unit = " + freqUnit + "; type = " + Type + "; format = " + format + "; Z0 = " + Z0);
                                    validFieldCount = 1 + (2 * PortCount * PortCount);
                                    validHeader = true;
                                }
                                else
                                    throw new Exception("Invalid SnP file format.");
                            }
                        }
                        else if (validHeader)
                        {
                            List<string> fields = new();
                            foreach (string s in line.Split(' '))
                            {
                                string field = s.Trim();
                                if (field.Length > 0)
                                {
                                    fields.Add(field);
                                }
                            }

                            if (fields.Count == validFieldCount)
                            {
                                double freq = fields[0].ToDouble() * freqUnit;
                                int pt = 1;
                                var row = new ParamRow(freq, linePt, this);
                                linePt++;

                                for (int i = 1; i <= PortCount; i++)
                                {
                                    for (int j = 1; j <= PortCount; j++)
                                    {
                                        if (format == TouchstoneFormat.RealImaginary)
                                        {
                                            row[j, i] = new(fields[pt].ToDouble(), fields[pt + 1].ToDouble());
                                        }

                                        pt += 2;
                                    }
                                }

                                ParamRows.Add(row);
                            }


                        }
                    }
                }
            }
        }

        ~ParamTable() => Dispose();

        public ParamType Type { get; }

        public int PortCount { get; }

        public double Z0 { get; }

        protected void Configure(double startFreq, double stopFreq, int numOfPts)
        {
            lock (DataLockObject)
            {
                ParamRows.Clear();
                Step = (stopFreq - startFreq) / (numOfPts - 1D);
                Start = startFreq;

                int pt = 0;
                for (int i = 0; i < numOfPts; i++)
                {
                    double freq = startFreq + (i * Step);
                    ParamRows.Add(new ParamRow(freq, pt, this));
                    Stop = freq;
                    pt++;
                }
            }
        }

        protected void Configure(double startFreq, double stopFreq, double stepFreq)
        {
            lock (DataLockObject)
            {
                ParamRows.Clear();
                Step = stepFreq;
                Start = startFreq;

                int pt = 0;
                for (double freq = startFreq; freq < stopFreq; freq += Step)
                {
                    ParamRows.Add(new ParamRow(freq, pt, this));
                    Stop = freq;
                    pt++;
                }
            }
        }

        public double Start { get; protected set; } = double.NaN; // => Count > 0 ? Rows.First().X : double.NaN;

        public double Stop { get; protected set; } = double.NaN; // => Count > 0 ? Rows.Last().X : double.NaN;

        public double Step { get; protected set; } = double.NaN;

        protected List<ParamRow> ParamRows { get; } = new();

        public override int Count => ParamRows.Count;

        public override void Clear()
        {
            lock (DataLockObject)
                ParamRows.Clear();
        }

        public static NumericColumn Column_S11 { get; } = new("S11");
        public static NumericColumn Column_S12 { get; } = new("S12");
        public static NumericColumn Column_S21 { get; } = new("S21");
        public static NumericColumn Column_S22 { get; } = new("S22");

        public void Calculate()
        {
            foreach (var row in ParamRows)
            {
                Complex s11 = row[1, 1];
                Complex s12 = row[1, 2];
                Complex s21 = row[2, 1];
                Complex s22 = row[2, 2];

                //Console.WriteLine("freq = " + row.Frequency + "; s11 = " + s11 + "; s21 = " + s21 + "; s12 = " + s12 + "; s22 = " + s22);
                Console.WriteLine("freq = " + row.Frequency + "; " + Type + "11 = " + s11 + "; " + Type + "21 = " + s21 + "; " + Type + "12 = " + s12 + "; " + Type + "22 = " + s22);

                row[Column_S11] = Type == ParamType.S ? 20 * Math.Log10(s11.Magnitude) : s11.Magnitude;
                row[Column_S21] = Type == ParamType.S ? 20 * Math.Log10(s21.Magnitude) : s21.Magnitude;
                row[Column_S12] = Type == ParamType.S ? 20 * Math.Log10(s12.Magnitude) : s12.Magnitude;
                row[Column_S22] = Type == ParamType.S ? 20 * Math.Log10(s22.Magnitude) : s22.Magnitude;
            }

        }

        public IEnumerable<double> FreqList => ParamRows.Select(n => n.Frequency).OrderBy(n => n);

        public IEnumerable<ParamRow> Rows => ParamRows.OrderBy(n => n.Frequency);

        public ParamRow this[int i]
        {
            get
            {
                lock (DataLockObject)
                    if (i >= Count || i < 0)
                        return null;
                    else
                        return ParamRows[i];
            }
        }

        public override double this[int i, NumericColumn column] => i >= Count || i < 0 ? double.NaN : ParamRows[i][column];

        public ParamTable GetZTable()
        {
            if (Type == ParamType.S && PortCount == 2)
            {
                double z0 = Z0;
                ParamTable zt = new(ParamType.Z, 2, z0);

                int pt = 0;
                foreach (var sr in ParamRows)
                {
                    ParamRow zr = new(sr.Frequency, pt, zt);
                    pt++;

                    Complex s11 = sr[1, 1];
                    Complex s12 = sr[1, 2];
                    Complex s21 = sr[2, 1];
                    Complex s22 = sr[2, 2];

                    Complex deltaS = ((1 - s11) * (1 - s22)) - (s12 * s21);

                    zr[1, 1] = (((1 + s11) * (1 - s22)) + (s12 * s21)) * z0 / deltaS;
                    zr[1, 2] = 2 * s12 * z0 / deltaS;
                    zr[2, 1] = 2 * s21 * z0 / deltaS;
                    zr[2, 2] = (((1 - s11) * (1 + s22)) + (s12 * s21)) * z0 / deltaS;

                    Console.WriteLine("freq = " + zr.Frequency + "; z11 = " + zr[1, 1] + "; z21 = " + zr[2, 1] + "; z12 = " + zr[1, 2] + "; z22 = " + zr[2, 2]);
                    zt.ParamRows.Add(zr);
                }

                return zt;
            }
            else
                throw new Exception("The function only supports 2 ports, S-Parameter table.");
        }

        public ParamTable Interpolate(IEnumerable<double> freqList, double startSlope = 0, double endSlope = 0)
        {
            freqList = freqList.OrderBy(n => n);
            ParamTable t = new(freqList, Type, PortCount, Z0);

            for (int i = 1; i <= PortCount; i++)
            {
                for (int j = 1; j <= PortCount; j++)
                {
                    CubicSpline spline_real = new(ParamRows.Select(n => n.Frequency), ParamRows.Select(n => n[i, j].Real), startSlope, endSlope);
                    CubicSpline spline_imag = new(ParamRows.Select(n => n.Frequency), ParamRows.Select(n => n[i, j].Imaginary), startSlope, endSlope);
                    var result_real = spline_real.Evaluate(freqList);
                    var result_imag = spline_imag.Evaluate(freqList);

                    Console.WriteLine("Getting S" + i + j);

                    int pt = 0;
                    foreach (ParamRow row in t.ParamRows)
                    {
                        row[i, j] = new Complex(result_real[pt], result_imag[pt]);
                        pt++;
                    }
                }
            }

            return t;
        }

        public Dictionary<double, Complex> GetImpedance(Dictionary<double, Complex> z2_list)
        {
            var freq_list = z2_list.Keys.OrderBy(n => n);

            ParamTable zt = Interpolate(freq_list);
            Dictionary<double, Complex> result = new();

            for (int i = 0; i < zt.Count; i++)
            {
                ParamRow zr = zt[i];
                double freq = zr.Frequency;

                Complex z11 = zr[1, 1]; // = new Complex(result_s11_real[pt], result_s11_imag[pt]);
                Complex z12 = zr[1, 2]; // = new Complex(result_s12_real[pt], result_s12_imag[pt]);
                Complex z21 = zr[2, 1]; // = new Complex(result_s21_real[pt], result_s21_imag[pt]);
                Complex z22 = zr[2, 2]; // = new Complex(result_s22_real[pt], result_s22_imag[pt]);
                Complex z2 = z2_list[freq];

                result[freq] = z11 - (z12 * z21 / (z22 + z2));
            }

            return result;
        }

        public static Dictionary<double, Complex> GetImpedance(Dictionary<double, Complex> z2_list, ParamTable st)
        {
            ParamTable zt0 = st.GetZTable();

            // Intrepolate zt
            double startSlope = 0D;
            double endSlope = double.NaN;

            CubicSpline spline_s11_real = new(zt0.ParamRows.Select(n => n.Frequency), zt0.ParamRows.Select(n => n[1, 1].Real), startSlope, endSlope);
            CubicSpline spline_s11_imag = new(zt0.ParamRows.Select(n => n.Frequency), zt0.ParamRows.Select(n => n[1, 1].Imaginary), startSlope, endSlope);
            CubicSpline spline_s12_real = new(zt0.ParamRows.Select(n => n.Frequency), zt0.ParamRows.Select(n => n[1, 2].Real), startSlope, endSlope);
            CubicSpline spline_s12_imag = new(zt0.ParamRows.Select(n => n.Frequency), zt0.ParamRows.Select(n => n[1, 2].Imaginary), startSlope, endSlope);
            CubicSpline spline_s21_real = new(zt0.ParamRows.Select(n => n.Frequency), zt0.ParamRows.Select(n => n[2, 1].Real), startSlope, endSlope);
            CubicSpline spline_s21_imag = new(zt0.ParamRows.Select(n => n.Frequency), zt0.ParamRows.Select(n => n[2, 1].Imaginary), startSlope, endSlope);
            CubicSpline spline_s22_real = new(zt0.ParamRows.Select(n => n.Frequency), zt0.ParamRows.Select(n => n[2, 2].Real), startSlope, endSlope);
            CubicSpline spline_s22_imag = new(zt0.ParamRows.Select(n => n.Frequency), zt0.ParamRows.Select(n => n[2, 2].Imaginary), startSlope, endSlope);
            /*
            CubicSpline spline_s11_mag = new(zt.ParamRows.Select(n => n.Frequency), zt.ParamRows.Select(n => n[1, 1].Magnitude), startSlope, endSlope);
            CubicSpline spline_s11_deg = new(zt.ParamRows.Select(n => n.Frequency), zt.ParamRows.Select(n => n[1, 1].Phase), startSlope, endSlope);
            CubicSpline spline_s12_mag = new(zt.ParamRows.Select(n => n.Frequency), zt.ParamRows.Select(n => n[1, 2].Magnitude), startSlope, endSlope);
            CubicSpline spline_s12_deg = new(zt.ParamRows.Select(n => n.Frequency), zt.ParamRows.Select(n => n[1, 2].Phase), startSlope, endSlope);
            CubicSpline spline_s21_mag = new(zt.ParamRows.Select(n => n.Frequency), zt.ParamRows.Select(n => n[2, 1].Magnitude), startSlope, endSlope);
            CubicSpline spline_s21_deg = new(zt.ParamRows.Select(n => n.Frequency), zt.ParamRows.Select(n => n[2, 1].Phase), startSlope, endSlope);
            CubicSpline spline_s22_mag = new(zt.ParamRows.Select(n => n.Frequency), zt.ParamRows.Select(n => n[2, 2].Magnitude), startSlope, endSlope);
            CubicSpline spline_s22_deg = new(zt.ParamRows.Select(n => n.Frequency), zt.ParamRows.Select(n => n[2, 2].Phase), startSlope, endSlope);
            */

            var freq_list = z2_list.Keys.OrderBy(n => n);

            var result_s11_real = spline_s11_real.Evaluate(freq_list);
            var result_s11_imag = spline_s11_imag.Evaluate(freq_list);
            var result_s12_real = spline_s12_real.Evaluate(freq_list);
            var result_s12_imag = spline_s12_imag.Evaluate(freq_list);
            var result_s21_real = spline_s21_real.Evaluate(freq_list);
            var result_s21_imag = spline_s21_imag.Evaluate(freq_list);
            var result_s22_real = spline_s22_real.Evaluate(freq_list);
            var result_s22_imag = spline_s22_imag.Evaluate(freq_list);

            ParamTable zt = new(ParamType.Z, 2, zt0.Z0);

            Dictionary<double, Complex> result = new();

            int pt = 0;
            foreach (double freq in freq_list)
            {
                ParamRow zr = new(freq, pt, zt);

                Complex z11 = zr[1, 1] = new Complex(result_s11_real[pt], result_s11_imag[pt]);
                Complex z12 = zr[1, 2] = new Complex(result_s12_real[pt], result_s12_imag[pt]);
                Complex z21 = zr[2, 1] = new Complex(result_s21_real[pt], result_s21_imag[pt]);
                Complex z22 = zr[2, 2] = new Complex(result_s22_real[pt], result_s22_imag[pt]);

                Complex z2 = z2_list[freq];

                result[freq] = z11 - (z12 * z21 / (z22 + z2));

                pt++;
                zt.ParamRows.Add(zr);
            }

            return result;
        }

        public ParamChart TestChart()
        {
            ParamChart pc = new("S-Parameter", this)
            {
                IndexCount = Count,
                ReadyToShow = true,
                StopPt = Count
            };

            pc.MainArea.AddSeries(new LineSeries(ParamTable.Column_S11)
            {
                Order = 0,
                Importance = Importance.Major,
                Name = "S11",
                LegendName = "S11",
                Color = Color.SkyBlue,
                IsAntialiasing = true,
                Tension = 0,
                HasTailTag = false
            });

            pc.MainArea.AddSeries(new LineSeries(ParamTable.Column_S12)
            {
                Order = 0,
                Importance = Importance.Major,
                Name = "S12",
                LegendName = "S12",
                Color = Color.DarkGreen,
                IsAntialiasing = true,
                Tension = 0,
                HasTailTag = false
            });

            pc.MainArea.AddSeries(new LineSeries(ParamTable.Column_S21)
            {
                Order = 0,
                Importance = Importance.Major,
                Name = "S21",
                LegendName = "S21",
                Color = Color.Red,
                IsAntialiasing = true,
                Tension = 0,
                HasTailTag = false
            });

            pc.MainArea.AddSeries(new LineSeries(ParamTable.Column_S22)
            {
                Order = 0,
                Importance = Importance.Major,
                Name = "S22",
                LegendName = "S22",
                Color = Color.Pink,
                IsAntialiasing = true,
                Tension = 0,
                HasTailTag = false
            });

            return pc;
        }
    }
}
