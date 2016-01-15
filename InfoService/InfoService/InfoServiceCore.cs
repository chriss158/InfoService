#region Usings
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Threading;
using FeedReader.Data;
using InfoService.Feeds;
using InfoService.GUIConfiguration;
using InfoService.GUIWindows;
using InfoService.Settings;
using InfoService.Settings.Data;
using InfoService.Twitter;
using InfoService.Utils;
using InfoService.Weather;
//using InfoService.RecentlyAddedWatched;
using MediaPortal.Configuration;
using MediaPortal.Dialogs;
using MediaPortal.GUI.Library;
using WeatherConnector;
#endregion

namespace InfoService
{
    [PluginIcons("InfoService.GUIConfiguration.images.InfoService_icon_enabled.png", "InfoService.GUIConfiguration.images.InfoService_icon_disabled.png")]
    public sealed class InfoServiceCore : GUIWindow, ISetupForm
    {
        #region Variables
        public const int GUIInfoServiceId = 16000;
        private static readonly Logger Logger = Logger.GetInstance();
        private Timer _developerModetimer;
        private Random _rnd;
        #endregion


        #region Properties
        public static string UserPluginName { get; set; }
        public static bool InitDone { get; private set; }
        #endregion

        #region ISetupForm Members

        public string PluginName()
        {
            return "InfoService";
        }

        public string Description()
        {
            return "InfoService is a plugin for that provides weather, feed and twitter informations on the home screen. It has also a feed reader and a twitter client built in. The plugin is for MediaPortal users and skinners who want to have some information on their MediaPortal homescreen.";
        }

        public string Author()
        {
            return "edsche, SilentException";
        }

        public void ShowPlugin()
        {
            ConfigForm config = new ConfigForm();
            config.ShowDialog();
        }

        public bool CanEnable()
        {
            return true;
        }

       

        public bool GetHome(out string strButtonText, out string strButtonImage, out string strButtonImageFocus, out string strPictureImage)
        {
            strButtonText = UserPluginName;
            strButtonImage = String.Empty;
            strButtonImageFocus = String.Empty;
            strPictureImage = String.Empty;
            return true;
        }

        public bool DefaultEnabled()
        {
            return true;
        }

        public bool HasSetup()
        {
            return true;
        }

        public int GetWindowId()
        {
            return GUIInfoServiceId;
        }

        #endregion

        #region Constructor
        public InfoServiceCore()
        {
#if DEBUG
            AttachDebugger();
#endif
            GetID = GUIInfoServiceId;
            InitDone = false;
        }
        #endregion

        #region Overrides
        public override bool SupportsDelayedLoad
        {
            get
            {
                return true;
            }
        }

        protected override void OnPageLoad()
        {
            /*FieldInfo fi = typeof(GUIWindow).GetField("_loadParameter", BindingFlags.NonPublic | BindingFlags.Instance);
            if (fi != null)
            {
                string loadParam = (string)fi.GetValue(this);
                string[] param = loadParam.Split(new char[] {':'}, 2, StringSplitOptions.None);
                if(param.Length >= 2)
                {
                    if(param[0] == "addFeedUrl")
                    {
                        FeedService.AddFeed(param[1]);
                        base.OnPageLoad();
                        return;
                    }
                }  
            }*/
 
            if (FeedService.Enabled && TwitterService.Enabled)
            {
                PropertyUtils.SetProperty("#currentmodule", "InfoService");
                GUIDialogMenu dlg = (GUIDialogMenu) GUIWindowManager.GetWindow((int) Window.WINDOW_DIALOG_MENU);
                if (dlg != null)
                {
                    dlg.Reset();
                    dlg.SetHeading(InfoServiceUtils.GetLocalizedLabel(28));
                    dlg.Add("Feeds");
                    dlg.Add("Twitter");
                    dlg.DoModal(GUIWindowManager.ActiveWindow);
                    Logger.WriteLog("Opened InfoService from Home menu. Showing InfoService moduls...", LogLevel.Info, InfoServiceModul.InfoService);
                    if (dlg.SelectedId > 0)
                    {
                        switch (dlg.SelectedLabelText)
                        {
                            case "Feeds":
                                Logger.WriteLog("Selected to show the Feeds window", LogLevel.Info, InfoServiceModul.InfoService);
                                GUIWindowManager.ActivateWindow(GUIWindows.GUIFeed.GUIFeedId, true);
                                break;
                            case "Twitter":
                                Logger.WriteLog("Selected to show the Twitter window", LogLevel.Info, InfoServiceModul.InfoService);
                                GUIWindowManager.ActivateWindow(GUIWindows.GUITwitter.GUITwitterId, true);
                                break;
                        }
                    }
                    else
                    {
                        GUIWindowManager.ShowPreviousWindow();
                    }
                }
                else
                {
                    Logger.WriteLog("Error creating GUIDialogMenu Object. Please contact plugin author!", LogLevel.Info, InfoServiceModul.InfoService);
                }
            }
            else
            {
                if (FeedService.Enabled) GUIWindowManager.ActivateWindow(GUIWindows.GUIFeed.GUIFeedId, true);
                else if(TwitterService.Enabled) GUIWindowManager.ActivateWindow(GUIWindows.GUITwitter.GUITwitterId, true);
            }
            base.OnPageLoad();
        }

