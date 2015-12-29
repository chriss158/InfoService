#region Usings

using System;
using System.Collections.Generic;
using System.Drawing;

#endregion

namespace FeedReader.Data
{
    public class FeedItem : ICloneable
    {
        public string Author { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Image Image { get; set; }
        public string ImagePath { get; set; }
        public DateTime PublishDate { get; set; }
        public string Url { get; set; }

        public object Clone()
        {
            FeedItem newItem = new FeedItem();
            newItem.Author = this.Author;
            newItem.Title = this.Title;
            newItem.Description = this.Description;
            if(this.Image != null) newItem.Image = this.Image.Clone() as Image;
            newItem.ImagePath = this.ImagePath;
            newItem.PublishDate = this.PublishDate;
            newItem.Url = this.Url;
            return newItem;
        }
    }
}