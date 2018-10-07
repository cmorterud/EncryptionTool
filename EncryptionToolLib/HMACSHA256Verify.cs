using System.Linq;
using System.Security.Cryptography;

namespace EncryptionToolLib
{
    public class HMACSHA256Verify : IVerify
    {
        private HMACSHA256 helper;
        public bool verify(byte[] text, byte[] key, byte[] signature)
        {
            helper = new HMACSHA256(key);
            var hash = helper.ComputeHash(text);
            return hash.SequenceEqual(signature);
        }
        public byte[] computeMAC(byte[] text, byte[] key)
        {
            helper = new HMACSHA256(key);
            return helper.ComputeHash(text);
        }
    }
}
