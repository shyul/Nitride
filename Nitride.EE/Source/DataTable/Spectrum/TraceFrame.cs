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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace Nitride.EE
{
    public class TraceFrame
    {
        public TraceFrame(int i, int width, int height)
        {
            Index = i;
            PersistWidth = width;
            PersistHeight = height;
            HighPixColumn = new("Pix H" + i);
            LowPixColumn = new("Pix L" + i);
            HighValueColumn = new("Value H" + i);
            LowValueColumn = new("Value L" + i);
            TraceColumn = new("Main " + i);
            PersistBuffer = new int[PersistWidth, PersistHeight];
            PersistBitmap = new(PersistWidth, PersistHeight);
            PersistBitmapGraphics = Graphics.FromImage(PersistBitmap);
            PersistBitmapValid = false;
        }

        public int Index { get; }
        public int PersistWidth { get; }
        public int PersistHeight { get; }

        public NumericColumn HighPixColumn { get; }
        public NumericColumn LowPixColumn { get; }
        public NumericColumn HighValueColumn { get; }
        public NumericColumn LowValueColumn { get; }
        public NumericColumn TraceColumn { get; }
        public int[,] PersistBuffer { get; }
        public Bitmap PersistBitmap { get; }
        public Graphics PersistBitmapGraphics { get; }
        public bool PersistBitmapValid { get; set; }

        public void ClearPersistBitmap()
        {
            PersistBitmapGraphics.Clear(Color.Transparent);
        }

        public void ClearPersistBuffer()
        {
            
            for (int x = 0; x < PersistWidth; x++)
            {
                for (int y = 0; y < PersistHeight; y++)
                {
                    PersistBuffer[x, y] = 0;
                }
            }

            //Array.Clear(PersistBuffer, 0, PersistBuffer.Length);
        }
    }
}
