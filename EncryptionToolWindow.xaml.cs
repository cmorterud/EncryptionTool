using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EncryptionToolLib;

namespace EncryptionTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // TODO: add dependency injection, dependency profile
        private IEncryptionHelper cryptoHelper;
        private IPasswordStretch stretchHelper;
        private AESFrontEnd.KEYSIZE size = AESFrontEnd.KEYSIZE.AES128;
        public MainWindow()
        {
            InitializeComponent();
            cryptoHelper = new AESFrontEnd(size);
            stretchHelper = new PBKDF2Stretch();
        }

        public void EncryptClick(object sender, RoutedEventArgs e)
        {
            CheckKeyRadio();
            DecryptTextBox.Text = cryptoHelper.Encrypt(EncryptTextBox.Text);
        }

        public void DecryptClick(object sender, RoutedEventArgs e)
        {
            //if(GenUsingPassword.IsChecked ?? false)
            //{
            //    GenerateKeyUsingPassword();
            //}
            try
            {
                EncryptTextBox.Text = cryptoHelper.Decrypt(DecryptTextBox.Text);
            }
            catch(Exception ex)
            {
                MessageBoxResult result = MessageBox.Show("Message failed to decrypt, check that you are using the correct key and message.\n" + ex.Message,
                                          "Message failed to decrypt",
                                          MessageBoxButton.OK,
                                          MessageBoxImage.Error);
                EncryptTextBox.Text = "";
            }
        }

        private void AES256RadioChecked(object sender, RoutedEventArgs e)
        {
            cryptoHelper = new AESFrontEnd(AESFrontEnd.KEYSIZE.AES256);
        }

        private void AES128RadioChecked(object sender, RoutedEventArgs e)
        {
            cryptoHelper = new AESFrontEnd(AESFrontEnd.KEYSIZE.AES128);
        }

        private void CheckKeyRadio()
        {
            if (SecurelyGenerate.IsChecked ?? false)
            {
                cryptoHelper.SetKey();
                Base64KeyTextBox.Text = cryptoHelper.GetKey();
            }
            else if(PasteInKey.IsChecked ?? false)
            {
                var key = "";
                key = Base64KeyTextBox.Text;
                cryptoHelper.SetKey(key);
                Base64KeyTextBox.Text = cryptoHelper.GetKey();
            }
            else if(GenUsingPassword.IsChecked ?? false)
            {
                GenerateKeyUsingPassword();
            }
            else
            {
                // error
            }
        }

        private void GenerateKeyUsingPassword()
        {
            var key = "";
            var pwStretch = StretchString(PasswordBox.Text);
            key = pwStretch;
            cryptoHelper.SetKey(key);
            Base64KeyTextBox.Text = cryptoHelper.GetKey();
        }

        private string StretchString(string text)
        {
            var pwBytes = Encoding.UTF8.GetBytes(text);
            var pwBytesStretched = stretchHelper.Stretch(pwBytes);
            var stretchedPW = Convert.ToBase64String(pwBytesStretched);
            return stretchedPW;
        }

        private void FileChecked(object sender, RoutedEventArgs e)
        {
            EncryptTextBox.IsEnabled = false;
            DecryptTextBox.IsEnabled = false;
        }

        private void TextChecked(object sender, RoutedEventArgs e)
        {
            EncryptTextBox.IsEnabled = true;
            DecryptTextBox.IsEnabled = true;
        }

        private void SecurelyGenerateChecked(object sender, RoutedEventArgs e)
        {
            PasswordBox.IsEnabled = false;
        }

        private void PasteInKeyChecked(object sender, RoutedEventArgs e)
        {
            PasswordBox.IsEnabled = false;
        }

        private void GenUsingPasswordChecked(object sender, RoutedEventArgs e)
        {
            PasswordBox.IsEnabled = true;
        }
    }
}
