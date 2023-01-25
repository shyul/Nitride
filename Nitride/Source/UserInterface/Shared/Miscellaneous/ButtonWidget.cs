/// ***************************************************************************
/// Nitride Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
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
    public class ButtonWidget : Widget
    {
        public ButtonWidget(Command cmd, bool hasIconColor = false, bool hasSmooth = false, bool hasEdge = true, int edgeWidth = 1, int order = 0, Importance importance = Importance.Minor) : base(order, importance)
        {
            Command = cmd;

            HasIconColor = hasIconColor;
            HasSmooth = hasSmooth;
            HasEdge = hasEdge;

            HoverFillBrush = new SolidBrush(Color.FromArgb(70, Command.Theme.FillColor));
            ClickFillBrush = new SolidBrush(Color.FromArgb(200, Command.Theme.FillColor));
            HoverEdgePen = new Pen(Color.FromArgb(70, Command.Theme.EdgeColor), edgeWidth);
            ClickEdgePen = new Pen(Color.FromArgb(200, Command.Theme.EdgeColor), edgeWidth);

            Coordinate();
            ResumeLayout(false);
            PerformLayout();
        }

        public Command Command { get; protected set; }


        public void SetCommand(Command cmd) 
        {
            Command = cmd;
            Coordinate();
        }

        public virtual void Execute()
        {
            Command.Start();
        }

        protected virtual bool HasIconColor { get; set; }

        protected virtual bool HasSmooth { get; set; }

        protected virtual bool HasEdge { get; set; }

        public override Color BackColor => Color.Transparent; // Command.Theme.FillColor;

        public override Color ForeColor => (Parent != null) ? Parent.ForeColor : Command.Theme.ForeColor;


        protected Rectangle m_IconRect = Rectangle.Empty;

        protected Rectangle m_LabelRects1 = Rectangle.Empty;

        protected Rectangle m_LabelRects2 = Rectangle.Empty;

        protected string[] m_LabelLines;

        protected int m_LineWidth = 0;

        public void BreakTextLine(int maxWidth, int maxLineCnt)
        {
            m_LabelLines = Label.Wordwarp(Main.Theme.Font, maxLineCnt, maxWidth, out m_LineWidth).ToArray();
        }

        public override void Coordinate() 
        {
            BreakTextLine(80, 2);
            int actualWidth = 36;
            if (m_LineWidth > actualWidth) actualWidth = m_LineWidth;
            if (actualWidth < 40) actualWidth = 40;
            //actualWidth += 6;
            Size = new Size(actualWidth, 66);
            //m_IconRect = new Rectangle(0, 0, actualWidth, 40);
            m_IconRect = new Rectangle((actualWidth - 32) / 2, 3, 32, 32);
            if (m_LabelLines.Length == 1)
            {
                m_LabelRects1 = new Rectangle(0, m_IconRect.Bottom, actualWidth, Height - m_IconRect.Height);
                m_LabelRects2 = Rectangle.Empty;
            }
            else
            {
                m_LabelRects1 = new Rectangle(0, m_IconRect.Bottom + 1, actualWidth, (Height - m_IconRect.Height) / 2 - 1);
                m_LabelRects2 = new Rectangle(0, m_LabelRects1.Bottom - 2, actualWidth, (Height - m_IconRect.Height) / 2 - 1);
            }
        }

        public override Brush HoverFillBrush { get; protected set; }

        public override Brush ClickFillBrush { get; protected set; }

        public override Pen HoverEdgePen { get; protected set; }

        public override Pen ClickEdgePen { get; protected set; }

        protected override void OnPaint(PaintEventArgs pe)
        {
            Graphics g = pe.Graphics;

            if (HasSmooth)
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.SmoothingMode = SmoothingMode.AntiAlias;
            }
            else
            {
                g.SmoothingMode = SmoothingMode.HighSpeed;
                g.SmoothingMode = SmoothingMode.Default;
            }

            //g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            g.Clear(BackColor);

            PaintControl(g);
        }

        public virtual void PaintControl(Graphics g)
        {
            if (Command.Enabled && Enabled)
                if (MouseState == MouseState.Hover)
                {
                    g.FillRectangle(HoverFillBrush, ControlRect);
                    if (HasEdge) g.DrawRectangle(HoverEdgePen, ControlRect);
                }
                else if (MouseState == MouseState.Down)
                {
                    g.FillRectangle(ClickFillBrush, ControlRect);
                    if (HasEdge) g.DrawRectangle(ClickEdgePen, ControlRect);
                }

            if (!HasIconColor)
                Command.DrawIconCenter(g, new Size(16, 16), ClientRectangle, MouseState, false, Enabled);
            else
                Command.DrawIconCenter(g, new Size(16, 16), ClientRectangle, ForeColor, MouseState, false, Enabled);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            Point pt = new(e.X, e.Y);
            if (e.Button == MouseButtons.Left && ClientRectangle.Contains(pt) && Command.Enabled && Enabled)
                Execute();
            Invalidate(true);
        }
    }
}