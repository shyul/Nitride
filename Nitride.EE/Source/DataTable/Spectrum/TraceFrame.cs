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
            PersistBitmap = new(PersistWidth, PersistHeight);
            PersistBitmapGraphcis = Graphics.FromImage(PersistBitmap);
            PersistBuffer = new int[width, height];
        }

        public int Index { get; }
        public NumericColumn HighPixColumn { get; }
        public NumericColumn LowPixColumn { get; }
        public NumericColumn HighValueColumn { get; }
        public NumericColumn LowValueColumn { get; }
        public NumericColumn TraceColumn { get; }
        public int[,] PersistBuffer { get; }
        public Bitmap PersistBitmap { get; set; }

        public Graphics PersistBitmapGraphcis { get; }

        public void ClearPersistBuffer()
        {
            for (int x = 0; x < PersistWidth; x++)
            {
                for (int y = 0; y < PersistHeight; y++)
                {
                    PersistBuffer[x, y] = 0;
                }
            }

            // Array.Clear(PersistBuffer, 0, PersistBuffer.Length);
        }

        static SolidBrush TransparentBrush { get; } = new(Color.Transparent);

        public int PersistWidth { get; }

        public int PersistHeight { get; }

        public void ClearPersistBitmap()
        {
            try
            {
                PersistBitmap = new(PersistWidth, PersistHeight);
                // PersistBitmapGraphcis = Graphics.FromImage(PersistBitmap);
                // PersistBitmapGraphcis.FillRectangle(TransparentBrush, 0, 0, PersistWidth, PersistHeight);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /*
        public void ApplyPersistBitmap(IEnumerable< Color> colorMap) 
        {
            lock (PersistBitmap)
            {
                for (int x = 0; x < PersistBitmap.Width; x++)
                {
                    for (int y = 0; y < PersistBitmap.Height; y++)
                    {
                        int z = PersistBuffer[x, y] - 1;
                        if (z >= 0)
                        {
                            //Console.WriteLine("############### z = " + z);
                            PersistBitmap.SetPixel(x, y, colorMap.ElementAt(z));
                        }
                        else
                        {
                            PersistBitmap.SetPixel(x, y, Color.Transparent);
                        }
                    }
                }
            }
        }*/
    }
}
