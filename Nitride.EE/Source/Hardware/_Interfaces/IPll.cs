using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nitride.EE
{
    public interface IPll : IDerivedClock
    {
        bool IsLocked { get; }
    }

    public abstract class PLL : IPll
    {
        public virtual bool IsLocked { get; set; }

        public virtual IClock Reference { get; set; }

        public virtual double DivRatio => (N_Div + ((double)F_Num * 1D / (double)F_Den)) / R_Ratio;

        public virtual double Frequency => Reference.Frequency * DivRatio;

        public virtual bool Enabled { get; set; }

        public virtual double PhaseDetectFreqency => Reference.Frequency / R_Ratio;

        public abstract double R_Ratio { get; }

        public virtual uint N_Div { get; set; }

        public virtual uint F_Num { get; set; }

        public virtual uint F_Den { get; set; }

    }
}
