﻿using System;
using System.Windows.Navigation;
using ISEP_ACM_Student_Chapter.Helpers;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using ISEP_ACM_Student_Chapter.Resources;
using ISEP_ACM.Core;
using Microsoft.Phone.Tasks;

namespace ISEP_ACM_Student_Chapter
{
    public partial class Details
    {
        private Post _post;
        private WebBrowserHelperScroll browserHelper;
        public Details()
        {
            InitializeComponent();

            browserHelper = new WebBrowserHelperScroll(BrowserView);
            browserHelper.ScrollDisabled = true;
        }

        // Sample code for building a localized ApplicationBar
        private void BuildLocalizedApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            ApplicationBar = new ApplicationBar();

            // Create a new button and set the text value to the localized string from AppResources.
            ApplicationBarIconButton appBarButtonLink = new ApplicationBarIconButton(new Uri("/Assets/AppBar/Window-New-Open.png", UriKind.RelativeOrAbsolute));
            appBarButtonLink.Text = AppResources.Details_AppBarBtn_OpenBrowser;
            appBarButtonLink.Click += appBarButton_Click;
            ApplicationBar.Buttons.Add(appBarButtonLink);

            ApplicationBarIconButton appBarButtonShare = new ApplicationBarIconButton(new Uri("/Assets/AppBar/Share.png", UriKind.RelativeOrAbsolute));
            appBarButtonShare.Text = AppResources.Share;
            appBarButtonShare.Click += ShareTask;
            ApplicationBar.Buttons.Add(appBarButtonShare);

            ApplicationBarIconButton appBarButtonEmail = new ApplicationBarIconButton(new Uri("/Assets/AppBar/Mail.png", UriKind.RelativeOrAbsolute));
            appBarButtonEmail.Text = AppResources.Details_SendEmail;
            appBarButtonEmail.Click += EmailNewTask;
            ApplicationBar.Buttons.Add(appBarButtonEmail);

            ApplicationBarMenuItem appBarMenuContact = new ApplicationBarMenuItem();
            appBarMenuContact.Text = AppResources.ContactUs;
            appBarMenuContact.Click += ContactUs;
            ApplicationBar.MenuItems.Add(appBarMenuContact);
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
                    _post.content = WebBrowserHelper.WrapHtml(item.content, 0);
                }
            }
            BuildLocalizedApplicationBar();
            DataContext = _post;
        }

        private void BrowserView_ScriptNotify(object sender, NotifyEventArgs e)
        {

            WebBrowserTask webBrowserTask = new WebBrowserTask();
            webBrowserTask.Uri = new Uri(e.Value); //Uri of the link clicked
            webBrowserTask.Show();
        }

        private void ShareTask(object sender, EventArgs e)
        {
            ShareLinkTask shareLinkTask = new ShareLinkTask();

            shareLinkTask.Title = _post.title;
            shareLinkTask.LinkUri = new Uri(_post.url, UriKind.Absolute);
            shareLinkTask.Message = "";

            shareLinkTask.Show();
        }

        private void EmailNewTask(object sender, EventArgs e)
        {
            EmailComposeTask emailComposeTask = new EmailComposeTask();

            emailComposeTask.Subject = _post.title;
            emailComposeTask.Body = _post.url;

            emailComposeTask.Show();
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