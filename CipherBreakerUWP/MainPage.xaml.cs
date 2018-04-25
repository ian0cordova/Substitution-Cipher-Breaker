/// MainPage.xaml.cs
/// Ian Cordova
/// Senior Project

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace CipherBreakerUWP
{
    /// <summary>
    /// Main page/menu page
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
