/// ***************************************************************************
/// Nitride Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Basic functions for processing strings
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Nitride
{
    public static partial class StringTool
    {
        /// <summary>
        /// Clean up the string before parsing it to numbers.
        /// </summary>
        private static readonly char[] m_charsToTrimEnd = { ',', '.', ' ' };

        private static string CleanUpNumString(string input) => Regex.Replace(input, "[^0-9.Ee-]", "").TrimEnd(m_charsToTrimEnd).Trim();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static float ToFloat(this string input, float defaultValue = 0)
        {
            if (input == null) return defaultValue;

            try
            {
                string str = CleanUpNumString(input);
                if (string.IsNullOrWhiteSpace(str))
                    return defaultValue;
                else
                    return float.Parse(str, NumberStyles.Float | NumberStyles.Number | NumberStyles.AllowExponent);
            }
            catch (Exception e) when (e is FormatException || e is OverflowException)
            {
                Console.WriteLine("ToFloat Error: " + e + ", Input was: " + input);
                return defaultValue;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="input"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static double ToDouble(this string input, double defaultValue = double.NaN)
        {
            if (input == null) return defaultValue;

            try
            {
                string str = CleanUpNumString(input);
                if (string.IsNullOrWhiteSpace(str))
                    return defaultValue;
                else
                    return double.Parse(str, NumberStyles.Float | NumberStyles.Number | NumberStyles.AllowExponent); //return Convert.ToDouble(decimal.Parse(str));
            }
            catch (Exception e) when (e is FormatException || e is OverflowException)
            {
                Console.WriteLine("ToDouble Error: " + e + ", Input was: " + input);
                return defaultValue;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static ulong ToUInt64(this string input, ulong defaultValue = 0)
        {
            if (input == null) return defaultValue;

            try
            {
                string str = CleanUpNumString(input);
                if (str.Length > 0)
                {
                    return ulong.Parse(str);
                }
                else
                    return defaultValue;
            }
            catch (Exception e) when (e is FormatException || e is OverflowException)
            {
                Console.WriteLine("ToInt64 Error: " + e + ", Input was: " + input);
                return defaultValue;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static long ToInt64(this string input, long defaultValue = 0)
        {
            if (input == null) return defaultValue;

            try
            {
                string str = CleanUpNumString(input);
                if (str.Length > 0)
                {
                    return long.Parse(str);
                }
                else
                    return defaultValue;
            }
            catch (Exception e) when (e is FormatException || e is OverflowException)
            {
                Console.WriteLine("ToInt64 Error: " + e + ", Input was: " + input);
                return defaultValue;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static long ToInt64(this double input, long defaultValue = 0)
        {
            if ((!(input is double.NaN)) && (!double.IsInfinity(input)))
                return Convert.ToInt64(Math.Round(input, MidpointRounding.AwayFromZero));
            else
                return defaultValue;
        }

        /// <summary>
        /// Convert Double to Int32 directly with truncation, and possibly but improbably crash your system
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static int ToInt32(this double input, int defaultValue = 0) => (int)ToInt64(input, defaultValue);

        public static uint ToUInt32(this double input, uint defaultValue = 0) => (uint)ToInt64(input, defaultValue);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static uint ToUInt32(this string input, uint defaultValue = 0)
        {
            if (input == null) return defaultValue;

            try
            {
                string str = CleanUpNumString(input);
                if (str.Length > 0)
                {
                    return uint.Parse(str);
                }
                else
                    return defaultValue;
            }
            catch (Exception e) when (e is FormatException || e is OverflowException)
            {
                Console.WriteLine("ToInt64 Error: " + e + ", Input was: " + input);
                return defaultValue;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int ToInt32(this string input, int defaultValue = 0)
        {
            if (input == null) return defaultValue;

            try
            {
                string str = CleanUpNumString(input);
                if (str.Length > 0)
                {
                    return int.Parse(str);
                }
                else
                    return defaultValue;
            }
            catch (Exception e) when (e is FormatException || e is OverflowException)
            {
                Console.WriteLine("ToInt64 Error: " + e + ", Input was: " + input);
                return defaultValue;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static int ToInt32(this float input) => Convert.ToInt32(Math.Round(input, MidpointRounding.AwayFromZero));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buff"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static int ToInt32(this byte[] buff, int offset)
        {
            if (offset + 5 > buff.Length) throw new OutOfMemoryException();

            if (BitConverter.IsLittleEndian)
            {
                return BitConverter.ToInt32(new Byte[] { buff[offset + 3], buff[offset + 2], buff[offset + 1], buff[offset] }, 0);
            }
            else
            {
                return BitConverter.ToInt32(new Byte[] { buff[offset], buff[offset + 1], buff[offset + 2], buff[offset + 3] }, 0);
            }
        }

        public static short ToInt16(this string input, short defaultValue = 0)
        {
            if (input == null) return defaultValue;

            try
            {
                string str = CleanUpNumString(input);
                if (str.Length > 0)
                {
                    return short.Parse(str);
                }
                else
                    return defaultValue;
            }
            catch (Exception e) when (e is FormatException || e is OverflowException)
            {
                Console.WriteLine("ToInt64 Error: " + e + ", Input was: " + input);
                return defaultValue;
            }
        }

        public static ushort ToUInt16(this string input, ushort defaultValue = 0)
        {
            if (input == null) return defaultValue;

            try
            {
                string str = CleanUpNumString(input);
                if (str.Length > 0)
                {
                    return ushort.Parse(str);
                }
                else
                    return defaultValue;
            }
            catch (Exception e) when (e is FormatException || e is OverflowException)
            {
                Console.WriteLine("ToInt64 Error: " + e + ", Input was: " + input);
                return defaultValue;
            }
        }

        public static byte ToByte(this string input, byte defaultValue = 0)
        {
            if (input is null) 
                return defaultValue;

            try
            {
                string str = CleanUpNumString(input);
                if (str.Length > 0)
                {
                    return byte.Parse(str);
                }
                else
                    return defaultValue;
            }
            catch (Exception e) when (e is FormatException || e is OverflowException)
            {
                Console.WriteLine("ToInt64 Error: " + e + ", Input was: " + input);
                return defaultValue;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        /// <param name="dacades"></param>
        /// <returns></returns>
        public static double FitDacades(this double val, double[] dacades)
        {
            for (int i = -12; i <= 12; i++) // from small to big
            {
                double r = Math.Pow(10, i);
                for (int j = 0; j < dacades.Length; j++)
                {
                    double res = val / (r * dacades[j]);
                    if (res <= 1 && res >= -1)
                    {
                        return r * dacades[j];
                    }
                }
            }
            return val;
        }

        /// <summary>
        /// Convert large double number to finance number string
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static (double Number, string String) ToUnitPrefixNumber3String(this double d, string format = "")
        {
            double num = d;
            if (d > -1e-12 && d < 1e-12)
            {
                num = d * 1e15;
                return (num, num.ToNumber3String(format) + "f");
            }
            if ((d > -1e-9 && d <= -1e-12) || (d < 1e-9 && d >= 1e-12))
            {
                num = d * 1e12;
                return (num, num.ToNumber3String(format) + "p");
            }
            else if ((d > -1e-6 && d <= -1e-9) || (d < 1e-6 && d >= 1e-9))
            {
                num = d * 1e9;
                return (num, num.ToNumber3String(format) + "n");
            }
            else if ((d > -1e-3 && d <= -1e-6) || (d < 1e-3 && d >= 1e-6))
            {
                num = d * 1e6;
                return (num, num.ToNumber3String(format) + "μ");
            }
            else if ((d > -1 && d <= -1e-3) || (d < 1 && d >= 1e-3))
            {
                num = d * 1e3;
                return (num, num.ToNumber3String(format) + "m");
            }
            else if ((d >= 1e3 && d < 1e6) || (d <= -1e3 && d > -1e6))
            {
                num = d / 1e3;
                return (num, num.ToNumber3String(format) + "K");
            }
            else if ((d >= 1e6 && d < 1e9) || (d <= -1e6 && d > -1e9))
            {
                num = d / 1e6;
                return (num, num.ToNumber3String(format) + "M");
            }
            else if ((d >= 1e9 && d < 1e12) || (d <= -1e9 && d > -1e12))
            {
                num = d / 1e9;
                return (num, num.ToNumber3String(format) + "G");
            }
            else if (d >= 1e12 || d <= -1e12)
            {
                num = d / 1e12;
                return (num, num.ToNumber3String(format) + "T");
            }
            else
            {
                return (num, d.ToString(format));
            }
        }

        /// <summary>
        /// Convert double number to finance number string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToNumber3String(this double input, string format)
        {
            if (Double.IsNaN(input))
            {
                return "N/A";
            }
            else if (input >= 10000 || input <= -10000)
            {
                return input.ToString("N0", CultureInfo.InvariantCulture);
            }
            else if (input > -10 && input < 10)
            {
                input = ((input * 1000.0).ToInt64() / 1000.0);
                return input.ToString(format); //("0.###");
            }
            else
            {
                input = ((input * 1000.0).ToInt64() / 1000.0);
                return input.ToString(format); // ("0.##");
            }
        }


        public static (double num, string prefix) ToUnitPrefixNumberString(this double d)
        {
            double num = 1;
            if (d == 0) 
            {
                return (num, string.Empty);
            }
            if (d > -1e-12 && d < 1e-12)
            {
                num = 1e15;
                return (num, "f");
            }
            if ((d > -1e-9 && d <= -1e-12) || (d < 1e-9 && d >= 1e-12))
            {
                num = 1e12;
                return (num, "p");
            }
            else if ((d > -1e-6 && d <= -1e-9) || (d < 1e-6 && d >= 1e-9))
            {
                num = 1e9;
                return (num, "n");
            }
            else if ((d > -1e-3 && d <= -1e-6) || (d < 1e-3 && d >= 1e-6))
            {
                num = 1e6;
                return (num, "μ");
            }
            else if ((d > -1 && d <= -1e-3) || (d < 1 && d >= 1e-3))
            {
                num = 1e3;
                return (num, "m");
            }
            else if ((d >= 1e3 && d < 1e6) || (d <= -1e3 && d > -1e6))
            {
                num = 1 / 1e3;
                return (num, "K");
            }
            else if ((d >= 1e6 && d < 1e9) || (d <= -1e6 && d > -1e9))
            {
                num = 1 / 1e6;
                return (num, "M");
            }
            else if ((d >= 1e9 && d < 1e12) || (d <= -1e9 && d > -1e12))
            {
                num = 1 / 1e9;
                return (num, "G");
            }
            else if (d >= 1e12 || d <= -1e12)
            {
                num = 1 / 1e12;
                return (num, "T");
            }
            else
            {
                return (num, string.Empty);
            }
        }
    }
}
