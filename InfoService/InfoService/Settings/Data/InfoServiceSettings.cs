using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using MediaPortal.Configuration;
using InfoService.Enums;

namespace InfoService.Settings.Data
{
    #region General
    public class SettingsGeneral
    {
        public SettingsGeneral()
        {
            LogDebug = false;
            LogError = true;
            LogWarning = false;
            PluginName = "InfoService";
            FeedCacheFolder = Config.GetFolder(Config.Dir.Thumbs) + @"\InfoService\Feeds\";
            TwitterCacheFolder = Config.GetFolder(Config.Dir.Thumbs) + @"\InfoService\Twitter\";
            DeveloperMode = false;
        }

        public bool LogWarning { get; set; }
        public bool LogError { get; set; }
        public bool LogDebug { get; set; }
        public string PluginName { get; set; }
        public string FeedCacheFolder { get; set; }
        public string TwitterCacheFolder { get; set; }
        public bool DeveloperMode { get; set; }

    }
    #endregion

    #region Web browser
    public class SettingsWebBrowser
    {
        public SettingsWebBrowser() 
        {
        }

        public WebBrowserType BrowserType { get; set; }
        public string WindowID { get; set; }

        [XmlIgnore]
        private List<SettingsWebBrowserGUIProperty> _guiProperties;

        [XmlElement("GUIProperty")]
        public List<SettingsWebBrowserGUIProperty> GUIProperties
        {
            get { return _guiProperties ?? (_guiProperties = new List<SettingsWebBrowserGUIProperty>()); }
            set
            {
                if (_guiProperties == null) _guiProperties = new List<SettingsWebBrowserGUIProperty>();
                _guiProperties = value;
            }
        }
    }

    public class SettingsWebBrowserGUIProperty
    {
        public string Property { get; set; }
        public string Value { get; set; }
    }
    #endregion

    #region Feeds
    public class SettingsFeeds
    {
        public SettingsFeeds()
        {
            Feeds = new List<SettingsFeed>();
            Enabled = true;
            RandomFeedOnUpdate = false;
            RandomFeedOnStartup = false;
            DeletionInterval = DeleteCache.Week;
            Separator = "+++";
            SeparatorAll = "+++";
            ItemsCountPerFeed = 3;
            ItemsCount = 10;
            RefreshInterval = 30;
            UpdateOnStartup = true;
            ShowItemPublishTime = true;
            TickerMask = "%itemtitle%";
            TickerAllMask = "%itemtitle% (%itemsource%)";
            StartupFeedIndex = 0;
            ItemPublishTimeAllFeeds = ItemPublishTimeAllFeedsType.FeedName;
            ShowPopup = true;
            PopupTimeout = 10;
            PopupWhileFullScreenVideo = true;
        }

        public void CreateDefaultFeed()
        {
            SettingsFeed sf = new SettingsFeed();
            sf.UrlPath = "http://www.team-mediaportal.com/rss-feeds";
            sf.ImagePath = "";
            sf.Title = "MediaPortal News";
            sf.DefaultZoom = 100;
            sf.ShowPopup = true;
            Feeds.Clear();
            Feeds.Add(sf);
        }

        public bool Enabled { get; set; }
        public bool RandomFeedOnUpdate { get; set; }
        public bool RandomFeedOnStartup { get; set; }
        public int StartupFeedIndex { get; set; }
        public InfoService.Enums.DeleteCache DeletionInterval { get; set; }
        public DateTime LastCacheDeletion { get; set; }
        public decimal ItemsCountPerFeed { get; set; }
        public decimal ItemsCount { get; set; }
        public decimal RefreshInterval { get; set; }
        public bool UpdateOnStartup { get; set; }
        public bool ShowItemPublishTime { get; set; }
        public string TickerMask { get; set; }
        public string TickerAllMask { get; set; }
        public string Separator { get; set; }
        public string SeparatorAll { get; set; }
        public bool ShowPopup { get; set; }
        public decimal PopupTimeout { get; set; }
        public bool PopupWhileFullScreenVideo { get; set; }
        public ItemPublishTimeAllFeedsType ItemPublishTimeAllFeeds { get; set; }
        [XmlElement("Feed")]
        public List<SettingsFeed> Feeds { get; set; }
    }

    public class SettingsFeed
    {
        [XmlAttribute("Title")]
        public string Title { get; set; }
        public string UrlPath { get; set; }
        public int DefaultZoom { get; set; }
        public string ImagePath { get; set; }
        public bool ShowPopup { get; set; }
    }
    #endregion

    #region Weather
    public class SettingsWeather
    {
        public SettingsWeather()
        {
            Enabled = true;
            RefreshInterval = 60;
            UpdateOnStartup = true;
        }

