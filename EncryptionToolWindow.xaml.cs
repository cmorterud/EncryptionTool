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
        private PasswordStretch stretchHelper;
        private AESFrontEnd.KEYSIZE size = AESFrontEnd.KEYSIZE.AES128;
        public MainWindow()
        {
            InitializeComponent();
            cryptoHelper = new AESFrontEnd(size);
            stretchHelper = new PasswordStretch();
        }

        public void EncryptClick(object sender, RoutedEventArgs e)
        {
            CheckKeyRadio();
            DecryptTextBox.Text = cryptoHelper.Encrypt(EncryptTextBox.Text);
        }

        public void DecryptClick(object sender, RoutedEventArgs e)
        {
            if(GenUsingPassword.IsChecked ?? false)
            {
                GenerateKeyUsingPassword();
            }
            EncryptTextBox.Text = cryptoHelper.Decrypt(DecryptTextBox.Text);
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
            var pwStretch = stretchHelper.Hash(PasswordBox.Text);
            key = pwStretch;
            cryptoHelper.SetKey(key);
            Base64KeyTextBox.Text = cryptoHelper.GetKey();
        }
    }
}
