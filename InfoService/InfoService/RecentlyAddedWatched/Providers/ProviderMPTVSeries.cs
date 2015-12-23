/*
using System;
using System.Collections.Generic;
using InfoService.Enums;
using InfoService.Settings;
using InfoService.Settings.Data;
using InfoService.Utils;
using MediaPortal.GUI.Library;
using InfoService.RecentlyAddedWatched.Data;
using InfoService.RecentlyAddedWatched.Interfaces;
using WindowPlugins.GUITVSeries;

namespace InfoService.RecentlyAddedWatched.Providers
{
    public class ProviderMPTVSeries : IRecentlyAddedWatchedProvider<RecentlySeriesItem>
    {
        private static readonly Logger logger = Logger.GetInstance();
        private static List<RecentlySeriesItem> SeriesAdded;
        private static List<RecentlySeriesItem> SeriesWatched;
        private const int MaxRecentlyItems = 3;
        private Random _rnd = new Random();

        public event EventHandler<NewItemHandler<RecentlySeriesItem>> OnNewItem;

        public ProviderMPTVSeries()
        {
            logger.WriteLog("MPTVSeries: Init MPTVSeries RecentlyAddedWatchedProvider", LogLevel.Info, InfoServiceModul.RecentlyAddedWatched);
            OnlineParsing.OnlineParsingCompleted += new OnlineParsing.OnlineParsingCompletedHandler(OnlineParsing_OnlineParsingCompleted);
            VideoHandler.EpisodeWatched += new VideoHandler.EpisodeWatchedDelegate(VideoHandler_EpisodeWatched);
            SeriesAdded = RetrieveRecentlyAddedFromPlugin();
            SeriesWatched = RetrieveRecentlyWatchedFromPlugin();
        }

        void VideoHandler_EpisodeWatched(DBEpisode episode)
        {
            RecentlySeriesItem rs = new RecentlySeriesItem();
            rs = GetRecentlySeriesItemFromEpisode(episode);
            if (rs == null) return;
            rs.Mode = RecentlyAddedWatchedMode.Watched;
            AddNewItem(rs);
            if (OnNewItem != null) OnNewItem(this, new NewItemHandler<RecentlySeriesItem>(rs));
        }


        void OnlineParsing_OnlineParsingCompleted(bool bDataUpdated)
        {
            List<DBEpisode> episodes = DBEpisode.GetMostRecent(MostRecentType.Created);

            List<RecentlySeriesItem> items = new List<RecentlySeriesItem>();
            int min = 0;
            foreach (DBEpisode episode in episodes)
            {
                RecentlySeriesItem ra = new RecentlySeriesItem();
                ra = GetRecentlySeriesItemFromEpisode(episode);
                if (ra == null) continue;
                if (!ra.Equals(SeriesAdded[min]))
                {
                    min++;
                    AddNewItem(ra);
                    if (OnNewItem != null) OnNewItem(this, new NewItemHandler<RecentlySeriesItem>(ra));
                }
                else break;
            }
        }
        public List<RecentlySeriesItem> GetRecentlyAdded()
        {
            return SeriesAdded;
        }

        public List<RecentlySeriesItem> GetRecentlyWatched()
        {
            return SeriesWatched;
        }

        private void AddNewItem(RecentlySeriesItem episode)
        {
            if (episode != null)
            {
                if (episode.Mode == RecentlyAddedWatchedMode.Added)
                {
                    logger.WriteLog(string.Format("MPTVSeries: New series/episode added: {0}", episode.SeriesTitle), LogLevel.Info, InfoServiceModul.RecentlyAddedWatched);
                    SeriesAdded.Insert(0, episode);
                    if (SeriesAdded.Count > MaxRecentlyItems) SeriesAdded.RemoveAt(MaxRecentlyItems);
                }
                else if (episode.Mode == RecentlyAddedWatchedMode.Watched)
                {
                    logger.WriteLog(string.Format("MPTVSeries: New series/episode watched: {0}", episode.SeriesTitle), LogLevel.Info, InfoServiceModul.RecentlyAddedWatched);
                    SeriesWatched.Insert(0, episode);
                    if (SeriesWatched.Count > MaxRecentlyItems) SeriesWatched.RemoveAt(MaxRecentlyItems);
                }
            }
        }

        private List<RecentlySeriesItem> RetrieveRecentlyAddedFromPlugin()
        {
            logger.WriteLog("MPTVSeries: Get Most Recent Added Series/Episodes", LogLevel.Info, InfoServiceModul.RecentlyAddedWatched);

            List<DBEpisode> episodes = DBEpisode.GetMostRecent (MostRecentType.Created);

            logger.WriteLog(string.Format("MPTVSeries: {0} Series/Episodes found in database", episodes.Count.ToString()), LogLevel.Info, InfoServiceModul.RecentlyAddedWatched);

            int i = 1;
            List<RecentlySeriesItem> items = new List<RecentlySeriesItem>();
            foreach (DBEpisode episode in episodes)
            {
                RecentlySeriesItem ra = new RecentlySeriesItem();
                ra = GetRecentlySeriesItemFromEpisode(episode);
                if (ra == null) continue;
                ra.Mode = RecentlyAddedWatchedMode.Added;
                logger.WriteLog(string.Format("MPTVSeries: Recently Added Series/Episode {0} is {1}", i, ra.SeriesTitle), LogLevel.Debug, InfoServiceModul.RecentlyAddedWatched);
                items.Add(ra);
                i++;
                if (i == MaxRecentlyItems + 1) break;
            }
            return items;
        }


        private List<RecentlySeriesItem> RetrieveRecentlyWatchedFromPlugin()
        {
            logger.WriteLog("MPTVSeries: Get Most Recent Watched Series/Episodes", LogLevel.Info, InfoServiceModul.RecentlyAddedWatched);

            List<DBEpisode> episodes = DBEpisode.GetMostRecent(MostRecentType.Watched);

            logger.WriteLog(string.Format("MPTVSeries: {0} Series/Episodes found in database", episodes.Count.ToString()), LogLevel.Info, InfoServiceModul.RecentlyAddedWatched);

            // Update properties    
            int i = 1;
            List<RecentlySeriesItem> items = new List<RecentlySeriesItem>();
            foreach (DBEpisode episode in episodes)
            {
                RecentlySeriesItem ra = new RecentlySeriesItem();
                ra = GetRecentlySeriesItemFromEpisode(episode);
                if (ra == null) continue;
                ra.Mode = RecentlyAddedWatchedMode.Watched;
                logger.WriteLog(string.Format("MPTVSeries: Recently Watched Series/Episode {0} is {1}", i, ra.SeriesTitle), LogLevel.Debug, InfoServiceModul.RecentlyAddedWatched);
                items.Add(ra);
                i++;
                if (i == MaxRecentlyItems + 1) break;
            }
            return items;

        }
        private TimeSpan GetEpisodeRuntime(string runtime)
        {
            if (!string.IsNullOrEmpty(runtime))
            {
                try
                {
                    int runtimeInt = 0;
                    if (Int32.TryParse(runtime, out runtimeInt))
                    {
                       return new TimeSpan(0, runtimeInt / 60000, 0);
                    }
                }
                catch
                {
                }
            }
            return new TimeSpan();
        }
        private DateTime GetDateTime(string datetime)
        {
            if (!string.IsNullOrEmpty(datetime))
            {
                try
                {
                    DateTime newDateTime = new DateTime();
                    if (DateTime.TryParse(datetime, out newDateTime))
                    {
                        return newDateTime;
                    }
                }
                catch
                {
                }
            }
            return new DateTime();
        }
        private string GetRoundedScore(string score)
        {
            if (!string.IsNullOrEmpty(score))
            {
                try
                {
                    Decimal newScore;
                    if (Decimal.TryParse(score, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture,  out newScore))
                    {
                        return Math.Round(newScore, MidpointRounding.AwayFromZero).ToString();
                    }
                }
                catch
                {
                }
            }
            return string.Empty;
            
        }
        private string GetFanartPath(int seriesId)
        {
            List<DBFanart> fanarts = DBFanart.GetAll(seriesId, true);
            if (fanarts.Count > 0)
            {
                return fanarts[_rnd.Next(fanarts.Count - 1)].FullLocalPath;
            }
            return string.Empty;
        }
        private RecentlySeriesItem GetRecentlySeriesItemFromEpisode(DBEpisode episode)
        {
            DBSeries series = Helper.getCorrespondingSeries(episode[DBEpisode.cSeriesID]);

            if (series == null) return null;

            RecentlySeriesItem rs = new RecentlySeriesItem();
            rs.Episode = episode;
            rs.SeriesTitle = series[DBOnlineSeries.cPrettyName];
            rs.EpisodeTitle = episode[DBEpisode.cEpisodeName];
            rs.EpisodeNumber = episode[DBEpisode.cEpisodeIndex];
            rs.Season = episode[DBEpisode.cSeasonIndex];
            rs.Thumb = series.Poster;
            rs.Fanart = GetFanartPath(series[DBSeries.cID]);
            rs.WatchedCount = 0;
            rs.DateAdded = GetDateTime(episode[DBEpisode.cFileDateAdded].ToString());
            rs.Runtime = GetEpisodeRuntime(episode[DBEpisode.cLocalPlaytime].ToString());
            rs.DateWatched = GetDateTime(episode[DBEpisode.cDateWatched].ToString());
            rs.Certification = series[DBOnlineSeries.cContentRating];
            rs.Score = episode[DBOnlineEpisode.cRating];
            rs.RoundedScore = GetRoundedScore(episode[DBOnlineEpisode.cRating]);
            rs.Type = RecentlyAddedWatchedType.Series;
            return rs;
        }

    }
}
*/