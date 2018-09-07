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
        private IEncryptionHelper cryptoHelper;
        public MainWindow()
        {
            InitializeComponent();
            cryptoHelper = new SimpleEncrypt();
        }

        public void EncryptClick(object sender, RoutedEventArgs e)
        {
            DecryptTextBox.Text = cryptoHelper.Encrypt(EncryptTextBox.Text);
        }

        public void DecryptClick(object sender, RoutedEventArgs e)
        {
            EncryptTextBox.Text = cryptoHelper.Decrypt(DecryptTextBox.Text);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
