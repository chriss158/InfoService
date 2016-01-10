#region Usings

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using FeedReader;
using FeedReader.Data;
using FeedReader.Expections;
using InfoService.Enums;
using InfoService.Utils;
using MediaPortal.GUI.Library;
using System.Xml;
using InfoService.GUIWindows;
using InfoService.Utils.NotificationBar;
using InfoService.Utils.QueuedNotifyBar;
using MediaPortal.Player;

#endregion

namespace InfoService.Feeds
{
    public static class FeedService
    {
        #region Config Properties
        private static decimal _refreshInterval;
        public static decimal RefreshInterval
        {
            get { return _refreshInterval; }
            set
            {
                Logger.WriteLog("Set FeedRefreshInterval to " + value, LogLevel.Debug, InfoServiceModul.InfoService);
                _refreshInterval = value;
            }
        }

        private static string _separator;
        public static string Separator
        {
            get { return _separator; }
            set
            {
                Logger.WriteLog("Set FeedSeparator to " + value, LogLevel.Debug, InfoServiceModul.InfoService);
                _separator = value;
            }
        }

        private static string _separatorAll;
        public static string SeparatorAll
        {
            get { return _separatorAll; }
            set
            {
                Logger.WriteLog("Set FeedSeparatorAll to " + value, LogLevel.Debug, InfoServiceModul.InfoService);
                _separatorAll = value;
            }
        }

        private static decimal _items;
        public static decimal Items
        {
            get { return _items; }
            set
            {
                Logger.WriteLog("Set FeedItemsCount to " + value, LogLevel.Debug, InfoServiceModul.InfoService);
                _items = value;
            }
        }

        private static decimal _itemsPerFeed;
        public static decimal ItemsPerFeed
        {
            get { return _itemsPerFeed; }
            set
            {
                Logger.WriteLog("Set FeedItemsCountPerFeed to " + value, LogLevel.Debug, InfoServiceModul.InfoService);
                _itemsPerFeed = value;
            }
        }

        private static bool _randomFeedOnStartup;
        public static bool RandomFeedOnStartup
        {
            get { return _randomFeedOnStartup; }
            set
            {
                Logger.WriteLog("Set FeedRandomFeedOnStartup to " + value, LogLevel.Debug, InfoServiceModul.InfoService);
                _randomFeedOnStartup = value;
            }
        }

        private static bool _randomFeedOnUpdate;
        public static bool RandomFeedOnUpdate
        {
            get { return _randomFeedOnUpdate; }
            set
            {
                Logger.WriteLog("Set FeedRandomFeedOnUpdate to " + value, LogLevel.Debug, InfoServiceModul.InfoService);
                _randomFeedOnUpdate = value;
            }
        }

        private static bool _updateOnStartup;
        public static bool UpdateOnStartup
        {
            get { return _updateOnStartup; }
            set
            {
                Logger.WriteLog("Set FeedUpdateOnStartup to " + value, LogLevel.Debug, InfoServiceModul.InfoService);
                _updateOnStartup = value;
            }
        }

        private static bool _showPopup;
        public static bool ShowPopup
        {
            get { return _showPopup; }
            set
            {
                Logger.WriteLog("Set FeedsShowPopup to " + value, LogLevel.Debug, InfoServiceModul.InfoService);
                _showPopup = value;
            }
        }

        private static bool _popupWhileFullScreenVideo;
        public static bool PopupWhileFullScreenVideo
        {
            get { return _popupWhileFullScreenVideo; }
            set
            {
                Logger.WriteLog("Set FeedsPopupWhileFullScreenVideo to " + value, LogLevel.Debug, InfoServiceModul.InfoService);
                _popupWhileFullScreenVideo = value;
            }
        }

        private static decimal _popupTimeout;
        public static decimal PopupTimeout
        {
            get { return _popupTimeout; }
            set
            {
                Logger.WriteLog("Set FeedsPopupTimeout to " + value, LogLevel.Debug, InfoServiceModul.InfoService);
                _popupTimeout = value;
            }
        }

        private static bool _showPublishTime;
        public static bool ShowPublishTime
        {
            get { return _showPublishTime; }
            set
            {
                Logger.WriteLog("Set FeedShowItemPublishTime to " + value, LogLevel.Debug, InfoServiceModul.InfoService);
                _showPublishTime = value;
            }
        }

        private static DeleteCache _deletionInterval;
        public static DeleteCache DeletionInterval
        {
            get { return _deletionInterval; }
            set
            {
                Logger.WriteLog("Set FeedDeletionInterval to " + value, LogLevel.Debug, InfoServiceModul.InfoService);
                _deletionInterval = value;
            }
        }

