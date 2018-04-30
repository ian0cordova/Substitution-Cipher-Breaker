/// DecipherPage.xaml.cs
/// Ian Cordova
/// Senior Project

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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
    }
}
