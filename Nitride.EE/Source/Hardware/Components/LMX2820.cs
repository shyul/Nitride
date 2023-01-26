using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nitride.EE
{
    public class LMX2820 : PLL
    {
        public LMX2820(IClock reference)
        {
            Reference = reference;

            // Initialize Registers
        }

        public Reg16 Regs { get; } = new(123);

        // Buffer Buffer2 { get; }

        public override double R_Ratio => throw new NotImplementedException();
    }
}
