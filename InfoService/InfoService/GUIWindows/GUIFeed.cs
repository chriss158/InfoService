#region Usings

using System;
using System.Drawing;
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

        private GUIFacadeControl.Layout _currentLayout;

        #region SkinControls

        [SkinControl(6)]
#pragma warning disable 649
        private GUIMenuButton _btnLayouts;
#pragma warning restore 649

        [SkinControlAttribute(4)]
#pragma warning disable 649
        private GUIButtonControl _changeFeed;
#pragma warning restore 649

        [SkinControlAttribute(50)]
#pragma warning disable 649
            //private GUIListControl _feedListcontrol;
        private GUIFacadeControl _feedListcontrol;
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
        private bool AllowLayout(GUIFacadeControl.Layout layout)
        {
            return true;
        }
        private GUIFacadeControl.Layout GetLayoutNumber(string s)
        {
            switch (s.Trim().ToLowerInvariant())
            {
                case "list":
                    return GUIFacadeControl.Layout.List;
                case "icons":
                case "smallicons":
                    return GUIFacadeControl.Layout.SmallIcons;
                case "big icons":
                case "largeicons":
                    return GUIFacadeControl.Layout.LargeIcons;
                case "albums":
                case "albumview":
                    return GUIFacadeControl.Layout.AlbumView;
                case "filmstrip":
                    return GUIFacadeControl.Layout.Filmstrip;
                case "playlist":
                    return GUIFacadeControl.Layout.Playlist;
                case "coverflow":
                case "cover flow":
                    return GUIFacadeControl.Layout.CoverFlow;
            }
            if (!string.IsNullOrEmpty(s))
            {
                //Log.Error("{0}::GetLayoutNumber: Unknown String - {1}", "WindowPluginBase", s);
            }
            return GUIFacadeControl.Layout.List;
        }
        private void InitLayoutSelections()
        {
            _btnLayouts.ClearMenu();

            // Add the allowed layouts to choose from to the menu.
            int totalLayouts = Enum.GetValues(typeof(GUIFacadeControl.Layout)).Length;
            for (int i = 0; i < totalLayouts; i++)
            {
                string layoutName = Enum.GetName(typeof(GUIFacadeControl.Layout), i);
                GUIFacadeControl.Layout layout = GetLayoutNumber(layoutName);
                if (AllowLayout(layout))
                {
                    if (!_feedListcontrol.IsNullLayout(layout))
                    {
                        _btnLayouts.AddItem(GUIFacadeControl.GetLayoutLocalizedName(layout), (int)layout);
                    }
                }
            }

            // Have the menu select the currently selected layout.
            _btnLayouts.SetSelectedItemByValue((int)_currentLayout);
        }
        private void SetLayout(GUIFacadeControl.Layout layout)
        {
            // Set the selected layout.
            SwitchToNextAllowedLayout(layout);
        }
        private void SwitchToNextAllowedLayout(GUIFacadeControl.Layout selectedLayout)
        {
            int iSelectedLayout = (int)selectedLayout;
            int totalLayouts = Enum.GetValues(typeof(GUIFacadeControl.Layout)).Length - 1;

            if (iSelectedLayout > totalLayouts)
                iSelectedLayout = 0;

            bool shouldContinue = true;
            do
            {
                if (!AllowLayout(selectedLayout) || _feedListcontrol.IsNullLayout(selectedLayout))
                {
                    iSelectedLayout++;
                    if (iSelectedLayout > totalLayouts)
                        iSelectedLayout = 0;
                }
                else
                {
                    shouldContinue = false;
                }
            } while (shouldContinue);

            _currentLayout = (GUIFacadeControl.Layout)iSelectedLayout;

            SwitchLayout();

            GUIMessage msg = new GUIMessage(GUIMessage.MessageType.GUI_MSG_LAYOUT_CHANGED, 0, 0, 0, 0, 0, 0);
            GUIWindowManager.SendMessage(msg);
        }
        private void SwitchLayout()
        {
            if (_feedListcontrol == null)
            {
                return;
            }

            // if skin has not implemented layout control or requested layout is not allowed
            // then default to list layout
            if (_feedListcontrol.IsNullLayout(_currentLayout) || !AllowLayout(_currentLayout))
            {
                _feedListcontrol.CurrentLayout = GUIFacadeControl.Layout.List;
            }
            else
            {
                _feedListcontrol.CurrentLayout = _currentLayout;
            }

            PresentLayout();

            // The layout may be automatically switched via selection of a new view.
            // Here we need to ensure that the layout menu button reflects the proper state (this is redundant when the
            // layout button was used to change the layout).  Need to call facadeLayout to get the current layout since the
            // CurrentLayout getter is algorithmic.
            _btnLayouts.SetSelectedItemByValue((int)_feedListcontrol.CurrentLayout);
        }
        private void SelectCurrentItem()
        {
            if (_feedListcontrol == null)
            {
                return;
            }
            int iItem = _feedListcontrol.SelectedListItemIndex;
            if (iItem > -1)
            {
                GUIControl.SelectItemControl(GetID, _feedListcontrol.GetID, iItem);
            }
        }
        public void PresentLayout()
        {
            GUIControl.HideControl(GetID, _feedListcontrol.GetID);
            int iControl = _feedListcontrol.GetID;
            GUIControl.ShowControl(GetID, iControl);
            GUIControl.FocusControl(GetID, iControl);
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
                logger.WriteLog("Load Feed GUI", LogLevel.Info, InfoServiceModul.Feed);
                FeedUtils.SetFeedOnWindow(FeedService.ActiveFeedIndex, true, true);
                PropertyUtils.SetProperty("#currentmodule", InfoServiceCore.UserPluginName + " - Feeds");
                GUIControl.SetControlLabel(GetID, 2, InfoServiceUtils.GetLocalizedLabel(0));
                GUIControl.SetControlLabel(GetID, 4, InfoServiceUtils.GetLocalizedLabel(1));
                GUIControl.SetControlLabel(GetID, 5, InfoServiceUtils.GetLocalizedLabel(2));
            InitLayoutSelections();
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
                //InfoServiceUtils.ShowDialogNotifyWindow("TestHeader", "Laaaaaaaaaaaaaaaaaaaanger text",
                //    @"C:\ProgramData\Team MediaPortal\MediaPortal\skin\Titan\Media\InfoService\defaultTwitter.png",
                //    new Size(120, 120), 10);

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
            else if (control == _btnLayouts)
            {
                // Set the new layout and select the currently selected item in the layout.
                SetLayout((GUIFacadeControl.Layout)_btnLayouts.SelectedItemValue);
                SelectCurrentItem();

                // Refocus facade so item will be selected
                GUIControl.FocusControl(GetID, _feedListcontrol.GetID);

                //msgHandled = true;

                //if (_feedListcontrol.CurrentLayout == GUIFacadeControl.Layout.List) _feedListcontrol.CurrentLayout = GUIFacadeControl.Layout.SmallIcons;
                //else _feedListcontrol.CurrentLayout = GUIFacadeControl.Layout.List;
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
