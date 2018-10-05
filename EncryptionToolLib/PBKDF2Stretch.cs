using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace EncryptionToolLib
{
    public class PBKDF2Stretch : IPasswordStretch
    {
        public byte[] Stretch(byte[] text)
        {
            // https://security.stackexchange.com/questions/3959/recommended-of-iterations-when-using-pkbdf2-sha256
            const int iterations = 100000;
            byte[] salt = new byte[16];
            using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider())
            {
                // Fill the array with a random value.
                rngCsp.GetBytes(salt);
            }

            var keyGen = new Rfc2898DeriveBytes(text, salt, iterations);

            var pwStretch = new byte[16];

            return keyGen.GetBytes(16);
        }
    }
}
