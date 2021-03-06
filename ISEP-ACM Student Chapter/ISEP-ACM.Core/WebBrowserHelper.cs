﻿//
//    Copyright (c) 2011 Microsoft Corporation.  All rights reserved.
//    Use of this sample source code is subject to the terms of the Microsoft license
//    agreement under which you licensed this sample source code and is provided AS-IS.
//    If you did not accept the terms of the license agreement, you are not authorized
//    to use this sample source code.  For the terms of the license, please see the
//    license agreement between you and Microsoft.
//
//
using System;
using System.Text;
using System.Windows;
using System.Windows.Media;
using Microsoft.Phone.Tasks;

namespace ISEP_ACM.Core
{
    //credit to: http://www.ben.geek.nz for this WebBrowserHelper class to open WebBrowser Control links into full IE browser.
    public class WebBrowserHelper
    {
        public static string NotifyScript
        {
            get
            {
                return @"<script>
                    window.onload = function()
                    {
                        a = document.getElementsByTagName('a');
                        for(var i=0; i < a.length; i++){
                            a[i].onclick = function() {notify(this.href);};
                        }
                    }
                    function notify(msg)
                    {
	                    window.external.Notify(msg);
	                    event.returnValue=false;
	                    return false;
                    }
                    </script>";
            }
        }

        public static string WrapHtml(string htmlSubString, double viewportWidth)
        {
            var html = new StringBuilder();
            html.Append("<html>");
            html.Append(HtmlHeader(viewportWidth));
            html.Append("<body>");
            html.Append(htmlSubString);
            html.Append("</body>");
            html.Append("</html>");
            return html.ToString();
        }

        public static string HtmlHeader(double viewportWidth)
        {
            var head = new StringBuilder();
            head.Append("<head>");
            head.Append("<meta name=\"viewport\" content=\"width=" + viewportWidth + ", initial-scale=1, maximum-scale=1\">");
            head.Append("<style>");
            head.Append(string.Format(
                "body {{background-color:{0};color:{1};font-family:'Segoe WP';margin:0;padding:0 }}",
                GetBrowserColor("WebBrowserBackgroundColor"),
                GetBrowserColor("WebBrowserForegroundColor")));
            head.Append(string.Format(
                "a {{color:{0}}} a:link {{color:white}} a:visited {{color:white}} a:active{{color:white}}",
                GetBrowserColor("PhoneAccentColor")));
            head.Append("</style>");
            head.Append(NotifyScript);
            head.Append("<meta name=\"viewport\" content=\"user-scalable=no\" /> ");
            head.Append("</head>");

            return head.ToString();
        }

        public static void OpenBrowser(string url)
        {
            WebBrowserTask webBrowserTask = new WebBrowserTask { Uri = new Uri(url) };
            webBrowserTask.Show();
        }

        private static string GetBrowserColor(string sourceResource)
        {
            var item = Application.Current.Resources[sourceResource];
            if (item.GetType() == typeof(Color))
            {
                var color = (Color)item;
                return "#" + color.ToString().Substring(3, 6);
            }
            else if (item.GetType() == typeof(SolidColorBrush))
            {
                var brush = (SolidColorBrush)item;
                return "#" + brush.Color.ToString().Substring(3, 6);
            }
            else
            {
                throw new InvalidCastException("invalid color or brush: " + sourceResource);
            }
        }
    }
}