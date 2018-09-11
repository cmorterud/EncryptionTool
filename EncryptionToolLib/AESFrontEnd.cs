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
        private const int bytesToBit = 8;
        public enum KEYSIZE
        {
            AES128 = 128,
            AES256 = 256
        }
        KEYSIZE _size;
        private const int blockSize = 128;
        private const int bytesInBlock = blockSize / bytesToBit;
        public AESFrontEnd(KEYSIZE size)
        {
            // AES-128 for now
            aesService = new AesCryptoServiceProvider
            {
                BlockSize = blockSize,
                KeySize = (int)size,
                Padding = PaddingMode.PKCS7, 
                Mode = CipherMode.CBC
            };
            _size = size;
        }
        public void SetKey(string _key)
        {
            key = _key;
            aesService.Key = Convert.FromBase64String(key);
        }
        public string Encrypt(string Plaintext)
        {
            CheckKey();
            // generate new IV, part of standard
            aesService.GenerateIV();
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

            // encrypt
            ICryptoTransform xfrm = symAlg.CreateEncryptor();
            byte[] outBlock = xfrm.TransformFinalBlock(inBlock, 0, inBlock.Length);

            // write the IV to the beginning of the message
            var outArray = new byte[aesService.IV.Length + outBlock.Length];
            aesService.IV.CopyTo(outArray, 0);
            // write the encrypted message to the message
            outBlock.CopyTo(outArray, bytesInBlock);

            return Convert.ToBase64String(outArray);
        }
        private string DecryptString(SymmetricAlgorithm symAlg, string inString)
        {

            var inBlock = Convert.FromBase64String(inString);

            // read the IV from the message
            var bytesIV = new byte[bytesInBlock];
            Array.Copy(inBlock, bytesIV, bytesInBlock);
            aesService.IV = bytesIV;

            // decrypt using the IV read from the message
            ICryptoTransform xfrm = symAlg.CreateDecryptor();
            byte[] outBlock = xfrm.TransformFinalBlock(inBlock, bytesInBlock, inBlock.Length - bytesInBlock);

            return Encoding.Unicode.GetString(outBlock);
        }
    }
}
