using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ceaser
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] buf = new byte[] { };
            byte[] encoded = new byte[buf.Length];
            int shift = 2;
            for (int i = 0; i < buf.Length; i++)
            {
                encoded[i] = (byte)(((uint)buf[i] + shift) & 0xFF);
            }
            StringBuilder hex = new StringBuilder(encoded.Length * 2);

            foreach (byte b in encoded)
            {
                hex.AppendFormat("0x{0:x2}, ", b);
            }
            Console.WriteLine("Payload of {0} bytes with the key of {1}:\n\n" + hex.ToString() + "\n", buf.Length, shift);
        }
    }
}