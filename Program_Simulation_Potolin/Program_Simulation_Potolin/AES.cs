using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Program_Simulation_Potolin
{
    public class AES
    {
        public static byte[] Encrypt(byte[] toEncrypt, byte[] key)
        {
            using (var aes = Aes.Create())
            {
                aes.KeySize = 128;
                aes.BlockSize = 128;
                aes.Key = key;
                aes.IV = new byte[16]; // Zero IV for simplicity
                aes.Padding = PaddingMode.None;

                using (var encryptor = aes.CreateEncryptor())
                {
                    return encryptor.TransformFinalBlock(toEncrypt, 0, toEncrypt.Length);
                }
            }
        }

        public static byte[] Decrypt(byte[] toDecrypt, byte[] key)
        {
            using (var aes = Aes.Create())
            {
                aes.KeySize = 128;
                aes.BlockSize = 128;
                aes.Key = key;
                aes.IV = new byte[16]; // Zero IV for simplicity
                aes.Padding = PaddingMode.None;

                using (var decryptor = aes.CreateDecryptor())
                {
                    return decryptor.TransformFinalBlock(toDecrypt, 0, toDecrypt.Length);
                }
            }
        }
    }
}
