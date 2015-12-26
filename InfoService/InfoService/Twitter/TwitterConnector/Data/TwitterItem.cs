#region Usings

using System;
using System.Collections.Generic;
using System.Drawing;

#endregion

namespace TwitterConnector.Data
{
    public class TwitterItem
    {
        public string Id { get; set; }
        public DateTime PublishDate { get; set; }
        public TwitterUser User { get; set; }
        public string Text { get; set; }
        public string Source { get; set; }
        public List<TwitterItem> Retweets { get; set; }
        public string MediaPath { get; set; }
        public string MediaId { get; set; }
        public Image MediaImage { get; set; }
        public TwitterItem()
        {
            Retweets = new List<TwitterItem>();
            Id = string.Empty;
            Text = string.Empty;
            Source = string.Empty;
            MediaPath = string.Empty;
            MediaId = string.Empty;
        }
    }
   
}
