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
            //MultiCorrColumn = new("Main Trace " + i);
            HighPixColumn = new("Pix H" + i);
            LowPixColumn = new("Pix L" + i);
            MagnitudeHighColumn = new("Magnitude H" + i);
            MagnitudeLowColumn = new("Magnitude L" + i);
            MagnitudeColumn = new("Magnitude " + i);
            PersistBuffer = new int[PersistWidth, PersistHeight];
            PersistBitmap = new(PersistWidth, PersistHeight);
            PersistBitmapGraphics = Graphics.FromImage(PersistBitmap);
            PersistBitmapValid = false;
        }

        public int Index { get; }
        public int PersistWidth { get; }
        public int PersistHeight { get; }



        public NumericColumn MagnitudeColumn { get; }
        public NumericColumn MagnitudeHighColumn { get; }
        public NumericColumn MagnitudeLowColumn { get; }

        public NumericColumn HighPixColumn { get; }
        public NumericColumn LowPixColumn { get; }

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
