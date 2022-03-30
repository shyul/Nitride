/// ***************************************************************************
/// Nitride Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Nitride.WindowsNativeMethods;

namespace Nitride
{
    [DesignerCategory("Code")]
    public class Mosaic : Form
    {
        public Mosaic()
        {
            SuspendLayout();
            AutoScaleMode = AutoScaleMode.Dpi;
            DoubleBuffered = true;
            //SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            FormBorderStyle = FormBorderStyle.Sizable;
            DockCanvas.Dock = DockStyle.Fill;
            Controls.Add(DockCanvas);
            ResumeLayout(false);
            PerformLayout();
        }

        public DockCanvas DockCanvas { get; } = new DockCanvas();

        public void AddForm(DockForm df) => DockCanvas.AddForm(DockStyle.Fill, 0, df);
        public void AddForm(DockStyle postion, DockForm df) => DockCanvas.AddForm(postion, 0, df);
        public void AddForm(DockStyle postion, int index, DockForm df) => DockCanvas.AddForm(postion, index, df);
    }



}
