
using System.Collections.Generic;
using FeedReader;
using FeedReader.Data;

namespace InfoService.Feeds
{
    public class ExFeed : Feed
    {
        public ExFeed() : base()
        {
            Items = new List<ExFeedItem>();
        }

        public ExFeed(string url) : base(url)
        {
            Items = new List<ExFeedItem>();
        }
        public ExFeed(string url, string chacheFolder) : base(url, chacheFolder, new List<FeedItemFilter>())
        {
            Items = new List<ExFeedItem>();
        }
        public ExFeed(string url, string feedTitle, int defaultZoom, string feedImagePath, bool showPopup) : base(url)
        {
            Items = new List<ExFeedItem>();
            OwnFeedTitle = feedTitle;
            DefaultZoom = defaultZoom;
            ShowPopupDialog = showPopup;
            if (FeedReader.Utils.IsValidPath(feedImagePath))
            {
                OwnFeedImagePath = feedImagePath;
            }

        }
        public ExFeed(string url, string feedTitle, int defaultZoom, string feedImagePath, string cacheFolder, List<FeedItemFilter> itemFilters, bool showPopup) : base(url, cacheFolder, itemFilters, true)
        {
            Items = new List<ExFeedItem>();
            OwnFeedTitle = feedTitle;
            DefaultZoom = defaultZoom;
            ShowPopupDialog = showPopup;
            if (FeedReader.Utils.IsValidPath(feedImagePath))
            {
                OwnFeedImagePath = feedImagePath;
            }
            
        }
        public ExFeed(string url, string feedTitle, int defaultZoom, string feedImagePath, List<FeedItemFilter> itemFilters, bool showPopup) : base(url, itemFilters)
        {
            Items = new List<ExFeedItem>();
            OwnFeedTitle = feedTitle;
            DefaultZoom = defaultZoom;
            ShowPopupDialog = showPopup;
            if (FeedReader.Utils.IsValidPath(feedImagePath))
            {
                OwnFeedImagePath = feedImagePath;
            }

        }
        public bool Active { get; set; }
        public int DefaultZoom { get; set; }
        public string OwnFeedTitle { get; set; }
        public string OwnFeedImagePath { get; set; }
        public bool IsAllFeed { get; set; }
        public bool ShowPopupDialog { get; set; }

        public override bool Update(bool downloadImages)
        {
            bool success = base.Update(downloadImages);
            Items = new List<ExFeedItem>();
            ExFeedItem nItem;
            foreach (FeedItem feedItem in base.Items)
            {
                nItem = new ExFeedItem();
                nItem.Author = feedItem.Author;
                nItem.Description = feedItem.Description;
                nItem.Image = feedItem.Image;
                nItem.ImagePath = feedItem.ImagePath;
                nItem.PublishDate = feedItem.PublishDate;
                nItem.Title = feedItem.Title;
                nItem.Url = feedItem.Url;
                nItem.SourceTitle = OwnFeedTitle;
                nItem.SourceDefaultZoom = DefaultZoom;

                Items.Add(nItem);
            }
            return success;
        }
        public new List<ExFeedItem> Items { get; set; }
    }
}
