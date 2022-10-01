/// ***************************************************************************
/// Nitride Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Nitride
{
    public static class ColorTool
    {
        /// <summary>
        /// Quickly adjust the transparency of the color
        /// </summary>
        /// <param name="color">Color to correct.</param>
        /// <param name="alpha">New alpha channel.</param>
        /// <returns></returns>
        public static Color Opaque(this Color color, int alpha) => Color.FromArgb(alpha, color);

        /// <summary>
        /// Creates color with corrected brightness.
        /// * Developed by Pavel Vladov from Internet
        /// </summary>
        /// <param name="color">Color to correct.</param>
        /// <param name="factor">The brightness correction factor. 
        /// Must be between -1 and 1. 
        /// Negative values produce darker colors.</param>
        /// <returns></returns>
        public static Color Brightness(this Color color, float factor)
        {
            float r = color.R;
            float g = color.G;
            float b = color.B;

            if (factor < 0)
            {
                factor = 1 + factor;
                r *= factor;
                g *= factor;
                b *= factor;
            }
            else
            {
                r = (255 - r) * factor + r;
                g = (255 - g) * factor + g;
                b = (255 - b) * factor + b;
            }

            return Color.FromArgb(color.A, r.ToInt32(), g.ToInt32(), b.ToInt32());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="A"></param>
        /// <param name="H"></param>
        /// <param name="S"></param>
        /// <param name="L"></param>
        /// <returns></returns>
        public static Color FromAhsl(float A, float H, float S, float L)
        {
            int alpha = (A * 255).ToInt32();

            if (alpha < 0) alpha = 0;
            else if (alpha > 255) alpha = 255;

            while (H < 0) H += 360;
            while (H > 360) H -= 360;

            if (S < 0) S = 0;
            else if (S > 1) S = 1;

            if (L < 0) L = 0;
            else if (L > 1) L = 1;

            if (S == 0)
            {
                return Color.FromArgb(
                    alpha,
                    (L * 255).ToInt32(),
                    (L * 255).ToInt32(),
                    (L * 255).ToInt32());
            }

            float H0 = H / 60.0f;
            int index = Convert.ToInt32(Math.Floor(H0));

            float C = S * (1 - Math.Abs(2 * L - 1));
            float X = C * (1 - Math.Abs((H0 % 2) - 1));
            float m = L - C / 2;

            int iC = ((C + m) * 255).ToInt32();
            int iX = ((X + m) * 255).ToInt32();
            int i0 = (m * 255).ToInt32();

            return index switch
            {
                1 => Color.FromArgb(alpha, iX, iC, i0),
                2 => Color.FromArgb(alpha, i0, iC, iX),
                3 => Color.FromArgb(alpha, i0, iX, iC),
                4 => Color.FromArgb(alpha, iX, i0, iC),
                5 => Color.FromArgb(alpha, iC, i0, iX),
                _ => Color.FromArgb(alpha, iC, iX, i0),
            };
        }

        public static Color[] GetTransparentGradient(Color c, byte num)
        {
            List<Color> persistColor = new();

            double step = 255.0D / (double)num;

            for (int i = 0; i < num; i++)
            {
                persistColor.Add(Color.FromArgb(Convert.ToByte((double)i * step), c));
            }

            return persistColor.ToArray();
        }

        public static Color GetGradient(Color c_min, Color c_max, double ratio)
        {
            int a_min = c_min.A;
            int r_min = c_min.R;
            int g_min = c_min.G;
            int b_min = c_min.B;

            int a_max = c_max.A;
            int r_max = c_max.R;
            int g_max = c_max.G;
            int b_max = c_max.B;

            int a_average = Convert.ToInt32(a_min + ((a_max - a_min) * ratio));
            int r_average = Convert.ToInt32(r_min + ((r_max - r_min) * ratio));
            int g_average = Convert.ToInt32(g_min + ((g_max - g_min) * ratio));
            int b_average = Convert.ToInt32(b_min + ((b_max - b_min) * ratio));

            /*
            Range<int> a_range = new(c_min.A, c_max.A);
            Range<int> r_range = new(c_min.R, c_max.R);
            Range<int> g_range = new(c_min.G, c_max.G);
            Range<int> b_range = new(c_min.B, c_max.B);

            int a_average = Convert.ToInt32(a_range.Min + ((a_range.Max - a_range.Min) * ratio));
            int r_average = Convert.ToInt32(r_range.Min + ((r_range.Max - r_range.Min) * ratio));
            int g_average = Convert.ToInt32(g_range.Min + ((g_range.Max - g_range.Min) * ratio));
            int b_average = Convert.ToInt32(b_range.Min + ((b_range.Max - b_range.Min) * ratio));*/

            if (a_average < 0) a_average = 0;
            else if (a_average > 255) a_average = 255;

            if (r_average < 0) r_average = 0;
            else if (r_average > 255) r_average = 255;

            if (g_average < 0) g_average = 0;
            else if (g_average > 255) g_average = 255;

            if (b_average < 0) b_average = 0;
            else if (b_average > 255) b_average = 255;

            return Color.FromArgb(a_average, r_average, g_average, b_average);
        }

        public static Color[] GetGradient(IEnumerable<(double X, double A, double R, double G, double B)> vec, int cnt)
        {
            var vector = vec.OrderBy(n => n.X);
            var X = vector.Select(n => n.X);
            CubicSpline a_cs = new(X, vector.Select(n => n.A), 0, 0);
            CubicSpline r_cs = new(X, vector.Select(n => n.R), 0, 0);
            CubicSpline g_cs = new(X, vector.Select(n => n.G), 0, 0);
            CubicSpline b_cs = new(X, vector.Select(n => n.B), 0, 0);

            double[] NX = new double[cnt];
            double min_nx = X.Min();
            double delta_nx = X.Max() - min_nx;

            for (int i = 0; i < cnt; i++)
            {
                NX[i] = ((Convert.ToDouble(i) / (cnt - 1.0D)) * delta_nx) + min_nx;
            }

            double[] A = a_cs.Evaluate(NX);
            double[] R = r_cs.Evaluate(NX);
            double[] G = g_cs.Evaluate(NX);
            double[] B = b_cs.Evaluate(NX);
            Color[] res = new Color[cnt];

            for (int i = 0; i < cnt; i++)
            {
                double a = A[i];
                if (a > 255) a = 255; else if (a < 0) a = 0;

                double r = R[i];
                if (r > 255) r = 255; else if (r < 0) r = 0;

                double g = G[i];
                if (g > 255) g = 255; else if (g < 0) g = 0;

                double b = B[i];
                if (b > 255) b = 255; else if (b < 0) b = 0;

                res[i] = Color.FromArgb(Convert.ToByte(a), Convert.ToByte(r), Convert.ToByte(g), Convert.ToByte(b));
            }

            return res;
        }

        public static Color[] GetThermalGradient(int cnt, int range)
        {
            return GetGradient(ThermalColorVector.Take(range), cnt);
        }

        public static Color[] GetDarkSkyGradient(int cnt)
        {
            return GetGradient(DarkSkyColorVector, cnt);
        }

        private static (double X, double A, double R, double G, double B)[] ThermalColorVector { get; } =
        {
            (0, 16, 5, 0, 255),
            (1, 32, 4, 0, 255),
            (2, 48, 3, 0, 255),
            (3, 64, 2, 0, 255),
            (4, 80, 1, 0, 255),
            (5, 96, 0, 0, 255),
            (6, 112, 0, 2, 255),
            (7, 128, 0, 18, 255),
            (8, 112, 0, 34, 255),
            (9, 120, 0, 50, 255),

            (10, 128, 0, 68, 255),
            (11, 144, 0, 84, 255),
            (12, 160, 0, 100, 255),
            (13, 176, 0, 116, 255),
            (14, 192, 0, 132, 255),
            (15, 208, 0, 148, 255),
            (16, 224, 0, 164, 255),
            (17, 240, 0, 180, 255),
            (18, 255, 0, 196, 255),
            (19, 255, 0, 212, 255),

            (20, 255, 0, 228, 255),
            (21, 255, 0, 255, 244),
            (22, 255, 0, 255, 208),
            (23, 255, 0, 255, 168),
            (24, 255, 0, 255, 131),
            (25, 255, 0, 255, 92),
            (26, 255, 0, 255, 54),
            (27, 255, 0, 255, 16),
            (28, 255, 23, 255, 0),
            (29, 255, 62, 255, 0),
            
            (30, 255, 101, 255, 0),
            (31, 255, 138, 255, 0),
            (32, 255, 176, 255, 0),
            (33, 255, 215, 255, 0),
            (34, 255, 253, 255, 0),
            (35, 255, 255, 250, 0),
            (36, 255, 255, 240, 0),
            (37, 255, 255, 230, 0),
            (38, 255, 255, 220, 0),
            (39, 255, 255, 210, 0),

            (40, 255, 255, 200, 0),
            (41, 255, 255, 190, 0),
            (42, 255, 255, 180, 0),
            (43, 255, 255, 170, 0),
            (44, 255, 255, 160, 0),
            (45, 255, 255, 150, 0),
            (46, 255, 255, 140, 0),
            (47, 255, 255, 130, 0),
            (48, 255, 255, 120, 0),
            (49, 255, 255, 110, 0),

            (50, 255, 255, 100, 0),
            (51, 255, 255, 90, 0),
            (52, 255, 255, 80, 0),
            (53, 255, 255, 70, 0),
            (54, 255, 255, 60, 0),
            (55, 255, 255, 50, 0),
            (56, 255, 255, 40, 0),
            (57, 255, 255, 30, 0),
            (58, 255, 255, 20, 0),
            (59, 255, 255, 10, 0),

            (60, 255, 255, 0, 0),
            (61, 255, 255, 0, 16),
            (62, 255, 255, 0, 32),
            (63, 255, 255, 0, 48),
            (64, 255, 255, 0, 64),
            (65, 255, 255, 0, 80),
            (66, 255, 255, 0, 96),
            (67, 255, 255, 0, 112),
            (68, 255, 255, 0, 128),
            (69, 255, 255, 0, 144),
            (70, 255, 255, 0, 160),
            (71, 255, 255, 0, 176),
            (72, 255, 255, 0, 192),
            (73, 255, 255, 0, 208),
            (74, 255, 255, 0, 224),
            
            (75, 255, 255, 0, 240),
            (76, 255, 255, 1, 240),
            (77, 255, 255, 2, 240),
            (78, 255, 255, 3, 240),
            (79, 255, 255, 4, 240),
            (80, 255, 255, 5, 240),
            (81, 255, 255, 6, 240),
            (82, 255, 255, 7, 240),
            (83, 255, 255, 8, 240),
            (84, 255, 255, 9, 240),
            (85, 255, 255, 10, 240),
            (86, 255, 255, 11, 240),
            (87, 255, 255, 12, 240),
            (88, 255, 255, 13, 240),
            (89, 255, 255, 14, 240)
        };

        private static (double X, double A, double R, double G, double B)[] DarkSkyColorVector { get; } =
{
            (0, 16, 0, 4, 62),
            (1, 32, 0, 14, 75),
            (2, 64, 0, 18, 84),
            (3, 128, 0, 29, 97),
            (4, 196, 0, 37, 107),
            (5, 255, 0, 66, 130),
            (6, 255, 0, 80, 141),
            (7, 255, 29, 100, 144),
            (8, 255, 61, 116, 146),
            (9, 255, 29, 100, 144),
            (10, 255, 96, 130, 140),
            (11, 255, 140, 145, 132),
            (12, 255, 178, 148, 110),
            (13, 255, 227, 153, 88),
            (14, 255, 247, 157, 79),
        };
    }
}
