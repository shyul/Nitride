using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Nitride.EE
{
    public static class Altera
    {
        private static void AddPinFunction(this List<string> funcList, string func) 
        {
            if(!string.IsNullOrEmpty(func) && !funcList.Contains(func)) 
            {
                funcList.Add(func);
            }
            /*
            if (funcList.Contains("GND"))
            {
                funcList.Clear();
                funcList.Add("GND");
            }*/
        }

        public static void ReadPinoutFileMAX2(string txtFilename)
        {
            if (File.Exists(txtFilename))
            {
                using var fs = new FileStream(txtFilename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using StreamReader sr = new(fs);
                //string[] headers = sr.CsvReadFields();

                Dictionary<string, FPGAPin> pinList = new();
                List<string> vrefList = new();

                while (!sr.EndOfStream)
                {
                    string[] fields = sr.ReadLine().Trim().Split('\t');
                    Console.WriteLine(fields.ToStringWithIndex());
                }

            }
        }

        public static string ToStringWithIndexTrim<T>(this ICollection<T> list)
        {
            string s = string.Empty;
            for (int i = 0; i < list.Count; i++)
            {
                s += "(" + i.ToString() + ")\"" + list.ElementAt(i).ToString().Trim() + "\"-";
            }

            if (s.Length > 0) return s.TrimEnd('-');
            else return "Empty String Array";
        }

        public static void ReadQuartusPinFile(string pinFilename) 
        {
            if (File.Exists(pinFilename))
            {
                using var fs = new FileStream(pinFilename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using StreamReader sr = new(fs);
                //string[] headers = sr.CsvReadFields();

                Dictionary<string, FPGAPin> pinList = new();
                List<string> vrefList = new();
                int index = 0;
                while (!sr.EndOfStream)
                {
                    
                    string[] fields = sr.ReadLine().Trim().Split(':');

                    if (fields.Length == 7)
                    {
                        // Console.WriteLine(fields.ToStringWithIndexTrim());
                        //for (int i = 0; i < fields.Length; i++)
                        //{

                        PinType type = PinType.Passive;

                        string pinName = fields[0].Trim();
                        string pinNum = fields[1].Trim();
                        string direction = fields[2].Trim().ToUpper();
                        string standard = fields[3].Trim();
                        string voltage = fields[4].Trim();
                        string bank = fields[5].Trim();

                        //string label = pinName.StartsWith("RESERVED_") ? "IO" + bank : pinName;
                        string label = pinName;

                        if (pinName.StartsWith("RESERVED_")) 
                        {
                            type = PinType.IO;
                            label = "IO" + bank;

                        }
                        else if (voltage.Length > 0)
                        {
                            label = pinName + "(" + voltage + ")";
                        }

                        if (direction.Contains("POWER") || direction.Contains("GND")) direction = "PASSIVE";

                        if (direction.Contains("INPUT")) type = PinType.Input;

                        if (direction.Contains("OUTPUT")) type = PinType.Output;

                        if (index > 0)
                        {
                            pinList.Add(pinNum, new FPGAPin()
                            {
                                Designator = pinNum,
                                Name = label,
                                Type = type,
                                Bank = bank,
                            }); ;

                            //Console.WriteLine("Pin," + label + "," + pinNum + "," + direction);
                        }
                    

                        index++;
                    }
                }

                string title = "Object Kind,Name,Pin Designator,Electrical Type,X1,Y1";
                Console.WriteLine(title);

                int X = 0;
                int Y = 0;

                string lastBank = string.Empty;

                foreach (var pin in pinList.OrderBy(n => n.Value.Bank).ThenBy(n => n.Value.Name).ThenBy(n => n.Key))
                {
                    if (pin.Value.Bank != lastBank)
                    {
                        X += 500;
                        Y = 0;
                        lastBank = pin.Value.Bank;
                    }

                    string dirType = pin.Value.Type.ToString();
                    if (dirType == "IO") dirType = "I/O";

                    string line = "Pin," + pin.Value.Name + "," + pin.Key + "," + dirType + "," + X.ToString() + "," + Y.ToString();
                    Y -= 100;
                    Console.WriteLine(line);
                }

                Console.WriteLine("Total pin count = " + pinList.Count().ToString());
            }

        }

        public static void ReadPinoutFileCycloneV(string csvFileName)
        {
            if (File.Exists(csvFileName))
            {
                using var fs = new FileStream(csvFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using StreamReader sr = new(fs);
                //string[] headers = sr.CsvReadFields();

                Dictionary<string, FPGAPin> pinList = new();
                List<string> vrefList = new();

                while (!sr.EndOfStream)
                {
                    string[] fields = sr.CsvReadFields();

                    if (fields.Length == 8)
                    {
                        string bankName = fields[0].TrimCsvValueField();

                        string VREF = fields[1].TrimCsvValueField();
                        if (string.IsNullOrEmpty(VREF)) VREF = "General";

                        string basicPinName = fields[2].TrimCsvValueField().Replace(',', '/');
                        string optionalFunc = fields[3].TrimCsvValueField().Replace(',', '/');
                        string ConfigFunc = fields[4].TrimCsvValueField().Replace(',', '/');
                        string DiffChannels = fields[5].TrimCsvValueField();
                        string pinDesignator = fields[6].TrimCsvValueField();
                        string ddrFunc = fields[7].TrimCsvValueField();

                        if (!pinList.ContainsKey(pinDesignator)) pinList[pinDesignator] = new()
                        {
                            Designator = pinDesignator,
                            VREFGroup = VREF,
                            Bank = bankName
                        };

                        FPGAPin pin = pinList[pinDesignator];
                        if (!vrefList.Contains(VREF)) vrefList.Add(VREF);
                        List<string> pinFuncs = new();

                        pinFuncs.AddPinFunction(ConfigFunc);
                        pinFuncs.AddPinFunction(optionalFunc);
                        pinFuncs.AddPinFunction(ddrFunc);
                        pinFuncs.AddPinFunction(DiffChannels);
                        pinFuncs.AddPinFunction(basicPinName);

                        string diff = DiffChannels;

                        if (diff.Length > 2 && diff.Last() == 'p') diff = diff.Substring(0, diff.Length - 1) + "m";

                        pin.DiffName = diff;

                        switch (basicPinName)
                        {
                            case ("IO"):
                                pin.Type = PinType.IO;
                                break;
                            case ("TCK"):
                                pin.Type = PinType.Input;
                                pin.IsClock = true;
                                break;
                            case ("TMS"):
                            case ("TDI"):
                                pin.Type = PinType.Input;
                                break;
                            case ("TDO"):
                                pin.Type = PinType.Output;
                                break;

                            case ("nSTATUS"):
                                pin.Type = PinType.IO;
                                pin.IsLowActive = true;
                                break;

                            case ("CONF_DONE"):
                                pin.Type = PinType.IO;
                                break;

                            case ("nCONFIG"):
                            case ("nCE"):
                                pin.Type = PinType.Input;
                                pin.IsLowActive = true;
                                break;

                            case ("MSEL0"):
                            case ("MSEL1"):
                            case ("MSEL2"):
                            case ("MSEL3"):
                                pin.Type = PinType.Input;
                                break;
                        }

                        pin.Name = string.Join("/", pinFuncs.ToArray());
                        if (pin.Name.Contains("CLK") && !pin.Name.Contains("DPCLK"))
                        {
                            pin.IsClock = true;
                            if (pin.Type == PinType.Passive) pin.Type = PinType.Input;
                        }
                    }
                }

                StringBuilder sb = new("StartComponents\n\nComponent (Name \"Cyclone10LP\") (PartCount 1) (DesPrefix \"U ? \")\n");

                int LocationX = -15000;
                int LocationY = 1000;
                int pinCount = 0;
                foreach (string vref in vrefList)
                {
                    //Console.WriteLine("\n\nVREF Group: " + vref + "\n");
                    foreach (var pin in pinList.Where(n => n.Value.VREFGroup == vref).OrderBy(n => n.Value.DiffName).ThenBy(n => n.Value.Name).ThenBy(n => n.Value.Designator))
                    {
                        //Console.WriteLine(pin.Key + " | " + pin.Value.Type + " | " + pin.Value.Name + (pin.Value.IsLowActive ? " | IsLowActive " : string.Empty) + (pin.Value.IsClock ? " | IsClock " : string.Empty));
                        string row = "Pin ";
                        row += "(Location " + LocationX + ", " + LocationY + ") ";
                        row += "(Rotation 0) ";

                        string pinType = pin.Value.Type.ToString();
                        //if (pinType == "IO") pinType = "I/O";

                        row += "(PinType " + pinType + ") (Length 300) (Width 0) ";

                        if (pin.Value.IsClock)
                            row += "(HasClock 1) ";

                        if (pin.Value.IsLowActive)
                            row += "(HasDot 1) ";

                        row += "(Designator Visible \"" + pin.Key + "\") ";
                        row += "(Name Visible \"" + pin.Value.Name + "\")";
                        sb.AppendLine(row);
                        Console.WriteLine(row);
                        LocationY -= 100;
                        pinCount++;

                        if (LocationY < -8000)
                        {
                            LocationX += 2000;
                            LocationY = 1000;
                        }
                    }

                    LocationX += 2000;
                    LocationY = 1000;
                }

                sb.AppendLine("EndComponent\nEndComponents\n");
                sb.ToFile(csvFileName.Replace(".csv", string.Empty) + "_altium.txt");

                Console.WriteLine("Pin Count = " + pinCount);
            }
        }
        public static void ReadPinoutFile(string csvFileName)
        {
            if (File.Exists(csvFileName))
            {
                using var fs = new FileStream(csvFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using StreamReader sr = new(fs);
                //string[] headers = sr.CsvReadFields();

                Dictionary<string, FPGAPin> pinList = new();
                List<string> vrefList = new();

                while (!sr.EndOfStream)
                {
                    string[] fields = sr.CsvReadFields();

                    if (fields.Length == 7)
                    {
                        string bankName = fields[0].TrimCsvValueField();

                        string VREF = fields[1].TrimCsvValueField();
                        if (string.IsNullOrEmpty(VREF)) VREF = "General";

                        string basicPinName = fields[2].TrimCsvValueField();
                        string optionalFunc = fields[3].TrimCsvValueField();
                        string ConfigFunc = fields[4].TrimCsvValueField().Replace(',', '/');
                        string DiffChannels = fields[5].TrimCsvValueField();
                        string pinDesignator = fields[6].TrimCsvValueField();

                        if (!pinList.ContainsKey(pinDesignator)) pinList[pinDesignator] = new()
                        { 
                            Designator = pinDesignator, 
                            VREFGroup = VREF, 
                            Bank = bankName
                        };

                        FPGAPin pin = pinList[pinDesignator];
                        if (!vrefList.Contains(VREF)) vrefList.Add(VREF);
                        List<string> pinFuncs = new();

                        pinFuncs.AddPinFunction(ConfigFunc);
                        pinFuncs.AddPinFunction(optionalFunc);
                        pinFuncs.AddPinFunction(DiffChannels);
                        pinFuncs.AddPinFunction(basicPinName);

                        switch (basicPinName) 
                        {
                            case ("IO"):
                                pin.Type = PinType.IO;
                                break;
                            case ("TCK"):
                                pin.Type = PinType.Input;
                                pin.IsClock = true;
                                break;
                            case ("TMS"):
                            case ("TDI"):
                                pin.Type = PinType.Input;
                                break;
                            case ("TDO"):
                                pin.Type = PinType.Output;
                                break;

                            case ("nSTATUS"):
                                pin.Type = PinType.IO;
                                pin.IsLowActive = true;
                                break;

                            case ("CONF_DONE"):
                                pin.Type = PinType.IO;
                                break;

                            case ("nCONFIG"):
                            case ("nCE"):
                                pin.Type = PinType.Input;
                                pin.IsLowActive = true;
                                break;

                            case ("MSEL0"):
                            case ("MSEL1"):
                            case ("MSEL2"):
                            case ("MSEL3"):
                                pin.Type = PinType.Input;
                                break;
                        }

                        pin.Name = string.Join("/", pinFuncs.ToArray());
                        if (pin.Name.Contains("CLK") && !pin.Name.Contains("DPCLK"))
                        { 
                            pin.IsClock = true;
                            if (pin.Type == PinType.Passive) pin.Type = PinType.Input;
                        }
                    }
                }

                StringBuilder sb = new("StartComponents\n\nComponent (Name \"Cyclone10LP\") (PartCount 1) (DesPrefix \"U ? \")\n");

                int LocationX = -10000;
                int LocationY = 1000;
                int pinCount = 0;
                foreach(string vref in vrefList)
                {
                    //Console.WriteLine("\n\nVREF Group: " + vref + "\n");
                    foreach(var pin in pinList.Where(n => n.Value.VREFGroup == vref)) 
                    {
                        //Console.WriteLine(pin.Key + " | " + pin.Value.Type + " | " + pin.Value.Name + (pin.Value.IsLowActive ? " | IsLowActive " : string.Empty) + (pin.Value.IsClock ? " | IsClock " : string.Empty));
                        string row = "Pin ";
                        row += "(Location " + LocationX + ", " + LocationY + ") ";
                        row += "(Rotation 0) ";
                        
                        string pinType = pin.Value.Type.ToString();
                        //if (pinType == "IO") pinType = "I/O";

                        row += "(PinType " + pinType + ") (Length 300) (Width 0) ";

                        if (pin.Value.IsClock)
                            row += "(HasClock 1) ";

                        if (pin.Value.IsLowActive)
                            row += "(HasDot 1) ";

                        row += "(Designator Visible \"" + pin.Key + "\") ";
                        row += "(Name Visible \"" + pin.Value.Name + "\")";
                        sb.AppendLine(row);
                        Console.WriteLine(row);
                        LocationY -= 100;
                        pinCount++;

                        if (LocationY < -2000)
                        {
                            LocationX += 1000;
                            LocationY = 1000;
                        }
                    }

                    LocationX += 1000;
                    LocationY = 1000;
                }

                sb.AppendLine("EndComponent\nEndComponents\n");
                sb.ToFile(csvFileName.Replace(".csv", string.Empty) + "_altium.txt");
             
                Console.WriteLine("Pin Count = " + pinCount);
            }
        }

        // Pin (Location -1400, 1500) (Rotation 180) (PinType Power) (Length 300) (Width 0) (Designator Visible "J14") (Name Visible "VDDA_AVDAC") (PinSwap 0) (PartSwap 0) (PinSeq 1) (Part 14)
    }
}
