using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace EncryptionToolLib
{
    // Official AES standard
    // https://nvlpubs.nist.gov/nistpubs/fips/nist.fips.197.pdf
    // Microsoft standard
    // https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.symmetricalgorithm.createencryptor?view=netframework-4.7.2
    public class AESFrontEnd : IEncryptionHelper
    {
        private AesCryptoServiceProvider aesService = null;
        private string key;
        private int numBytesIV;
        private const int bytesToBit = 8;
        public enum KEYSIZE
        {
            AES128 = 128,
            AES256 = 256
        }
        KEYSIZE size = KEYSIZE.AES128;
        public AESFrontEnd()
        {
            // AES-128 for now
            aesService = new AesCryptoServiceProvider
            {
                BlockSize = 128,
                KeySize = (int)size,
                Padding = PaddingMode.PKCS7, 
                Mode = CipherMode.CBC
            };
            size = KEYSIZE.AES128;
        }
        public void SetKey(string _key)
        {
            key = _key;
            aesService.Key = Convert.FromBase64String(key);
        }
        public string Encrypt(string Plaintext)
        {
            CheckKey();
            aesService.GenerateIV();
            return EncryptString(aesService, Plaintext);
        }
        public string Decrypt(string EncryptedText)
        {
            CheckKey();
            return DecryptString(aesService, EncryptedText);
        }
        private void CheckKey()
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Key is null or empty");
            }
            if (key.Contains(" "))
            {
                throw new ArgumentException("Key has whitespace");
            }
        }
        public void SwitchKeySize(KEYSIZE _size)
        {
            aesService.KeySize = (int)_size;
            size = _size;
        }
        private string EncryptString(SymmetricAlgorithm symAlg, string inString)
        {
            numBytesIV = (int)size / bytesToBit;

            byte[] inBlock = Encoding.Unicode.GetBytes(inString);

            // encrypt
            ICryptoTransform xfrm = symAlg.CreateEncryptor();
            byte[] outBlock = xfrm.TransformFinalBlock(inBlock, 0, inBlock.Length);

            // write the IV to the beginning of the message
            var outArray = new byte[aesService.IV.Length + outBlock.Length];
            aesService.IV.CopyTo(outArray, 0);
            // write the encrypted message to the message
            outBlock.CopyTo(outArray, numBytesIV);

            return Convert.ToBase64String(outArray);
        }
        private string DecryptString(SymmetricAlgorithm symAlg, string inString)
        {
            numBytesIV = (int)size / bytesToBit;

            var inBlock = Convert.FromBase64String(inString);

            // read the IV from the message
            var bytesIV = new byte[numBytesIV];
            Array.Copy(inBlock, bytesIV, numBytesIV);
            aesService.IV = bytesIV;

            // decrypt using the IV read from the message
            ICryptoTransform xfrm = symAlg.CreateDecryptor();
            byte[] outBlock = xfrm.TransformFinalBlock(inBlock, numBytesIV, inBlock.Length - numBytesIV);

            return Encoding.Unicode.GetString(outBlock);
        }
    }
}
