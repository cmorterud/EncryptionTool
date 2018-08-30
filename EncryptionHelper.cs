namespace EncryptionTool
{
    internal interface EncryptionHelper
    {
        string encrypt(string PlainText);
        string decrypt(string EncryptedText);
    }
}