#region Usings

using System;
using System.Collections.Generic;
using FeedReader;
using FeedReader.Data;
using InfoService.BackgroundWorkers;
using InfoService.Feeds;
using InfoService.GUIWindows;
using System.Drawing;
using MediaPortal.GUI.Library;

#endregion

namespace InfoService.Utils
{
    public static class FeedUtils
    {
        private static readonly Logger logger = Logger.GetInstance();

        public static int lastSelectedItem = -1;

        public static void SetAllFeedsOnBasichome()
        {
            int safeIndex = FeedService.SetActive(0);
            if (safeIndex == 0)
            {
                PropertyUtils.SetProperty(PropertyUtils.Properties.Feed.Enabled, "true");
                PropertyUtils.SetProperty(PropertyUtils.Properties.Feed.Titles, MakeFeedMixLine(FeedService.Feeds[0], FeedService.TickerAllMask, FeedService.Items, FeedService.SeparatorAll));
                PropertyUtils.SetProperty(PropertyUtils.Properties.Feed.Alltitles, MakeFeedMixLine(FeedService.Feeds[0], FeedService.TickerAllMask, FeedService.Items, FeedService.SeparatorAll));
                PropertyUtils.SetProperty(PropertyUtils.Properties.Feed.Img, GUIGraphicsContext.Skin + @"\media\InfoService\defaultFeedALL.png");
                logger.WriteLog("Set all feeds on basichome", LogLevel.Debug, InfoServiceModul.Feed);
            }
            else
            {
                logger.WriteLog("Failed to set all feeds ticker on basichome... Returning to fallback feed index -> " + safeIndex, LogLevel.Warning, InfoServiceModul.Feed);
            }
        }

