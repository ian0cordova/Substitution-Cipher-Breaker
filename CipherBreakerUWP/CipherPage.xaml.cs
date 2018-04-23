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
        /// <summary>
        /// Initializes page
        /// </summary>
        public CipherPage()
        {
            InitializeComponent();           
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
            tbCipherAlphabet.Text = "";
            CipherCreator cipherCreator = new CipherCreator();
            Dictionary<char, char> cipher = new Dictionary<char, char>();
            cipher = cipherCreator.CreateSubCipher();

            foreach (KeyValuePair<char, char> entry in cipher)
            {
                tbCipherAlphabet.Text += entry.Value.ToString();
                tbCipherAlphabet.Text += "\n";
            }
        }
    }
}
