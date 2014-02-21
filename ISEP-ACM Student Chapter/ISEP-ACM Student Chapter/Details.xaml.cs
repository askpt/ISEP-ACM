using System;
using System.Linq;
using System.Windows.Navigation;
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

        public Details()
        {
            InitializeComponent();

            // //The browser scroll locker
            //var browserHelper = new WebBrowserHelperScroll(BrowserView)
            //{
            //    ScrollDisabled = true
            //};
        }

        // Sample code for building a localized ApplicationBar
        private void BuildLocalizedApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            ApplicationBar = new ApplicationBar();

            // Create a new button and set the text value to the localized string from AppResources.
            var appBarButtonLink = new ApplicationBarIconButton(new Uri("/Assets/AppBar/Window-New-Open.png", UriKind.RelativeOrAbsolute))
            {
                Text = AppResources.Details_AppBarBtn_OpenBrowser
            };
            appBarButtonLink.Click += appBarButton_Click;
            ApplicationBar.Buttons.Add(appBarButtonLink);

            var appBarButtonShare = new ApplicationBarIconButton(new Uri("/Assets/AppBar/Share.png", UriKind.RelativeOrAbsolute))
            {
                Text = AppResources.Share
            };
            appBarButtonShare.Click += ShareTask;
            ApplicationBar.Buttons.Add(appBarButtonShare);

            var appBarButtonEmail = new ApplicationBarIconButton(new Uri("/Assets/AppBar/Mail.png", UriKind.RelativeOrAbsolute))
            {
                Text = AppResources.Details_SendEmail
            };
            appBarButtonEmail.Click += EmailNewTask;
            ApplicationBar.Buttons.Add(appBarButtonEmail);

            var appBarMenuContact = new ApplicationBarMenuItem
            {
                Text = AppResources.ContactUs
            };
            appBarMenuContact.Click += ContactUs;
            ApplicationBar.MenuItems.Add(appBarMenuContact);
        }

       

        private void appBarButton_Click(object sender, EventArgs e)
        {
            var uri = new Uri(_post.url);
            var webBrowserTask = new WebBrowserTask
            {
                Uri = uri
            };
            webBrowserTask.Show(); 
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var index = Convert.ToInt32(NavigationContext.QueryString["id"]);

            var posts = Services.LoadPosts();
            foreach (var item in posts.posts.Where(item => item.id == index))
            {
                _post = item;
                _post.content = WebBrowserHelper.WrapHtml(item.content, 0);
            }
            BuildLocalizedApplicationBar();
            DataContext = _post;
        }

        private void BrowserView_ScriptNotify(object sender, NotifyEventArgs e)
        {

            var webBrowserTask = new WebBrowserTask
            {
                Uri = new Uri(e.Value)
            };
            webBrowserTask.Show();
        }

        private void ShareTask(object sender, EventArgs e)
        {
            var shareLinkTask = new ShareLinkTask
            {
                Title = _post.title,
                LinkUri = new Uri(_post.url, UriKind.Absolute),
                Message = ""
            };

            shareLinkTask.Show();
        }

        private void EmailNewTask(object sender, EventArgs e)
        {
            var emailComposeTask = new EmailComposeTask
            {
                Subject = _post.title, 
                Body = _post.url
            };

            emailComposeTask.Show();
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