using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfoService.Enums
{
    public enum WebBrowserType
    {
        [StringValue("WebBrowser")]
        [PropertyValue("#webbrowser.link : %link%  \n  #webbrowser.zoom : %zoom%")]
        [WindowID("16002")]
        WebBrowser = 0,

        [StringValue("GeckoBrowser")]
        [PropertyValue("#geckobrowser.link.url : %link%  \n  #geckobrowser.link.zoom : %zoom%")]
        [WindowID("16004")]
        GeckoBrowser = 1,

        [StringValue("Browse The Web")]
        [PropertyValue("#btWeb.startup.link : %link%")]
        [WindowID("54537689")]
        BrowseTheWeb = 2,

        [StringValue("Other")]
        Other = 3
    }
}
