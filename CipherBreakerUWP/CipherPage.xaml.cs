/// CipherPage.xaml.cs
/// Ian Cordova
/// Senior Project

using Cipher.CipherMaker;

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

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

        /// <summary>
        /// Loads text from a utf encoded .txt file
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        /// <author>
        /// Ian Cordova - 7:25pm - 4/30/2018
        /// </author>
        private async void btnLoadText_Click(object sender, RoutedEventArgs e)
        {
            var plainText = await SelectPlainTextFileAsync();
            rebPlainText.Document.SetText(Windows.UI.Text.TextSetOptions.None, plainText.ToString());
        }

        /// <summary>
        /// Allows user to select a txt file to be encoded using our cipher
        /// </summary>
        /// 
        /// <returns>
        /// string plainText which contains the contents of the specified txt file
        /// </returns>
        /// 
        /// <author>
        /// Ian Cordova - 7:25pm - 4/30/2018
        /// </author>
        private static async Task<string> SelectPlainTextFileAsync()
        {
            string plainText;

            // Set parameters for opening a file
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.List;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            picker.FileTypeFilter.Add(".txt");

            // Open file directory to choose a txt file
            StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                try
                {
                    plainText = await FileIO.ReadTextAsync(file, 0);
                }
                catch
                {
                    plainText = "The text file that you have chosen is not utf encoded.";
                }
            }
            else
            {
                plainText = "";
            }
            return plainText;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnSaveCipherText_Click(object sender, RoutedEventArgs e)
        {
            string cipherText;
            rebCipherText.Document.GetText(Windows.UI.Text.TextGetOptions.None, out cipherText);

            var savePicker = new Windows.Storage.Pickers.FileSavePicker();
            savePicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;

            // Dropdown of file types the user can save the file as
            savePicker.FileTypeChoices.Add("Plain Text", new List<string>() { ".txt" });
            // Default file name if the user does not type one in or select a file to replace
            savePicker.SuggestedFileName = "New Document";
            StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                // Prevent updates to the remote version of the file until
                // we finish making changes and call CompleteUpdatesAsync.
                CachedFileManager.DeferUpdates(file);
                // write to file
                await FileIO.WriteTextAsync(file, cipherText);
                // Let Windows know that we're finished changing the file so
                // the other app can update the remote version of the file.
                // Completing updates may require Windows to ask for user input.
                Windows.Storage.Provider.FileUpdateStatus status =  await CachedFileManager.CompleteUpdatesAsync(file);

            }

        }
    }
}
