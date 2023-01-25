using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nitride.EE
{
    public interface IHardwareDevice : IDisposable
    {
        bool Open();

        void Close();

        bool IsConnected { get; }

        string ResourceName { get; }
    }
}
