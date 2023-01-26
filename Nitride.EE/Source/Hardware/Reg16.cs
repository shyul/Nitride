using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nitride.EE
{
    public class Reg16 : Dictionary<ushort, ushort>
    {
        public Reg16(ushort size)
        {
            for (ushort i = 0; i < size; i++)
            {
                this[i] = 0;
            }
        }

        public byte[] Serialize(IEnumerable<ushort> addr_list)
        {
            var sorted_list = addr_list.Where(n => ContainsKey(n)).OrderByDescending(n => n);
            int length = sorted_list.Count();

            DataBuffer buffer = new (length * 4);
            int i = 0;
            foreach (ushort addr in sorted_list)
            {
                buffer.DU16[i].D1 = addr;
                buffer.DU16[i].D2 = this[addr];
                i++;
            }
            return buffer.Bytes;
        }

        public byte[] Serialize() => Serialize(Keys);

        public void Deserialize(byte[] bytes) 
        {
            DataBuffer buffer = new(bytes);
            int length = bytes.Length / 4;

            for (int i = 0; i < length; i++) 
            {
                this[buffer.DU16[i].D1] = buffer.DU16[i].D2;
            }
        }
    }
}
