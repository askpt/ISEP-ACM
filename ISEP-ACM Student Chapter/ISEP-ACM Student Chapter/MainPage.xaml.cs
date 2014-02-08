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
using Microsoft.Phone.Tasks;

namespace ISEP_ACM_Student_Chapter
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            BuildLocalizedApplicationBar();

            InitializeAll();
        }

        private async void InitializeAll()
        {
            RootObject root = new RootObject();

            try
            {
                Posts posts = await Services.LoadPosts();
                root.posts = posts.posts;
                root.videos = await Services.LoadVideos();

                DataContext = root;
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
            ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/Reload.png", UriKind.RelativeOrAbsolute));
            appBarButton.Text = AppResources.MainPage_AppBarBtn_Refresh;
            appBarButton.Click += appBarButton_Click;
            ApplicationBar.Buttons.Add(appBarButton);

            ApplicationBarMenuItem appBarMenuContact = new ApplicationBarMenuItem();
            appBarMenuContact.Text = AppResources.ContactUs;
            appBarMenuContact.Click += ContactUs;
            ApplicationBar.MenuItems.Add(appBarMenuContact);
        }

        async void appBarButton_Click(object sender, EventArgs e)
        {
            if (DeviceNetworkInformation.IsNetworkAvailable)
            {
                InitializeAll();
            }
            else
            {
                MessageBox.Show(AppResources.Error_Network);
            }
        }

        private void Videos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Apply cast to the object sender
            LongListSelector selector = sender as LongListSelector;
            if (selector == null)
            {
                return;
            }

            //Apply cast to the selected item
            Video data = selector.SelectedItem as Video;
            if (data == null)
            {
                return;
            }

            selector.SelectedItem = null;

            string uri = string.Format("http://www.youtube.com/watch?v={0}", data.VideoId);

            WebBrowserTask task = new WebBrowserTask();
            task.URL = uri;
            task.Show();
        }

        private void Posts_SelectionChanged(object sender, SelectionChangedEventArgs e)
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

        void ContactUs(object sender, EventArgs e)
        {
            EmailComposeTask emailComposeTask = new EmailComposeTask();

            emailComposeTask.To = "acm.student.chapter@isep.ipp.pt";
            emailComposeTask.Subject = "[WP8 App]";

            emailComposeTask.Show();
        }
    }
}