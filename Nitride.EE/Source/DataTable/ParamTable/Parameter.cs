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
    public abstract class Parameter
    {
        public string Name { get; protected set; }

        public int PortCount { get; protected set; }

        private Dictionary<(int p1, int p2), ComplexColumn> ParamColumnLUT { get; } = new();

        public ComplexColumn this[int p1, int p2]
        {
            get
            {
                if (!ParamColumnLUT.ContainsKey((p1, p2)))
                {
                    string label = this switch
                    {
                        S_Parameter => "S",
                        Z_Parameter => "Z",
                        _ => "Unknown"
                    } + p1 + p2;

                    ParamColumnLUT.Add((p1, p2), new ComplexColumn(GetType().Name + "_" + Name + "_" + p1 + p2, label));
                }

                return ParamColumnLUT[(p1, p2)];
            }
        }

        private Dictionary<(int p1, int p2, ComplexFormat format), NumericColumn> ParamFormatColumnLUT { get; } = new();

        public NumericColumn this[int p1, int p2, ComplexFormat format]
        {
            get
            {
                if (!ParamFormatColumnLUT.ContainsKey((p1, p2, format)))
                {
                    string label = this switch
                    {
                        S_Parameter => "S",
                        Z_Parameter => "Z",
                        _ => "Unknown"
                    } + p1 + p2;

                    label = format + "(" + label + ")";


                    ParamFormatColumnLUT[(p1, p2, format)] = new NumericColumn(GetType().Name + "_" + Name + "_" + format + "_" + p1 + p2, label);
                }

                return ParamFormatColumnLUT[(p1, p2, format)];
            }
        }

        public void CalculateOtherFormats(FreqTable ft)
        {
            foreach (var row in ft.Rows)
            {
                Complex p11 = row[this[1, 1]];
                Complex p12 = row[this[1, 2]];
                Complex p21 = row[this[2, 1]];
                Complex p22 = row[this[2, 2]];

                //Console.WriteLine("freq = " + row.Frequency + "; " + "Mag11 = " + p11.Magnitude + "; " + "Mag21 = " + p21.Magnitude + "; " + "Mag12 = " + p12.Magnitude + "; " + "Mag22 = " + p22.Magnitude);
                //Console.WriteLine("CalculateFormat: freq = " + row.Frequency + "; " + "p11 = " + p11 + "; " + "p21 = " + p21 + "; " + "p12 = " + p12 + "; " + "p22 = " + p22);

                row[this[1, 1, ComplexFormat.Magnitude]] = p11.Magnitude;
                row[this[1, 1, ComplexFormat.Phase]] = p11.Phase;
                row[this[1, 1, ComplexFormat.Real]] = p11.Real;
                row[this[1, 1, ComplexFormat.Image]] = p11.Imaginary;
                row[this[1, 1, ComplexFormat.dB]] = 20 * Math.Log10(p11.Magnitude);

                row[this[2, 1, ComplexFormat.Magnitude]] = p21.Magnitude;
                row[this[2, 1, ComplexFormat.Phase]] = p21.Phase;
                row[this[2, 1, ComplexFormat.Real]] = p21.Real;
                row[this[2, 1, ComplexFormat.Image]] = p21.Imaginary;
                row[this[2, 1, ComplexFormat.dB]] = 20 * Math.Log10(p21.Magnitude);

                row[this[1, 2, ComplexFormat.Magnitude]] = p12.Magnitude;
                row[this[1, 2, ComplexFormat.Phase]] = p12.Phase;
                row[this[1, 2, ComplexFormat.Real]] = p12.Real;
                row[this[1, 2, ComplexFormat.Image]] = p12.Imaginary;
                row[this[1, 2, ComplexFormat.dB]] = 20 * Math.Log10(p12.Magnitude);

                row[this[2, 2, ComplexFormat.Magnitude]] = p22.Magnitude;
                row[this[2, 2, ComplexFormat.Phase]] = p22.Phase;
                row[this[2, 2, ComplexFormat.Real]] = p22.Real;
                row[this[2, 2, ComplexFormat.Image]] = p22.Imaginary;
                row[this[2, 2, ComplexFormat.dB]] = 20 * Math.Log10(p22.Magnitude);
            }
        }

        public void Interpolate(FreqTable source, FreqTable dest, double startSlope = 0, double endSlope = 0)
        {
            var freqList = dest.FreqList;

            for (int i = 1; i <= PortCount; i++)
            {
                for (int j = 1; j <= PortCount; j++)
                {
                    CubicSpline spline_real = new(source.Rows.Select(n => n.Frequency), source.Rows.Select(n => n[this[i, j]].Real), startSlope, endSlope);
                    CubicSpline spline_imag = new(source.Rows.Select(n => n.Frequency), source.Rows.Select(n => n[this[i, j]].Imaginary), startSlope, endSlope);
                    var result_real = spline_real.Evaluate(freqList);
                    var result_imag = spline_imag.Evaluate(freqList);

                    //Console.WriteLine("Getting S" + i + j);

                    int pt = 0;
                    foreach (FreqRow row in dest.Rows)
                    {
                        row[this[i, j]] = new Complex(result_real[pt], result_imag[pt]);
                        pt++;
                    }
                }
            }
        }

        public static void LoadSnP(FreqTable ft, Parameter pr, string fileName)
        {
            string suffix = fileName.Split('.').Last();
            int validFieldCount = -1;
            double freqUnit = 0;
            int PortCount = 0;

            if (suffix.StartsWith("s") && suffix.EndsWith("p"))
            {
                PortCount = suffix.TrimStart('s').TrimEnd('p').ToInt32(-1);
            }
            //Console.WriteLine("PortCount = " + PortCount);
            if (PortCount == pr.PortCount)
            {
                using var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using StreamReader sr = new(fs);
                string format = string.Empty;
                bool validHeader = false;
                int linePt = 0;

                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine().Split('!').First().Trim();

                    if (line.Length > 0)// && (!line.StartsWith('!')))
                    {
                        if (line.StartsWith("#"))
                        {
                            while (line.Contains("  ")) line = line.Replace("  ", " ");
                            Console.WriteLine("Header line: " + line);

                            var fields = line.Split(' ');
                            if (fields.Length == 6)
                            {
                                freqUnit = fields[1].Trim().ToUpper() switch
                                {
                                    "HZ" => 1,
                                    "KHZ" => 1e3,
                                    "MHZ" => 1e6,
                                    "GHZ" => 1e9,
                                    _ => throw new Exception("Invalid SnP file format.")
                                };

                                format = fields[3].Trim().ToUpper();

                                if (format != "RI" && format != "DB" && format != "MA")
                                    throw new Exception("Invalid SnP file format.");

                                switch (fields[2].Trim().ToUpper())
                                {
                                    case "S": if (pr is S_Parameter sp && sp.Z0 == fields[5].Trim().ToDouble()) break; else return;
                                    case "Z": if (pr is Z_Parameter) break; else return;
                                    default: return;
                                };

                                validFieldCount = 1 + (2 * PortCount * PortCount);
                                validHeader = true;
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

                            if (fields.Count == validFieldCount && pr is Parameter)
                            {
                                double freq = fields[0].Trim().ToDouble() * freqUnit;
                                //if (freq < 500000) Console.WriteLine("freq = " + freq);
                                if (!ft.Contains(freq))
                                {
                                    int pt = 1;
                                    var row = new FreqRow(freq, linePt, ft);
                                    linePt++;

                                    for (int i = 1; i <= PortCount; i++)
                                    {
                                        for (int j = 1; j <= PortCount; j++)
                                        {
                                            row[pr[j, i]] = format switch
                                            {
                                                "RI" => new(fields[pt].ToDouble(), fields[pt + 1].ToDouble()),
                                                "DB" => Complex.FromPolarCoordinates(Math.Pow(10.0, fields[pt].ToDouble() / 20.0), fields[pt + 1].ToDouble() * Math.PI / 180.0),
                                                "MA" => Complex.FromPolarCoordinates(fields[pt].ToDouble(), fields[pt + 1].ToDouble() * Math.PI / 180.0),
                                                _ => throw new Exception("Invalid SnP file format.")
                                            };

                                            pt += 2;
                                        }
                                    }

                                    ft.FreqRows.Add(row);
                                }
                                else
                                {
                                    Console.WriteLine("Already has frequency point: " + freq);
                                }
                            }
                        }
                    }
                }
            }
        }



        public static (FreqTable, Parameter) LoadSnP(string fileName)
        {
            string suffix = fileName.Split('.').Last();

            //int portCount = -1;
            int validFieldCount = -1;
            //double normalized = -1;
            double freqUnit = 0;

            int PortCount = 0;
            FreqTable ft = new();
            Parameter pr = null;

            if (suffix.StartsWith("s") && suffix.EndsWith("p"))
            {
                PortCount = suffix.TrimStart('s').TrimEnd('p').ToInt32(-1);
            }
            Console.WriteLine("PortCount = " + PortCount);
            if (PortCount > 0)
            {
                using var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using StreamReader sr = new(fs);
                string format = string.Empty;
                bool validHeader = false;
                int linePt = 0;

                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine().Split('!').First().Trim();
                    if (line.Length > 0)// && (!line.StartsWith('!')))
                    {
                        if (line.StartsWith("#"))
                        {
                            while (line.Contains("  ")) line = line.Replace("  ", " ");
                            Console.WriteLine("Header line: " + line);

                            var fields = line.Split(' ');
                            if (fields.Length == 6)
                            {
                                freqUnit = fields[1].Trim().ToUpper() switch
                                {
                                    "HZ" => 1,
                                    "KHZ" => 1e3,
                                    "MHZ" => 1e6,
                                    "GHZ" => 1e9,
                                    _ => throw new Exception("Invalid SnP file format.")
                                };

                                format = fields[3].Trim().ToUpper();

                                if (format != "RI" && format != "DB" && format != "MA")
                                    throw new Exception("Invalid SnP file format.");

                                double Z0 = fields[5].Trim().ToDouble();

                                if (Z0 > 0)
                                {
                                    pr = fields[2].Trim().ToUpper() switch
                                    {
                                        "S" => new S_Parameter(fileName, PortCount, Z0), // ParamType.S,
                                        "Z" => new Z_Parameter(fileName, PortCount),
                                        //"Y" => ParamType.Y,
                                        //"G" => ParamType.G,
                                        _ => throw new Exception("Invalid SnP file format.")
                                    };

                                    Console.WriteLine("PortCount = " + PortCount + "; unit = " + freqUnit + "; type = " + fields[2] + "; format = " + format + "; Z0 = " + Z0);
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

                            if (fields.Count == validFieldCount && pr is Parameter)
                            {
                                double freq = fields[0].Trim().ToDouble() * freqUnit;

                                //if (freq < 500000) Console.WriteLine("freq = " + freq);

                                if (!ft.Contains(freq))
                                {
                                    int pt = 1;
                                    var row = new FreqRow(freq, linePt, ft);
                                    linePt++;

                                    //if (linePt < 20) Console.Write("freq = " + freq + " | ");

                                    for (int i = 1; i <= PortCount; i++)
                                    {
                                        for (int j = 1; j <= PortCount; j++)
                                        {
                                            /*
                                            if (format == "DB" && linePt < 20)
                                            {
                                                double dBMag = fields[pt].ToDouble();
                                                double magnitude = Math.Pow(10.0, dBMag / 20.0);


                                                double degree = fields[pt + 1].ToDouble();
                                                double radian = degree * Math.PI / 180;

                                                Complex c = Complex.FromPolarCoordinates(magnitude, radian);

                                                Console.Write("dBMag = " + dBMag + "Magnitude = " + magnitude + " | Degree = " + degree + " | Complex = " + c + " | ");

                                            }*/

                                            row[pr[j, i]] = format switch
                                            {
                                                "RI" => new(fields[pt].ToDouble(), fields[pt + 1].ToDouble()),
                                                "DB" => Complex.FromPolarCoordinates(Math.Pow(10.0, fields[pt].ToDouble() / 20.0), fields[pt + 1].ToDouble() * Math.PI / 180.0),
                                                "MA" => Complex.FromPolarCoordinates(fields[pt].ToDouble(), fields[pt + 1].ToDouble() * Math.PI / 180.0),
                                                _ => throw new Exception("Invalid SnP file format.")
                                            };

                                            pt += 2;
                                        }
                                    }

                                    //if (linePt < 20) Console.Write("\n");

                                    ft.FreqRows.Add(row);
                                }
                                else
                                {
                                    Console.WriteLine("Already has frequency point: " + freq);
                                }
                            }
                        }
                    }
                }
            }

            ft.Sort();

            return (ft, pr);
        }

        public List<Color> ColorIndex = new() { Color.YellowGreen, Color.Violet, Color.SkyBlue, Color.Pink, Color.Yellow, Color.Green, Color.Cyan };


        public FreqChart TestChart(FreqTable ft, ComplexFormat format = ComplexFormat.dB)
        {
            FreqChart pc = new("S-Parameter", ft)
            {
                IndexCount = ft.Count,
                ReadyToShow = true,
                StopPt = ft.Count
            };

            int pt = 0;

            for (int i = 1; i <= PortCount; i++)
            {
                for (int j = 1; j <= PortCount; j++)
                {
                    string label = this switch
                    {
                        S_Parameter => "S",
                        Z_Parameter => "Z",
                        _ => "Unknown"
                    } + i + j;

                    label = format + "(" + label + ")";

                    pc.MainArea.AddSeries(new LineSeries(this[i, j, format])
                    {
                        Order = 0,
                        Importance = Importance.Minor,
                        Name = label,
                        LegendName = label,
                        Color = ColorIndex[pt],
                        IsAntialiasing = true,
                        Tension = 0,
                        HasTailTag = false
                    });
                    pt++;
                }
            }

            return pc;
        }
    }
}