        private static string _tickerMask;
        public static string TickerMask
        {
            get { return _tickerMask; }
            set
            {
                Logger.WriteLog("Set FeedTickerMask to " + value, LogLevel.Debug, InfoServiceModul.InfoService);
                _tickerMask = value;
            }
        }

        private static string _tickerAllMask;
        public static string TickerAllMask
        {
            get { return _tickerAllMask; }
            set
            {
                Logger.WriteLog("Set FeedTickerAllMask to " + value, LogLevel.Debug, InfoServiceModul.InfoService);
                _tickerAllMask = value;
            }
        }

        public static bool MediaPortalStartup { get; set; }

        private static int _startupFeedIndex;
        public static int StartupFeedIndex
        {
            get { return _startupFeedIndex; }
            set
            {
                Logger.WriteLog("Set StartupFeedIndex to " + value, LogLevel.Debug, InfoServiceModul.InfoService);
                _startupFeedIndex = value;
            }
        }

        private static ItemPublishTimeAllFeedsType _itemPublishTimeAllFeeds;
        public static ItemPublishTimeAllFeedsType ItemPublishTimeAllFeeds
        {
            get { return _itemPublishTimeAllFeeds; }
            set
            {
                Logger.WriteLog("Set ItemPublishTimeAllFeeds to " + value, LogLevel.Debug, InfoServiceModul.InfoService);
                _itemPublishTimeAllFeeds = value;
            }
        }

        private static string _cacheFolder;
        public static string CacheFolder
        {
            get { return _cacheFolder; }
            set
            {
                Logger.WriteLog("Set CacheFolder to " + value, LogLevel.Debug, InfoServiceModul.InfoService);
                _cacheFolder = value;
            }
        }
        #endregion

        #region Fields
        private static readonly List<ExFeed> _feeds = new List<ExFeed>();
        //private static List<Feed> _newFeedItems = new List<Feed>();
        private static readonly Logger Logger = Logger.GetInstance();
        //private static bool newFeedItems = false;

        #endregion

        #region Contructor
        static FeedService()
        {
            ShowPublishTime = true;
            UpdateOnStartup = true;
            ActiveFeedIndex = -1;

        }
        #endregion

        #region Properties
        public static bool Enabled { get; private set; }
        public static bool UpdateInProgress { get; set; }
        public static int ActiveFeedIndex { get; private set; }
        public static string LastFeedline { get; set; }
        public static DateTime LastRefresh { get; set; }
        public static List<ExFeed> Feeds
        {
            get { return _feeds; }
        }

        #endregion

        #region Methods

        private static Dictionary<Feed, List<FeedItem>> _newFeedItems;

        public static void SetupFeeds()
        {
            Enabled = true;
            _newFeedItems = new Dictionary<Feed, List<FeedItem>>();
            ActiveFeedIndex = -1;
            LogEvents.OnDebug += LogEvents_OnDebug;
            LogEvents.OnError += LogEvents_OnError;
            LogEvents.OnWarning += LogEvents_OnWarning;
            LogEvents.OnInfo += LogEvents_OnInfo;
            ExFeed allFeed = new ExFeed();
            allFeed.Title = InfoServiceUtils.GetLocalizedLabel(25);
            allFeed.Type = FeedType.All;
            allFeed.IsAllFeed = true;
            allFeed.Description = InfoServiceUtils.GetLocalizedLabel(26);
            allFeed.ImagePath = GUIGraphicsContext.Skin + @"\media\InfoService\defaultFeedALL.png";
            _feeds.Add(allFeed);

        }

        private static string ReplaceProblemChars(string text)
        {
            string output = text;
            Dictionary<string, string> replaceChars = new Dictionary<string, string>();
            replaceChars.Add("[\u2018-\u201b]", "'");
            replaceChars.Add("[\u201c-\u201f]", "\"");
            replaceChars.Add(@"\p{Pi}", "\"");
            replaceChars.Add(@"\p{Pf}", "\"");
            replaceChars.Add(@"\p{Pd}", "-");
            replaceChars.Add("\u2024", ".");
            replaceChars.Add("\u2025", "..");
            replaceChars.Add("\u2026", "...");
            replaceChars.Add("\u2047", "??");
            replaceChars.Add("\u2048", "?!");
            replaceChars.Add("\u2049", "!?");
            replaceChars.Add("€", "Euro");
            //replaceChars.Add("[^\u0000-\u00FF]", "");


            foreach (KeyValuePair<string, string> keyValuePair in replaceChars)
            {
                Regex regEx = new Regex(keyValuePair.Key, RegexOptions.Compiled);
                foreach (Match m in regEx.Matches(output))
                {
                    output = output.Replace(m.Value, keyValuePair.Value);

                }
            }
            return output;
        }

