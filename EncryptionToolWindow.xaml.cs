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
        public MainWindow()
        {
            InitializeComponent();
            cryptoHelper = new AESFrontEnd();
            stretchHelper = new PasswordStretch();
        }

        public void EncryptClick(object sender, RoutedEventArgs e)
        {
            SetKey();
            DecryptTextBox.Text = cryptoHelper.Encrypt(EncryptTextBox.Text);
        }

        public void DecryptClick(object sender, RoutedEventArgs e)
        {
            SetKey();
            EncryptTextBox.Text = cryptoHelper.Decrypt(DecryptTextBox.Text);
        }

        private void SetKey()
        {
            string key = "";

            // account for null-able bool
            var isChecked = GenerateKeyFromHash.IsChecked ?? false;
            if (isChecked)
            {
                var pwStretch = stretchHelper.Hash(PasswordBox.Text);
                key = pwStretch;
            }
            else
            {
                key = Base64_key_text_box.Text;
            }

            cryptoHelper.SetKey(key);
            Base64_key_text_box.Text = key;
        }
    }
}
