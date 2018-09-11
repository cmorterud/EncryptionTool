using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EncryptionToolLib;

namespace EncryptionToolLibUnitTests
{
    [TestClass]
    public class AESUnitTest
    {
        private AESFrontEnd aesService128;
        private AESFrontEnd aesService256;
        [TestInitialize]
        public void SetUp()
        {
            aesService128 = new AESFrontEnd(AESFrontEnd.KEYSIZE.AES128);
            aesService256 = new AESFrontEnd(AESFrontEnd.KEYSIZE.AES256);

        }
        [TestMethod]
        public void TestSimple128()
        {
            string text = "hello world";
            string key128 = "AAECAwQFBgcICQoLDA0ODw==";
            aesService128.SetKey(key128);

            var encryptedText = aesService128.Encrypt(text);
            var decryptedText = aesService128.Decrypt(encryptedText);
            Assert.AreEqual(text, decryptedText);
        }

        [TestMethod]
        public void TestSimple256()
        {
            string text = "hello world";
            string key256 = "INzelqOUVBoeb+A7fvwhXhCtcf6GKWU+oc1xMRYTWaU=";
            aesService128.SetKey(key256);

            var encryptedText = aesService128.Encrypt(text);
            var decryptedText = aesService128.Decrypt(encryptedText);
            Assert.AreEqual(text, decryptedText);
        }
    }
}
