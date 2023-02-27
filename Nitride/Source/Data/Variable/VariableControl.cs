/// ***************************************************************************
/// Nitride Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace Nitride
{
    [DesignerCategory("Code")]
    public class VariableEntryTextBox : TextBox 
    {
        public VariableEntryTextBox(VariableControl vc) 
        {
            Control = vc;
            Location = new Point(0, 0);
            Dock = DockStyle.Left;
            Visible = false;
        }

        VariableControl Control { get; }

        public void UpdateValue()
        {
            string s = Text.Trim();
            double n;


            if (!string.IsNullOrWhiteSpace(s))
            {
                char c = s[s.Length - 1];

                switch (c)
                {
                    case 'T':
                        s = s.Substring(0, s.Length - 1);
                        n = s.ToDouble() * 1e12;
                        break;

                    case 'g':
                    case 'G':
                        s = s.Substring(0, s.Length - 1);
                        n = s.ToDouble() * 1e9;
                        break;

                    case 'M':
                        s = s.Substring(0, s.Length - 1);
                        n = s.ToDouble() * 1e6;
                        break;

                    case 'k':
                    case 'K':
                        s = s.Substring(0, s.Length - 1);
                        n = s.ToDouble() * 1e3;
                        break;

                    case 'm':
                        s = s.Substring(0, s.Length - 1);
                        n = s.ToDouble() * 1e-3;
                        break;

                    case 'u':
                    case 'μ':
                        s = s.Substring(0, s.Length - 1);
                        n = s.ToDouble() * 1e-6;
                        break;

                    case 'n':
                        s = s.Substring(0, s.Length - 1);
                        n = s.ToDouble() * 1e-9;
                        break;

                    case 'p':
                        s = s.Substring(0, s.Length - 1);
                        n = s.ToDouble() * 1e-12;
                        break;

                    default:
                        n = s.ToDouble();
                        break;
                }


                Console.WriteLine(s + " | " + n);

                // Text = n.ToString(Control.Format);
                Control.Update(n);
            }
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) 
            {
                Visible = false;

            }
            else if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                Visible = false;
                UpdateValue();

            }

            base.OnKeyDown(e);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
         
            if (!Bounds.Contains(new Point(e.X, e.Y))) 
            {
                //Visible = false;
            }
            else
            {
                Focus();
            }
        }

        protected override void OnLostFocus(EventArgs e)
        {
           if (Visible) Visible = false;
          //  UpdateValue();
            base.OnLostFocus(e);
        }

        /*
        protected override void OnValidated(EventArgs e)
        {
            Visible = false;

            base.OnValidated(e);
        }*/

        protected override void OnTextChanged(EventArgs e)
        {
            //UpdateValue();
            base.OnTextChanged(e);
        }
    }

    // Define Max Digits, like 15 places? Depends on the INCR
    // Data Represented align T. G, M, K
     
    public class VariableControlDigit
    {
        public Rectangle Bound { get; set; }

        //public Point Center { get; set; }

        public int Radix { get; set; }

        public int Number { get; set; }
    }


    [DesignerCategory("Code")]
    public class VariableControl : UserControl
    {
        public VariableControl(VariableData vd, Font font, int places)
        {
            DoubleBuffered = true;
            Dock = DockStyle.Fill;

            VariableData = vd;

            BackColor = Color.FromArgb(255, 253, 245);
            ColorTheme = new(Color.DimGray, Color.DimGray, Color.Gray);

            Font = font;// new("Consolas", 15F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));

            Controls.Add(TextBox = new(this)
            {
                Font = Font,
                BackColor = BackColor,
                Visible = false
            });

            Places = places;
            Digits = new VariableControlDigit[Places];

            for (int i = 0; i < Digits.Length; i++)
            {
                Digits[i] = new VariableControlDigit();
            }

            // VariableData.Update(123.234321);
        }

        public VariableData VariableData { get; set; }

        public virtual void Update(double val)
        {
            VariableData.Update(val);
            Coordinate();
        }

        public override Font Font
        {
            get => m_Font;

            set
            {
                m_Font = value;
                FontWidth = Convert.ToInt32(TextRenderer.MeasureText("0", m_Font).Width * 0.6f);
                /*
                if (TextBox is not null)
                {
                    TextBox.Size = new Size(Width, Height);
                    TextBox.Font = m_Font; 
                }*/
            }
        }

        protected Font m_Font;

        protected int FontWidth;

        public int Places
        {
            get => m_Places;
            set
            {
                m_Places = value;
                Format = "0." + new string('0', m_Places);
            }
        }

        protected int m_Places;

        public string Format { get; private set; }

        protected VariableControlDigit[] Digits { get; }

        protected Point PrefixLocation { get; set; }

        protected string Prefix { get; set; }

        protected double PrefixGain { get; set; }

        protected VariableEntryTextBox TextBox { get; }

        public ColorTheme ColorTheme { get; }

        public Rectangle EdgeBounds { get; set; }

        public bool UsePrefix { get; set; } = true;

        protected virtual void CoordinateLayout()
        {
            ResumeLayout(true);

            EdgeBounds = new Rectangle(0, 0, Width - 1, Height - 1);
            TextBox.Size = new Size(Width, Height);

            int c_x = Margin.Left;

            double d = VariableData.Value;
            double res;
            string s;

            if (UsePrefix) 
            {
                var (num, pfx) = d.ToUnitPostfixNumberString();

                Prefix = pfx + VariableData.Unit;
   
                PrefixGain = num;

                res = d * num;
                s = res.ToString(Format);
            }
            else
            {
                Prefix = VariableData.Unit;
                PrefixGain = 1;
                s = d.ToString(Format);
            }

            PrefixLocation = new Point(Right - Margin.Right, (Height / 2) + 1);

            if (Digits is not null)
            {
                int zeroPt = -1;
                int i;

                for (i = 0; i < Digits.Length; i++)
                {
                    VariableControlDigit digi = Digits[i];
                    //digi.Center = digi.Bound.Center();// new Point(c_x, c_y);

                    digi.Bound = new Rectangle(c_x, 0, FontWidth, Height);

                    c_x += FontWidth;

                    if (s[i] != '.')
                    {
                        digi.Number = s[i] - 0x30;
                        digi.Radix = 1;
                    }
                    else
                    {
                        zeroPt = i;
                        digi.Number = 0;
                        digi.Radix = 0;
                    }
                }

                if (zeroPt < 0) zeroPt = i - 1;
                int j = 1;

                for (i = zeroPt - 1; i >= 0; i--)
                {
                    VariableControlDigit digi = Digits[i];
                    digi.Radix = j;
                    j++;
                }

                j = -1;
                for (i = zeroPt + 1; i < Digits.Length; i++)
                {
                    VariableControlDigit digi = Digits[i];
                    digi.Radix = j;
                    j--;
                }

                /*
                Console.WriteLine("\n" + Prefix + " | " + PrefixGain);
                for (i = 0; i < Digits.Length; i++)
                {
                    VariableControlDigit digi = Digits[i];
                    Console.WriteLine("Num = " + digi.Number + " | Radix = " + digi.Radix + " | x = " + digi.Bound.Left );
                }
                */
            }



            PerformLayout();
    
        }

        public virtual void Coordinate()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate
                {
                    CoordinateLayout();
                });
            }
            else
            {
                CoordinateLayout();
            }
            Invalidate(false);
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

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            Graphics g = pe.Graphics;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            g.DrawRectangle(ColorTheme.EdgePen, EdgeBounds);

            for (int i = 0; i < Digits.Length; i++)
            {
                VariableControlDigit digi = Digits[i];
                int radix = digi.Radix;
                if (radix > 0) radix--;

                if (digi.Radix != 0)
                {
                    double pt = Math.Pow(10, radix) / PrefixGain;
                    if (digi.Number * pt >= VariableData.MinimumStep || pt >= VariableData.MinimumStep)
                        g.DrawString(digi.Number.ToString(), Font, ColorTheme.ForeBrush, digi.Bound.Center(), AppTheme.TextAlignCenter);
                }
                else
                {
                    g.DrawString(".", Font, ColorTheme.ForeBrush, digi.Bound.Center(), AppTheme.TextAlignCenter);
                }
            }

            g.DrawString(Prefix, Font, ColorTheme.ForeBrush, PrefixLocation, AppTheme.TextAlignRight);
        }

        // 1. Display
        // 2. Mouse Wheel
        // 3. Text Entry

        public virtual MouseState MouseState { get; protected set; }

        public virtual Point MousePoint { get; protected set; }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (!TextBox.Visible)
            {
                TextBox.Text = string.Empty;
                TextBox.Visible = true;
                Thread.Sleep(10);
                TextBox.Focus();
            }
            //base.OnMouseClick(e);
            //TextBox.Text = VariableData.Value.ToString(Format);

            //TextBox.Focus();
            // base.OnMouseClick(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {

            MousePoint = new Point(e.X, e.Y);


        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {

           // int num = -e.Delta * SystemInformation.MouseWheelScrollLines / 360;
            //int num = e.Delta > 0 ? 1 : -1;
            //if (e.Delta == 0) num = 0;
            int num = e.Delta / 120;
            MousePoint = new Point(e.X, e.Y);

            if (num != 0)
            {
                if (ModifierKeys.HasFlag(Keys.Control))
                {

                }
                else
                {

                }

                if (Digits.Where(n => n.Bound.Contains(MousePoint)).FirstOrDefault() is VariableControlDigit digi && digi.Radix != 0)
                {
                    int radix = digi.Radix;
                    if (radix > 0) radix--;

                    if (!(radix >= Digits.Select(n => n.Radix).Max() - 1 && digi.Number + num <= 0)) 
                    {
                        double incr = num * Math.Pow(10, radix) / PrefixGain;
                        if (Math.Abs(incr) < VariableData.MinimumStep) incr = incr > 0 ? VariableData.MinimumStep : -VariableData.MinimumStep;

                        // Console.WriteLine("radix = " + radix + " | dnum  = " + digi.Number + " | num = " + num);
                        Update(VariableData.Value + incr);
                    }

      
                }
            }

            //Console.WriteLine("Wheel Done");
        }
    }
}