        public static bool UpdateAllFeeds()
        {
            if (!Enabled)
            {
                Logger.WriteLog("Feeds are not enabled", LogLevel.Error, InfoServiceModul.Feed);
                return false;
            }

            UpdateInProgress = true;
            bool returnValue = false;
            try
            {
                int errorCounter = 0;
                _newFeedItems.Clear();
                Logger.WriteLog("Check if feed image needs a default image or should be get a own feed image...", LogLevel.Debug, InfoServiceModul.Feed);
                foreach (ExFeed feed in _feeds.Where(feed => !feed.IsAllFeed))
                {
                    //If feed update was succesful
                    if (feed.Update(true))
                    {
                        //Replace Problem Chars
                        foreach (ExFeedItem item in feed.Items)
                        {
                            item.Title = ReplaceProblemChars(item.Title);
                        }
                        feed.Title = ReplaceProblemChars(feed.Title);

                        if (ActiveFeedIndex == -1) SetActive(1);

                        //Load Feed title from Config
                        if (!string.IsNullOrEmpty(feed.OwnFeedTitle)) feed.Title = feed.OwnFeedTitle;

                        //Load feed image from config, if set!
                        if (!string.IsNullOrEmpty(feed.OwnFeedImagePath))
                        {
                            Logger.WriteLog("Feed[" + feed.Title + "] should get a own feed image. Try to load own feed image from " + feed.OwnFeedImagePath, LogLevel.Debug, InfoServiceModul.Feed);
                            try
                            {
                                using (FileStream fs = new FileStream(feed.OwnFeedImagePath, FileMode.Open,
                                        FileAccess.Read))
                                {
                                    using (Image cacheImage = Image.FromStream(fs))
                                    {
                                        feed.Image = cacheImage.Clone() as Image;
                                    }
                                }
                                if(feed.Image != null) feed.Image.Dispose();
                                feed.Image = null;
                                feed.ImagePath = feed.OwnFeedImagePath;
                                Logger.WriteLog("Loading own feed[" + feed.Title + "] image successful", LogLevel.Debug, InfoServiceModul.Feed);
                            }
                            catch (Exception ex)
                            {
                                Logger.WriteLog(new FeedArgs("Error loading own feed image from " + feed.OwnFeedImagePath + " .", ex.Message, ex.StackTrace), LogLevel.Error, InfoServiceModul.Feed);
                                feed.ImagePath = string.Empty;
                            }
                        }
                        //If not check if feed has an image
                        else
                        {
                            Logger.WriteLog("Feed[" + feed.Title + "] don't need a own feed image", LogLevel.Debug, InfoServiceModul.Feed);
                            if (string.IsNullOrEmpty(feed.ImagePath))
                            {
                                //Feed has no image... Load default feed image
                                string path = string.Empty;
                                switch (feed.Type)
                                {
                                    case FeedType.RDF:
                                        path = GUIGraphicsContext.Skin + @"\media\InfoService\defaultFeedRDF.png";
                                        break;
                                    case FeedType.RSS:
                                        path = GUIGraphicsContext.Skin + @"\media\InfoService\defaultFeedRSS.png";
                                        break;
                                    case FeedType.ATOM:
                                        path = GUIGraphicsContext.Skin + @"\media\InfoService\defaultFeedAtom.png";
                                        break;
                                }
                                try
                                {
                                    Logger.WriteLog("Feed[" + feed.Title + "] image is empty. Try to load default feed image from " + path, LogLevel.Debug, InfoServiceModul.Feed);
                                    using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                                    {
                                        using (Image cacheImage = Image.FromStream(fs))
                                        {
                                            feed.Image = cacheImage.Clone() as Image;
                                        }
                                    }
                                    if(feed.Image != null) feed.Image.Dispose();
                                    feed.Image = null;
                                    feed.ImagePath = path;
                                    Logger.WriteLog("Loading default feed[" + feed.Title + "] image successful", LogLevel.Debug, InfoServiceModul.Feed);
                                }
                                catch (Exception ex)
                                {
                                    Logger.WriteLog(new FeedArgs("Error loading default feed logo from " + path + " . It seems that there is no ..\\media\\InfoService folder in the skin folder. Please reinstall skin/plugin.", ex.Message, ex.StackTrace), LogLevel.Error, InfoServiceModul.Feed);
                                    feed.ImagePath = string.Empty;
                                }
                            }
                            else Logger.WriteLog("Feed[" + feed.Title + "] don't need a default feed image", LogLevel.Debug, InfoServiceModul.Feed);

                        }

                        //Load images...
                        LoadImagesForFeedItems(feed);
                    }
                    //If not increase error counter
                    else
                    {
                        errorCounter++;
                    }
                }

                if (_feeds.Where(feed => !feed.IsAllFeed).Count() <= 0)
                {
                    Logger.WriteLog("No need to update. No feeds available to update. Now setting up properties...", LogLevel.Info, InfoServiceModul.Feed);
                    LastRefresh = DateTime.Now;
                    returnValue = true;
                }
                else
                {

                    //Error counter = 0 -> all feeds were successfull downloaded
                    if (errorCounter == 0)
                    {
                        Logger.WriteLog("Update of Feed successful. Now setting up properties...", LogLevel.Info, InfoServiceModul.Feed);
                        LastRefresh = DateTime.Now;
                        returnValue = true;
                    }

                    //Error counter >= 1 or < all feed count -> not all feeds were successfully downloaded
                    else if (errorCounter >= 1 && errorCounter < _feeds.Count - 1)
                    {
                        Logger.WriteLog("Update of FeedService partially successful. See above for errors. Anyway now setting up properties...", LogLevel.Warning, InfoServiceModul.Feed);
                        LastRefresh = DateTime.Now;
                        returnValue = true;
                    }
                    //No feed was successfully downloaded
                    else
                    {
                        Logger.WriteLog("Update of Feed unsuccessful. Check your internet connection!", LogLevel.Error, InfoServiceModul.Feed);
                        returnValue = false;
                    }
                }

                //Set all feeds feed
                Logger.WriteLog("Setup the all feeds ticker...", LogLevel.Debug, InfoServiceModul.Feed);
                _feeds[0] = FillAllFeeds(_feeds[0]);
                _feeds[0].LastUpdateSuccessful = true;
                _feeds[0].LastUpdate = DateTime.Now;

                
            }
            finally
            {
                UpdateInProgress = false;
                FeedUtils.SetFeedProperties(returnValue);
                ShowNewFeedItemPopup();
            }
            return returnValue;
        }

