using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Nitride.EE
{
    public class FPGAPin : SchematicPin
    {
        public string DiffName { get; set; }

        public string Bank { get; set; }

        public string VREFGroup { get; set; }

        public string PinName { get; set; }

        public string MemoryByteGroup { get; set; }

        public string IOType { get; set; }

        public string SuperLogicRegion { get; set; }

        public bool IsIO { get; set; } = false;

        public string NetName { get; set; }

        public string AssignedName { get; set; }

        public string PairName { get; set; }

        public string IOStandard { get; set; } //= "LVCMOS33";

        public string PullType { get; set; }

        public string CurrentDrive { get; set; }

        public double PackageDelayTime { get; set; } = 0;

        public double PackageDelayLength { get; set; } = 0;

        public int X { get; set; } = 0;

        public int Y { get; set; } = 0;

        public const string CsvHeader = "Object Kind,Pin Designator,Name,Net Name,Assigned Name,IO Standard,PullType,CurrentDrive,Bank,IO Type,Memory Byte Group,Super Logic Region,Propagation Delay,Pin/Pkg Length,X1,Y1\n";

        public string CsvLine => "Pin," + Designator + "," + PinName + "," + NetName + "," + AssignedName + "," + IOStandard + "," + PullType + "," + CurrentDrive + "," + Bank + "," + IOType + "," + MemoryByteGroup + "," + SuperLogicRegion + "," 
            + (double.IsNaN(PackageDelayTime) ? string.Empty : PackageDelayTime.ToString("0.###") + "ps") + "," + (double.IsNaN(PackageDelayLength) ? string.Empty : PackageDelayLength.ToString("0.##") + "mil") + "," + X + "," + Y;
    }
}
