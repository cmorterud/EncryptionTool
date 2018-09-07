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
            return "";
        }
        public string Decrypt(string EncryptedText)
        {
            CheckKey();
            return "";
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
    }
}
