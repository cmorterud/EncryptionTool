

namespace EncryptionToolLib
{
    interface IEncryptionHelper
    {
        string Encrypt(string PlainText);
        string Decrypt(string EncryptedText);
    }
}