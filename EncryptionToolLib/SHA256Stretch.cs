using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace EncryptionToolLib
{
    public class SHA256Stretch : IPasswordStretch
    {
        private SHA256 SHAProvider;
        // TODO: Dependency Injection
        public SHA256Stretch()
        {
            SHAProvider = new SHA256Managed();
        }
        public byte[] Stretch(byte[] text)
        {
            var hash = SHAProvider.ComputeHash(text);
            return text;
        }
    }
}
