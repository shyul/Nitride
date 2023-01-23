/// ***************************************************************************
/// Nitride Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2023 Xu Li - me@xuli.us
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
    public class MosaicForm : Form
    {
        #region Caption Geometry
        public const int UpEdgeResizeGripMargin = 8;
        public const int RibbonToLeftWindowEdgeMargin = 0; // 6
        public const int RibbonToToolBarMargin = 2;
        public static int PaneGripMargin = 4;

        #endregion

        public MosaicForm()
        {
            SuspendLayout();

            Ribbon = new Ribbon() { Dock = DockStyle.Top };
            OrbMenu = new OrbMenu(this) { Visible = false };
            StatusPane = new StatusStrip();
            DockCanvas = new DockCanvas(); // { Dock = DockStyle.Fill };

            AutoScaleMode = AutoScaleMode.Dpi;
            DoubleBuffered = true;
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            FormBorderStyle = FormBorderStyle.Sizable;

            Controls.Add(Ribbon.RibbonContainer);
            Controls.Add(DockCanvas);
            Controls.Add(StatusPane);
            Controls.Add(Ribbon);

            ResumeLayout(false);
            PerformLayout();
        }

        #region Drop Menus ####################################################################### t.b.d

        public OrbMenu OrbMenu { get; protected set; }
        public static ContextPane ContextPane { get; } = new();
        public static ContextDropMenu ContextDropMenu { get; } = new();
        public void SetActivateOrbMenu(bool isActivate)
        {
            OrbMenu.Visible = isActivate;
            ContextPane.Show(this, new OrbMenuHost(OrbMenu), new Point(Ribbon.Bounds.Left, Ribbon.Bounds.Bottom));
        }

        #endregion

        public Ribbon Ribbon { get; protected set; }
        public DockCanvas DockCanvas { get; protected set; }
        public StatusStrip StatusPane { get; protected set; }

        public void AddForm(DockForm df) => DockCanvas.AddForm(DockStyle.Fill, 0, df);
        public void AddForm(DockStyle postion, DockForm df) => DockCanvas.AddForm(postion, 0, df);
        public void AddForm(DockStyle postion, int index, DockForm df) => DockCanvas.AddForm(postion, index, df);

        #region Coordinate

        public virtual bool IsRibbonShrink
        {
            get
            {
                return m_IsRibbonShrink;
            }
            set
            {
                m_IsRibbonShrink = value;
                UpdateRibbonLocation();
                Coordinate();
            }
        }

        protected bool m_IsRibbonShrink = false;

        protected virtual void UpdateRibbonLocation()
        {
            if (m_IsRibbonShrink)
            {
                Ribbon.RibbonContainer.Visible = false;
                Ribbon.RibbonContainer.Coordinate();
                Ribbon.RibbonContainer.DeActivate();
                DockCanvas.Location = new(0, Ribbon.Bounds.Bottom);
            }
            else
            {
                Ribbon.RibbonContainer.Coordinate();
                Ribbon.RibbonContainer.Visible = true;
                Ribbon.RibbonContainer.ActivateDefaultTab();
            }
        }

        protected virtual void Coordinate()
        {
            SuspendLayout();

            int ribbonWidth = Ribbon.OrbWidth;

            foreach (var rt in Ribbon.Tabs)
            {
                ribbonWidth += rt.TabRect.Width;
            }
            ribbonWidth += 60;

            Ribbon.Bounds = new Rectangle(RibbonToLeftWindowEdgeMargin, UpEdgeResizeGripMargin, ribbonWidth, Ribbon.TabHeight);
            Ribbon.RibbonContainer.Location = new Point(0, Ribbon.Height);
            Ribbon.RibbonContainer.Size = new Size(ClientRectangle.Width, Ribbon.PanelHeight);
            Ribbon.RibbonContainer.Visible = !IsRibbonShrink;

            //OrbMenu.Location = new Point(Ribbon.Bounds.Left, Ribbon.Bounds.Bottom);
            if (m_IsRibbonShrink)
            {
                DockCanvas.Location = new(0, Ribbon.Height);
            }
            else
            {
                DockCanvas.Location = new(0, Ribbon.RibbonContainer.Bounds.Bottom);
            }

            DockCanvas.Size = new Size(ClientRectangle.Width, ClientRectangle.Height - DockCanvas.Location.Y - StatusPane.Height);

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
    }



}