        private static void ShowNewFeedItemPopup()
        {
            foreach (KeyValuePair<Feed, List<FeedItem>> feedItem in _newFeedItems)
            {
                if (_feeds.Find(f => f.UrlPath == feedItem.Key.UrlPath).ShowPopupDialog)
                {
                    Logger.WriteLog("Popups are enabled and new feed items loaded...", LogLevel.Info,
                        InfoServiceModul.Feed);
                    Logger.WriteLog("Popups for feed[" + feedItem.Key.Title + "] are enabled...", LogLevel.Debug,
                        InfoServiceModul.Feed);


                    bool notificationBarPluginEnabled = false;
                    using (MediaPortal.Profile.Settings settings = new MediaPortal.Profile.MPSettings())
                    {
                        notificationBarPluginEnabled =
                            settings.GetValueAsString("plugins", "MPNotificationBar", "no") == "yes";
                    }
                    if (InfoServiceUtils.IsAssemblyAvailable("MPNotificationBar", new Version(0, 8, 3, 0)) &&
                        System.IO.File.Exists(GUIGraphicsContext.Skin + @"\NotificationBar.xml") &&
                        notificationBarPluginEnabled)
                    {
                        string text = string.Empty;
                        foreach (FeedItem item in feedItem.Value)
                        {
                            text += item.Title + "\n";
                        }

                        if (!string.IsNullOrEmpty(text))
                        {
                            if (text.Length >= 2) text = text.Substring(0, text.Length - 1);
                            if (PopupWhileFullScreenVideo || !GUIGraphicsContext.IsFullScreenVideo)
                            {
                                Logger.WriteLog(
                                    "Showing new Popup (NotificationBar) for Feed[" + feedItem.Key.Title + "] with text \"" +
                                    text +
                                    "\"", LogLevel.Info, InfoServiceModul.Feed);
                                NotificationBar.ShowNotificationBar(
                                    String.Format(InfoServiceUtils.GetLocalizedLabel(38), feedItem.Value.Count.ToString(),
                                        feedItem.Key.Title), text, feedItem.Key.ImagePath, PopupWhileFullScreenVideo, (int) PopupTimeout);
                            }
                            else
                                Logger.WriteLog(
                                    "Showing new Popup (NotificationBar) for Feed[" + feedItem.Key.Title + "] with text \"" +
                                    text +
                                    "\" is not allowed - Fullscreen Video is running...", LogLevel.Info,
                                    InfoServiceModul.Feed);
                        }
                    }
                    else
                    {
                        if (InfoServiceUtils.AreNotifyBarSkinFilesInstalled())
                        {


                            foreach (FeedItem item in feedItem.Value)
                            {
                                string header = item.Title + " (" + feedItem.Key.Title + ")";
                                string text = item.Description;
                                if (PopupWhileFullScreenVideo || !GUIGraphicsContext.IsFullScreenVideo)
                                {
                                    Logger.WriteLog(
                                        "Showing new Popup (MediaPortal Dialog) for Feed[" + feedItem.Key.Title +
                                        "] with header \"" +
                                        header + "\" and text \"" + text + "\"", LogLevel.Info, InfoServiceModul.Feed);
                                    InfoServiceUtils.ShowDialogNotifyWindow(header, text, feedItem.Key.ImagePath,
                                        new Size(120, 120),
                                        (int)PopupTimeout,
                                        () =>
                                        {
                                            if (GUIGraphicsContext.IsTvWindow() && GUIGraphicsContext.IsFullScreenVideo)
                                            {
                                                GUIWindowManager.IsOsdVisible = false;
                                                GUIGraphicsContext.IsFullScreenVideo = false;
                                                GUIWindowManager.ShowPreviousWindow();
                                            }
                                            GUIWindowManager.ActivateWindow(GUIFeed.GUIFeedId,
                                                string.Format("feedGuid:{0},feedItemIndex:{1}", feedItem.Key.Guid, item.Index));
                                        });
                                    //NotifyBarQueue.ShowDialogNotifyWindowQueued(header, text, feedItem.Key.ImagePath,
                                    //    new Size(120, 120),
                                    //    (int) PopupTimeout,
                                    //    () =>
                                    //    {
                                    //        if (GUIGraphicsContext.IsTvWindow() && GUIGraphicsContext.IsFullScreenVideo)
                                    //        {
                                    //            GUIWindowManager.IsOsdVisible = false;
                                    //            GUIGraphicsContext.IsFullScreenVideo = false;
                                    //            GUIWindowManager.ShowPreviousWindow();
                                    //        }
                                    //        GUIWindowManager.ActivateWindow(GUIFeed.GUIFeedId,
                                    //            string.Format("feedGuid:{0},feedItemIndex:{1}", feedItem.Key.Guid, item.Index));
                                    //    });
                                }

                                else
                                    Logger.WriteLog(
                                        "Showing new Popup (MediaPortal Dialog) for Feed[" + feedItem.Key.Title +
                                        "] with header \"" +
                                        header + "\" and text \"" + text +
                                        "\" is not allowed - Fullscreen Video is running...", LogLevel.Info,
                                        InfoServiceModul.Feed);
                            }

                        }
                        else
                        {
                            string text = feedItem.Value.Count == 1
                                ? feedItem.Value[0].Title
                                : String.Format(InfoServiceUtils.GetLocalizedLabel(39), feedItem.Key.Title);
                            if (!string.IsNullOrEmpty(text))
                            {
                                if (PopupWhileFullScreenVideo || !GUIGraphicsContext.IsFullScreenVideo)
                                {
                                    Logger.WriteLog(
                                        "Showing new Popup (MediaPortal Dialog) for Feed[" + feedItem.Key.Title +
                                        "] with text \"" +
                                        text + "\"", LogLevel.Info, InfoServiceModul.Feed);
                                    InfoServiceUtils.ShowDialogNotifyWindow(String.Format(InfoServiceUtils.GetLocalizedLabel(38), feedItem.Value.Count.ToString(), feedItem.Key.Title), text, feedItem.Key.ImagePath, new Size(120, 120), (int)PopupTimeout);
                                    //NotifyBarQueue.ShowDialogNotifyWindowQueued(
                                    //    String.Format(InfoServiceUtils.GetLocalizedLabel(38), feedItem.Value.Count.ToString(),
                                    //        feedItem.Key.Title), text, feedItem.Key.ImagePath, new Size(120, 120), (int)PopupTimeout);
                                }
                                else
                                    Logger.WriteLog(
                                        "Showing new Popup (MediaPortal Dialog) for Feed[" + feedItem.Key.Title +
                                        "] with text \"" +
                                        text + "\" is not allowed - Fullscreen Video is running...", LogLevel.Info,
                                        InfoServiceModul.Feed);
                            }
                        }
                    }
                }
                else
                {
                    Logger.WriteLog("Popups for feed[" + feedItem.Key.Title + "] are disabled...", LogLevel.Debug,
                        InfoServiceModul.Feed);
                }
            }
        }

