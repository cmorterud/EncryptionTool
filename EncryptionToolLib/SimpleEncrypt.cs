using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptionToolLib
{
    public class SimpleEncrypt : IEncryptionHelper
    {
        private string _key = "ENCRYPTED";
        public string Decrypt(string text)
        {
            var positionBegin = text.IndexOf(' ') + 1;
            var positionEnd = text.IndexOf(']');
            return text.Substring(positionBegin, positionEnd - positionBegin);
        }
        public string Encrypt(string text)
        {
            return "[" + _key + " " + text + "]";
        }
        public void SetKey(string key)
        {
            _key = key;
        }
    }
}
