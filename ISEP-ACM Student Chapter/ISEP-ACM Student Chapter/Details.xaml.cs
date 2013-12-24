﻿using System;
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
using System.Threading.Tasks;
using Microsoft.Phone.Tasks;

namespace ISEP_ACM_Student_Chapter
{
    public partial class Details : PhoneApplicationPage
    {

        private Post _post;
        public Details()
        {
            InitializeComponent();

                        
        }

        // Sample code for building a localized ApplicationBar
        private void BuildLocalizedApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            ApplicationBar = new ApplicationBar();

            // Create a new button and set the text value to the localized string from AppResources.
            ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
            appBarButton.Text = AppResources.Details_AppBarBtn_OpenBrowser;
            appBarButton.Click += appBarButton_Click;
            ApplicationBar.Buttons.Add(appBarButton);
        }

        private void appBarButton_Click(object sender, EventArgs e)
        {
            var uri = new Uri(_post.url);
            WebBrowserTask webBrowserTask = new WebBrowserTask { Uri = uri };
            webBrowserTask.Show(); 
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
                    _post = item;
                    LoadAll(item);
                }
            }
        }

        private void LoadAll(Post item)
        {
            DataContext = item;

            BrowserView.NavigateToString(WebBrowserHelper.WrapHtml(item.content, 0));

            BuildLocalizedApplicationBar();
        }

        private void BrowserView_ScriptNotify(object sender, NotifyEventArgs e)
        {

            WebBrowserTask webBrowserTask = new WebBrowserTask();
            webBrowserTask.Uri = new Uri(e.Value); //Uri of the link clicked
            webBrowserTask.Show();
        }

    }
}