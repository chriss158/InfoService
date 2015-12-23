/*
using System.Collections.Generic;
using InfoService.Enums;
using InfoService.RecentlyAddedWatched.Data;
using MediaPortal.Plugins.MovingPictures.MainUI;
using MediaPortal.Plugins.MovingPictures.Database;
using WindowPlugins.GUITVSeries;

namespace InfoService.Utils
{
    public static class RecentlyAddedWatchedUtils
    {
        private static readonly Logger logger = Logger.GetInstance();

        public static void SetRecentlyAddedWatchedProperties(List<RecentlyMovieItem> items)
        {
            int number = 1;
            const string rType = "movie";
            string mode = string.Empty;
            foreach (RecentlyMovieItem item in items)
            {

                if (item.Type != RecentlyAddedWatchedType.Movie) continue;


                if (item.Mode == RecentlyAddedWatchedMode.Watched) mode = "watched";
                else if (item.Mode == RecentlyAddedWatchedMode.Added) mode = "added";

                PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.RecentlyAddedWatched.Title, mode, rType, number), item.Movie.Title);
                PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.RecentlyAddedWatched.Thumb, mode, rType, number), item.Thumb);
                PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.RecentlyAddedWatched.Fanart, mode, rType, number), item.Fanart);
                PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.RecentlyAddedWatched.DateAdded, mode, rType, number), item.DateAdded.ToShortDateString());
                PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.RecentlyAddedWatched.Runtime, mode, rType, number), item.Runtime.TotalMinutes + " mins");
                PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.RecentlyAddedWatched.Certification, mode, rType, number), item.Certification);
                PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.RecentlyAddedWatched.Score, mode, rType, number), item.Score);
                PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.RecentlyAddedWatched.RoundedScore, mode, rType, number), item.RoundedScore);
                PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.RecentlyAddedWatched.DateWatched, mode, rType, number), item.DateWatched.ToShortDateString());
                PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.RecentlyAddedWatched.WatchedCount, mode, rType, number), item.WatchedCount.ToString());

                number++;
            }
        }
        public static void SetRecentlyAddedWatchedProperties(List<RecentlySeriesItem> items)
        {
            int number = 1;
            const string rType = "series";
            string mode = string.Empty;
            foreach (RecentlySeriesItem item in items)
            {

                if (item.Type != RecentlyAddedWatchedType.Series) continue;
                if (item.Mode == RecentlyAddedWatchedMode.Watched) mode = "watched";
                else if (item.Mode == RecentlyAddedWatchedMode.Added) mode = "added";

                PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.RecentlyAddedWatched.Title, mode, rType, number), item.SeriesTitle);
                PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.RecentlyAddedWatched.Season, mode, rType, number), item.Season);
                PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.RecentlyAddedWatched.Episodenumber, mode, rType, number), item.EpisodeNumber);
                PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.RecentlyAddedWatched.Episodetitle, mode, rType, number), item.EpisodeTitle);

                PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.RecentlyAddedWatched.Thumb, mode, rType, number), item.Thumb);
                PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.RecentlyAddedWatched.Fanart, mode, rType, number), item.Fanart);
                PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.RecentlyAddedWatched.DateAdded, mode, rType, number), item.DateAdded.ToShortDateString());
                PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.RecentlyAddedWatched.Runtime, mode, rType, number), item.Runtime.TotalMinutes + " mins");
                PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.RecentlyAddedWatched.Certification, mode, rType, number), item.Certification);
                PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.RecentlyAddedWatched.Score, mode, rType, number), item.Score);
                PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.RecentlyAddedWatched.RoundedScore, mode, rType, number), item.RoundedScore);
                PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.RecentlyAddedWatched.DateWatched, mode, rType, number), item.DateWatched.ToShortDateString());
                PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.RecentlyAddedWatched.WatchedCount, mode, rType, number), item.WatchedCount.ToString());

                number++;

            }
        }
        public static void PlayMovie(DBMovieInfo movie)
        {
            MoviePlayer moviePlayer = new MoviePlayer(new MovingPicturesGUI());
            moviePlayer.Play(movie);
        }
        public static void PlayEpisode(DBEpisode episode)
        {
            VideoHandler episodePlayer = new VideoHandler();
            episodePlayer.ResumeOrPlay(episode);
        }
    }
}
*/