        public override bool Init()
        {


            Version ver = InfoServiceVersion();
            Logger.WriteLog(string.Format("InfoService ({0}.{1}.{2}.{3})", ver.Major, ver.Minor, ver.Build, ver.Revision), LogLevel.Info, InfoServiceModul.InfoService);
            InfoServiceUtils.InitLog();
            InfoServiceUtils.LoadLocalization();
            PropertyUtils.InitAllProperties();

            #region Default settings

            //if (!System.IO.File.Exists(Config.GetFile(Config.Dir.Config, "InfoService.xml")))
            //{
            //    Logger.WriteLog("InfoService is used the first time. Adding example feed and enabled weather", LogLevel.Debug, InfoServiceModul.InfoService);
            //    SettingsFeed sf = new SettingsFeed();
            //    sf.UrlPath = "http://www.team-mediaportal.com/rss-feeds";
            //    sf.ImagePath = "";
            //    sf.Title = "MediaPortal News";
            //    sf.DefaultZoom = 100;
            //    SettingsManager.Properties.FeedSettings.Feeds.Clear();
            //    SettingsManager.Properties.FeedSettings.Feeds.Add(sf);
            //    SettingsManager.Properties.FeedSettings.Enabled = true;
            //    SettingsManager.Properties.FeedSettings.RandomFeedOnUpdate = false;
            //    SettingsManager.Properties.FeedSettings.RandomFeedOnStartup = false;
            //    SettingsManager.Properties.FeedSettings.DeletionInterval = Enums.DeleteCache.Week;
            //    SettingsManager.Properties.FeedSettings.Separator = "+++";
            //    SettingsManager.Properties.FeedSettings.ItemsCountPerFeed = 3;
            //    SettingsManager.Properties.FeedSettings.ItemsCount = 10;
            //    SettingsManager.Properties.FeedSettings.RefreshInterval = 30;
            //    SettingsManager.Properties.FeedSettings.UpdateOnStartup = true;
            //    SettingsManager.Properties.FeedSettings.ShowItemPublishTime = true;
            //    SettingsManager.Properties.FeedSettings.TickerMask = "%itemtitle%";
            //    SettingsManager.Properties.FeedSettings.StartupFeedIndex = 0;
            //    SettingsManager.Properties.FeedSettings.ItemPublishTimeAllFeeds = ItemPublishTimeAllFeedsType.FeedName;
            //    SettingsManager.Properties.WeatherSettings.Enabled = true;
            //    SettingsManager.Properties.WeatherSettings.RefreshInterval = 60;
            //    SettingsManager.Properties.WeatherSettings.UpdateOnStartup = true;
            //    SettingsManager.Properties.TwitterSettings.ItemsCount = 10;
            //    SettingsManager.Properties.TwitterSettings.RefreshInterval = 45;
            //    SettingsManager.Properties.TwitterSettings.TickerMask = "%message%";
            //    SettingsManager.Properties.TwitterSettings.Separator = "+++";
            //    SettingsManager.Properties.TwitterSettings.TwitterStatusUpdate.WatchMask = "I'm just watching %video% on my MediaPortal HTPC!";
            //    SettingsManager.Properties.GeneralSettings.LogDebug = false;
            //    SettingsManager.Properties.GeneralSettings.LogError = true;
            //    SettingsManager.Properties.GeneralSettings.LogWarning = true;
            //    SettingsManager.Properties.GeneralSettings.PluginName = "InfoService";
            //    SettingsManager.Properties.GeneralSettings.FeedCacheFolder = Config.GetFolder(Config.Dir.Thumbs) + @"\InfoService\Feeds\";
            //    SettingsManager.Properties.GeneralSettings.TwitterCacheFolder = Config.GetFolder(Config.Dir.Thumbs) + @"\InfoService\Twitter\";
            //    SettingsManager.Save(Config.GetFile(Config.Dir.Config, "InfoService.xml"));
            //}

            #endregion

            #region Settings
            Logger.WriteLog("Loading settings from InfoService.xml, skin settings will take priority", LogLevel.Debug, InfoServiceModul.InfoService);
            try
            {
                SettingsManager.Load(Config.GetFile(Config.Dir.Config, "InfoService.xml"));
                Logger.WriteLog("Loading settings from InfoService.xml successfull.", LogLevel.Debug, InfoServiceModul.InfoService);
            }
            catch (Exception ex)
            {
                string logMessage = "Loading settings from InfoService.xml unsuccessfull. Exiting InfoService..." + "\n\t\t\t\t\t\t" + ex.Message + "\n\t\t\t\t\t\t" + ex.StackTrace;
                Logger.WriteLog(logMessage, LogLevel.Error, InfoServiceModul.InfoService);
                Logger.FlushTempLog(Config.GetFile(Config.Dir.Log, "InfoService.log"));
                Logger.CloseLog();
                return false;
            }
            #endregion

            #region SkinSettings
            Logger.WriteLog("Loading skin settings from " + GUIGraphicsContext.Skin + @"\infoservice.xml, skin settings have priority", LogLevel.Debug, InfoServiceModul.SkinSettings);
            InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.loadSkinSettings(GUIGraphicsContext.Skin + @"\infoservice.xml");
            #endregion

            #region Applying Settings
            UserPluginName = SettingsManager.Properties.GeneralSettings.PluginName;
            FeedService.CacheFolder = SettingsManager.Properties.GeneralSettings.FeedCacheFolder;
            TwitterService.CacheFolder = SettingsManager.Properties.GeneralSettings.TwitterCacheFolder;
            #region Feeds
            //if (SettingsManager.Properties.FeedSettings.Enabled)
            if (InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Feeds_Enabled)
            {
                Logger.WriteLog("Init FeedService", LogLevel.Info, InfoServiceModul.InfoService);

                FeedService.SetupFeeds();
                //FeedService.RefreshInterval = SettingsManager.Properties.FeedSettings.RefreshInterval;
                //FeedService.ShowPublishTime = SettingsManager.Properties.FeedSettings.ShowItemPublishTime;
                //FeedService.ItemPublishTimeAllFeeds = SettingsManager.Properties.FeedSettings.ItemPublishTimeAllFeeds;
                //FeedService.Items = SettingsManager.Properties.FeedSettings.ItemsCount;
                //FeedService.ItemsPerFeed = SettingsManager.Properties.FeedSettings.ItemsCountPerFeed;
                //FeedService.TickerMask = SettingsManager.Properties.FeedSettings.TickerMask;
                //FeedService.Separator = SettingsManager.Properties.FeedSettings.Separator;
                FeedService.RefreshInterval = InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Feeds_General_RefreshFeedEvery;
                FeedService.ShowPublishTime = InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Feeds_Layout_ShowFeedItemPublishTime;
                FeedService.ItemPublishTimeAllFeeds = InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Feeds_Layout_ItemPublishTimeAllFeeds;
                FeedService.Items = InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Feeds_Layout_MaxItemsForFeedTicker;
                FeedService.ItemsPerFeed = InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Feeds_Layout_MaxItemsPerFeedForAllFeeds;
                FeedService.TickerMask = InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Feeds_TickerLayout_TickerMask;
                FeedService.TickerAllMask = InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Feeds_TickerLayout_TickerAllMask;
                FeedService.Separator = InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Feeds_TickerLayout_Separator;
                FeedService.SeparatorAll = InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Feeds_TickerLayout_SeparatorAll;
                FeedService.RandomFeedOnStartup = SettingsManager.Properties.FeedSettings.RandomFeedOnStartup;
                FeedService.StartupFeedIndex = SettingsManager.Properties.FeedSettings.StartupFeedIndex;
                FeedService.UpdateOnStartup = SettingsManager.Properties.FeedSettings.UpdateOnStartup;
                FeedService.DeletionInterval = SettingsManager.Properties.FeedSettings.DeletionInterval;
                FeedService.RandomFeedOnUpdate = SettingsManager.Properties.FeedSettings.RandomFeedOnUpdate;
                FeedService.ShowPopup = SettingsManager.Properties.FeedSettings.ShowPopup;
                FeedService.PopupTimeout = SettingsManager.Properties.FeedSettings.PopupTimeout;
                FeedService.PopupWhileFullScreenVideo = SettingsManager.Properties.FeedSettings.PopupWhileFullScreenVideo;
                FeedService.MediaPortalStartup = true;
                FeedUpdater.SetupUpdater();
                Logger.WriteLog("Load feeds...", LogLevel.Info, InfoServiceModul.Feed);
                foreach(SettingsFeed feed in SettingsManager.Properties.FeedSettings.Feeds)
                {
                    List<FeedItemFilter> fFilter = new List<FeedItemFilter>();
                    foreach (SettingsFeedItemsFilter filter in SettingsManager.Properties.FeedItemsFiltersSettings.FeedItemFilters)
                    {
                        if (filter.IsEnabled)
                        {
                            fFilter.Add(new FeedItemFilter(filter.IsRegEx, filter.ReplaceThis,
                                                           filter.ReplaceWith, filter.UseInTitle, filter.UseInBody,
                                                           filter.CleanBefore));
                        }
                    }
                    FeedService.AddFeed(feed.UrlPath, feed.Title, feed.DefaultZoom, feed.ImagePath, fFilter, feed.ShowPopup);
                }
                PropertyUtils.SetProperty(PropertyUtils.Properties.Feed.Separator, FeedService.Separator);
                PropertyUtils.SetProperty(PropertyUtils.Properties.Feed.SeparatorAll, FeedService.SeparatorAll);
                PropertyUtils.SetProperty(PropertyUtils.Properties.Feed.Enabled, FeedService.Enabled.ToString());
                Logger.WriteLog("Successfully initiated FeedService", LogLevel.Info, InfoServiceModul.Feed);

            }
            else
            {
                Logger.WriteLog("FeedService is not enabled", LogLevel.Debug, InfoServiceModul.InfoService);
                PropertyUtils.SetProperty(PropertyUtils.Properties.Feed.Titles, InfoServiceUtils.GetLocalizedLabel(32));
                PropertyUtils.SetProperty(PropertyUtils.Properties.Feed.Alltitles, InfoServiceUtils.GetLocalizedLabel(32));
            }
            #endregion

            #region Weather
            //if (SettingsManager.Properties.WeatherSettings.Enabled)

            //Weather is disabled until a new weather provider is found :/

            //if (InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Weather_Enabled)
            //{
            //    Logger.WriteLog("Init WeatherService", LogLevel.Info, InfoServiceModul.Weather);
            //    using (MediaPortal.Profile.Settings settings = new MediaPortal.Profile.MPSettings())
            //    {
            //        string location = settings.GetValueAsString("weather", "location", string.Empty);


            //        //Location is empty -> That means the user was never in the MP Weather GUI.
            //        if (string.IsNullOrEmpty(location))
            //        {
            //            //Then use the ID from code0

            //            location = settings.GetValueAsString("weather", "code0", string.Empty);
            //            Logger.WriteLog("No location ID found in the location field of mediaportal.xml. Will using the one of code0 -> " + location, LogLevel.Warning, InfoServiceModul.Weather);
            //            settings.SetValue("weather", "location", location);
            //            WeatherService.CurrentLocationId = location;
            //        }

            //        bool success = WeatherService.SetupWeather(location, settings.GetValueAsString("weather", "temperature", "C") == "C"
            //                                                                 ? TemperatureFormat.Grad
            //                                                                 : TemperatureFormat.Fahrenheit,
            //                                                   "1117489152", "93c392356341e017");
            //        if (success)
            //        {
            //            //WeatherService.RefreshInterval = SettingsManager.Properties.WeatherSettings.RefreshInterval;
            //            WeatherService.RefreshInterval = InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Weather_General_RefreshWeatherEvery;
            //            WeatherService.UpdateOnStartup = SettingsManager.Properties.WeatherSettings.UpdateOnStartup;
            //            PropertyUtils.SetProperty(PropertyUtils.Properties.Weather.Enabled, WeatherService.Enabled.ToString());
            //            WeatherUpdater.SetupUpdater();
            //            Logger.WriteLog("Successfully initiated WeatherService", LogLevel.Info, InfoServiceModul.Weather);
            //        }
            //        else
            //        {
            //            Logger.WriteLog("Cannot initialize WeatherService. See above for errors", LogLevel.Error, InfoServiceModul.Weather);
            //        }
            //    }
            //}
            //else Logger.WriteLog("WeatherService is not enabled", LogLevel.Debug, InfoServiceModul.InfoService);
            #endregion

            #region Twitter
            if (InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Twitter_Enabled)
            {
                Logger.WriteLog("Init TwitterService", LogLevel.Info, InfoServiceModul.Twitter);
                bool success = TwitterService.SetupTwitter(SettingsManager.Properties.TwitterSettings.TokenValue,
                                                           SettingsManager.Properties.TwitterSettings.TokenSecret);
                if (success)
                {
                    TwitterService.UpdateOnStartup = SettingsManager.Properties.TwitterSettings.UpdateOnStartup;
                    //TwitterService.RefreshInterval = SettingsManager.Properties.TwitterSettings.RefreshInterval;
                    //TwitterService.TickerMask = SettingsManager.Properties.TwitterSettings.TickerMask;
                    //TwitterService.Separator = SettingsManager.Properties.TwitterSettings.Separator;
                    //TwitterService.Items = SettingsManager.Properties.TwitterSettings.ItemsCount;
                    TwitterService.RefreshInterval = InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Twitter_General_RefreshTwitterEvery;
                    TwitterService.TickerMask = InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Twitter_TickerLayout_TickerMask;
                    TwitterService.Separator = InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Twitter_TickerLayout_Separator;
                    TwitterService.Items = InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Twitter_Layout_MaxItemsForTwitterTicker;
                    TwitterService.UseHomeTimeline = SettingsManager.Properties.TwitterSettings.UsedTimelines.HomeTimeline;
                    //TwitterService.UsePublicTimeline = SettingsManager.Properties.TwitterSettings.UsedTimelines.PublicTimeline;
                    TwitterService.UseUserTimeline = SettingsManager.Properties.TwitterSettings.UsedTimelines.UserTimeline;
                    //TwitterService.UseFriendsTimeline = SettingsManager.Properties.TwitterSettings.UsedTimelines.FriendsTimeline;
                    TwitterService.UseMentionsTimeline = SettingsManager.Properties.TwitterSettings.UsedTimelines.MentionsTimeline;
                    //TwitterService.UseRetweetedByMeTimeline = SettingsManager.Properties.TwitterSettings.UsedTimelines.RetweetedByMeTimeline;
                    //TwitterService.UseRetweetedToMeTimeline = SettingsManager.Properties.TwitterSettings.UsedTimelines.RetweetedToMeTimeline;
                    TwitterService.UseRetweetsOfMeTimeline = SettingsManager.Properties.TwitterSettings.UsedTimelines.RetweetsOfMeTimeline;
                    TwitterService.DeletionInterval = SettingsManager.Properties.TwitterSettings.DeletionInterval;
                    TwitterService.PostWatchingVideos = SettingsManager.Properties.TwitterSettings.TwitterStatusUpdate.Enabled;
                    TwitterService.PostWatchMoviesMask = SettingsManager.Properties.TwitterSettings.TwitterStatusUpdate.WatchMoviesMask;
                    TwitterService.PostWatchSeriesMask = SettingsManager.Properties.TwitterSettings.TwitterStatusUpdate.WatchSeriesMask;
                    TwitterService.PostUsingMovingPictures = SettingsManager.Properties.TwitterSettings.TwitterStatusUpdate.WithMovingPictures;
                    TwitterService.PostUsingTVSeries = SettingsManager.Properties.TwitterSettings.TwitterStatusUpdate.WithMPTVSeries;
                    TwitterService.PostUsingMyVideos = SettingsManager.Properties.TwitterSettings.TwitterStatusUpdate.WithMyVideo;
                    TwitterService.ShowPopup = SettingsManager.Properties.TwitterSettings.ShowPopup;
                    TwitterService.PopupTimeout = SettingsManager.Properties.TwitterSettings.PopupTimeout;
                    TwitterService.PopupWhileFullScreenVideo = SettingsManager.Properties.TwitterSettings.PopupWhileFullScreenVideo;
                    PropertyUtils.SetProperty(PropertyUtils.Properties.Twitter.Separator, TwitterService.Separator);
                    PropertyUtils.SetProperty(PropertyUtils.Properties.Twitter.Enabled, TwitterService.Enabled.ToString());
                    TwitterUpdater.SetupUpdater();
                    Logger.WriteLog("Successfully initiated TwitterService", LogLevel.Info, InfoServiceModul.Twitter);
                }
                else
                {
                    Logger.WriteLog("Cannot initialize TwitterService. See above for errors", LogLevel.Error, InfoServiceModul.Twitter);
                }
            }
            else
            {
                Logger.WriteLog("TwitterService is not enabled", LogLevel.Debug, InfoServiceModul.InfoService);
                PropertyUtils.SetProperty(PropertyUtils.Properties.Twitter.Messages, InfoServiceUtils.GetLocalizedLabel(33));
            }
            #endregion

            #endregion

            // after the settings have been loaded, try to initialize log one more time, it should flush the log buffer
            InfoServiceUtils.InitLog();
            Logger.FlushTempLog();

            //RecentlyAddedUtils.LoadLastRecentlyAddedItems();
            //RecentlyAddedWatchedUpdater.SetupUpdater();
            UpdateTimer.SetTimer(false);

            #region Developer mode

            if (SettingsManager.Properties.GeneralSettings.DeveloperMode)
            {
                Logger.WriteLog("Enabled InfoService developer mode. Showing NotifyBar Popup every 30 seconds.", LogLevel.Debug, InfoServiceModul.InfoService);
                _rnd = new Random();
                _developerModetimer = new Timer(state =>
                {
                    InfoServiceUtils.ShowDialogNotifyWindow(InfoServiceUtils.GenerateLoremIpsum(4, 10, 1, 1, 1), InfoServiceUtils.GenerateLoremIpsum(20, 40, 1, 4, 1), GUIGraphicsContext.Skin + @"\media\InfoService\defaultFeedALL.png", new Size(120, 120), 20,
                    //InfoServiceUtils.ShowDialogNotifyWindow(FeedService.Feeds[3].Items[0].Title + " (" + FeedService.Feeds[3].Title + ")", FeedService.Feeds[3].Items[0].Description, FeedService.Feeds[3].ImagePath, new Size(120, 120), 20,
                    () =>
                        {
                            if (FeedService.Enabled)
                            {
                                if (GUIGraphicsContext.IsFullScreenVideo)
                                {
                                    GUIWindowManager.ShowPreviousWindow();
                                }

                                bool gotoFeed = _rnd.NextDouble() >= 0.5;

                                if (gotoFeed)
                                {
                                    int randomFeedIndex = _rnd.Next(0, FeedService.Feeds.Count - 1);
                                    GUIWindowManager.ActivateWindow(GUIFeed.GUIFeedId,
                                        string.Format("feedIndex:\"{0}\",feedItemIndex:\"{1}\"", randomFeedIndex, 0));

                                }
                                else
                                {
                                    int randomTwitterIndex = _rnd.Next(0, TwitterService.UsedTimelines.Count - 1);
                                    GUIWindowManager.ActivateWindow(GUITwitter.GUITwitterId,
                                                string.Format("twitterTimeline:\"{0}\",twitterItemIndex:\"{1}\"", TwitterService.UsedTimelines[randomTwitterIndex], 0));
                                }
                            }
                        });

                }, null, 10000, 25000);
            }
            #endregion



            #region Load infoservice.xml
            Logger.WriteLog("Loading InfoService GUI skin file from " + GUIGraphicsContext.Skin + @"\infoservice.xml", LogLevel.Debug, InfoServiceModul.InfoService);
            bool loadedSkinSuccess = Load(GUIGraphicsContext.Skin + @"\infoservice.xml");
            if (loadedSkinSuccess) Logger.WriteLog("Loaded InfoService GUI skin file successful", LogLevel.Debug, InfoServiceModul.InfoService);
            else
            {
                if (!System.IO.File.Exists(GUIGraphicsContext.Skin + @"\infoservice.xml"))
                {
                    Logger.WriteLog("Loading Twitter GUI skin file failed. Skin file " + GUIGraphicsContext.Skin + @"\infoservice.xml" + " doesn't exist.", LogLevel.Debug, InfoServiceModul.InfoService);
                }
                else
                {
                    Logger.WriteLog("Loading InfoService GUI skin file failed. Unknown error.", LogLevel.Debug, InfoServiceModul.InfoService);
                }
            }
            InitDone = true;
            return loadedSkinSuccess;
            #endregion
        }

