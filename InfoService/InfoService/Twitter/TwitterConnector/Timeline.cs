#region Usings

using System;
using System.Collections.Generic;
using TwitterConnector.Data;
using TwitterConnector.OAuth;
using TwitterConnector.Json;

#endregion

namespace TwitterConnector
{
    public class Timeline
    {
        public Timeline(TimelineType type, string user, string password, AccessToken accessToken)
        {
            _items = new List<TwitterItem>();
            _type = type;
            _user = user;
            _password = password;
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
        }
        public bool LastUpdateSuccessful { get; set; }
        public DateTime LastUpdate { get; set; }
        private string _user;
        private string _password;
        private AccessToken _accessToken;
        private bool _authSettingsSupplied;


        public bool Update(string cacheFolder = "")
        {
            if (_authSettingsSupplied)
            {
                LogEvents.InvokeOnInfo(string.IsNullOrEmpty(cacheFolder)
                    ? new TwitterArgs("Updating Twitter " + _type + " timeline without using cache")
                    : new TwitterArgs("Updating Twitter " + _type + " timeline using cache"));
                List<TwitterItem> oldTwitterItems = _items;
                if (TimelineJsonParser.TryParse(ref _items, _type, _user, _password, _accessToken, cacheFolder))
                {
                    LogEvents.InvokeOnInfo(new TwitterArgs("Update of Twitter " + _type + " timeline successful"));
                    LastUpdateSuccessful = true;
                    LastUpdate = DateTime.Now;
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
    }
}
