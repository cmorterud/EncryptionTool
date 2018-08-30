using Microsoft.VisualStudio.TestTools.UnitTesting;
using EncryptionToolLib;

namespace EncryptionToolLibUnitTests
{
    [TestClass]
    public class SimpleEncryptTest
    {
        private SimpleEncrypt helper;
        private string text;
        [TestInitialize]
        public void Setup()
        {
            helper = new SimpleEncrypt();
            text = "hello world";
    }
    [TestMethod]
        public void TestSimple()
        {
            Assert.AreEqual(helper.Decrypt(helper.Encrypt(text)), text);
        }
    }
}
