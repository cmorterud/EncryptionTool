using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptionToolLib
{
    public class SimpleEncrypt : IEncryptionHelper
    {
        private const string ENC_KEY = "ENCRYPTED";
        public string Decrypt(string text)
        {
            var positionBegin = text.IndexOf(' ') + 1;
            var positionEnd = text.IndexOf(']');
            return text.Substring(positionBegin, positionEnd - positionBegin);
        }
        public string Encrypt(string text)
        {
            return "[" + ENC_KEY + " " + text + "]";
        }
    }
}
