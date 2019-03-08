using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EncryptionToolLib;

namespace EncryptionToolLibUnitTests
{
    /// <summary>
    /// Summary description for IntegrationTest
    /// </summary>
    [TestClass]
    public class KeyDeriveIntegrationTest
    {
        IPasswordStretch pwStretch;
        IEncryptionHelper encryptHelper;
        [TestInitialize]
        public void SetUp()
        {
            encryptHelper = new AESFrontEnd(AESFrontEnd.KEYSIZE.AES128);
            pwStretch = new PBKDF2Stretch();
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestBasic()
        {
            string pw = "hello world";
            string text = "the man who sold the world";

            var stretchBytes = pwStretch.Stretch(Encoding.UTF8.GetBytes(pw));
            var stretchedPW = Convert.ToBase64String(stretchBytes);

            encryptHelper.SetKey(stretchedPW);

            var encrypted = encryptHelper.Encrypt(text);

            var decrypted = encryptHelper.Decrypt(encrypted);

            Assert.AreEqual(decrypted, text);
        }
    }
}
