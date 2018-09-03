using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtisanCode.SimpleAesEncryption;

namespace EncryptionToolLib
{
    public class AESFrontEnd : IEncryptionHelper
    {
        public string key = null;
        public RijndaelMessageEncryptor EncryptionHelper = null;
        public RijndaelMessageDecryptor DecryptionHelper = null;
        public AESFrontEnd()
        {
            EncryptionHelper = new RijndaelMessageEncryptor();
            DecryptionHelper = new RijndaelMessageDecryptor();
        }
        public void SetKey(string _key)
        {
            key = _key;
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