        public static void SetFeedsOnBasicHome()
        {
            if (FeedService.Feeds == null || FeedService.Feeds.Count < 1) return;

            for (int i = FeedService.Feeds.Count - 1; i >= 0; i--)
            {
                if (i == 0)
                {
                    PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.Feed.TitlesOfFeedIndex, i),
                        MakeFeedMixLine(FeedService.Feeds[i], FeedService.TickerAllMask, FeedService.Items, FeedService.SeparatorAll));
                    PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.Feed.ImgOfFeedIndex, i),
                        GUIGraphicsContext.Skin + @"\media\InfoService\defaultFeedALL.png");
                }
                else
                {
                    PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.Feed.TitlesOfFeedIndex, i),
                        MakeFeedLine(FeedService.Feeds[i], FeedService.TickerMask, FeedService.Items, FeedService.Separator));
                    PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.Feed.ImgOfFeedIndex, i),
                        FeedService.Feeds[i].ImagePath);
                }
            }
        }

        public static void SetFeedOnBasichome(int index)
        {
            if(index == 0)
            {
                SetAllFeedsOnBasichome();
                return;
            }
            int safeIndex = FeedService.SetActive(index);
            if (safeIndex == index)
            {
                PropertyUtils.SetProperty(PropertyUtils.Properties.Feed.Isalltitles, "false");
                PropertyUtils.SetProperty(PropertyUtils.Properties.Feed.Titles, MakeFeedLine(FeedService.Feeds[FeedService.ActiveFeedIndex], FeedService.TickerMask, FeedService.Items, FeedService.Separator));
                PropertyUtils.SetProperty(PropertyUtils.Properties.Feed.Alltitles, MakeFeedMixLine(FeedService.Feeds[0], FeedService.TickerAllMask, FeedService.Items, FeedService.SeparatorAll));
                PropertyUtils.SetProperty(PropertyUtils.Properties.Feed.Img, FeedService.Feeds[FeedService.ActiveFeedIndex].ImagePath);
                logger.WriteLog("Set feed[" + FeedService.Feeds[FeedService.ActiveFeedIndex].Title + "]/[" + FeedService.ActiveFeedIndex + "] on basichome", LogLevel.Debug, InfoServiceModul.Feed);
            }
            else
            {
                logger.WriteLog("Failed to set feed[" + index + "] on basichome... Returning to fallback feed index -> " + safeIndex, LogLevel.Warning, InfoServiceModul.Feed);
            }
        }

        public static void SetFeedOnWindow(int index)
        {
            SetFeedOnWindow(index, false, false);
        }

        public delegate void SetFeedOnWindowDelegate(int index, bool selectLastItemIndex, bool updateGUI);
        public delegate void SetFeedOnWindowIndexDelegate(int index, int feedItemIndex, bool updateGUI);

        public static void SetFeedOnWindow(int index, bool selectLastItemIndex, bool updateGUI)
        {
            SetFeedOnWindow(index, -1, selectLastItemIndex, updateGUI);
        }
        public static void SetFeedOnWindow(int index, int feedItemIndex, bool updateGUI)
        {
            SetFeedOnWindow(index, feedItemIndex, false, updateGUI);
        }

        private static void SetFeedOnWindow(int index, int feedItemIndex, bool selectLastItemIndex, bool updateGUI)
        {
            if(index >= FeedService.Feeds.Count ||
               index < 0)
            {
                return;
            }

            // if we are calling from thread, function will be invoked and none of the GUIControl functions will be executed
            // they are not needed since this will be called again in OnPageLoad

            if (GUIGraphicsContext.form.InvokeRequired)
            {
                
                if (selectLastItemIndex || feedItemIndex < 0)
                {
                    SetFeedOnWindowDelegate d = new SetFeedOnWindowDelegate(SetFeedOnWindow);
                    GUIGraphicsContext.form.Invoke(d, index, selectLastItemIndex, false);
                }
                else
                {
                    SetFeedOnWindowIndexDelegate d = new SetFeedOnWindowIndexDelegate(SetFeedOnWindow);
                    GUIGraphicsContext.form.Invoke(d, index, feedItemIndex, false);
                }
                
                return;
            }

            if (GUIWindowManager.ActiveWindow == GUIFeed.GUIFeedId)
                updateGUI = true;

            if (!selectLastItemIndex)
                lastSelectedItem = -1;

            PropertyUtils.SetProperty(PropertyUtils.Properties.Feed.SelectedFeed, FeedService.Feeds[index].Title);
            PropertyUtils.SetProperty(PropertyUtils.Properties.Feed.SelectedType, FeedService.Feeds[index].Type.ToString());
            PropertyUtils.SetProperty(PropertyUtils.Properties.Feed.ItemCount, MediaPortal.Util.Utils.GetObjectCountLabel(FeedService.Feeds[index].Items.Count));
            PropertyUtils.SetProperty(PropertyUtils.Properties.Feed.ItemType, InfoServiceUtils.GetLocalizedLabel(34));
            PropertyUtils.SetProperty(PropertyUtils.Properties.Feed.Img, FeedService.Feeds[index].ImagePath);
            if (selectLastItemIndex || updateGUI)
                GUIListControl.ClearControl(GUIFeed.GUIFeedId, GUIFeed.GUIFeedList);

            if (FeedService.Feeds[index].Items.Count > 0)
            {
                PropertyUtils.SetProperty(PropertyUtils.Properties.Feed.SelectedItemimg, FeedService.Feeds[index].Items[0].ImagePathBig);
                PropertyUtils.SetProperty(PropertyUtils.Properties.Feed.SelectedTitle, FeedService.Feeds[index].Items[0].Title);
                PropertyUtils.SetProperty(PropertyUtils.Properties.Feed.SelectedSourcefeed, FeedService.Feeds[index].Items[0].SourceTitle);

                if (selectLastItemIndex || updateGUI)
                    GUIControl.SetControlLabel(GUIFeed.GUIFeedId, GUIFeed.GUIFeedDescription, FeedService.Feeds[index].Items[0].Description);
                
                if (selectLastItemIndex || updateGUI)
                {
                    for (int i = 0; i < FeedService.Feeds[index].Items.Count; i++)
                    {

                        GUIListItem item = new GUIListItem
                        {
                            IconImage = FeedService.Feeds[index].Items[i].ImagePath,
                            Label = FeedService.Feeds[index].Items[i].Title
                        };
                        
                        if (FeedService.ShowPublishTime)
                        {
                            if (!FeedService.Feeds[index].IsAllFeed || FeedService.ItemPublishTimeAllFeeds == InfoService.Enums.ItemPublishTimeAllFeedsType.PublishTime)
                            {
                                item.Label2 = InfoServiceUtils.GetTimeDifferenceToNow(FeedService.Feeds[index].Items[i].PublishDate);
                                logger.WriteLog("Set/Calc time difference for feed[" + FeedService.Feeds[index].Title + "]/[" + index + "]. For " + FeedService.Feeds[index].Items[i].PublishDate + " its \"" + item.Label2 + "\"", LogLevel.Debug, InfoServiceModul.Feed);
                            }
                            else
                            {
                                if (FeedService.ItemPublishTimeAllFeeds == InfoService.Enums.ItemPublishTimeAllFeedsType.FeedName)
                                    item.Label2 = FeedService.Feeds[index].Items[i].SourceTitle;
                                else if (FeedService.ItemPublishTimeAllFeeds == InfoService.Enums.ItemPublishTimeAllFeedsType.Both)
                                {
                                    string label2 = string.Empty;
                                    label2 = FeedService.Feeds[index].Items[i].SourceTitle;
                                    string difference = InfoServiceUtils.GetTimeDifferenceToNow(FeedService.Feeds[index].Items[i].PublishDate);
                                    label2 = string.IsNullOrEmpty(difference) ? label2 : label2 + ", " + difference;
                                    item.Label2 = label2;
                                }
                            }
                        }
                        GUIListControl.AddListItemControl(GUIFeed.GUIFeedId, GUIFeed.GUIFeedList, item);
                    }
                }
                
                logger.WriteLog("Set feed[" + FeedService.Feeds[index].Title + "]/[" + index + "] on window", LogLevel.Debug, InfoServiceModul.Feed);

                if (lastSelectedItem >= 0 || feedItemIndex >= 0)
                {
                    int setIndex = selectLastItemIndex ? lastSelectedItem : feedItemIndex;
                    logger.WriteLog("Set selected item [" + setIndex + "] for feed[" + FeedService.Feeds[index].Title + "]/[" + index + "] on window", LogLevel.Debug, InfoServiceModul.Feed);
                    GUIListControl.SelectItemControl(GUIFeed.GUIFeedId, GUIFeed.GUIFeedList, setIndex);
                }
            }
            else
            {
                PropertyUtils.SetProperty(PropertyUtils.Properties.Feed.SelectedItemimg, " ");
                PropertyUtils.SetProperty(PropertyUtils.Properties.Feed.SelectedTitle, " ");
                PropertyUtils.SetProperty(PropertyUtils.Properties.Feed.SelectedSourcefeed, " ");
                if (selectLastItemIndex || updateGUI)
                    GUIControl.SetControlLabel(GUIFeed.GUIFeedId, GUIFeed.GUIFeedDescription, "");
            }
        }

        public static string MakeFeedLine(ExFeed feed, string mask, decimal items, string separator)
        {
            if(feed == null) return "";
            logger.WriteLog("Make feed[" + feed.Title + "] line with mask " + mask, LogLevel.Debug, InfoServiceModul.Feed);
            if (feed.Items.Count > 0)
            {
                string _feedAsLine = String.Empty;
                int _feedCounter = 0;
                foreach (FeedItem item in feed.Items)
                {
                    _feedAsLine += ReplaceTickerMask(feed, mask, _feedCounter) + " " + separator + " ";
                    if (_feedCounter >= items - 1)
                    {
                        break;
                    }
                    _feedCounter++;
                }
                if (_feedAsLine.Length >= separator.Length + 1) _feedAsLine = _feedAsLine.Substring(0, _feedAsLine.Length - separator.Length - 2);
                return _feedAsLine;
            }
            return "";
        }
        public static string MakeFeedMixLine(ExFeed feed, string mask, decimal items, string separator)
        {
            if (feed == null) return "";
            if (feed.Items.Count <= 0) return "";
            logger.WriteLog("Make feed mix line", LogLevel.Debug, InfoServiceModul.Feed);
            string mixfeedline = String.Empty;

            int _feedCounter = 0;
            foreach (FeedItem item in feed.Items)
            {
                //mixfeedline += item.Title + " (" + item.SourceTitle + ") " + separator + " ";
                mixfeedline += ReplaceTickerMask(feed, mask, _feedCounter) + " " + separator + " ";
                if (_feedCounter >= items + 1)
                {
                    break;
                }
                _feedCounter++;
            }
            if (mixfeedline.Length >= separator.Length + 1) mixfeedline = mixfeedline.Substring(0, mixfeedline.Length - separator.Length - 2);
            return mixfeedline;
        }

        public static string ReplaceTickerMask(ExFeed feed, string mask, int itemnumber)
        {
            if (feed == null) return "";
            if (feed.Items.Count <= 0) return "";
            string newText = mask;

            newText = newText.Replace("%feedtitle%", feed.Title);
            newText = newText.Replace("%feeddescription%", feed.Description);
            newText = newText.Replace("%itemtitle%", feed.Items[itemnumber].Title);
            newText = newText.Replace("%itemdescription%", feed.Items[itemnumber].Description);
            newText = newText.Replace("%itemindex%", (itemnumber+1).ToString());
            newText = newText.Replace("%itemsource%", feed.Items[itemnumber].SourceTitle);
            newText = newText.Replace("%author%", feed.Items[itemnumber].Author);
            newText = newText.Replace("%d%", feed.Items[itemnumber].PublishDate.ToString("d ").Substring(0, feed.Items[itemnumber].PublishDate.ToString("d ").Length - 1));
            newText = newText.Replace("%dd%", feed.Items[itemnumber].PublishDate.ToString("dd"));
            newText = newText.Replace("%M%", feed.Items[itemnumber].PublishDate.ToString("M ").Substring(0, feed.Items[itemnumber].PublishDate.ToString("M ").Length - 1));
            newText = newText.Replace("%MM%", feed.Items[itemnumber].PublishDate.ToString("MM"));
            newText = newText.Replace("%yy%", feed.Items[itemnumber].PublishDate.ToString("yy"));
            newText = newText.Replace("%yyyy%", feed.Items[itemnumber].PublishDate.ToString("yyyy"));
            newText = newText.Replace("%h%", feed.Items[itemnumber].PublishDate.ToString("h ").Substring(0, feed.Items[itemnumber].PublishDate.ToString("h ").Length - 1));
            newText = newText.Replace("%hh%", feed.Items[itemnumber].PublishDate.ToString("hh"));
            newText = newText.Replace("%H%", feed.Items[itemnumber].PublishDate.ToString("H ").Substring(0, feed.Items[itemnumber].PublishDate.ToString("H ").Length - 1));
            newText = newText.Replace("%HH%", feed.Items[itemnumber].PublishDate.ToString("HH"));
            newText = newText.Replace("%m%", feed.Items[itemnumber].PublishDate.ToString("m ").Substring(0, feed.Items[itemnumber].PublishDate.ToString("m ").Length - 1));
            newText = newText.Replace("%mm%", feed.Items[itemnumber].PublishDate.ToString("mm"));
            newText = newText.Replace("%s%", feed.Items[itemnumber].PublishDate.ToString("s ").Substring(0, feed.Items[itemnumber].PublishDate.ToString("s ").Length - 1));
            newText = newText.Replace("%ss%", feed.Items[itemnumber].PublishDate.ToString("ss"));
            return newText;
        }

        /*
                private void ShowUpdatingDialog()
                {
                    _dialogUpdateNotify = (GUIDialogNotify)GUIWindowManager.GetWindow((int)Window.WINDOW_DIALOG_NOTIFY);
                    _dialogUpdateNotify.TimeOut = 8;
                    _dialogUpdateNotify.SetImage(GUIGraphicsContext.Skin + @"\media\InfoService\UpdateLogo.png");
                    _dialogUpdateNotify.SetHeading("InfoService");
                    _dialogUpdateNotify.SetText("Updating Feeds/Weather/Twitter...");
                    _dialogUpdateNotify.DoModal(GUIWindowManager.ActiveWindow);
                }
        */

       

        public static void SetFeedProperties(bool success)
        {
            if (!success)
            {
                PropertyUtils.SetProperty(PropertyUtils.Properties.Feed.Titles, InfoServiceUtils.GetLocalizedLabel(14));
                PropertyUtils.SetProperty(PropertyUtils.Properties.Feed.Alltitles, InfoServiceUtils.GetLocalizedLabel(14));
            }
            else
            {
                int index = 0;
                if (FeedService.MediaPortalStartup)
                {
                    if (!FeedService.RandomFeedOnStartup)
                    {
                        index = FeedService.StartupFeedIndex;
                    }
                    else
                    {
                        Random rnd = new Random();
                        index = rnd.Next(0, FeedService.Feeds.Count - 1);
                    }
                    FeedService.MediaPortalStartup = false;
                }
                else
                {
                    if (!FeedService.RandomFeedOnUpdate)
                    {
                        index = FeedService.ActiveFeedIndex;
                    }
                    else
                    {
                        Random rnd = new Random();
                        index = rnd.Next(0, FeedService.Feeds.Count - 1);
                    }

                }
                SetFeedsOnBasicHome();
                SetFeedOnBasichome(index);
                SetFeedOnWindow(index);
                PropertyUtils.SetProperty(PropertyUtils.Properties.Feed.LastupdatedMessage, string.Format(InfoServiceUtils.GetLocalizedLabel(29), FeedService.LastRefresh));
                PropertyUtils.SetProperty(PropertyUtils.Properties.Feed.LastupdatedDatetime, FeedService.LastRefresh.ToString());
            }
        }


    }
}
