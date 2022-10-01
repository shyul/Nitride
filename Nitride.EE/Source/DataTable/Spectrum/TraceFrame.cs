/// ***************************************************************************
/// Nitride Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// // when data length is longer than trace length.
/// 
/// ***************************************************************************
/// 
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nitride;
using Nitride.Chart;

namespace Nitride.EE
{
    public class TraceFrame
    {
        public TraceFrame(int i, int width, int height)
        {
            Index = i;
            HighPixColumn = new("Pix H" + i);
            LowPixColumn = new("Pix L" + i);
            HighValueColumn = new("Value H" + i);
            LowValueColumn = new("Value L" + i);
            TraceColumn = new("Main " + i);
            PersistBitmap = new(width, height);
        }

        public int Index { get; }
        public NumericColumn HighPixColumn { get; }
        public NumericColumn LowPixColumn { get; }
        public NumericColumn HighValueColumn { get; }
        public NumericColumn LowValueColumn { get; }
        public NumericColumn TraceColumn { get; }
        public Bitmap PersistBitmap { get; set; }
    }
}
