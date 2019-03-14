using System.IO;
using System.Text;
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
        public void TestInterop256()
        {
            string text = "hello world";
            string key256 = "INzelqOUVBoeb+A7fvwhXhCtcf6GKWU+oc1xMRYTWaU=";
            aesService128.SetKey(key256);

            var encryptedText = aesService128.Encrypt(text);
            var decryptedText = aesService128.Decrypt(encryptedText);
            Assert.AreEqual(text, decryptedText);
        }

        [TestMethod]
        public void TestSimple256()
        {
            string text = "hello world";
            string key256 = "INzelqOUVBoeb+A7fvwhXhCtcf6GKWU+oc1xMRYTWaU=";
            aesService256.SetKey(key256);

            var encryptedText = aesService256.Encrypt(text);
            var decryptedText = aesService256.Decrypt(encryptedText);
            Assert.AreEqual(text, decryptedText);
        }

        [TestMethod]
        public void TestGen128()
        {
            string text = "hello world";
            aesService128.SetKey();

            var encryptedText = aesService128.Encrypt(text);
            var decryptedText = aesService128.Decrypt(encryptedText);
            Assert.AreEqual(text, decryptedText);
        }

        [TestMethod]
        public void TestGen256()
        {
            string text = "hello world";
            aesService256.SetKey();

            var encryptedText = aesService256.Encrypt(text);
            var decryptedText = aesService256.Decrypt(encryptedText);
            Assert.AreEqual(text, decryptedText);
        }

        [TestMethod]
        public void TestEncryptLargeText()
        {
            var text = Properties.Resources.TextFile1;

            aesService128.SetKey();

            var encryptedText = aesService128.Encrypt(text);
            var decryptedText = aesService128.Decrypt(encryptedText);
            Assert.AreEqual(text, decryptedText);
        }

        [TestMethod]
        public void TestEncryptFile()
        {
            var filename = Path.GetTempFileName();

            File.WriteAllText(filename, Properties.Resources.TextFile1);

            var text = File.ReadAllText(filename, Encoding.UTF8);

            var encryptedText = aesService128.Encrypt(text);

            var encFilename = Path.GetTempFileName();
            File.WriteAllText(encFilename, encryptedText);

            var encText = File.ReadAllText(encFilename, Encoding.UTF8);
            var decText = aesService128.Decrypt(encText);

            Assert.AreEqual(text, decText);
        }
    }
}
