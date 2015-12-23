using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OPMLManager.Data
{
    public class OPMLFeedItem
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
        public string XmlUrl { get; set; }
        public OPMLFeedItem(string Title, string Link, string XmlUrl, string Description)
        {
            this.Title = Title;
            this.Link = Link;
            this.Description = Description;
            this.XmlUrl = XmlUrl;
        }
    }
}