        private static void LoadImagesForFeedItems(ExFeed feed)
        {
            Logger.WriteLog("Check if feed item images needs default images...", LogLevel.Debug, InfoServiceModul.Feed);

            //Check all feed items, if they need a default image
            for (int i = 0; i < feed.Items.Count; i++)
            {
                if (string.IsNullOrEmpty(feed.Items[i].ImagePath))
                {
                    string path = string.Empty;
                    string pathBig = string.Empty;
                    switch (feed.Type)
                    {
                        case FeedType.RDF:
                            path = GUIGraphicsContext.Skin + @"\media\InfoService\defaultFeedItemRDF.png";
                            pathBig = GUIGraphicsContext.Skin + @"\media\InfoService\defaultFeedItemRDFBig.png";
                            break;
                        case FeedType.RSS:
                            path = GUIGraphicsContext.Skin + @"\media\InfoService\defaultFeedItemRSS.png";
                            pathBig = GUIGraphicsContext.Skin + @"\media\InfoService\defaultFeedItemRSSBig.png";
                            break;
                        case FeedType.ATOM:
                            path = GUIGraphicsContext.Skin + @"\media\InfoService\defaultFeedItemAtom.png";
                            pathBig = GUIGraphicsContext.Skin + @"\media\InfoService\defaultFeedItemAtomBig.png";
                            break;
                    }
                    //Load normal image
                    try
                    {
                        Logger.WriteLog("Feed[" + feed.Title + "] item[" + i + "] image is empty. Try to load default feed image from " + path, LogLevel.Debug, InfoServiceModul.Feed);
                        using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                        {
                            using (Image cacheImage = Image.FromStream(fs))
                            {
                                feed.Items[i].Image = cacheImage.Clone() as Image;
                            }
                        }
                        if(feed.Items[i].Image != null) feed.Items[i].Image.Dispose();
                        feed.Items[i].Image = null;
                        feed.Items[i].ImagePath = path;
                        Logger.WriteLog("Loading default feed[" + feed.Title + "] item[" + i + "] image successful", LogLevel.Debug, InfoServiceModul.Feed);
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLog(new FeedArgs("Error loading default feed item logo from " + path + " . It seems that there is no ..\\media\\InfoService folder in the skin folder. Please reinstall skin/plugin.", ex.Message, ex.StackTrace), LogLevel.Error, InfoServiceModul.Feed);
                        feed.Items[i].ImagePath = string.Empty;
                    }
                    //Load big image
                    try
                    {
                        Logger.WriteLog("Feed[" + feed.Title + "] item[" + i + "] BIG image is empty. Try to load default BIG feed image from " + pathBig, LogLevel.Debug, InfoServiceModul.Feed);
                        using (FileStream fs = new FileStream(pathBig, FileMode.Open, FileAccess.Read))
                        {
                            using (Image cacheImage = Image.FromStream(fs))
                            {
                                feed.Items[i].ImageBig = cacheImage.Clone() as Image;
                            }
                        }
                        if(feed.Items[i].ImageBig != null) feed.Items[i].ImageBig.Dispose();
                        feed.Items[i].ImageBig = null;
                        feed.Items[i].ImagePathBig = pathBig;
                        Logger.WriteLog("Loading default feed[" + feed.Title + "] item[" + i + "] BIG image successful", LogLevel.Debug, InfoServiceModul.Feed);
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLog(new FeedArgs("Error loading default feed item logo from " + pathBig + " . It seems that there is no ..\\media\\InfoService folder in the skin folder. Please reinstall skin/plugin.", ex.Message, ex.StackTrace), LogLevel.Error, InfoServiceModul.Feed);
                        feed.Items[i].ImagePathBig = string.Empty;
                    }
                }
                //If feed item has an image, use the same for the big image
                else
                {
                    feed.Items[i].ImagePathBig = feed.Items[i].ImagePath;
                    //feed.Items[i].ImageBig = feed.Items[i].Image.Clone() as Image;
                }
            }
        }
        private static ExFeed FillAllFeeds(ExFeed allFeed)
        {
            if (allFeed == null) return null;
            allFeed.Items.Clear();
            foreach (ExFeed feed in _feeds)
            {
                if (feed == null) continue;
                if (feed.Items.Count <= 0) continue;
                if (feed.IsAllFeed) continue;
                int feedCounter = 0;
                foreach (ExFeedItem item in feed.Items)
                {
                    feedCounter++;
                    allFeed.Items.Add(item);
                    if (feedCounter >= ItemsPerFeed)
                    {
                        break;
                    }
                }
            }
            return allFeed;
        }

