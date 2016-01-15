#region Usings

using System;
using System.Linq;
using InfoService.Feeds;
using InfoService.Utils;
using MediaPortal.Dialogs;
using MediaPortal.GUI.Library;
using Action = MediaPortal.GUI.Library.Action;

#endregion

namespace InfoService.GUIWindows
{
    public sealed class GUIFeed : GUIWindow
    {
        private static readonly Logger logger = Logger.GetInstance();

        #region SkinControls

        [SkinControlAttribute(4)]
#pragma warning disable 649
        private GUIButtonControl _changeFeed;
#pragma warning restore 649

        [SkinControlAttribute(50)]
#pragma warning disable 649
        private GUIListControl _feedListcontrol;
#pragma warning restore 649

        [SkinControlAttribute(2)]
#pragma warning disable 649
        private GUIButtonControl _refreshFeeds;
#pragma warning restore 649

        [SkinControlAttribute(5)]
#pragma warning disable 649
        private GUIButtonControl _setAllFeeds;
#pragma warning restore 649

        #endregion

        #region Constants
        public const int GUIFeedDescription = 506;
        public const int GUIFeedList = 50;
        public const int GUIFeedId = 16001;
        #endregion

        #region Constructor
        public GUIFeed()
        {
            GetID = GUIFeedId;
        }
        #endregion

        #region Overrides
        public override bool SupportsDelayedLoad
        {
            get
            {
                return false;
            }
        }

        public override bool Init()
        {
            //InfoServiceUtils.InitLog();
            logger.WriteLog("Init Feed GUI", LogLevel.Debug, InfoServiceModul.Feed);
            logger.WriteLog("Loading Feed GUI skin file from " + GUIGraphicsContext.Skin + @"\infoservice.feeds.xml", LogLevel.Debug, InfoServiceModul.Feed);
            bool success = Load(GUIGraphicsContext.Skin + @"\infoservice.feeds.xml");
            if (success) logger.WriteLog("Loaded Feed GUI skin file successful", LogLevel.Debug, InfoServiceModul.Feed);
            else
            {
                if(!System.IO.File.Exists(GUIGraphicsContext.Skin + @"\infoservice.feeds.xml"))
                {

                    logger.WriteLog("Loading Feed GUI skin file failed. Skin file " + GUIGraphicsContext.Skin + @"\infoservice.feeds.xml" + " doesn't exist.", LogLevel.Debug, InfoServiceModul.Feed);
                }
                else
                {
                    logger.WriteLog("Loading Feed GUI skin file failed. Unknown error.", LogLevel.Debug, InfoServiceModul.Feed);
                }
            }
            DownloadWorker.StartDownload += DownloadWorker_StartDownload;
            return success;
        }

        public override void DeInit()
        {
            Logger.CloseLog();
            base.DeInit();
        }

        void DownloadWorker_StartDownload(bool updateWeather, bool updateTwitter, bool updateFeeds)
        {
            if (FeedService.Enabled)
            {
                if (updateFeeds)
                {
                    using (new WaitCursor())
                    {
                        if (FeedService.Feeds.Count >= 1) FeedUpdater.UpdateFeedDataSync();
                    }
                }
            }
        }

