using System;
using System.Text;
using System.Security.Cryptography;

namespace Program_Simulation_Potolin
{
    public class DiffieHelman
    {
        public static (byte[], byte[], byte[]) GenerateParametersAndKeys(int p, int g)
        {
            using (var dh = new ECDiffieHellmanCng())
            {
                dh.KeySize = 256;  // Although we're using small p and g, ECDH in CNG does not directly allow setting them.
                dh.HashAlgorithm = CngAlgorithm.Sha256;
                byte[] publicKey = dh.PublicKey.ToByteArray();
                byte[] privateKey = dh.Key.Export(CngKeyBlobFormat.EccPrivateBlob);
                byte[] parameters = { Convert.ToByte(p), Convert.ToByte(g) };
                return (parameters, publicKey, privateKey);
            }
        }

        public static byte[] GenerateSharedKey(byte[] privateKey, byte[] otherPublicKey)
        {
            using (var dh = new ECDiffieHellmanCng(CngKey.Import(privateKey, CngKeyBlobFormat.EccPrivateBlob)))
            {
                dh.HashAlgorithm = CngAlgorithm.Sha256;
                return dh.DeriveKeyMaterial(CngKey.Import(otherPublicKey, CngKeyBlobFormat.EccPublicBlob));
            }
        }
    }
}
