#region Usings

using System;

#endregion

namespace TwitterConnector.Data
{
    public class TwitterItem
    {
        public DateTime PublishDate { get; set; }
        public TwitterUser User { get; set; }
        public string Text { get; set; }
        public string Source { get; set; }
        public TwitterItem Retweet { get; set; }
    }
}
