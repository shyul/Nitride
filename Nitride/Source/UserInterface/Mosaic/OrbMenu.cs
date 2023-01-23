/// ***************************************************************************
/// Nitride Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2023 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace Nitride
{
    [DesignerCategory("Code")]
    public class OrbMenuContextHost : ContextHost
    {
        public OrbMenuContextHost(OrbMenu om) : base(om)
        {
            OrbMenu = om;
        }

        protected OrbMenu OrbMenu { get; set; }

        public override void Close()
        {
            OrbMenu.MoForm.Invalidate(true);
            /*
            if (RibbonTab != null)
            {
                RibbonTab.HostContainer.DeActivate();
                RibbonTab.Ribbon.Invalidate(true);
            }*/
        }
    }


    [DesignerCategory("Code")]
    public class OrbMenu : UserControl
    {
        #region Ctor
        public OrbMenu(MosaicForm fm) // : base()
        {
            MoForm = fm;
        }
        #endregion

        #region Components
        public MosaicForm MoForm { get; protected set; }

        public List<ButtonWidget> ButtonWidgets { get; protected set; } = new();

        public void AddRange(IEnumerable<ButtonWidget> buttons)
        {
            lock (ButtonWidgets)
            {
                Controls.Clear();

                ButtonWidgets = buttons.OrderBy(n => n.Order).ToList();

                for (int i = 0; i < ButtonWidgets.Count; i++)
                {
                    ButtonWidget c = ButtonWidgets[i];
                    c.Order = i;
                    Controls.Add(c);
                }

                Coordinate();
            }

        }

        #endregion

        #region Coordinate

        public const int MaximumWidth = 100;

        public Rectangle OrbButtonRect => MoForm.Ribbon.OrbRect;

        public virtual void Coordinate()
        {
            SuspendLayout();

            int x = 0;
            int y = 0;
            int x_base = 0;


            lock (ButtonWidgets)
                foreach (ButtonWidget rbc in ButtonWidgets)
                {
                    rbc.Coordinate();
                    rbc.Location = new Point(x, y);
              
                    y += rbc.Height;
                }



            ResumeLayout(false);
            PerformLayout();
        }

        protected override void OnResize(EventArgs e)
        {
            Coordinate();
            base.OnResize(e);
        }

        protected override void OnClientSizeChanged(EventArgs e)
        {
            Coordinate();
            base.OnClientSizeChanged(e);
        }

        #endregion

        #region Paint

        protected override void OnPaint(PaintEventArgs pe)
        {
            Graphics g = pe.Graphics;

            // g.FillRectangle(Main.Theme.Panel.EdgeBrush, Bounds);

            g.DrawLine(Main.Theme.Panel.EdgePen, new Point(OrbButtonRect.Width, 0), new Point(Width, 0));
            g.DrawLine(Main.Theme.Panel.EdgePen, new Point(0, 0), new Point(0, Height));
            g.DrawLine(Main.Theme.Panel.EdgePen, new Point(0, Height - 1), new Point(Width, Height - 1));
            g.DrawLine(Main.Theme.Panel.EdgePen, new Point(Width - 1, 0), new Point(Width - 1, Height));

            int last_y = 0;


        }

        #endregion

        #region Mouse
        public MouseState ButtonMouseState = MouseState.Out;
        public bool ButtonIsHot
        {
            get { return (Enabled && ButtonMouseState == MouseState.Down); }
        }
        public void ButtonReset() { ButtonMouseState = MouseState.Out; }
        public bool ButtonMouseMove(Point pt)
        {
            bool GotPt = OrbButtonRect.Contains(pt) & Enabled;
            ButtonMouseState = (GotPt) ? MouseState.Hover : MouseState.Out;
            return GotPt;
        }
        public bool ButtonMouseDown(Point pt)
        {
            bool GotPt = OrbButtonRect.Contains(pt) & Enabled;
            ButtonMouseState = (GotPt) ? MouseState.Down : MouseState.Out;
            return GotPt;
        }
        public bool ButtonMouseUp(Point pt)
        {
            bool IsExec = OrbButtonRect.Contains(pt) & Enabled;
            ButtonMouseState = (IsExec) ? MouseState.Hover : MouseState.Out;
            if (IsExec)
            {



            }
            return IsExec;
        }
        #endregion
    }
}
