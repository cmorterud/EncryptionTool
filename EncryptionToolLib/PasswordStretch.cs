using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace EncryptionToolLib
{
    class PasswordStretch
    {
        private SHA256 SHAProvider;
        public string Hash(string text)
        {
            var base64 = Convert.FromBase64String(text);
            var hash = SHAProvider.ComputeHash(base64);
            text = Convert.ToBase64String(hash);
            return text;
        }
    }
}
