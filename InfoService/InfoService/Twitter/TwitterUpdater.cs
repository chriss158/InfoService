using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InfoService.Utils;
using MediaPortal.Player;
using MediaPortal.GUI.Library;
using InfoService.BackgroundWorkers;

namespace InfoService.Twitter
{
    public static class TwitterUpdater
    {
        private static readonly Logger logger = Logger.GetInstance();

        private static List<string> windowIDsMovies = new List<string> { "96742", "6" };
        private static List<string> windowIDsSeries = new List<string> { "9811" };

        public static void SetupUpdater()
        {
            UpdateTimer.OnTimeForUpdateProperties += new UpdateTimer.UpdatePropertiesHandler(UpdateTimer_UpdateProperties);
            g_Player.PlayBackStarted += new g_Player.StartedHandler(g_Player_PlayBackStarted);
        }

        static void g_Player_PlayBackStarted(g_Player.MediaType type, string filename)
        {
            if (type == g_Player.MediaType.Video && TwitterService.PostWatchingVideos)
            {
                logger.WriteLog("Try to post tweet for video " + filename, LogLevel.Debug, InfoServiceModul.Twitter);

                string title = string.Empty;
                string curId = GUIPropertyManager.GetProperty("#currentmoduleid");
                string[] seriesInfo = null;
                bool isSeries = windowIDsSeries.Contains(curId);
                bool isMovies = windowIDsMovies.Contains(curId);

                if (curId == "96742" && TwitterService.PostUsingMovingPictures)
                {
                    title = GUIPropertyManager.GetProperty("#MovingPictures.SelectedMovie.title");
                }
                else if (curId == "9811" && TwitterService.PostUsingTVSeries) 
                {
                    title = GUIPropertyManager.GetProperty("#TVSeries.Extended.Title");
                    if (!string.IsNullOrEmpty(title)) title.Trim();
                    if (!string.IsNullOrEmpty(title))
                        seriesInfo = title.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                    else
                        title = GUIPropertyManager.GetProperty("#TVSeries.Extended.Title");
                }
                else if (curId == "6" && TwitterService.PostUsingMyVideos)
                {
                    title = GUIPropertyManager.GetProperty("#Play.Current.Title");
                }

                if (!string.IsNullOrEmpty(title)) title.Trim();

                if (!string.IsNullOrEmpty(title))
                {
                    if (TwitterService.Enabled)
                    {
                        string tweetFor = string.Empty;
                        if (isSeries) tweetFor = "TV show";
                        else if (isMovies) tweetFor = "Video (movie)";
                        logger.WriteLog(string.Format("Creating {0} tweet from title {1}", tweetFor, title), LogLevel.Debug, InfoServiceModul.Twitter);

                        string postText = string.Empty;
                        if (isSeries)
                        {
                            postText = TwitterService.PostWatchSeriesMask.Trim();
                            int season = -1;
                            int episode = -1;
                            int.TryParse(seriesInfo[1], out season);
                            int.TryParse(seriesInfo[2], out episode);
                            if (!string.IsNullOrEmpty(postText) && seriesInfo != null && seriesInfo.Length == 4 && season >= 0 && episode >= 0)
                            {
                                postText = postText.Replace("%showname%", seriesInfo[0]);
                                postText = postText.Replace("%season%", seriesInfo[1]);
                                postText = postText.Replace("%seasonL%", string.Format("{0:00}", season));
                                postText = postText.Replace("%episode%", seriesInfo[2]);
                                postText = postText.Replace("%episodeL%", string.Format("{0:00}", episode));
                                postText = postText.Replace("%episodename%", seriesInfo[3]);
                            }
                            else
                                postText = string.Empty;
                        }
                        else if (isMovies) {
                            postText = TwitterService.PostWatchMoviesMask.Trim();
                            if (!string.IsNullOrEmpty(postText))
                            {
                                postText = postText.Replace("%video%", title);
                            }
                        }

                        postText = postText.Replace("%filename%", System.IO.Path.GetFileNameWithoutExtension(filename));
                        postText = postText.Replace("%ext%", System.IO.Path.GetExtension(filename));

                        if (!string.IsNullOrEmpty(postText))
                        {
                            logger.WriteLog(string.Format("Tweeting \"{0}\"!", postText), LogLevel.Debug, InfoServiceModul.Twitter);
                            TwitterService.PostStatus(postText);
                        }
                    }
                }
            }
        }

        static void UpdateTimer_UpdateProperties()
        {
            if (TwitterService.Enabled)
            {
                if (!TwitterService.UpdateOnStartup)
                {
                    TwitterService.LastRefresh = DateTime.Now;
                    TwitterService.UpdateOnStartup = true;
                }
                TimeSpan spanTwitter = DateTime.Now - TwitterService.LastRefresh;
                if (((int)spanTwitter.TotalMinutes) >= TwitterService.RefreshInterval)
                {
                    UpdateTwitterDataAsync();
                }
            }
        }
        public static void UpdateTwitterDataAsync()
        {
            if (TwitterService.Enabled)
            {
                if (TwitterService.UpdateInProgress)
                    return;

                PropertyUtils.SetProperty(PropertyUtils.Properties.Twitter.LastupdatedMessage, string.Format("{0}...", InfoServiceUtils.GetLocalizedLabel(8)));
                PropertyUtils.SetProperty(PropertyUtils.Properties.Twitter.LastupdatedDatetime, " ");

                InfoServiceUtils.DeleteTwitterCache();
                PropertyUtils.SetProperty(PropertyUtils.Properties.Twitter.Messages, InfoServiceUtils.GetLocalizedLabel(15));
                TwitterWorker tWorker = new TwitterWorker();
                tWorker.RunWorkerCompleted += TWorker_RunWorkerCompleted;
                tWorker.RunWorkerAsync();
            }
        }

        private static void TWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            //if (e.Cancelled || !(bool)e.Result)
            //{
            //    TwitterUtils.SetTwitterProperties(false);
            //}
            //else TwitterUtils.SetTwitterProperties(true);
        }

        public static void UpdateTwitterDataSync()
        {
            if (TwitterService.Enabled)
            {
                if (TwitterService.UpdateInProgress)
                    return;

                PropertyUtils.SetProperty(PropertyUtils.Properties.Twitter.LastupdatedMessage, string.Format("{0}...", InfoServiceUtils.GetLocalizedLabel(8)));
                PropertyUtils.SetProperty(PropertyUtils.Properties.Twitter.LastupdatedDatetime, " ");

                InfoServiceUtils.DeleteTwitterCache();
                PropertyUtils.SetProperty(PropertyUtils.Properties.Twitter.Messages, InfoServiceUtils.GetLocalizedLabel(15));

                TwitterService.UpdateTwitter();
                //if (TwitterService.UpdateTwitter())
                //{
                //    TwitterUtils.SetTwitterProperties(true);
                //}
                //else TwitterUtils.SetTwitterProperties(false);
            }
        }
    }
}
