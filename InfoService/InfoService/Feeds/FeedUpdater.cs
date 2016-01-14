using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InfoService.Utils;
using InfoService.BackgroundWorkers;

namespace InfoService.Feeds
{
    public static class FeedUpdater
    {
        public static bool FeedUpaterCompleted { get; private set; }
        public static void SetupUpdater()
        {
            UpdateTimer.OnTimeForUpdateProperties += new UpdateTimer.UpdatePropertiesHandler(UpdateTimer_UpdateProperties);
        }
        static void UpdateTimer_UpdateProperties()
        {
            if (FeedService.Enabled)
            {
                //Jump over the first Update and fake the LastRefresh to NOW
                if (!FeedService.UpdateOnStartup)
                {
                    FeedService.LastRefresh = DateTime.Now;
                    FeedService.UpdateOnStartup = true;
                }
                TimeSpan spanFeed = DateTime.Now - FeedService.LastRefresh;
                if (((int)spanFeed.TotalMinutes) >= FeedService.RefreshInterval)
                {
                    if (FeedService.Feeds.Count >= 1)
                    {
                        UpdateFeedDataAsync();
                    }
                }
            }
        }
        public static void UpdateFeedDataAsync()
        {
            if (FeedService.Enabled)
            {
                if (FeedService.UpdateInProgress)
                    return;

                PropertyUtils.SetProperty(PropertyUtils.Properties.Feed.LastupdatedMessage, string.Format("{0}...", InfoServiceUtils.GetLocalizedLabel(8)));
                PropertyUtils.SetProperty(PropertyUtils.Properties.Feed.LastupdatedDatetime, " ");

                InfoServiceUtils.DeleteFeedCache();
                PropertyUtils.SetProperty(PropertyUtils.Properties.Feed.Titles, InfoServiceUtils.GetLocalizedLabel(13));
                PropertyUtils.SetProperty(PropertyUtils.Properties.Feed.Alltitles, InfoServiceUtils.GetLocalizedLabel(13));
                FeedUpaterCompleted = false;
                FeedWorker fWorker = new FeedWorker();
                fWorker.RunWorkerCompleted += FWorker_RunWorkerCompleted;
                fWorker.RunWorkerAsync();
            }
        }

        private static void FWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            //if (e.Cancelled || !(bool)e.Result)
            //{
            //    FeedUtils.SetFeedProperties(false);
            //}
            //else FeedUtils.SetFeedProperties(true);
            FeedUpaterCompleted = true;
        }

        public static void UpdateFeedDataSync()
        {
            FeedUpaterCompleted = false;
            if (FeedService.Enabled)
            {
                if (FeedService.UpdateInProgress)
                    return;

                PropertyUtils.SetProperty(PropertyUtils.Properties.Feed.LastupdatedMessage, string.Format("{0}...", InfoServiceUtils.GetLocalizedLabel(8)));
                PropertyUtils.SetProperty(PropertyUtils.Properties.Feed.LastupdatedDatetime, " ");

                InfoServiceUtils.DeleteFeedCache();
                PropertyUtils.SetProperty(PropertyUtils.Properties.Feed.Titles, InfoServiceUtils.GetLocalizedLabel(13));
                PropertyUtils.SetProperty(PropertyUtils.Properties.Feed.Alltitles, InfoServiceUtils.GetLocalizedLabel(13));

                FeedService.UpdateAllFeeds();

                //if (FeedService.UpdateAllFeeds())
                //{
                //    FeedUtils.SetFeedProperties(true);
                //}
                //else
                //{
                //    FeedUtils.SetFeedProperties(false);
                //}

            }
            FeedUpaterCompleted = true;

        }
    }
}
