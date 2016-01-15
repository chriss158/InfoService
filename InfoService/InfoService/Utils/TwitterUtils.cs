#region Usings

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using InfoService.BackgroundWorkers;
using InfoService.GUIWindows;
using InfoService.Twitter;
using MediaPortal.GUI.Library;
using TwitterConnector;
using TwitterConnector.Data;

#endregion

namespace InfoService.Utils
{
    public static class TwitterUtils
    {
        private static readonly Logger logger = Logger.GetInstance();

        public static int lastSelectedItem = -1;

   

        public static void SetTwitterProperties(bool success)
        {
            if (success)
            {
                SetTimelineOnWindow(TwitterService.GetTimeline(TwitterService.ActiveTimeline));
                SetTimelineOnBasichome(TwitterService.GetTimeline(TwitterService.ActiveTimeline));
                PropertyUtils.SetProperty(PropertyUtils.Properties.Twitter.LastupdatedMessage, string.Format(InfoServiceUtils.GetLocalizedLabel(30), TwitterService.LastRefresh));
                PropertyUtils.SetProperty(PropertyUtils.Properties.Twitter.LastupdatedDatetime, TwitterService.LastRefresh.ToString());
            }
            else
            {
                PropertyUtils.SetProperty(PropertyUtils.Properties.Twitter.Messages, InfoServiceUtils.GetLocalizedLabel(16));
            }
        }


        private static string ReplaceTickerMask(Timeline timeline, string mask, int itemnumber)
        {
            if (timeline == null) return "";
            if (timeline.Items.Count <= 0) return "";
            string newText = mask;

            newText = newText.Replace("%message%", timeline.Items[itemnumber].Text);
            newText = newText.Replace("%user%", timeline.Items[itemnumber].User.ScreenName);
            newText = newText.Replace("%source%", timeline.Items[itemnumber].Source);
            newText = newText.Replace("%d%", timeline.Items[itemnumber].PublishDate.ToString("d ").Substring(0, timeline.Items[itemnumber].PublishDate.ToString("d ").Length - 1));
            newText = newText.Replace("%dd%", timeline.Items[itemnumber].PublishDate.ToString("dd"));
            newText = newText.Replace("%M%", timeline.Items[itemnumber].PublishDate.ToString("M ").Substring(0, timeline.Items[itemnumber].PublishDate.ToString("M ").Length - 1));
            newText = newText.Replace("%MM%", timeline.Items[itemnumber].PublishDate.ToString("MM"));
            newText = newText.Replace("%yy%", timeline.Items[itemnumber].PublishDate.ToString("yy"));
            newText = newText.Replace("%yyyy%", timeline.Items[itemnumber].PublishDate.ToString("yyyy"));
            newText = newText.Replace("%h%", timeline.Items[itemnumber].PublishDate.ToString("h ").Substring(0, timeline.Items[itemnumber].PublishDate.ToString("h ").Length - 1));
            newText = newText.Replace("%hh%", timeline.Items[itemnumber].PublishDate.ToString("hh"));
            newText = newText.Replace("%H%", timeline.Items[itemnumber].PublishDate.ToString("H ").Substring(0, timeline.Items[itemnumber].PublishDate.ToString("H ").Length - 1));
            newText = newText.Replace("%HH%", timeline.Items[itemnumber].PublishDate.ToString("HH"));
            newText = newText.Replace("%m%", timeline.Items[itemnumber].PublishDate.ToString("m ").Substring(0, timeline.Items[itemnumber].PublishDate.ToString("m ").Length - 1));
            newText = newText.Replace("%mm%", timeline.Items[itemnumber].PublishDate.ToString("mm"));
            newText = newText.Replace("%s%", timeline.Items[itemnumber].PublishDate.ToString("s ").Substring(0, timeline.Items[itemnumber].PublishDate.ToString("s ").Length - 1));
            newText = newText.Replace("%ss%", timeline.Items[itemnumber].PublishDate.ToString("ss"));
            return newText;

        }

