using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nitride.EE.Visa
{
    /*
     :OUTP:CVCC?\sCH1\n

:OUTP:MODE?\sCH1\n

Command List:
:OUTPut:CVCC?
:OUTPut:MODE?
:OUTPut:OCP:ALAR?
:OUTPut:OCP:QUES?
:OUTPut:OCP:CLEAR
:OUTPut:OCP[:STATe]
:OUTPut:OCP:VALue
:OUTPut:OVP:ALAR?
:OUTPut:OVP:QUES?
:OUTPut:OVP:CLEAR
:OUTPut:OVP[:STATe]
:OUTPut:OVP:VALue
:OUTPut:RANGe
:OUTP:SENS
:OUTPut[:STATe]
:OUTPut:TIMEr
:OUTPut:TIMEr:STATe
:OUTPut:TRACk

:SOUR1:CURR?
:SOUR1:CURR 0.5
:SOUR2:CURR 0.5
:SOUR3:CURR 0.5

:SOUR1:VOLT 0.5 | MIN | MAX
:SOUR2:VOLT 6.5

:SOUR1:VOLT:PROT:TRIP? => NO
:SOUR1:VOLT:PROT:STAT? => OFF

:SOUR2:VOLT:PROT?
:SOUR2:VOLT:PROT 8
:SOUR2:CURR:PROT 1

:MEAS:ALL?
:MEAS:POWE? CH2
:MEAS:ALL?\sCH2  => V, I, P, 6.5059,0.0003,0.002\n


:OUTP? CH1
:OUTP CH1,OFF
:OUTP CH2,ON


     */

    public class PowerSupply : ViClient, IPowerSupply
    {
        public PowerSupply(string resourceName) : base(resourceName)
        {


        }

        public Dictionary<string, PowerSupplyChannel> PowerSupplyChannels { get; } = new();

        public void PowerSupply_ON(string channelName = "all")
        {

        }

        public void PowerSupply_OFF(string channelName = "all")
        {

        }

        public bool PowerSupply_Enabled(string channelName = "all")
        {
            return false;
        }

        public void PowerSupply_WriteSetting(string channelName)
        {

        }

        public void PowerSupply_ReadSetting(string channelName)
        {

        }

        public (double voltage, double current) PowerSupply_ReadOutput(string channelName)
        {
            return (0, 0);
        }
    }
}
