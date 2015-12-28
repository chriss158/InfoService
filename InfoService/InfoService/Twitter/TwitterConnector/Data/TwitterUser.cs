using System;
using System.Drawing;

namespace TwitterConnector.Data
{
    public class TwitterUser : ICloneable, IDisposable
    {
        private bool _isDisposed;

        public TwitterUser()
        {
            
        }
        public TwitterUser(string name, string location, string description, string screenName, int followersCount, int friendsCount)
        {
            Name = name;
            Location = location;
            Description = description;
            ScreenName = screenName;
            FollowersCount = followersCount;
            FriendsCount = friendsCount;
        }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string ScreenName { get; set; }
        public int FollowersCount { get; set; }
        public int FriendsCount { get; set; }
        public Image Picture { get; set; }
        public string PicturePath { get; set; }


        public object Clone()
        {
            TwitterUser newUser = new TwitterUser();
            newUser.Description = this.Description;
            newUser.Location = this.Location;
            newUser.Name = this.Name;
            if (newUser.Picture != null) newUser.Picture = this.Picture.Clone() as Image;
            newUser.PicturePath = this.PicturePath;
            newUser.ScreenName = this.ScreenName;
            return newUser;
        }

        #region IDisposable Members

        /// <summary>
        /// Performs the disposal.
        /// </summary>
        private void Dispose(bool disposing)
        {
            if (disposing && !_isDisposed)
            {
                this.Description = null;
                this.Location = null;
                this.Name = null;
                this.Picture.Dispose();
                this.Picture = null;
                this.PicturePath = null;
                this.ScreenName = null;
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