        private static string MakeTwitterLine(Timeline timeline, string separator, string mask)
        {
            if (timeline == null) return "";
            if (timeline.Items.Count > 0)
            {
                
                logger.WriteLog("Make twitter[" + timeline.Type + "] line with mask " + mask, LogLevel.Debug, InfoServiceModul.Twitter);
                string _twitterFeedAsLine = String.Empty;
                int _twitterCounter = 0;

                foreach (TwitterItem item in timeline.Items)
                {
                    _twitterCounter++;
                    //_twitterFeedAsLine += item.Text + " (" + item.User.ScreenName + ") " + TwitterService.Separator + " ";
                    _twitterFeedAsLine += ReplaceTickerMask(timeline, mask, _twitterCounter - 1) + " " + separator + " ";
                    if (_twitterCounter >= TwitterService.Items) break;
                }
                if (_twitterFeedAsLine.Length >= separator.Length + 1)
                    _twitterFeedAsLine = _twitterFeedAsLine.Substring(0, _twitterFeedAsLine.Length - separator.Length - 2);
                return _twitterFeedAsLine;
            }
            return "";
        }

        public static void SetTimelineOnWindow(Timeline timeline)
        {
            SetTimelineOnWindow(timeline, false, false);
        }

        public delegate void SetTimelineOnWindowDelegate(Timeline timeline, bool selectLastItemIndex, bool updateGUI);
        public delegate void SetTimelineOnWindowIndexDelegate(Timeline timeline, int twitterItemIndex, bool updateGUI);

        public static void SetTimelineOnWindow(Timeline timeline, bool selectLastItemIndex, bool updateGUI)
        {
            SetTimelineOnWindow(timeline, -1, selectLastItemIndex, updateGUI);
        }
        public static void SetTimelineOnWindow(Timeline timeline, int twitterItemIndex, bool updateGUI)
        {
            SetTimelineOnWindow(timeline, twitterItemIndex, false, updateGUI);
        }

