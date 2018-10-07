using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EncryptionToolLib;
using System.Text;
using System.Linq;

namespace EncryptionToolLibUnitTests
{
    [TestClass]
    public class HMACUnitTest
    {
        private IVerify helper;
        [TestInitialize]
        public void Setup()
        {
            helper = new HMACSHA256Verify();
        }
        [TestMethod]
        public void TestBasic()
        {

            byte[] key = Convert.FromBase64String("FL96VAFThaXJjsotPMphqd5gudvNB9aT");
            byte[] text = Encoding.Unicode.GetBytes("Hello world!");

            byte[] MAC = helper.computeMAC(text, key);

            byte[] MACsecond = helper.computeMAC(text, key);

            Assert.IsTrue(MAC.SequenceEqual(MACsecond));
        }
        [TestMethod]
        public void TestVerify()
        {

            byte[] key = Convert.FromBase64String("FL96VAFThaXJjsotPMphqd5gudvNB9aT");
            byte[] text = Encoding.Unicode.GetBytes("Hello world!");

            byte[] MAC = helper.computeMAC(text, key);

            Assert.IsTrue(helper.verify(text, key, MAC));
        }
        [TestMethod]
        public void TestBadVerify()
        {

            byte[] key = Convert.FromBase64String("FL96VAFThaXJjsotPMphqd5gudvNB9aT");
            byte[] text = Encoding.Unicode.GetBytes("Hello world!");

            byte[] MAC = helper.computeMAC(text, key);
            MAC[8] = 1;
            Assert.IsFalse(helper.verify(text, key, MAC));
        }
    }
}
