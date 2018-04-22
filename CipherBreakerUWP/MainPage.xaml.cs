using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CipherBreakerUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        /// <summary>
        /// Initializes page
        /// </summary>
        /// 
        /// <author>
        /// Ian Cordova
        /// </author>
        public MainPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Navigates to Cipher page.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        /// <author>
        /// Ian Cordova - 2:00am 4/22/2018
        /// </author>
        private void NavigateToCipherPage_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CipherPage));
        }
    }
}
