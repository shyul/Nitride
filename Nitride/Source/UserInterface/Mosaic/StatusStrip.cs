/// ***************************************************************************
/// Nitride Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2023 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Nitride
{
    [DesignerCategory("Code")]
    public class StatusStrip : Control
    {
        public const int StatusStripHeight = 24;

        #region Ctor
        public StatusStrip()
        {
            SuspendLayout();
            Name = "MosaicStatusStrip";
            components = new Container();
            DoubleBuffered = true;
            //SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.Selectable, false);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            //SetStyle(ControlStyles.UserPaint, true);
            //SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            Dock = DockStyle.Bottom;
            Height = StatusStripHeight;
            ForeColor = Main.Theme.Panel.ForeColor;
            BackColor = Main.Theme.Panel.FillColor;
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion

        #region Components

        private IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion

        public override Font Font => Main.Theme.Font;

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            Graphics g = pe.Graphics;

            g.SmoothingMode = SmoothingMode.HighQuality;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;

            //g.FillRectangle(Main.Theme.DarkTextBrush, Bounds);

            g.DrawString("Default Status Information", Font, Main.Theme.DarkTextBrush, new Point(Width / 2, Height / 2), AppTheme.TextAlignCenter);
        }
    }
}
