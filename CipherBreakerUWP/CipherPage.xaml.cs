using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using CipherMaker;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CipherBreakerUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
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

            CipherCreator cipherCreator = new CipherCreator();
            Dictionary<char, char> cipher = new Dictionary<char, char>();
            cipher = cipherCreator.CreateSubCipher();

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
            CipherCreator cipherCreator = new CipherCreator();
            cipherText = cipherCreator.EncodeText(plainText, m_currentCipher);

            // Set cipher text to rich edit box
            rebCipherText.IsReadOnly = false;
            rebCipherText.Document.SetText(Windows.UI.Text.TextSetOptions.None, cipherText);
            rebCipherText.IsReadOnly = true;
        }
    }
}