        public bool Enabled { get; set; }
        public decimal RefreshInterval { get; set; }
        public bool UpdateOnStartup { get; set; }
    }
    #endregion

    #region Twitter
    public class SettingsTwitter
    {
        public SettingsTwitter()
        {
            ItemsCount = 10;
            RefreshInterval = 45;
            TickerMask = "%message%";
            Separator = "+++";
            UpdateOnStartup = true;
            PopupTimeout = 10;
            ShowPopup = true;
            PopupWhileFullScreenVideo = true;
        }

        public bool Enabled { get; set; }
        public bool UpdateOnStartup { get; set; }
        public InfoService.Enums.DeleteCache DeletionInterval { get; set; }
        public DateTime LastCacheDeletion { get; set; }
        public string TokenValue { get; set; }
        public string TokenSecret { get; set; }
        public string Pin { get; set; }
        public string Separator { get; set; }
        public decimal ItemsCount { get; set; }
        public decimal RefreshInterval { get; set; }
        public UsedTimelines UsedTimelines;
        public string TickerMask { get; set; }
        public TwitterStatusUpdate TwitterStatusUpdate = new TwitterStatusUpdate();
        public bool ShowPopup { get; set; }
        public decimal PopupTimeout { get; set; }
        public bool PopupWhileFullScreenVideo { get; set; }
    }

    public class TwitterStatusUpdate
    {
        public TwitterStatusUpdate() {
            WatchMoviesMask = "I'm just watching %video% on my MediaPortal HTPC!";
            WatchSeriesMask = "I'm just watching episode %season%x%episode% of %showname% on my MediaPortal HTPC!";
        }

        [XmlAttribute("Enabled")]
        public bool Enabled { get; set; }
        public bool WithMovingPictures { get; set; }
        public bool WithMPTVSeries { get; set; }
        public bool WithMyVideo { get; set; }
        public string WatchMoviesMask { get; set; }
        public string WatchSeriesMask { get; set; }
    }

    public struct UsedTimelines
    {
        public bool PublicTimeline { get; set; }
        public bool HomeTimeline { get; set; }
        public bool UserTimeline { get; set; }
        public bool FriendsTimeline { get; set; }
        public bool MentionsTimeline { get; set; }
        public bool RetweetedByMeTimeline { get; set; }
        public bool RetweetedToMeTimeline { get; set; }
        public bool RetweetsOfMeTimeline { get; set; }
    }
    #endregion

    #region Recently Added
    public struct SettingsRecentlyAdded
    {
        [XmlIgnore]
        private List<SettingsRecentlyAddedItem> _recentlyAdded;

        [XmlElement("RecentlyAddedItem")]
        public List<SettingsRecentlyAddedItem> RecentlyAdded
        {
            get { return _recentlyAdded ?? (_recentlyAdded = new List<SettingsRecentlyAddedItem>()); }
            set
            {
                if (_recentlyAdded == null) _recentlyAdded = new List<SettingsRecentlyAddedItem>();
                _recentlyAdded = value;
            }
        }
    }

    public class SettingsRecentlyAddedItem
    {
        public SettingsRecentlyAddedItem()
        {
            DateAdded = DateTime.Now; 
        }

        [XmlAttribute("Title")]
        public string Title { get; set; }

        [XmlAttribute("Type")]
        public RecentlyAddedWatchedType Type { get; set; }

        public string Season { get; set; }
        public string EpisodeNumber { get; set; }
        public string EpisodeTitle { get; set; }
        public string Thumb { get; set; }
        public string Fanart { get; set; }

        public DateTime DateAdded { get; set; }
    }
    #endregion

    #region FeedItemsFilters
    public class SettingsFeedItemsFilters {
        public SettingsFeedItemsFilters() {
        }

        [XmlIgnore]
        private List<SettingsFeedItemsFilter> _feedItemFilters;

        [XmlElement("FeedItemFilter")]
        public List<SettingsFeedItemsFilter> FeedItemFilters {
            get { return _feedItemFilters ?? (_feedItemFilters = new List<SettingsFeedItemsFilter>()); }
            set {
                if (_feedItemFilters == null) _feedItemFilters = new List<SettingsFeedItemsFilter>();
                _feedItemFilters = value;
            }
        }
    }

    public class SettingsFeedItemsFilter {
        public bool IsEnabled { get; set; }
        public bool IsRegEx { get; set; }
        public string ReplaceThis { get; set; }
        public string ReplaceWith { get; set; }
        public bool UseInTitle { get; set; }
        public bool UseInBody { get; set; }
        public bool CleanBefore { get; set; }
    }
    #endregion
}