        protected override void OnPageLoad()
        {
            PropertyUtils.SetProperty("#currentmodule", InfoServiceCore.UserPluginName + " - Feeds");
            GUIControl.SetControlLabel(GetID, 2, InfoServiceUtils.GetLocalizedLabel(0));
            GUIControl.SetControlLabel(GetID, 4, InfoServiceUtils.GetLocalizedLabel(1));
            GUIControl.SetControlLabel(GetID, 5, InfoServiceUtils.GetLocalizedLabel(2));
            if (string.IsNullOrEmpty(_loadParameter))
            {
                logger.WriteLog("Load Feed GUI", LogLevel.Info, InfoServiceModul.Feed);
                FeedUtils.SetFeedOnWindow(FeedService.ActiveFeedIndex, true, true);
            }
            else
            {
                logger.WriteLog("Load Feed GUI with params \"" + _loadParameter + "\"", LogLevel.Info, InfoServiceModul.Feed);
                LoadParameterParser parser = new LoadParameterParser(parameter);
                
                int feedIndex = -1;
                int feedItemIndex = 0;
                
                parser.Parse();
                foreach(LoadParameter parameter in parser.GetAllParameters())
                {
                    switch(parameter.ParameterName)
                    {
                        case "feedIndex":
                            feedIndex = parameter.ParameterSetting.ParseSetting<int>();
                            logger.WriteLog("Parsed load parameter \"" + parameter.ParameterName + "\" with value \"" + feedIndex + "\"", LogLevel.Debug, InfoServiceModul.Feed);
                            break;
                        case "feedTitle":
                            string feedTitle = parameter.ParameterSetting.ParseSetting<string>();
                            logger.WriteLog("Parsed load parameter \"" + parameter.ParameterName + "\" with value \"" + feedTitle + "\"", LogLevel.Debug, InfoServiceModul.Feed);
                            feedIndex = FeedService.GetIndexFromFeeds(feedTitle);
                            logger.WriteLog("Converted feed title \"" + feedTitle + "\" to index \"" + feedIndex + "\"", LogLevel.Debug, InfoServiceModul.Feed);
                            break;
                        case "feedGuid":
                            Guid feedGuid = parameter.ParameterSetting.ParseSetting<Guid>();
                            logger.WriteLog("Parsed load parameter \"" + parameter.ParameterName + "\" with value \"" + feedGuid.ToString() + "\"", LogLevel.Debug, InfoServiceModul.Feed);
                            feedIndex = FeedService.GetIndexFromFeeds(feedGuid);
                            logger.WriteLog("Converted feed guid \"" + feedGuid.ToString() + "\" to index \"" + feedIndex + "\"", LogLevel.Debug, InfoServiceModul.Feed);
                            break;
                        case "feedItemIndex":
                            feedItemIndex = parameter.ParameterSetting.ParseSetting<int>();
                            logger.WriteLog("Parsed load parameter \"" + parameter.ParameterName + "\" with value \"" + feedItemIndex + "\"", LogLevel.Debug, InfoServiceModul.Feed);
                            break;
                        default:
                            logger.WriteLog("Unknown parameter \"" + parameter.ParameterName + ". Parameter will be skipped", LogLevel.Warning, InfoServiceModul.Feed);
                            break;
                    }
                }   
                if(feedIndex < 0)
                {
                    logger.WriteLog("Parameter feedIndex out of bounds. Defaulting parameter to 0", LogLevel.Warning, InfoServiceModul.Feed);
                    feedIndex = FeedService.ActiveFeedIndex;
                }
                FeedService.SetActive(feedIndex);
                
                if(feedItemIndex < 0 || feedItemIndex >= FeedService.Feeds[FeedService.ActiveFeedIndex].Count)
                {
                    logger.WriteLog("Parameter feedItemIndex out of bounds. Defaulting parameter to 0", LogLevel.Warning, InfoServiceModul.Feed);
                    feedItemIndex = 0;
                }

                logger.WriteLog("Open Feed GUI with feed index \"" + feedIndex + "\" and feed item index \"" + feedItemIndex + "\"", LogLevel.Info, InfoServiceModul.Feed);
                FeedUtils.SetFeedOnWindow(FeedService.ActiveFeedIndex, feedItemIndex, true);
            }
            base.OnPageLoad();
        }

