/// ***************************************************************************
/// Nitride Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2023 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Nitride
{
    [DesignerCategory("Code")]
    public class TestDockForm : DockForm
    {
        public TestDockForm(string formName) : base(formName)
        {
            HasIcon = true;
            Btn_Pin.Enabled = true;
            Btn_Close.Enabled = true;
            BackColor = Color.FromArgb(255, 255, 253, 245);

            ResumeLayout(false);
            PerformLayout();
        }

        Rectangle rect;
        Rectangle rect2;
        protected override void CoordinateLayout()
        {
            rect = new Rectangle(10, 10, ClientRectangle.Width - 21, ClientRectangle.Height - 21);
            rect2 = new Rectangle(ClientRectangle.Width / 4, 50 + ClientRectangle.Height / 4, ClientRectangle.Width / 2, ClientRectangle.Height / 2);
            //if(TabText == "To-Do") Log.Print(rect.ToString());
            //base.OnClientSizeChanged(e);
        }
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            Graphics g = pe.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            g.DrawRectangle(new Pen(new SolidBrush(Color.LightGray)), rect);

            //g.DrawRectangle(new Pen(new SolidBrush(Color.LightGray)), ClientRectangle);

            using (Font tFont = new("Segoe UI", 30F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0))))
            {
                Box(g, rect, tFont, Color.LightGray, TabName);
            }

            using (Font tFont = new("Segoe UI", 30F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0))))
            {
                int level = 0;
                DockContainer topx = (DockContainer)HostContainer;
                while (!topx.IsRoot)
                {
                    topx = (DockContainer)topx.HostDockPane.Parent;
                    level++;
                }


                string info = ClientRectangle.Width.ToString() + " - " + ClientRectangle.Height.ToString() + " / Level: " + level;
                //info = Parent.ToString();
                Box(g, rect2, new("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0))), Color.LightGray, info);
            }
        }

        public static void Box(Graphics g, Rectangle rect, Font font, Color color, string text)
        {
            using (StringFormat format = new StringFormat() { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center })
            {
                g.DrawString(text, font, new SolidBrush(color), rect, format);
            }
        }

        public static void Test(MosaicForm mo) 
        {
            mo.AddForm(DockStyle.Fill, 0, new TestDockForm("Hello"));
            mo.AddForm(DockStyle.Fill, 0, new TestDockForm("World"));
            mo.AddForm(DockStyle.Fill, 0, new TestDockForm("Gateway"));
            mo.AddForm(DockStyle.Fill, 0, new TestDockForm("Wireless"));
            mo.AddForm(DockStyle.Fill, 1, new TestDockForm("CAN Bus"));
            mo.AddForm(DockStyle.Fill, 2, new TestDockForm("LIN Bus"));
            mo.AddForm(DockStyle.Fill, 2, new TestDockForm("FlexRay"));
            mo.AddForm(DockStyle.Fill, 2, new TestDockForm("ARM"));
            mo.AddForm(DockStyle.Fill, 1, new TestDockForm("FPGA"));
            mo.AddForm(DockStyle.Fill, 1, new TestDockForm("google"));
            mo.AddForm(DockStyle.Fill, 2, new TestDockForm("10:20:33"));

            mo.AddForm(DockStyle.Left, 0, new TestDockForm("google"));
            mo.AddForm(DockStyle.Left, 0, new TestDockForm("yahoo"));
            mo.AddForm(DockStyle.Left, 1, new TestDockForm("juniper"));
            mo.AddForm(DockStyle.Left, 1, new TestDockForm("I-280"));
            mo.AddForm(DockStyle.Left, 2, new TestDockForm("Golden Gate"));
            mo.AddForm(DockStyle.Left, 2, new TestDockForm("Marin Headland"));

            mo.AddForm(DockStyle.Right, 0, new TestDockForm("palo alto"));
            mo.AddForm(DockStyle.Right, 0, new TestDockForm("Mountain View"));
            mo.AddForm(DockStyle.Right, 0, new TestDockForm("san jose"));
            mo.AddForm(DockStyle.Right, 0, new TestDockForm("san martin"));
            mo.AddForm(DockStyle.Right, 0, new TestDockForm("Gilroy"));
            mo.AddForm(DockStyle.Right, 1, new TestDockForm("milpitas"));
            mo.AddForm(DockStyle.Right, 1, new TestDockForm("Reid Hillview"));

            mo.AddForm(DockStyle.Bottom, 0, new TestDockForm("National Instruments"));
            mo.AddForm(DockStyle.Bottom, 0, new TestDockForm("San Francisco"));
            mo.AddForm(DockStyle.Bottom, 0, new TestDockForm("San Leandro"));
            mo.AddForm(DockStyle.Bottom, 1, new TestDockForm("N300"));
            mo.AddForm(DockStyle.Bottom, 1, new TestDockForm("Cessna"));
            mo.AddForm(DockStyle.Bottom, 1, new TestDockForm("Skyhawk"));
            mo.AddForm(DockStyle.Bottom, 2, new TestDockForm("Citabria"));
            mo.AddForm(DockStyle.Bottom, 2, new TestDockForm("10:20:33"));

            mo.AddForm(DockStyle.Top, 0, new TestDockForm("Oakland"));
            mo.AddForm(DockStyle.Top, 0, new TestDockForm("Richmond"));
            mo.AddForm(DockStyle.Top, 0, new TestDockForm("KOAK"));
            mo.AddForm(DockStyle.Top, 1, new TestDockForm("Livermore"));
            mo.AddForm(DockStyle.Top, 1, new TestDockForm("Tracy"));
            mo.AddForm(DockStyle.Top, 2, new TestDockForm("Pleasanton"));
            mo.AddForm(DockStyle.Top, 2, new TestDockForm("KLVK"));

            Console.WriteLine("SystemInformation.CaptionHeight = " + SystemInformation.CaptionHeight.ToString());



            Command c_StockChart = new Command()
            {
                //Enabled = false,
                Label = "StockChart",
                IconList = new Dictionary<IconType, Dictionary<Size, Bitmap>>() { { IconType.Normal, new Dictionary<Size, Bitmap>() {
                    { new Size(16, 16), Properties.Resources.Arrow_Up_16 },
                    { new Size(32, 32), Properties.Resources.Arrow_Up_32 }
                } } },
                //Action = (IObject sender, string[] args, Progress<Event> progress, CancellationTokenSource cts) => { Console.WriteLine("StockChart is clicked"); },
            };

            Command c_Power = new Command()
            {
                //Enabled = false,
                Label = "Power Unit",
                IconList = new Dictionary<IconType, Dictionary<Size, Bitmap>>() { { IconType.Normal, new Dictionary<Size, Bitmap>() {
                    { new Size(16, 16), Nitride.Properties.Resources.PowerUnit_16},
                    //{ new Size(24, 24), Xu.Properties.Resources.PowerUnit_16},
                    { new Size(33, 33), Nitride.Properties.Resources.PowerUnit_32 } ///??????
                } } },
                //Action = (IObject sender, string[] args, Progress<Event> progress, CancellationTokenSource cts) => { Console.WriteLine("Power Unit is clicked"); },
            };

            RibbonButton rbtn_1 = new RibbonButton(Main.Command_File_Open, 0, Importance.Major);

            RibbonButton rbtn_2 = new RibbonButton(Main.Command_File_Save, 1, Importance.Minor);

            rbtn_2.IsLineEnd = true;

            RibbonButton rbtn_3 = new RibbonButton(Main.Command_Nav_Back, 2, Importance.Minor);

            rbtn_3.IsSectionEnd = true;

            RibbonButton rbtn_4 = new RibbonButton(Main.Command_Clip_Paste, 3, Importance.Minor);

            rbtn_4.IsLineEnd = true;

            RibbonButton rbtn_5 = new RibbonButton(Main.Command_Nav_Next, 4, Importance.Minor);

            rbtn_5.IsLineEnd = true;

            RibbonButton rbtn_6 = new RibbonButton(Main.Command_Clip_Copy, 5, Importance.Minor);

            rbtn_6.IsLineEnd = true;

            RibbonButton rbtn_7 = new RibbonButton(c_StockChart, 10, Importance.Major);

            RibbonPane rbtpane = new RibbonPane("Home Pane", 1);
            rbtpane.CornerButtonCommand = c_StockChart;
            rbtpane.Add(rbtn_1);
            rbtpane.Add(rbtn_2);
            rbtpane.Add(rbtn_3);
            rbtpane.Add(rbtn_4);
            rbtpane.Add(rbtn_5);
            rbtpane.Add(rbtn_6);
            rbtpane.Add(rbtn_7);

            RibbonButton rbtn_8 = new RibbonButton(Main.Command_File_Open, 0, Importance.Major);

            RibbonButton rbtn_9 = new RibbonButton(c_StockChart, 0, Importance.Major);

            RibbonPane rbtpane2 = new RibbonPane("Test Pane", 2);
            rbtpane2.CornerButtonCommand = c_StockChart;
            rbtpane2.Add(rbtn_8);
            rbtpane2.Add(rbtn_9);

            RibbonButton rbtn_20 = new RibbonButton(Main.Command_Clip_Paste, 0, Importance.Major);
            RibbonButton rbtn_21 = new RibbonButton(Main.Command_Clip_Cut, 1, Importance.Minor) { IsLineEnd = true };
            RibbonButton rbtn_22 = new RibbonButton(Main.Command_Clip_Copy, 2, Importance.Minor) { IsLineEnd = true };
            RibbonButton rbtn_23 = new RibbonButton(Main.Command_Clip_Delete, 3, Importance.Minor) { IsSectionEnd = true };

            RibbonPane rbtpane_clip = new RibbonPane("Clip Board", 0);
            rbtpane_clip.CornerButtonCommand = c_StockChart;
            rbtpane_clip.Add(rbtn_20);
            rbtpane_clip.Add(rbtn_21);
            rbtpane_clip.Add(rbtn_22);
            rbtpane_clip.Add(rbtn_23);

            RibbonButton rbtn_120 = new RibbonButton(Main.Command_Clip_Paste, 0, Importance.Major);
            RibbonButton rbtn_121 = new RibbonButton(Main.Command_Clip_Cut, 1, Importance.Minor) { IsLineEnd = true };
            RibbonButton rbtn_122 = new RibbonButton(Main.Command_Clip_Copy, 2, Importance.Minor) { IsLineEnd = true };
            RibbonButton rbtn_123 = new RibbonButton(Main.Command_Clip_Delete, 3, Importance.Minor) { IsSectionEnd = true };
            RibbonButton rbtn_124 = new RibbonButton(c_StockChart, 3, Importance.Major) { IsSectionEnd = true };

            RibbonPane rbtpane_clip2 = new RibbonPane("Clip Board Pro", 0);
            rbtpane_clip2.CornerButtonCommand = c_StockChart;
            rbtpane_clip2.Add(rbtn_120);
            rbtpane_clip2.Add(rbtn_121);
            rbtpane_clip2.Add(rbtn_122);
            rbtpane_clip2.Add(rbtn_123);
            rbtpane_clip2.Add(rbtn_124);

            RibbonTabItem rbtHome = new RibbonTabItem("Home");
            rbtHome.Add(rbtpane);
            rbtHome.Add(rbtpane_clip);

            RibbonTabItem rbtView = new RibbonTabItem("View");
            rbtView.Add(rbtpane2);

            RibbonTabItem rbtCharts = new RibbonTabItem("Charts");
            rbtCharts.Add(rbtpane_clip2);

            RibbonButton rbtn_220 = new RibbonButton(c_Power, 0, Importance.Major);
            RibbonButton rbtn_221 = new RibbonButton(Main.Command_Clip_Cut, 1, Importance.Minor) { IsLineEnd = false };
            RibbonButton rbtn_222 = new RibbonButton(Main.Command_Clip_Copy, 2, Importance.Tiny) { IsLineEnd = false };
            RibbonButton rbtn_223 = new RibbonButton(Main.Command_Clip_Delete, 3, Importance.Tiny) { IsSectionEnd = true };
            RibbonButton rbtn_224 = new RibbonButton(c_StockChart, 3, Importance.Major) { IsSectionEnd = true };

            RibbonPane rbtpane_clip3 = new RibbonPane("Strategy Board Pro", 0);
            rbtpane_clip3.Add(rbtn_220);
            rbtpane_clip3.Add(rbtn_221);
            rbtpane_clip3.Add(rbtn_222);
            rbtpane_clip3.Add(rbtn_223);
            rbtpane_clip3.Add(rbtn_224);

            RibbonTabItem rbtStrategy = new RibbonTabItem("Strategy");
            rbtStrategy.Add(rbtpane_clip3);

            mo.Ribbon.AddRibbonTab(rbtHome);
            mo.Ribbon.AddRibbonTab(rbtView);
            mo.Ribbon.AddRibbonTab(rbtCharts);
            mo.Ribbon.AddRibbonTab(rbtStrategy);


            // mo.OrbMenu.Add(rbtpane);
        }
    }
}
