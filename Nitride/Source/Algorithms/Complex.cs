/// ***************************************************************************
/// Nitride Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Math and numeric related basic functions.
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;

namespace Nitride
{
    public static partial class Algorithms
    {
        public static Complex GetComplexByMagnitudeAndDegree(double mag, double degree) => GetComplexByMagnitudeAndRadian(mag, degree * Math.PI / 180);

        public static Complex GetComplexByMagnitudeAndRadian(double mag, double radian)
        {
            // 1rad × 180/π = 57.296°

            double real = mag * Math.Cos(radian);
            double imag = mag * Math.Sin(radian);
            return new Complex(real, imag);
        }
    }
}