        public static int SetActive(int index)
        {
            Logger.WriteLog("Try to set feed[" + index + "] as active feed...", LogLevel.Debug, InfoServiceModul.Feed);
            if (Feeds.Count >= 1)
            {
                int oldActiveAndDownloadedIndex = -1;
                int oldDownloadedIndex = -1;
                Logger.WriteLog("Unset the last active feed...", LogLevel.Debug, InfoServiceModul.Feed);
                for (int i = 0; i < Feeds.Count; i++)
                {
                    if (Feeds[i].Active && Feeds[i].LastUpdateSuccessful) oldActiveAndDownloadedIndex = i;
                    if (Feeds[i].LastUpdateSuccessful) oldDownloadedIndex = i;
                    Feeds[i].Active = false;
                }
                Logger.WriteLog("The last active feed and successfully downloaded feed index was \"" + oldActiveAndDownloadedIndex + "\"", LogLevel.Debug, InfoServiceModul.Feed);
                Logger.WriteLog("The last successfully downloaded feed index was \"" + oldDownloadedIndex + "\"", LogLevel.Debug, InfoServiceModul.Feed);
                Logger.WriteLog("These two feeds are fallback feed, if active feed could not be set", LogLevel.Debug, InfoServiceModul.Feed);
                if (Feeds.Count > index && Feeds[index] != null)
                {
                    Logger.WriteLog("The new index[" + index + "] can be set. Check if the new feed was downloaded successful...", LogLevel.Debug, InfoServiceModul.Feed);
                    if (Feeds[index].LastUpdateSuccessful)
                    {
                        Logger.WriteLog("The new index[" + index + "] was downloaded successful...", LogLevel.Debug, InfoServiceModul.Feed);
                        ActiveFeedIndex = index;
                        Feeds[index].Active = true;
                        Logger.WriteLog("Feed[" + Feeds[ActiveFeedIndex].Title + "]/[" + index + "] is now the active feed", LogLevel.Debug, InfoServiceModul.Feed);
                    }
                    else
                    {
                        Logger.WriteLog("The new index[" + index + "] could not be set because it wasn't downloaded successful. Fallback to an old active feed...", LogLevel.Debug, InfoServiceModul.Feed);
                        if (oldActiveAndDownloadedIndex == -1)
                        {
                            ActiveFeedIndex = oldDownloadedIndex;
                            Logger.WriteLog("There was no fallback active and successfully downloaded feed. Try to set the last successfully downloaded[" + oldDownloadedIndex + "] feed as active feed...", LogLevel.Debug, InfoServiceModul.Feed);
                        }
                        else
                        {
                            ActiveFeedIndex = oldActiveAndDownloadedIndex;
                            Logger.WriteLog("Try to set the last active and successfully downloaded[" + oldActiveAndDownloadedIndex + "] feed as active feed...", LogLevel.Debug, InfoServiceModul.Feed);
                        }
                        if (Feeds[ActiveFeedIndex] != null)
                        {
                            Logger.WriteLog("Feed[" + Feeds[ActiveFeedIndex].Title + "]/[" + ActiveFeedIndex + "] is now the active feed", LogLevel.Debug, InfoServiceModul.Feed);
                            Feeds[ActiveFeedIndex].Active = true;
                        }
                        else
                        {
                            Logger.WriteLog("There was a problem to set the feed index[" + index + "] as active feed. Please contact the plugin author", LogLevel.Error, InfoServiceModul.Feed);
                        }
                    }
                }
                else
                {
                    Logger.WriteLog("The new index[" + index + "] could not be set. Fallback to an old active feed...", LogLevel.Debug, InfoServiceModul.Feed);
                    if (oldActiveAndDownloadedIndex == -1)
                    {
                        ActiveFeedIndex = oldDownloadedIndex;
                        Logger.WriteLog("There was no fallback active and successfully downloaded feed. Try to set the last successfully downloaded[" + oldDownloadedIndex + "] feed as active feed...", LogLevel.Debug, InfoServiceModul.Feed);
                    }
                    else
                    {
                        ActiveFeedIndex = oldActiveAndDownloadedIndex;
                        Logger.WriteLog("Try to set the last active and successfully downloaded[" + oldActiveAndDownloadedIndex + "] feed as active feed...", LogLevel.Debug, InfoServiceModul.Feed);
                    }
                    if (Feeds[ActiveFeedIndex] != null)
                    {
                        Logger.WriteLog("Feed[" + Feeds[ActiveFeedIndex].Title + "]/[" + ActiveFeedIndex + "] is now the active feed", LogLevel.Debug, InfoServiceModul.Feed);
                        Feeds[ActiveFeedIndex].Active = true;
                    }
                    else
                    {
                        Logger.WriteLog("There was a problem to set the feed index[" + index + "] as active feed. Please contact the plugin author", LogLevel.Error, InfoServiceModul.Feed);
                    }
                }
                return ActiveFeedIndex;
            }
            Logger.WriteLog(String.Format("Cannot set {0} as active feed, because there are no feeds loaded", index), LogLevel.Debug, InfoServiceModul.Feed);
            return -1;
        }
        public static void AddFeed(string url)
        {
            try
            {
                ExFeed addFeed = new ExFeed(url, CacheFolder);
                if (ShowPopup) addFeed.OnNewItems += new Feed.OnNewItemsEventHandler(addFeed_OnNewItems);
                _feeds.Add(addFeed);
                Logger.WriteLog(string.Format("Added new feed (Url: {0})", url), LogLevel.Info, InfoServiceModul.Feed);
            }
            catch (FeedNoCacheFolderExpection fncfe)
            {
                Logger.WriteLog(fncfe.Message, LogLevel.Error, InfoServiceModul.Feed);
            }
            catch (FeedCacheFolderNotValid fcfnv)
            {
                Logger.WriteLog(fcfnv.Message, LogLevel.Error, InfoServiceModul.Feed);
            }
        }

