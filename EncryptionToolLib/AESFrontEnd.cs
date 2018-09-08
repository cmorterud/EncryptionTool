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
        public AESFrontEnd()
        {
            // AES-128 for now
            aesService = new AesCryptoServiceProvider
            {
                BlockSize = 128,
                KeySize = 128,
                Padding = PaddingMode.PKCS7,
                Mode = CipherMode.CBC
            };
            aesService.GenerateIV();


        }
        public void SetKey(string _key)
        {
            key = _key;
            aesService.Key = Convert.FromBase64String(key);
        }
        public string Encrypt(string Plaintext)
        {
            CheckKey();
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
        private string EncryptString(SymmetricAlgorithm symAlg, string inString)
        {
            byte[] inBlock = Encoding.Unicode.GetBytes(inString);
            ICryptoTransform xfrm = symAlg.CreateEncryptor();
            byte[] outBlock = xfrm.TransformFinalBlock(inBlock, 0, inBlock.Length);

            return Convert.ToBase64String(outBlock);
        }
        private string DecryptString(SymmetricAlgorithm symAlg, string inString)
        {
            byte[] inBytes = Convert.FromBase64String(inString);
            ICryptoTransform xfrm = symAlg.CreateDecryptor();
            byte[] outBlock = xfrm.TransformFinalBlock(inBytes, 0, inBytes.Length);

            return Encoding.Unicode.GetString(outBlock);
        }
    }
}
