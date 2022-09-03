/// ***************************************************************************
/// Nitride Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2022 Xu Li - me@xuli.us
/// 
/// 
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Nitride;

namespace Nitride
{
    public class StringDatum : IDatum
    {
        public StringDatum(string value)
        {
            Value = value;
        }

        public string Value { get; }
    }
}