        public static void AddFeed(string url, string ownFeedTitle, int defaultZoom, string ownFeedImagePath, List<FeedItemFilter> itemFilters, bool showPopup)
        {
            //bool cacheError = false;
            //ExFeed addFeed = null;
            //try
            //{
            //    addFeed = new ExFeed(url, ownFeedTitle, defaultZoom, ownFeedImagePath, CacheFolder, itemFilters, showPopup);
            //}
            //catch (FeedNoCacheFolderExpection fncfe)
            //{
            //    Logger.WriteLog(fncfe.Message, LogLevel.Error, InfoServiceModul.Feed);
            //    cacheError = true;
            //}
            //catch (FeedCacheFolderNotValid fcfnv)
            //{
            //    Logger.WriteLog(fcfnv.Message, LogLevel.Error, InfoServiceModul.Feed);
            //    cacheError = true;

            //}
            //if(cacheError)
            //{
            //    Logger.WriteLog("There were a error with the cache folder. Please look at the messages above. Now InfoService will run without using a cache.", LogLevel.Warning, InfoServiceModul.Feed);
            //    addFeed = new ExFeed(url, ownFeedTitle, defaultZoom, ownFeedImagePath, itemFilters, showPopup);

            //}
            //if (addFeed != null)
            //{
            //    if (ShowPopup) addFeed.OnNewItems += new Feed.OnNewItemsEventHandler(addFeed_OnNewItems);
            //    _feeds.Add(addFeed);
            //    Logger.WriteLog(string.Format("Added new feed (Title: {0}, Default Zoom: {1}, Own Feed Image Path: {2}, Show Popup: {3})", ownFeedTitle, defaultZoom, ownFeedImagePath, showPopup), LogLevel.Info, InfoServiceModul.Feed);
            //}
            //else
            //{
            //    Logger.WriteLog("There were a problem when adding a new feed. This error message should no occur. Please contact the plugin author. This feed is now disabled.", LogLevel.Warning, InfoServiceModul.Feed);
            //}
            ExFeed addFeed = new ExFeed(url, ownFeedTitle, defaultZoom, ownFeedImagePath, CacheFolder, itemFilters, showPopup);
            if (ShowPopup) addFeed.OnNewItems += new Feed.OnNewItemsEventHandler(addFeed_OnNewItems);
            _feeds.Add(addFeed);
            Logger.WriteLog(string.Format("Added new feed (Title: {0}, Default Zoom: {1}, Own Feed Image Path: {2}, Show Popup: {3})", ownFeedTitle, defaultZoom, ownFeedImagePath, showPopup), LogLevel.Info, InfoServiceModul.Feed);
        }

        static void addFeed_OnNewItems(Feed feed, List<FeedItem> newItems)
        {
            newItems.Reverse();
            _newFeedItems.Add(feed, newItems);
        }

        public static void DeleteCache()
        {
            Feed.DeleteCache(); //The whole cache!!
        }
        #endregion

        #region Log Events
        static void LogEvents_OnInfo(FeedArgs feedArguments)
        {
            Logger.WriteLog(feedArguments, LogLevel.Info, InfoServiceModul.Feed);
        }

        static void LogEvents_OnWarning(FeedArgs feedArguments)
        {
            Logger.WriteLog(feedArguments, LogLevel.Warning, InfoServiceModul.Feed);
        }

        static void LogEvents_OnError(FeedArgs feedArguments)
        {
            Logger.WriteLog(feedArguments, LogLevel.Error, InfoServiceModul.Feed);
        }

        static void LogEvents_OnDebug(FeedArgs feedArguments)
        {
            Logger.WriteLog(feedArguments, LogLevel.Debug, InfoServiceModul.Feed);
        }
        #endregion
    }
}
