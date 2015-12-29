#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using TwitterConnector.Data;
using TwitterConnector.OAuth;
using TwitterConnector.Json;
using TwitterConnector;
#endregion

namespace TwitterConnector
{
    public class Timeline : IDisposable
    {
        public Timeline(TimelineType type, AccessToken accessToken)
        {
            _items = new List<TwitterItem>();
            _type = type;
            _accessToken = accessToken;
            _authSettingsSupplied = true;
        }
        public Timeline(TimelineType type)
        {
            _items = new List<TwitterItem>();
            _type = type;
            _authSettingsSupplied = false;
        }

        public Timeline()
        {
            _items = new List<TwitterItem>();
            _authSettingsSupplied = false;
        }

        private List<TwitterItem> _items;
        public List<TwitterItem> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        private TimelineType _type;
        public TimelineType Type
        {
            get { return _type;  }
            set { _type = value; }
        }
        public bool LastUpdateSuccessful { get; set; }
        public DateTime LastUpdate { get; set; }
        public bool RetweetsDownloaded { get; private set; }
        private AccessToken _accessToken;
        private bool _authSettingsSupplied;
        private bool _isDisposed;

        public delegate void OnNewItemsEventHandler(Timeline timeline, List<TwitterItem> newItems);

        private OnNewItemsEventHandler _onNewItems;
        public event OnNewItemsEventHandler OnNewItems
        {
            add
            {
                if (_onNewItems == null || !_onNewItems.GetInvocationList().Contains(value))
                {
                    _onNewItems += value;
                }
            }
            remove
            {
                if (_onNewItems != null)
                {
                    _onNewItems -= value;
                }
            }
        }

        public bool Update(bool withRetweets = true, string cacheFolder = "")
        {
            if (_authSettingsSupplied)
            {
                LogEvents.InvokeOnInfo(string.IsNullOrEmpty(cacheFolder)
                    ? new TwitterArgs("Updating Twitter " + _type + " timeline without using cache")
                    : new TwitterArgs("Updating Twitter " + _type + " timeline using cache"));
                List<TwitterItem> oldTwitterItems = _items.CloneList();
                RetweetsDownloaded = withRetweets;
                if (TimelineJsonParser.TryParse(ref _items, _type, _accessToken, withRetweets, cacheFolder))
                {
                    LogEvents.InvokeOnInfo(new TwitterArgs("Update of Twitter " + _type + " timeline successful"));
                    LastUpdateSuccessful = true;
                    LastUpdate = DateTime.Now;
                    CheckForNewItems(oldTwitterItems, _items);
                    return true;
                }
                _items = oldTwitterItems;
                LastUpdateSuccessful = false;
                LastUpdate = DateTime.Now;
                LogEvents.InvokeOnInfo(new TwitterArgs("Update of Twitter " + _type + " timeline unsuccessful. See above for errors or warnings"));
                return false;
            }
            LogEvents.InvokeOnError(new TwitterArgs("Cannot update " + _type + " timeline. No auth settings are supplied"));
            return false;
        }

        private void CheckForNewItems(List<TwitterItem> oldItems, List<TwitterItem> items)
        {
            if (oldItems.Count >= 1 && items.Count >= 1 && _onNewItems != null)
            {
                List<TwitterItem> newItems = items.Except(oldItems, new TwitterItemComparer()).ToList();
                if (newItems.Count > 0)
                {
                    _onNewItems((Timeline)this.Clone(), newItems.CloneList<TwitterItem>());
                }
            }
        }

        public virtual object Clone()
        {
            Timeline timeline = new Timeline(this.Type, this._accessToken);
            timeline.LastUpdate = this.LastUpdate;
            timeline.LastUpdateSuccessful = this.LastUpdateSuccessful;
            timeline.RetweetsDownloaded = this.RetweetsDownloaded;
            timeline.Items = this.Items.CloneList();
            return timeline;
        }
        #region IDisposable Members

        /// <summary>
        /// Performs the disposal.
        /// </summary>
        private void Dispose(bool disposing)
        {
            if (disposing && !_isDisposed)
            {
                this.Items.Clear();
                this.Items = null;
                this._accessToken = null;
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
