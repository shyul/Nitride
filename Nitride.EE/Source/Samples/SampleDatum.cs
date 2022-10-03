/// ***************************************************************************
/// Nitride Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// SampleBuffer
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Nitride.EE
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Dual16BitDatum
    {
        public short D1 { get; set; } //= 0;

        public short D2 { get; set; } //= 0;
    }

    /*
    [StructLayout(LayoutKind.Sequential)]
    public struct Quad16BitDatum
    {
        public short D1 { get; set; } //= 0;

        public short D2 { get; set; } //= 0;

        public short D3 { get; set; } //= 0;

        public short D4 { get; set; } //= 0;
    }*/

    [StructLayout(LayoutKind.Sequential)]
    public struct Dual32BitDatum
    {
        public int D1 { get; set; } //= 0;

        public int D2 { get; set; } //= 0;
    }

    /*
    [StructLayout(LayoutKind.Sequential)]
    public struct Quad32BitDatum
    {
        public int D1 { get; set; } //= 0;

        public int D2 { get; set; } //= 0;

        public int D3 { get; set; } //= 0;

        public int D4 { get; set; } //= 0;
    }*/

    [StructLayout(LayoutKind.Sequential)]
    public struct Dual64BitDatum
    {
        public long D1 { get; set; } //= 0;

        public long D2 { get; set; } //= 0;
    }
}