        protected override void OnClicked(int controlId, GUIControl control, Action.ActionType actionType)
        {
            if (control == _changeFeed)
            {
                if (!FeedService.UpdateInProgress)
                {
                    GUIDialogMenu dlg = (GUIDialogMenu) GUIWindowManager.GetWindow((int) Window.WINDOW_DIALOG_MENU);
                    if (dlg != null)
                    {
                        dlg.Reset();
                        dlg.SetHeading("Feeds");
                        int selectedIndex = 0;
                        int setSelectedIndex = 0;
                        foreach (ExFeed t in FeedService.Feeds)
                        {
                            if (!string.IsNullOrEmpty(t.Title))
                            {
                                GUIListItem item = new GUIListItem
                                                       {
                                                           IconImage = t.ImagePath,
                                                           Label = t.Title
                                                       };
                                if (t.Active) setSelectedIndex = selectedIndex;
                                selectedIndex++;
                                dlg.Add(item);
                            }
                        }
                        dlg.SelectedLabel = setSelectedIndex;
                        dlg.DoModal(GUIWindowManager.ActiveWindow);

                        logger.WriteLog("Show Feeds dialog menu", LogLevel.Info, InfoServiceModul.Feed);
                        if (dlg.SelectedId > 0)
                        {
                            for (int i = 0; i < FeedService.Feeds.Count; i++)
                            {
                                if (FeedService.Feeds[i].Title != dlg.SelectedLabelText) continue;
                                if (FeedService.Feeds[i].LastUpdateSuccessful)
                                {
                                    FeedUtils.SetFeedOnBasichome(i);
                                    FeedUtils.SetFeedOnWindow(i, false, true);
                                }
                                else
                                {
                                    if (FeedService.Feeds[i].Items == null || FeedService.Feeds[i].Items.Count == 0)
                                    {
                                        InfoServiceUtils.ShowDialogOkWindow(FeedService.Feeds[i].Title, InfoServiceUtils.GetLocalizedLabel(19));
                                    }
                                    else InfoServiceUtils.ShowDialogOkWindow(FeedService.Feeds[i].Title, InfoServiceUtils.GetLocalizedLabel(18));  
                                }
                            }
                        }

                    }
                    else
                    {
                        logger.WriteLog("Error creating GUIDialogMenu Object. Please contact plugin author!", LogLevel.Error, InfoServiceModul.InfoService);
                    }
                }
                else
                {
                    InfoServiceUtils.ShowDialogOkWindow(InfoServiceUtils.GetLocalizedLabel(8), InfoServiceUtils.GetLocalizedLabel(17));
                    logger.WriteLog("Feed update in progress. Cannot show Feed select menu", LogLevel.Warning, InfoServiceModul.Feed);
                }
            }
            else if (control == _refreshFeeds)
            {
                logger.WriteLog("User started manual feed update...", LogLevel.Debug, InfoServiceModul.Feed);
                if(!FeedService.UpdateInProgress) DownloadWorker.StartDownloadWorker(false, false, true);
                else
                {
                    InfoServiceUtils.ShowDialogOkWindow(InfoServiceUtils.GetLocalizedLabel(8), InfoServiceUtils.GetLocalizedLabel(17));
                    logger.WriteLog("Feed update in progress. Cannot refresh feeds", LogLevel.Warning, InfoServiceModul.Feed);
                }
            }
            else if (control == _setAllFeeds)
            {
                if (!FeedService.UpdateInProgress)
                {
                    FeedUtils.SetAllFeedsOnBasichome();
                    GUIWindowManager.ShowPreviousWindow();
                }
                else
                {
                    InfoServiceUtils.ShowDialogOkWindow(InfoServiceUtils.GetLocalizedLabel(8), InfoServiceUtils.GetLocalizedLabel(17));
                    logger.WriteLog("Feed update in progress. Cannot set all feeds on home", LogLevel.Warning, InfoServiceModul.Feed);
                }
            }

            if (control == _feedListcontrol && actionType == Action.ActionType.ACTION_SELECT_ITEM) // some other events raise onClicked too for some reason
            {
                string zoomlevel = string.Empty;
                if (FeedService.Feeds[FeedService.ActiveFeedIndex].IsAllFeed)
                    zoomlevel = FeedService.Feeds[FeedService.ActiveFeedIndex].Items[_feedListcontrol.SelectedListItemIndex].SourceDefaultZoom.ToString();
                else
                    zoomlevel = FeedService.Feeds[FeedService.ActiveFeedIndex].DefaultZoom.ToString();

                int webBrowserWindowID = InfoServiceUtils.GetWebBrowserWindowId(FeedService.Feeds[FeedService.ActiveFeedIndex].Items[_feedListcontrol.SelectedListItemIndex].Url, zoomlevel);
                if (webBrowserWindowID > 0)
                {
                    logger.WriteLog(string.Format("Trying to open web browser with window ID {0}, url {1} and zoom {2}", webBrowserWindowID, FeedService.Feeds[FeedService.ActiveFeedIndex].Items[_feedListcontrol.SelectedListItemIndex].Url, zoomlevel), LogLevel.Info, InfoServiceModul.Feed);
                    GUIWindowManager.ActivateWindow(webBrowserWindowID, false);
                }
            }
            base.OnClicked(controlId, control, actionType);
        }

        public override bool OnMessage(GUIMessage message)
        {
            switch (message.Message)
            {
                case GUIMessage.MessageType.GUI_MSG_ITEM_FOCUS_CHANGED:
                    {
                        int iControl = message.SenderControlId;
                        if (iControl == GUIFeedList)
                        {
                            if (_feedListcontrol != null)
                            {
                                FeedUtils.lastSelectedItem = _feedListcontrol.SelectedListItemIndex;
                            }

                            GUIListItem item = GUIControl.GetSelectedListItem(GetID, GUIFeedList);
                            if (item != null)
                            {
                                foreach (ExFeed feed in FeedService.Feeds.Where(feed => feed.Active))
                                {
                                    PropertyUtils.SetProperty(PropertyUtils.Properties.Feed.SelectedTitle, feed.Items[_feedListcontrol.SelectedListItemIndex].Title);
                                    PropertyUtils.SetProperty(PropertyUtils.Properties.Feed.SelectedSourcefeed, feed.Items[_feedListcontrol.SelectedListItemIndex].SourceTitle);
                                    PropertyUtils.SetProperty(PropertyUtils.Properties.Feed.SelectedIndex, _feedListcontrol.SelectedListItemIndex.ToString());
                                    PropertyUtils.SetProperty(PropertyUtils.Properties.Feed.SelectedItemimg, feed.Items[_feedListcontrol.SelectedListItemIndex].ImagePathBig);
                                    PropertyUtils.SetProperty(PropertyUtils.Properties.Feed.SelectedItemage, InfoServiceUtils.GetTimeDifferenceToNow(feed.Items[_feedListcontrol.SelectedListItemIndex].PublishDate));
                                    PropertyUtils.SetProperty(PropertyUtils.Properties.Feed.SelectedDescription, feed.Items[_feedListcontrol.SelectedListItemIndex].Description);
                                    GUIControl.SetControlLabel(GetID, GUIFeedDescription, feed.Items[_feedListcontrol.SelectedListItemIndex].Description);
                                    break;
                                }
                            }
                        }
                        break;
                    }
            }
            return base.OnMessage(message);
        }
        #endregion
    }
}
