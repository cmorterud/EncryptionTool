using System;
using System.IO;
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
using Microsoft.Win32;

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

        enum CryptoMode
        {
            Encrypt, Decrypt
        }

        private bool? fileValid = false;

        public MainWindow()
        {
            InitializeComponent();
            cryptoHelper = new AESFrontEnd(size);
            stretchHelper = new PBKDF2Stretch();
        }

        public void EncryptClick(object sender, RoutedEventArgs e)
        {
            CheckKeyRadio();
            CheckFile();
            Encrypt();
        }

        public void DecryptClick(object sender, RoutedEventArgs e)
        {
            CheckFile();
            Decrypt();
        }

        private void CheckFile()
        {
            try
            {
                if (FileMode.IsChecked ?? false)
                {
                    // if file is not valid
                    if (!(fileValid ?? false))
                    {
                        throw new System.ArgumentNullException();
                    }
                }
            }
            catch (System.ArgumentNullException ex)
            {
                MessageBoxResult result = MessageBox.Show("File is invalid, please try selecting a different file.\n" + ex.Message,
                                          "Message failed to decrypt",
                                          MessageBoxButton.OK,
                                          MessageBoxImage.Error);
            }
        }


        private void Encrypt()
        {
            try
            {
                if (TextMode.IsChecked ?? false)
                {
                    DecryptTextBox.Text = cryptoHelper.Encrypt(EncryptTextBox.Text);
                }
                else if (FileMode.IsChecked ?? false)
                {
                    var fileString = File.ReadAllText(FileTextBox.Text);
                    var encryptedString = cryptoHelper.Encrypt(fileString);
                    SaveFile(CryptoMode.Encrypt, encryptedString);
                }
            }
            catch(FileNotFoundException ex)
            {
                MessageBoxResult result = MessageBox.Show("Selected file was not found.\n" + ex.Message,
                                          "File not found",
                                          MessageBoxButton.OK,
                                          MessageBoxImage.Error);
                EncryptTextBox.Text = "";
            }
            catch (Exception ex)
            {
                MessageBoxResult result = MessageBox.Show("Message failed to decrypt, check that you are using the correct key and message.\n" + ex.Message,
                                          "Message failed to encrypt",
                                          MessageBoxButton.OK,
                                          MessageBoxImage.Error);
                EncryptTextBox.Text = "";
            }
        }
        private void Decrypt()
        {
            try
            {
                if (TextMode.IsChecked ?? false)
                {
                    EncryptTextBox.Text = cryptoHelper.Decrypt(DecryptTextBox.Text);
                }
                else if (FileMode.IsChecked ?? false)
                {
                    var fileString = File.ReadAllText(FileTextBox.Text);
                    var encryptedString = cryptoHelper.Encrypt(fileString);
                    SaveFile(CryptoMode.Encrypt, encryptedString);
                }
            }
            catch (FileNotFoundException ex)
            {
                MessageBoxResult result = MessageBox.Show("Selected file was not found.\n" + ex.Message,
                                          "File not found",
                                          MessageBoxButton.OK,
                                          MessageBoxImage.Error);
                EncryptTextBox.Text = "";
            }
            catch (Exception ex)
            {
                MessageBoxResult result = MessageBox.Show("Message failed to decrypt, check that you are using the correct key and message.\n" + ex.Message,
                                          "Message failed to encrypt",
                                          MessageBoxButton.OK,
                                          MessageBoxImage.Error);
                EncryptTextBox.Text = "";
            }
        }

        private void SaveFile(CryptoMode mode, string text)
        {
            // Displays a SaveFileDialog so the user can save the encrypted text
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            if(mode == CryptoMode.Encrypt)
            {
                saveFileDialog1.Title = "Save encrypted file";
            }
            else if(mode == CryptoMode.Decrypt)
            {
                saveFileDialog1.Title = "Save decrypted file";
                //saveFileDialog1.f
            }
            saveFileDialog1.ShowDialog();

            // If the file name is not an empty string open it for saving.
            if (saveFileDialog1.FileName != "")
            {
               File.WriteAllText(saveFileDialog1.FileName, text);
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
            BrowseFileButton.IsEnabled = true;
            FileTextBox.IsEnabled = true;
        }

        private void TextChecked(object sender, RoutedEventArgs e)
        {
            EncryptTextBox.IsEnabled = true;
            DecryptTextBox.IsEnabled = true;
            BrowseFileButton.IsEnabled = false;
            FileTextBox.IsEnabled = false;
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

        private void BrowseFileButtonClick(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Document"; // Default file name

            fileValid = dlg.ShowDialog();

            if (fileValid == true)
            {
                // Open document
                string filename = dlg.FileName;
                FileTextBox.Text = filename;
            }
        }
    }
}
