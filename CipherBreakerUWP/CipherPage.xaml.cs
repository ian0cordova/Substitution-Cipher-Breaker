/// CipherPage.xaml.cs
/// Ian Cordova
/// Senior Project

using Cipher.CipherMaker;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace CipherBreakerUWP
{
    /// <summary>
    /// Page in which cipher creation and translation to cipher text is done.
    /// </summary>
    public sealed partial class CipherPage : Page
    {
        private Dictionary<char, char> m_currentCipher;

        /// <summary>
        /// Initializes page
        /// </summary>
        public CipherPage()
        {
            InitializeComponent();
            m_currentCipher = new Dictionary<char, char>();
        }

        /// <summary>
        /// Provides functionality to back button.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        /// <author>
        /// Ian Cordova - 4/20/2018
        /// </author>
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        /// <summary>
        /// Generates a cipher for the user
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        /// <author>
        /// Ian Cordova - 4:55pm, 4/23/2018 
        /// </author>
        private void createCipherBtn_Click(object sender, RoutedEventArgs e)
        {
            // Clears textbox before new generation
            tbCipherAlphabet.Text = "";

            Dictionary<char, char> cipher = new Dictionary<char, char>();
            cipher = CipherMaker.CreateSubCipher();

            // Adds each value from cipher to text box to be displayed
            foreach (KeyValuePair<char, char> entry in cipher)
            {
                tbCipherAlphabet.Text += entry.Value.ToString();
                tbCipherAlphabet.Text += "\n";
            }

            m_currentCipher = cipher;
        }

        /// <summary>
        /// Encodes user inputted text using the generated cipher
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        /// <author>
        /// Ian Cordova - 9:30pm, 4/23/2018
        /// </author>
        private void btnTranslate_Click(object sender, RoutedEventArgs e)
        {
            string plainText;
            string cipherText;

            // If the user has not generated a cipher yet, print this error message.
            string errorMessage = "Please generate a cipher to translate your text first.";
            if(m_currentCipher.Count == 0)
            {
                rebCipherText.IsReadOnly = false;
                rebCipherText.Document.SetText(Windows.UI.Text.TextSetOptions.None, errorMessage);
                rebCipherText.IsReadOnly = true;
                return;
            }

            // Get text from rich edit box and save to plainText string
            rebPlainText.Document.GetText(Windows.UI.Text.TextGetOptions.None, out plainText);

            // Encode plain text with the generated cipher
            cipherText = CipherMaker.EncodeText(plainText, m_currentCipher);

            // Set cipher text to rich edit box
            rebCipherText.IsReadOnly = false;
            rebCipherText.Document.SetText(Windows.UI.Text.TextSetOptions.None, cipherText);
            rebCipherText.IsReadOnly = true;
        }
    }
}
