using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nitride.EE
{
    public class LMX2572 : PLL
    {

        public override double R_Ratio => PreR * R_Div / (ReferenceMulti * (EnableRefDoubler ? 2 : 1));

        public bool EnableRefDoubler { get; set; }

        public uint PreR { get; set; }

        public uint ReferenceMulti { get; set; }

        public uint R_Div { get; set; }



        public uint PFD_DLY_SEL { get; set; }

        public uint Mash_Order { get; set; }

        public bool Mash_Reset_N { get; set; }

        public uint Mash_Reset_Counter { get; set; }

        public uint Mash_Seed { get; set; }





        public uint VcoSel { get; set; }

        public bool VcoSel_Force { get; set; }



        public bool RFOutA_Enable { get; set; }

        public bool RFOutB_Enable { get; set; }

        public uint RFOutA_Mux { get; set; }

        public uint RFOutB_Mux { get; set; }

        public uint RFOutA_Level { get; set; }

        public uint RFOutB_Level { get; set; }
    }
}
