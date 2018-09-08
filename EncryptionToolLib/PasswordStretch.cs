using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace EncryptionToolLib
{
    public class PasswordStretch
    {
        private SHA256 SHAProvider;
        // TODO: Dependency Injection
        public PasswordStretch()
        {
            SHAProvider = new SHA256Managed();
        }
        public string Hash(string text)
        {
            var bytes = Encoding.Unicode.GetBytes(text);
            var hash = SHAProvider.ComputeHash(bytes);
            text = Convert.ToBase64String(hash);
            return text;
        }
    }
}
