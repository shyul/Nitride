/// ***************************************************************************
/// Nitride Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Text based data functions.
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Nitride
{
    public class Tunable : Range<double>
    {
        public Tunable(double value, Action<double> updated = null) : base(value)
        {
            ValueIsUpdated = updated;
            m_Value = value;
            Percent = 0.5;
        }

        // public Range<double> Bound { get; set; }

        public double MaxBound { get; set; } = double.MaxValue;

        public double MinBound { get; set; } = double.MinValue;

        public double MaxRange => MaxBound - MinBound;

        public double Delta => Maximum - Minimum;

        public double Percent
        {
            get => m_Percent;
            set
            {
                m_Percent = value;
                UpdatePercent();
            }
        }

        private double m_Percent = 0.5;

        public void UpdatePercent()
        {
            if (m_Percent < 0) m_Percent = 0;
            OriginalValue = Value;

            Maximum = OriginalValue * (1 + Percent);
            if (Maximum > MaxBound) Maximum = MaxBound;

            Minimum = OriginalValue * (1 - Percent);
            if (Minimum < MinBound) Minimum = MinBound;

        }

        public void UpdateValue(double ratio) => Value = (Delta * ratio) + Minimum;

        public double Value
        {
            get => m_Value;
            set
            {
                m_Value = value;
                ValueIsUpdated?.Invoke(m_Value);
            }
        }

        private double m_Value;

        private Action<double> ValueIsUpdated;

        public double OriginalValue { get; private set; }

        public double Offset => (Value / OriginalValue) - 1;
    }

    public class TunableControl
    {
        public TunableControl(Tunable tunable, TextBox textBoxValue, TextBox textBoxPercent, TrackBar trackBar)
        {
            Tunable = tunable;
            TextBoxValue = textBoxValue;
            TextBoxPercent = textBoxPercent;
            TrackBar = trackBar;

            TextBoxValue.Validated += new EventHandler(TextBoxValue_Validated);
            TextBoxValue.Leave += new EventHandler(TextBoxValue_Validated);
            TextBoxValue.KeyDown += new KeyEventHandler(TextBoxValue_KeyDown);

            TextBoxPercent.Validated += new EventHandler(TextBoxPercent_Validated);
            TextBoxPercent.Leave += new EventHandler(TextBoxPercent_Validated);

            TrackBar.Value = 50;
            TrackBar.Minimum = 0;
            TrackBar.Maximum = 100;
            TrackBar.Scroll += new EventHandler(TrackBar_Scroll);
            TrackBar.Validated += new EventHandler(TrackBar_Validated);

            UpdateTune();
        }

        private Tunable Tunable { get; }

        public TrackBar TrackBar { get; }

        public TextBox TextBoxValue { get; }

        public TextBox TextBoxPercent { get; }

        private void TextBoxPercent_Validated(object sender, EventArgs e)
        {
            try
            {
                Tunable.Percent = TextBoxPercent.Text.ToDouble() / 100;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            UpdateTune();
        }

        private void TrackBar_Scroll(object sender, EventArgs e)
        {
            double ratio = (TrackBar.Value - TrackBar.Minimum) / (1.0D * (TrackBar.Maximum - TrackBar.Minimum));
            Tunable.UpdateValue(ratio);
            TextBoxValue.Text = Tunable.Value.ToString();
            TextBoxPercent.Text = (Tunable.Offset * 100).ToString();
        }

        private void TrackBar_Validated(object sender, EventArgs e)
        {
            UpdateTune();
        }

        private void TextBoxValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TextBoxValue_Validated(sender, e);
            }
        }

        private void TextBoxValue_Validated(object sender, EventArgs e)
        {
            try
            {
                Tunable.Value = TextBoxValue.Text.ToDouble();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            UpdateTune();
        }

        public void UpdateTune()
        {
            Tunable.UpdatePercent();
            TextBoxValue.Text = Tunable.Value.ToString();
            TextBoxPercent.Text = (Tunable.Percent * 100).ToString();
            TrackBar.Enabled = (Tunable.Value < Tunable.MaxBound && Tunable.Value > Tunable.MinBound);
            TrackBar.Value = 50;
        }
    }
}