        private static void SetTimelineOnWindow(Timeline timeline, int twitterItemIndex, bool selectLastItemIndex, bool updateGUI)
        {
            // if we are calling from thread, function will be invoked and none of the GUIControl functions will be executed
            // they are not needed since this will be called again in OnPageLoad
            
            if (GUIGraphicsContext.form.InvokeRequired)
            {
                if (selectLastItemIndex || twitterItemIndex < 0)
                {
                    SetTimelineOnWindowDelegate d = new SetTimelineOnWindowDelegate(SetTimelineOnWindow);
                    GUIGraphicsContext.form.Invoke(d, timeline, selectLastItemIndex, false);
                }
                else
                {
                    SetTimelineOnWindowIndexDelegate d = new SetTimelineOnWindowIndexDelegate(SetTimelineOnWindow);
                    GUIGraphicsContext.form.Invoke(d, timeline, twitterItemIndex, false);
                }

                return;
            }

            if (timeline == null) return;

            if (GUIWindowManager.ActiveWindow == GUITwitter.GUITwitterId)
                updateGUI = true;

            if (!selectLastItemIndex)
                lastSelectedItem = -1;

            PropertyUtils.SetProperty(PropertyUtils.Properties.Twitter.ItemCount, MediaPortal.Util.Utils.GetObjectCountLabel(timeline.Items.Count));
            PropertyUtils.SetProperty(PropertyUtils.Properties.Twitter.ItemType, InfoServiceUtils.GetLocalizedLabel(35));
            PropertyUtils.SetProperty(PropertyUtils.Properties.Twitter.SelectedTimeline, timeline.Type.GetPrettyName());
            TwitterService.ActiveTimeline = timeline.Type;

            if (selectLastItemIndex || updateGUI)
                GUIListControl.ClearControl(GUITwitter.GUITwitterId, GUITwitter.GUITwitterList);

            if (timeline.Items.Count > 0)
            {
                PropertyUtils.SetProperty(PropertyUtils.Properties.Twitter.SelectedUsername, timeline.Items[0].User.ScreenName);
                PropertyUtils.SetProperty(PropertyUtils.Properties.Twitter.SelectedUserpicture, timeline.Items[0].User.PicturePath);
                PropertyUtils.SetProperty(PropertyUtils.Properties.Twitter.SelectedMediaImage, timeline.Items[0].MediaPath);
                if (selectLastItemIndex || updateGUI)
                    GUIControl.SetControlLabel(GUITwitter.GUITwitterId, GUITwitter.GUITwitterMessage, timeline.Items[0].Text);

                if (selectLastItemIndex || updateGUI)
                {
                    for (int i = 0; i < timeline.Items.Count; i++)
                    {
                        GUIListItem item = new GUIListItem
                        {
                            IconImage = timeline.Items[i].User.PicturePath,
                            Label = timeline.Items[i].User.ScreenName
                        };
                        item.Label2 = InfoServiceUtils.GetTimeDifferenceToNow(timeline.Items[i].PublishDate) + " " + GUILocalizeStrings.Get(1024) + " @" + timeline.Items[i].User.ScreenName;
                        logger.WriteLog("Set/Calc time diffrence for twitter timeline[" + timeline.Type + "]. For " + timeline.Items[i].PublishDate + " its \"" + item.Label2 + "\"", LogLevel.Debug, InfoServiceModul.Feed);
                        GUIListControl.AddListItemControl(GUITwitter.GUITwitterId, GUITwitter.GUITwitterList, item);
                    }
                }

                logger.WriteLog("Set twitter timeline[" + timeline.Type + "] on window", LogLevel.Debug, InfoServiceModul.Twitter);

                if (lastSelectedItem >= 0 || twitterItemIndex >= 0)
                {
                    int setIndex = selectLastItemIndex ? lastSelectedItem : twitterItemIndex;
                    logger.WriteLog("Set selected item [" + setIndex + "] for twitter timeline[" + timeline.Type + "] on window", LogLevel.Debug, InfoServiceModul.Twitter);
                    GUIListControl.SelectItemControl(GUITwitter.GUITwitterId, GUITwitter.GUITwitterList, setIndex);
                }
            }
            else
            {
                PropertyUtils.SetProperty(PropertyUtils.Properties.Twitter.SelectedUsername, " ");
                PropertyUtils.SetProperty(PropertyUtils.Properties.Twitter.SelectedUserpicture, " ");
                PropertyUtils.SetProperty(PropertyUtils.Properties.Twitter.SelectedMediaImage, " ");
                if (selectLastItemIndex || updateGUI)
                    GUIControl.SetControlLabel(GUITwitter.GUITwitterId, GUITwitter.GUITwitterMessage, "");
            }
        }

        public static void SetTimelineOnBasichome(Timeline timeline)
        {
            if (timeline == null) return;
            logger.WriteLog("Set twitter timeline[" + timeline.Type + "] on basichome", LogLevel.Debug, InfoServiceModul.Twitter);
            PropertyUtils.SetProperty(PropertyUtils.Properties.Twitter.Messages, MakeTwitterLine(timeline, TwitterService.Separator, TwitterService.TickerMask));
        }

        public static List<string> ParseUrls(string txt)
        {
            //Regex regx = new Regex("http://([\\w+?\\.\\w+])+([a-zA-Z0-9\\~\\!\\@\\#\\$\\%\\^\\&amp;\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\'\\,]*)?", RegexOptions.IgnoreCase);
            Regex regx = new Regex("(http|ftp|https)://([a-zA-Z0-9\\~\\!\\@\\#\\$\\%\\^\\&amp;\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\'\\,]*)?", RegexOptions.IgnoreCase);
            MatchCollection mactches = regx.Matches(txt);

            List<string> urls = new List<string>();
            foreach (Match match in mactches)
            {
                urls.Add(match.Value);
            }

            return urls;
        }
    }
}
