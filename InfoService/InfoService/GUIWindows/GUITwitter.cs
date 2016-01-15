#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using InfoService.Feeds;
using InfoService.Twitter;
using InfoService.Utils;
using InfoService.Utils.LoadParameterParsing;
using InfoService.Utils.LoadParameterParsing.Data;
using MediaPortal.Dialogs;
using MediaPortal.GUI.Library;
using TwitterConnector;
using Action = MediaPortal.GUI.Library.Action;
#endregion

namespace InfoService.GUIWindows
{
    public sealed class GUITwitter : GUIWindow
    {
        private static readonly Logger logger = Logger.GetInstance();

        #region SkinControls

        [SkinControlAttribute(4)]
#pragma warning disable 649
        private GUIButtonControl _changeTimeline;
#pragma warning restore 649

        [SkinControlAttribute(2)]
#pragma warning disable 649
        private GUIButtonControl _refreshTwitter;
#pragma warning restore 649

        [SkinControlAttribute(50)]
#pragma warning disable 649
        private GUIListControl _twitterListcontrol;
#pragma warning restore 649

        [SkinControlAttribute(5)]
#pragma warning disable 649
        private GUIButtonControl _updateStatus;
#pragma warning restore 649

        #endregion

        #region Constants
        public const int GUITwitterMessage = 506;
        public const int GUITwitterList = 50;
        public const int GUITwitterId = 16003;
        #endregion

