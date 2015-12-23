using System.Drawing;
using FeedReader.Data;

namespace InfoService.Feeds
{
    public class ExFeedItem : FeedItem
    {
        public ExFeedItem()
        {
            SourceTitle = string.Empty;
            ImageBig = null;
            ImagePathBig = string.Empty;
            SourceDefaultZoom = 100;
        }
        public ExFeedItem(Image bigImage, string bigImagePath, string sourceTitle, int sourceDefaultZoom)
        {
            this.ImageBig = bigImage;
            this.ImagePathBig = bigImagePath;
            this.SourceTitle = sourceTitle;
            this.SourceDefaultZoom = sourceDefaultZoom;
            
        }

        public ExFeedItem(FeedItem item, Image bigImage, string bigImagePath, string sourceTitle, int sourceDefaultZoom)
        {
            this.Author = item.Author;
            this.Description = item.Description;
            this.Image = item.Image;
            this.ImagePath = item.ImagePath;
            this.PublishDate = item.PublishDate;
            this.Title = item.Title;
            this.Url = item.Url;
            this.ImageBig = bigImage;
            this.ImagePathBig = bigImagePath;
            this.SourceTitle = sourceTitle;
            this.SourceDefaultZoom = sourceDefaultZoom;

        }

        public string SourceTitle { get; set; }
        public int SourceDefaultZoom { get; set; }
        public Image ImageBig { get; set; }
        public string ImagePathBig { get; set; }
    }
}
