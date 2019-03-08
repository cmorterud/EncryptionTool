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
        private byte[] key;
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
            aesService = new AesCryptoServiceProvider
            {
                BlockSize = blockSize,
                KeySize = (int)size,
                Padding = PaddingMode.PKCS7, 
                Mode = CipherMode.CBC
            };
            _size = size;
            SetKey();
        }
        public void SetKey()
        {
            aesService.GenerateKey();
            key = aesService.Key;
        }

        public string GetKey() {
            return Convert.ToBase64String(key);
        }

        public void SetKey(string _key)
        {
            CheckKey(_key);   
            key = Convert.FromBase64String(_key);
            aesService.Key = key;
        }
        private void CheckKey(string key)
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
        public string Encrypt(string Plaintext)
        {
            // generate new IV, part of standard
            aesService.GenerateIV();
            return EncryptString(aesService, Plaintext);
        }
        public string Decrypt(string EncryptedText)
        {
            return DecryptString(aesService, EncryptedText);
        }
        private string EncryptString(SymmetricAlgorithm symAlg, string inString)
        {

            byte[] inBlock = Encoding.UTF8.GetBytes(inString);

            var outArray = EncryptBytes(symAlg, inBlock);

            return Convert.ToBase64String(outArray);
        }
        private byte[] EncryptBytes(SymmetricAlgorithm symAlg, byte[] inBlock)
        {
            // encrypt
            ICryptoTransform xfrm = symAlg.CreateEncryptor();
            byte[] outBlock = xfrm.TransformFinalBlock(inBlock, 0, inBlock.Length);

            // write the IV to the beginning of the message
            var outArray = new byte[aesService.IV.Length + outBlock.Length];
            aesService.IV.CopyTo(outArray, 0);
            // write the encrypted message to the message
            outBlock.CopyTo(outArray, bytesInBlock);

            return outArray;
        }
        private string DecryptString(SymmetricAlgorithm symAlg, string inString)
        {

            var inBlock = Convert.FromBase64String(inString);

            var outBlock = DecryptBytes(symAlg, inBlock);

            return Encoding.UTF8.GetString(outBlock);
        }
        private byte[] DecryptBytes(SymmetricAlgorithm symAlg, byte[] inBlock)
        {

            // read the IV from the message
            var bytesIV = new byte[bytesInBlock];
            Array.Copy(inBlock, bytesIV, bytesInBlock);
            aesService.IV = bytesIV;

            // decrypt using the IV read from the message
            ICryptoTransform xfrm = symAlg.CreateDecryptor();
            byte[] outBlock = xfrm.TransformFinalBlock(inBlock, bytesInBlock, inBlock.Length - bytesInBlock);
            return outBlock;
        }
    }
}
