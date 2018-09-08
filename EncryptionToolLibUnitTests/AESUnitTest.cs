using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EncryptionToolLib;

namespace EncryptionToolLibUnitTests
{
    [TestClass]
    public class AESUnitTest
    {
        private AESFrontEnd aesService;
        [TestInitialize]
        public void SetUp()
        {
            aesService = new AESFrontEnd();
        }
        [TestMethod]
        public void TestSimple()
        {
            string text = "hello world";
            string key128 = "AAECAwQFBgcICQoLDA0ODw==";
            aesService.SetKey(key128);

            var encryptedText = aesService.Encrypt(text);
            var decryptedText = aesService.Decrypt(encryptedText);
            Assert.AreEqual(text, decryptedText);
        }
    }
}
