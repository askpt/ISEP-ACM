using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using ISEP_ACM_Student_Chapter.Resources;
using ISEP_ACM.Core;
using Microsoft.Phone.Net.NetworkInformation;

namespace ISEP_ACM_Student_Chapter
{
    public partial class MainPage : PhoneApplicationPage
    {
        public static Posts posts;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            BuildLocalizedApplicationBar();

            InitializePosts();
        }

        private async void InitializePosts()
        {
            try
            {
                DataContext = await Services.LoadPosts();
            }
            catch (Exception)
            {

                MessageBox.Show(AppResources.Error_Network);
            }
            
        }

        // Sample code for building a localized ApplicationBar
        private void BuildLocalizedApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            ApplicationBar = new ApplicationBar();

            // Create a new button and set the text value to the localized string from AppResources.
            ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
            appBarButton.Text = AppResources.MainPage_AppBarBtn_Refresh;
            appBarButton.Click += appBarButton_Click;
            ApplicationBar.Buttons.Add(appBarButton);
        }

        async void appBarButton_Click(object sender, EventArgs e)
        {
            if (DeviceNetworkInformation.IsNetworkAvailable)
            {
                await Services.CreatePosts();
                InitializePosts();
            }
            else
            {
                MessageBox.Show(AppResources.Error_Network);
            }
        }

        private void LongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Apply cast to the object sender
            LongListSelector selector = sender as LongListSelector;
            if (selector == null)
            {
                return;
            }

            //Apply cast to the selected item
            Post data = selector.SelectedItem as Post;
            if (data == null)
            {
                return;
            }

            selector.SelectedItem = null;

            Uri uri = new Uri(string.Format("/Details.xaml?id={0}", data.id), UriKind.Relative);

            NavigationService.Navigate(uri);
        }
    }
}