         private static void AttachDebugger()
         {
             if (!Debugger.IsAttached)
             {
                 Process[] vsProcesses = System.Diagnostics.Process.GetProcessesByName("devenv");
                 if (vsProcesses.Length > 0)
                 {
                     // do a search for any Visual Studio processes that also have our solution currently open
                     var vsProcess = DebuggerAttacher.GetVisualStudioForSolutions(new List<string>() {"InfoService.sln"});
                     bool attached = false;
                     if (vsProcess != null)
                     {
                         Process[] mpProcesses = System.Diagnostics.Process.GetProcessesByName("MediaPortal");
                         if (mpProcesses.Length > 0)
                         {
                             DebuggerAttacher.AttachVisualStudioToProcess(vsProcesses[0], mpProcesses[0]);
                             attached = true;
                         }
                     }
                     if (!attached)
                     {
                         // try and attach the old fashioned way
                         Debugger.Launch();
                     }
                 }
             }
         }

        public override void DeInit()
        {
            //RecentlyAddedUtils.SaveLastRecentlyAddedItems();

            Logger.WriteLog("Saving settings to InfoService.xml...", LogLevel.Debug, InfoServiceModul.InfoService);
            try
            {
                SettingsManager.Save(Config.GetFile(Config.Dir.Config, "InfoService.xml"));
                Logger.WriteLog("Saving settings to InfoService.xml successfull.", LogLevel.Debug, InfoServiceModul.InfoService);
            }
            catch (Exception ex)
            {
                string logMessage = "Saving settings to InfoService.xml unsuccessfull..." + "\n\t\t\t\t\t\t" + ex.Message + "\n\t\t\t\t\t\t" + ex.StackTrace;
                Logger.WriteLog(logMessage, LogLevel.Error, InfoServiceModul.InfoService);
            }
            Logger.CloseLog();
            InitDone = false;
            base.DeInit();
        }
        #endregion

        #region Private methods
        private Version InfoServiceVersion()
        {
            Version ver = new Version(0, 0, 0, 0);
            try
            {
                ver = Assembly.GetExecutingAssembly().GetName().Version;
            }
            catch
            {
            }
            return ver;
        }

        #endregion
    }
}


