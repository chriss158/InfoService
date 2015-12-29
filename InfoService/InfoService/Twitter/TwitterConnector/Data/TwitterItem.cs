#region Usings

using System;
using System.Collections.Generic;
using System.Drawing;

#endregion

namespace TwitterConnector.Data
{
    public class TwitterItem : ICloneable, IDisposable
    {
        private bool _isDisposed;
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

        public object Clone()
        {
            TwitterItem newItem = new TwitterItem();
            newItem.Id = this.Id;
            newItem.PublishDate = this.PublishDate;
            if(this.User != null) newItem.User = this.User.Clone() as TwitterUser;
            newItem.Text = this.Text;
            newItem.Source = this.Source;
            newItem.Retweets = this.Retweets.CloneList();
            newItem.MediaPath = this.MediaPath;
            if (this.MediaImage != null) newItem.MediaImage = this.MediaImage.Clone() as Image;
            newItem.MediaId = this.MediaId;
            return newItem;
        }
        #region IDisposable Members

        /// <summary>
        /// Performs the disposal.
        /// </summary>
        private void Dispose(bool disposing)
        {
            if (disposing && !_isDisposed)
            {
                this.Id = null;
                this.MediaId = null;
                this.MediaImage.Dispose();
                this.MediaImage = null;
                this.MediaPath = null;
                this.Retweets.Clear();
                this.Retweets = null;
                this.Source = null;
                this.Text = null;
                this.User.Dispose();
                this.User = null;
            }

            _isDisposed = true;
        }

        /// <summary>
        /// Releases the object to the garbage collector
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
  
