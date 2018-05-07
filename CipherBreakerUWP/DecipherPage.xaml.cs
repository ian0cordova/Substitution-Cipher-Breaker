/// DecipherPage.xaml.cs
/// Ian Cordova
/// Senior Project

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Cipher.CipherSolver;
using Cipher.CipherUtility;
using System.Threading.Tasks;
using Windows.Storage;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;

namespace CipherBreakerUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DecipherPage : Page
    {
        public DecipherPage()
        {
            this.InitializeComponent();
            rebCipherOutput.IsReadOnly = true;
            m_trainingData = new Dictionary<string, int>();
            m_cipherKey = new Dictionary<char, char>();
        }

        private Dictionary<string, int> m_trainingData;
        private static Dictionary<char, char> m_cipherKey;

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
        /// Calls proper libraries to decipher to entered cipher text.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        /// <author>
        /// Ian Cordova - 6:30pm, 4/6/2018
        /// </author>
        private void BtnDecipherText_Click(object sender, RoutedEventArgs e)
        {
            string cipherText;
            rebCipherInput.Document.GetText(Windows.UI.Text.TextGetOptions.None, out cipherText);
            if(m_trainingData.Count == 0)
            {
                rebCipherOutput.IsReadOnly = false;
                rebCipherOutput.Document.SetText(Windows.UI.Text.TextSetOptions.None, "Please select training data to use!");
                rebCipherOutput.IsReadOnly = true;
                return;
            }

            CipherStats stats = CipherSolver.Decrypt(m_trainingData, cipherText, m_cipherKey);
            Debug.WriteLine(stats.decodedText);
            rebCipherOutput.IsReadOnly = false;
            rebCipherOutput.Document.SetText(Windows.UI.Text.TextSetOptions.None, stats.decodedText);
            rebCipherOutput.IsReadOnly = true;

            tbStats.Text = (stats.accuracy) + "% accuracy";
        }

        /// <summary>
        /// Allows user to specify a file containing ciphered text from it
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        /// <author>
        /// Ian Cordova - 6:30pm, 4/6/2018
        /// </author>
        private async void BtnGetCipherText_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var cipherText = await SelectCipherTextFileAsync();
                rebCipherInput.Document.SetText(Windows.UI.Text.TextSetOptions.None, cipherText);

            }
            catch (Exception err)
            {
                tbCipherFile.Text = "Error with selected file!";
            }
        }

        /// <summary>
        /// Handles the user loading cipher text from a file
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        /// <author>
        /// Ian Cordova - 4/5/2018
        /// </author>
        private async void BtnGetTrainingData_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                m_trainingData = await SelectTrainingDataAsync();
            }
            catch(Exception err)
            {
                tbTrainingFile.Text = "Error with selected file!";
            }
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
        private async Task<Dictionary<string, int>> SelectTrainingDataAsync()
        {
            string dataText;

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
                    dataText = await FileIO.ReadTextAsync(file, 0);
                }
                catch
                {
                    dataText = "The text file that you have chosen is not utf encoded.";
                }
            }
            else
            {
                dataText = "";
            }

            Dictionary<string, int> trainingBiGrams = new Dictionary<string, int>();
            using (StringReader sr = new StringReader(dataText))
            {
                string line;

                // read file and populate dict
                while ((line = sr.ReadLine()) != null)
                {
                    try
                    {
                        Tuple<string, int> nGramData = CipherUtility.ParseDataSetLine(line);
                        trainingBiGrams.Add(nGramData.Item1, nGramData.Item2);
                    }
                    catch(Exception err)
                    {
                        tbTrainingFile.Text = "Training data selected not in correct format!";
                    }                   
                }
            }

            tbTrainingFile.Text = "Loaded: " + file.Name;
            return trainingBiGrams;
            
        }

        /// <summary>
        /// Allows user to select a txt file to be encoded using our cipher
        /// </summary>
        /// 
        /// <returns>
        /// string plainText which contains the contents of the specified txt file
        ///
        /// </returns>
        /// 
        /// <author>
        /// Ian Cordova - 7:25pm - 4/30/2018
        /// </author>
        private static async Task<string> SelectCipherTextFileAsync()
        {
            string cipherText;

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
                    cipherText = await FileIO.ReadTextAsync(file, 0);
                }
                catch
                {
                    cipherText = "The text file that you have chosen is not utf encoded.";
                }
            }
            else
            {
                cipherText = "";
            }
            
            // if file contents contains a cipherkey
            if(cipherText[1] == ':')
            {
                return GetCipherKeyFromFile(cipherText);
            }
            else
            {
                return cipherText;
            }
            
        }

        /// <summary>
        /// Reads the cipher key from the file and stores it to member variable
        /// </summary>
        /// 
        /// <param name="a_fileContents"> contents from cipher file </param>
        /// 
        /// <returns>
        /// string containing the parsed cipher text from the file
        /// </returns>
        /// 
        /// <author>
        /// Ian Cordova - 7:50pm, 4/6/2018
        /// </author>
        private static string GetCipherKeyFromFile(string a_fileContents)
        {
            string cipherText = "";
            using (StringReader sr = new StringReader(a_fileContents))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    // file contains a cipherkey
                    if (line[1] == ':')
                    {
                        // format is A:Z - so we know that spot 0 and 2 will be the desired characters
                        m_cipherKey.Add(line[0], line[2]);
                    }
                    // line consists of cipher text
                    else
                    {
                        cipherText += line;
                    }
                }
            }
            return cipherText;
        }
    }
}
