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

namespace ISEP_ACM_Student_Chapter
{
    public partial class Details : PhoneApplicationPage
    {
        public Details()
        {
            InitializeComponent();

            BuildLocalizedApplicationBar();
                        
        }

        // Sample code for building a localized ApplicationBar
        private void BuildLocalizedApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            ApplicationBar = new ApplicationBar();

            // Create a new button and set the text value to the localized string from AppResources.
            ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
            appBarButton.Text = AppResources.AppBarButtonText;
            appBarButton.Click += appBarButton_Click;
            ApplicationBar.Buttons.Add(appBarButton);
        }

        private void appBarButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            int index = Convert.ToInt32(NavigationContext.QueryString["id"]);

            Posts posts = await Services.LoadPosts();
            foreach (Post item in posts.posts)
            {
                if (item.id == index)
                {
                    DataContext = item;

                    BrowserView.NavigateToString(WebBrowserHelper.WrapHtml(item.content, BrowserView.ActualWidth)); 
                }
            }

            

        }
    }
}