        #region Constructor
        public GUITwitter()
        {
            GetID = GUITwitterId;
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
            logger.WriteLog("Init Twitter GUI", LogLevel.Debug, InfoServiceModul.Twitter);
            logger.WriteLog("Loading Twitter GUI skin file from " + GUIGraphicsContext.Skin + @"\infoservice.twitter.xml", LogLevel.Debug, InfoServiceModul.Twitter);
            bool success = Load(GUIGraphicsContext.Skin + @"\infoservice.twitter.xml");
            if (success) logger.WriteLog("Loaded Twitter GUI skin file successful", LogLevel.Debug, InfoServiceModul.Twitter);
            else
            {
                if (!System.IO.File.Exists(GUIGraphicsContext.Skin + @"\infoservice.twitter.xml"))
                {

                    logger.WriteLog("Loading Twitter GUI skin file failed. Skin file " + GUIGraphicsContext.Skin + @"\infoservice.twitter.xml" + " doesn't exist.", LogLevel.Debug, InfoServiceModul.Twitter);
                }
                else
                {
                    logger.WriteLog("Loading Twitter GUI skin file failed. Unknown error.", LogLevel.Debug, InfoServiceModul.Twitter);
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
            if (TwitterService.Enabled)
            {
                if (!updateTwitter) return;
                using (new WaitCursor())
                {
                    TwitterUpdater.UpdateTwitterDataSync();
                }
            }
        }

        protected override void OnPageLoad()
        {
            PropertyUtils.SetProperty("#currentmodule", InfoServiceCore.UserPluginName + " - Twitter");
            GUIControl.SetControlLabel(GetID, 2, InfoServiceUtils.GetLocalizedLabel(0));
            GUIControl.SetControlLabel(GetID, 4, InfoServiceUtils.GetLocalizedLabel(3));
            GUIControl.SetControlLabel(GetID, 5, InfoServiceUtils.GetLocalizedLabel(4));
            if (string.IsNullOrEmpty(_loadParameter))
            {
                logger.WriteLog("Load Twitter GUI", LogLevel.Info, InfoServiceModul.Twitter);
                TwitterUtils.SetTimelineOnWindow(TwitterService.GetTimeline(TwitterService.ActiveTimeline), true, true);
                TwitterUtils.SetTimelineOnBasichome(TwitterService.GetTimeline(TwitterService.ActiveTimeline));
            }
            else
            {
                logger.WriteLog("Load Twitter GUI with params \"" + _loadParameter + "\"", LogLevel.Info, InfoServiceModul.Twitter);
                LoadParameterParser parser = new LoadParameterParser(_loadParameter);

                TimelineType twitterTimelineType = TimelineType.None;
                string timelineName = string.Empty;
                string twitterItemId = string.Empty;
                int twitterItemIndex = 0;

                parser.Parse();
                LoadParameters parameters = parser.GetAllParameters();
                foreach (LoadParameter parameter in parser.GetAllParameters())
                {
                    switch (parameter.ParameterName)
                    {
                        case "twitterTimeline":
                            timelineName = parameter.ParameterSetting.ParseSetting<string>();
                            logger.WriteLog("Parsed load parameter \"" + parameter.ParameterName + "\" with value \"" + timelineName + "\"", LogLevel.Debug, InfoServiceModul.Twitter);
                            break;
                        case "twitterItemIndex":
                            twitterItemIndex = parameter.ParameterSetting.ParseSetting<int>();
                            logger.WriteLog("Parsed load parameter \"" + parameter.ParameterName + "\" with value \"" + twitterItemIndex + "\"", LogLevel.Debug, InfoServiceModul.Twitter);
                            break;
                        case "twitterItemId":
                            twitterItemId = parameter.ParameterSetting.ParseSetting<string>();
                            logger.WriteLog("Parsed load parameter \"" + parameter.ParameterName + "\" with value \"" + twitterItemId + "\"", LogLevel.Debug, InfoServiceModul.Twitter);
                            break;
                        default:
                            logger.WriteLog("Unknown parameter \"" + parameter.ParameterName + ". Parameter will be skipped", LogLevel.Warning, InfoServiceModul.Twitter);
                            break;
                    }
                }
                if (parameters.Count <= 0)
                {
                    logger.WriteLog("None of the parameters could be parsed. Defaulting parameters \"twitterTimeline\" and \"twitterItemIndex\"", LogLevel.Warning, InfoServiceModul.Twitter);
                }

                if (!TimelineType.TryParse(timelineName, out twitterTimelineType))
                { 
                    logger.WriteLog("Parameter \"twitterTimeline\" is invalid. Defaulting parameter to \"" + TwitterService.ActiveTimeline + "\"", LogLevel.Warning, InfoServiceModul.Twitter);
                    twitterTimelineType = TwitterService.ActiveTimeline;
                }
                else
                {
                    if (twitterTimelineType == TimelineType.None)
                    {
                        logger.WriteLog("Parameter \"twitterTimeline\" is invalid. Defaulting parameter to \"" + TwitterService.ActiveTimeline + "\"", LogLevel.Warning, InfoServiceModul.Twitter);
                        twitterTimelineType = TwitterService.ActiveTimeline;
                    }
                }


                if (!string.IsNullOrEmpty(twitterItemId) && twitterTimelineType != TimelineType.None)
                {
                    twitterItemIndex = TwitterService.GetItemIndexFromTimeline(twitterTimelineType, twitterItemId);
                    logger.WriteLog("Converted twitter item id \"" + twitterItemId + "\" to index \"" + twitterItemIndex + "\"", LogLevel.Debug, InfoServiceModul.Twitter);
                }

                if (twitterItemIndex < 0 ||
                    twitterItemIndex >= TwitterService.GetTimeline(twitterTimelineType).Items.Count)
                {
                    logger.WriteLog("Parameter \"twitterItemIndex\" is invalid. Defaulting parameter to 0", LogLevel.Warning, InfoServiceModul.Twitter);
                    twitterItemIndex = 0;
                }

    
                logger.WriteLog("Open Twitter GUI with Timeline \"" + twitterTimelineType + "\" and index \"" + twitterItemIndex + "\"", LogLevel.Info, InfoServiceModul.Twitter);
                TwitterUtils.SetTimelineOnWindow(TwitterService.GetTimeline(twitterTimelineType), twitterItemIndex, true);
                TwitterUtils.SetTimelineOnBasichome(TwitterService.GetTimeline(twitterTimelineType));
    
            }

            base.OnPageLoad();
        }

        protected override void OnClicked(int controlId, GUIControl control, Action.ActionType actionType)
        {
            if (control == _changeTimeline)
            {
                if (!TwitterService.UpdateInProgress)
                {
                    GUIDialogMenu dlg = (GUIDialogMenu) GUIWindowManager.GetWindow((int) Window.WINDOW_DIALOG_MENU);
                    if (dlg != null)
                    {
                        dlg.Reset();
                        dlg.SetHeading("Twitter timelines");
                        int count = 0;
                        int selectedIndex = 0;
                        foreach (TimelineType usedTimeline in TwitterService.UsedTimelines)
                        {
                            if(TwitterService.ActiveTimeline == usedTimeline) selectedIndex = count;
                            GUIListItem item = new GUIListItem { Label = usedTimeline.GetPrettyName() };
                            dlg.Add(item);
                            count++;
                        }
                      
                        dlg.SelectedLabel = selectedIndex;

                        dlg.DoModal(GUIWindowManager.ActiveWindow);

                        logger.WriteLog("Show Twitter dialog menu", LogLevel.Info, InfoServiceModul.InfoService);
                        if (dlg.SelectedId > 0)
                        {
                            bool downloadFailed = false;
                            Timeline timeline = new Timeline();
                            foreach (TimelineType usedTimeline in TwitterService.UsedTimelines)
                            {
                                if(dlg.SelectedLabelText == usedTimeline.GetPrettyName())
                                {
                                    timeline = TwitterService.GetTimeline(usedTimeline);
                                    if (!timeline.LastUpdateSuccessful)
                                    {
                                        if (timeline.Items == null || timeline.Items.Count == 0)
                                        {
                                            InfoServiceUtils.ShowDialogOkWindow(usedTimeline.GetPrettyName(), InfoServiceUtils.GetLocalizedLabel(6));
                                            downloadFailed = true;
                                        }
                                        else InfoServiceUtils.ShowDialogOkWindow(usedTimeline.GetPrettyName(), InfoServiceUtils.GetLocalizedLabel(5));
                                    }
                                    break;
                                }
                            }
                            
                            if (!downloadFailed)
                            {
                                TwitterUtils.SetTimelineOnWindow(timeline, false, true);
                                TwitterUtils.SetTimelineOnBasichome(timeline);
                            }
                        }
                    }
                    else
                    {
                        logger.WriteLog("Error creating GUIDialogMenu Object. Please contact plugin author!",
                                        LogLevel.Error, InfoServiceModul.InfoService);
                    }
                }
                else
                {
                    InfoServiceUtils.ShowDialogOkWindow(InfoServiceUtils.GetLocalizedLabel(8), InfoServiceUtils.GetLocalizedLabel(7));
                    logger.WriteLog("Twitter update in progress. Cannot show Twitter timeline select menu", LogLevel.Warning, InfoServiceModul.Twitter);
                }
            }
            else if (control == _updateStatus)
            {
                string status = string.Empty;
                if (InfoServiceUtils.ShowKeyboard(GUITwitterId, ref status))
                {
                    if(!string.IsNullOrEmpty(status))
                    {
                        if (TwitterService.PostStatus(status))
                        {
                            InfoServiceUtils.ShowDialogOkWindow(InfoServiceUtils.GetLocalizedLabel(10), InfoServiceUtils.GetLocalizedLabel(9)); 
                        }
                        else InfoServiceUtils.ShowDialogOkWindow(InfoServiceUtils.GetLocalizedLabel(11), InfoServiceUtils.GetLocalizedLabel(12)); 
                    }
                }
            }
            else if (control == _refreshTwitter)
            {
                logger.WriteLog("User started manual twitter update...", LogLevel.Debug, InfoServiceModul.Twitter);
                if(!TwitterService.UpdateInProgress) DownloadWorker.StartDownloadWorker(false, true, false);
                else
                {
                    InfoServiceUtils.ShowDialogOkWindow(InfoServiceUtils.GetLocalizedLabel(8), InfoServiceUtils.GetLocalizedLabel(7));
                    logger.WriteLog("Twitter update in progress. Cannot refresh Twitter timelines", LogLevel.Warning, InfoServiceModul.Twitter);
                }
            }
            if (control == _twitterListcontrol && actionType == Action.ActionType.ACTION_SELECT_ITEM) // some other events raise onClicked too for some reason
            {
                GUIDialogMenu dlg = (GUIDialogMenu)GUIWindowManager.GetWindow((int)Window.WINDOW_DIALOG_MENU);
                if (dlg != null)
                {
                    dlg.Reset();
                    dlg.SetHeading(InfoServiceUtils.GetLocalizedLabel(27));
                    List<string> urls = TwitterUtils.ParseUrls(TwitterService.GetTimeline(TwitterService.ActiveTimeline).Items[_twitterListcontrol.SelectedListItemIndex].Text);
                    if (urls.Count > 0)
                    {
                        foreach (string url in urls)
                        {
                            GUIListItem item = new GUIListItem { Label = url };
                            dlg.Add(item);
                        }
                        dlg.DoModal(GUIWindowManager.ActiveWindow);

                        logger.WriteLog("Show Twitter select url dialog menu", LogLevel.Info, InfoServiceModul.InfoService);
                        if (dlg.SelectedId > 0)
                        {
                            int webBrowserWindowID = InfoServiceUtils.GetWebBrowserWindowId(dlg.SelectedLabelText, "");
                            if (webBrowserWindowID > 0)
                            {
                                logger.WriteLog(string.Format("Trying to open web browser with window ID {0}, url {1}", webBrowserWindowID, dlg.SelectedLabelText), LogLevel.Info, InfoServiceModul.Feed);
                                GUIWindowManager.ActivateWindow(webBrowserWindowID, false);
                            }
                        }
                    }
                }
            }
            base.OnClicked(controlId, control, actionType);
        }

        public override bool OnMessage(GUIMessage message)
        {
            int iControl;
            switch (message.Message)
            {
                case GUIMessage.MessageType.GUI_MSG_ITEM_FOCUS_CHANGED:
                    {
                        iControl = message.SenderControlId;
                        if (iControl == GUITwitterList)
                        {
                            if (_twitterListcontrol != null)
                            {
                                TwitterUtils.lastSelectedItem = _twitterListcontrol.SelectedListItemIndex;
                            }

                            GUIListItem item = GUIControl.GetSelectedListItem(GetID, GUITwitterList);
                            if (item != null)
                            {
                                if (_twitterListcontrol != null)
                                {
                                    PropertyUtils.SetProperty(PropertyUtils.Properties.Twitter.SelectedUsername,
                                        TwitterService.GetTimeline(TwitterService.ActiveTimeline).Items
                                            [_twitterListcontrol.SelectedListItemIndex].User.ScreenName);
                                    PropertyUtils.SetProperty(PropertyUtils.Properties.Twitter.SelectedIndex,
                                        _twitterListcontrol.SelectedListItemIndex.ToString());
                                    PropertyUtils.SetProperty(PropertyUtils.Properties.Twitter.SelectedUserpicture,
                                        TwitterService.GetTimeline(TwitterService.ActiveTimeline).Items
                                            [_twitterListcontrol.SelectedListItemIndex].User.PicturePath);
                                    PropertyUtils.SetProperty(PropertyUtils.Properties.Twitter.SelectedMediaImage, TwitterService.GetTimeline(TwitterService.ActiveTimeline).Items
                                        [_twitterListcontrol.SelectedListItemIndex].MediaPath);
                                    GUIControl.SetControlLabel(GetID, GUITwitterMessage,
                                        TwitterService.GetTimeline(TwitterService.ActiveTimeline).Items
                                            [_twitterListcontrol.SelectedListItemIndex].Text);
                                }
                                break;


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
