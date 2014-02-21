using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using ISEP_ACM_Student_Chapter.Resources;
using ISEP_ACM.Core;
using Microsoft.Phone.Net.NetworkInformation;
using Microsoft.Phone.Tasks;

namespace ISEP_ACM_Student_Chapter
{
    public partial class MainPage
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
            var root = new RootObject
            {
                posts = new List<Post>(), 
                videos = new List<Video>()
            };

            try
            {
                await Services.CreatePosts();
                await Services.CreateVideos();
            }
            catch (Exception)
            {
                MessageBox.Show(AppResources.Error_Network);
            }
            finally
            {
                var posts = Services.LoadPosts();
                root.posts = posts.posts;
                root.videos = Services.LoadVideos();

                DataContext = root;
            }
        }

        // Sample code for building a localized ApplicationBar
        private void BuildLocalizedApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            ApplicationBar = new ApplicationBar();

            // Create a new button and set the text value to the localized string from AppResources.
            var appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/Reload.png", UriKind.RelativeOrAbsolute))
            {
                Text = AppResources.MainPage_AppBarBtn_Refresh
            };
            appBarButton.Click += appBarButton_Click;
            ApplicationBar.Buttons.Add(appBarButton);

            var appBarMenuContact = new ApplicationBarMenuItem
            {
                Text = AppResources.ContactUs
            };
            appBarMenuContact.Click += ContactUs;
            ApplicationBar.MenuItems.Add(appBarMenuContact);
        }

        void appBarButton_Click(object sender, EventArgs e)
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
            var selector = sender as LongListSelector;
            if (selector == null)
            {
                return;
            }

            //Apply cast to the selected item
            var data = selector.SelectedItem as Video;
            if (data == null)
            {
                return;
            }

            selector.SelectedItem = null;

            var uri = new Uri(string.Format("http://www.youtube.com/watch?v={0}", data.VideoId), UriKind.Absolute);

            var task = new WebBrowserTask
            {
                Uri = uri
            };
            task.Show();
        }

        private void Posts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Apply cast to the object sender
            var selector = sender as LongListSelector;
            if (selector == null)
            {
                return;
            }

            //Apply cast to the selected item
            var data = selector.SelectedItem as Post;
            if (data == null)
            {
                return;
            }

            selector.SelectedItem = null;

            var uri = new Uri(string.Format("/Details.xaml?id={0}", data.id), UriKind.Relative);

            NavigationService.Navigate(uri);
        }

        static void ContactUs(object sender, EventArgs e)
        {
            var emailComposeTask = new EmailComposeTask
            {
                To = "acm.student.chapter@isep.ipp.pt", 
                Subject = "[WP8 App]"
            };

            emailComposeTask.Show();
        }
    }
}