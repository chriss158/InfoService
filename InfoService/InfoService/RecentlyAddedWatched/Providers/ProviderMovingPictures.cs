/*
using System;
using System.Collections.Generic;
using System.Linq;
using Cornerstone.Tools;
using Cornerstone.Database;
using Cornerstone.Database.Tables;
using InfoService.Enums;
using InfoService.RecentlyAddedWatched.Data;
using InfoService.RecentlyAddedWatched.Interfaces;
using InfoService.Utils;
using MediaPortal.Plugins.MovingPictures;
using MediaPortal.Plugins.MovingPictures.Database;


namespace InfoService.RecentlyAddedWatched.Providers
{
    public class ProviderMovingPictures : IRecentlyAddedWatchedProvider<RecentlyMovieItem>
    {
        private static readonly Logger logger = Logger.GetInstance();

        private static List<RecentlyMovieItem> MoviesAdded;
        private static List<RecentlyMovieItem> MoviesWatched;
        private const int MaxRecentlyItems = 3;

        public event EventHandler<NewItemHandler<RecentlyMovieItem>> OnNewItem;
    
        public ProviderMovingPictures()
        {
            logger.WriteLog("MovingPictures: Init MovingPictures  RecentlyAddedWatchedProvider", LogLevel.Info, InfoServiceModul.RecentlyAddedWatched);
            MovingPicturesCore.DatabaseManager.ObjectInserted += new DatabaseManager.ObjectAffectedDelegate(DatabaseManager_ObjectInserted);
            MoviesWatched = new List<RecentlyMovieItem>();
            MoviesAdded = new List<RecentlyMovieItem>();
            foreach (RecentlyMovieItem movie in RetrieveRecentlyAddedFromPlugin())
            {
                AddNewItem(movie);
            }
            foreach (RecentlyMovieItem movie in RetrieveRecentlyWatchedFromPlugin())
            {
                AddNewItem(movie);
            }

        }
        public List<RecentlyMovieItem> GetRecentlyAdded()
        {
            return MoviesAdded;
        }
        public List<RecentlyMovieItem> GetRecentlyWatched()
        {
            return MoviesWatched;
        }

        private void AddNewItem(RecentlyMovieItem item)
        {
            if (item != null)
            {
                if (item.Mode == RecentlyAddedWatchedMode.Watched)
                {
                    logger.WriteLog(string.Format("MovingPictures: New movie watched: {0}", item.Movie.Title), LogLevel.Info, InfoServiceModul.RecentlyAddedWatched);
                    MoviesWatched.Insert(0, item);
                    if (MoviesWatched.Count > MaxRecentlyItems) MoviesWatched.RemoveAt(MaxRecentlyItems);
                }
                else if (item.Mode == RecentlyAddedWatchedMode.Added)
                {
                    logger.WriteLog(string.Format("MovingPictures: New movie added: {0}", item.Movie.Title), LogLevel.Info, InfoServiceModul.RecentlyAddedWatched);
                    MoviesAdded.Insert(0, item);
                    if (MoviesAdded.Count > MaxRecentlyItems) MoviesAdded.RemoveAt(MaxRecentlyItems);
                }
            }
        }

        private void DatabaseManager_ObjectInserted(DatabaseTable obj)
        {

            if (obj.GetType() == typeof(DBMovieInfo))
            {
                DBMovieInfo movie = (DBMovieInfo) obj;
                RecentlyMovieItem ra = new RecentlyMovieItem();
                ra = GetRecentlyMovieItemFromEpisode(movie);
                ra.Mode = RecentlyAddedWatchedMode.Added;
                AddNewItem(ra);
                if (OnNewItem != null) OnNewItem(this, new NewItemHandler<RecentlyMovieItem>(ra));

                
            }
            else if(obj.GetType() == typeof(DBWatchedHistory))
            {
                DBWatchedHistory movie = (DBWatchedHistory) obj;
                RecentlyMovieItem ra = new RecentlyMovieItem();
                ra = GetRecentlyMovieItemFromEpisode(movie.Movie);
                ra.Mode = RecentlyAddedWatchedMode.Watched;
                AddNewItem(ra);
                if (OnNewItem != null) OnNewItem(this, new NewItemHandler<RecentlyMovieItem>(ra));
                
            }
        }

        private List<RecentlyMovieItem> RetrieveRecentlyAddedFromPlugin()
        {
            logger.WriteLog("MovingPictures: Get Most Recent Added Movies", LogLevel.Info, InfoServiceModul.RecentlyAddedWatched);

            // get list of movies in database
            List<DBMovieInfo> movies = DBMovieInfo.GetAll();



            // get filter criteria of movies protected by parental conrols
            bool pcFilterEnabled = MovingPicturesCore.Settings.ParentalControlsEnabled;
            DBFilter<DBMovieInfo> pcFilter = MovingPicturesCore.Settings.ParentalControlsFilter;

            // apply parental control filter to movie list
            List<DBMovieInfo> filteredMovies = pcFilterEnabled ? pcFilter.Filter(movies).ToList() : movies;

            logger.WriteLog(string.Format("MovingPictures: {0} Movies found in database", movies.Count.ToString()), LogLevel.Info, InfoServiceModul.RecentlyAddedWatched);
            logger.WriteLog(string.Format("MovingPictures: {0} Movies filtered by parental controls", movies.Count - filteredMovies.Count), LogLevel.Info, InfoServiceModul.RecentlyAddedWatched);

            // Sort list in to most recent first
            filteredMovies.Sort((m1, m2) =>
            {
                return m2.DateAdded.CompareTo(m1.DateAdded);
            });


            // Update properties    
            int i = 1;
            List<RecentlyMovieItem> items = new List<RecentlyMovieItem>();
            foreach (DBMovieInfo movie in filteredMovies)
            {
                RecentlyMovieItem ra = new RecentlyMovieItem();
                ra = GetRecentlyMovieItemFromEpisode(movie);
                ra.Mode = RecentlyAddedWatchedMode.Added;
                logger.WriteLog(string.Format("MovingPictures: Recently Added Movie {0} is {1}", i, movie.Title), LogLevel.Debug, InfoServiceModul.RecentlyAddedWatched);
                items.Add(ra);
                i++;
                if (i == MaxRecentlyItems + 1) break;
            }
            return items;
        }


        private List<RecentlyMovieItem> RetrieveRecentlyWatchedFromPlugin()
        {
            logger.WriteLog("Moving Pictures: Get Most Recent Watched Movies", LogLevel.Info, InfoServiceModul.RecentlyAddedWatched);

            // get list of movies in database
            List<DBMovieInfo> movies = DBMovieInfo.GetAll();

            // get filter criteria of movies protected by parental conrols
            bool pcFilterEnabled = MovingPicturesCore.Settings.ParentalControlsEnabled;
            DBFilter<DBMovieInfo> pcFilter = MovingPicturesCore.Settings.ParentalControlsFilter;

            // apply parental control filter to movie list
            List<DBMovieInfo> filteredMovies = pcFilterEnabled ? pcFilter.Filter(movies).ToList() : movies;

            // sort based on most recently watched
            filteredMovies.Sort((m1, m2) =>
            {
                // get watched count for each movie         
                int watchCount1 = m1.WatchedHistory.Count;
                int watchCount2 = m2.WatchedHistory.Count;

                // compare most recently watched dates
                // WatchedHistory stores a list of dates each time the movie was watched
                DateTime lastWatchDate1 = watchCount1 > 0 ? m1.WatchedHistory[watchCount1 - 1].DateWatched : DateTime.MinValue;
                DateTime lastWatchDate2 = watchCount2 > 0 ? m2.WatchedHistory[watchCount2 - 1].DateWatched : DateTime.MinValue;
                return lastWatchDate2.CompareTo(lastWatchDate1);
            });


            int i = 1;
            List<RecentlyMovieItem> items = new List<RecentlyMovieItem>();
            foreach (DBMovieInfo movie in filteredMovies)
            {
                if (movie.WatchedHistory.Count > 0)
                {
                    RecentlyMovieItem ra = new RecentlyMovieItem();
                    ra = GetRecentlyMovieItemFromEpisode(movie);
                    ra.Mode = RecentlyAddedWatchedMode.Watched;
                    items.Add(ra);
                    logger.WriteLog(string.Format("Moving Pictures: Recently Watched Movie {0} is {1}", i, movie.Title), LogLevel.Debug, InfoServiceModul.RecentlyAddedWatched);
                    i++;
                    if (i == MaxRecentlyItems + 1)  break;
                }
            }
            return items;

        }

        private RecentlyMovieItem GetRecentlyMovieItemFromEpisode(DBMovieInfo movie)
        {
            RecentlyMovieItem ra = new RecentlyMovieItem();
            ra.Movie = movie;
            ra.Thumb = movie.CoverThumbFullPath;
            ra.Fanart = movie.BackdropFullPath;
            ra.DateAdded = movie.DateAdded;
            ra.Runtime = GetMovieRuntime(movie);
            ra.Certification = movie.Certification;
            ra.Score = movie.Score.ToString();
            ra.RoundedScore = Math.Round(movie.Score, MidpointRounding.AwayFromZero).ToString();
            ra.Mode = RecentlyAddedWatchedMode.Watched;
            ra.Type = RecentlyAddedWatchedType.Movie;
            return ra;
        }

        private static TimeSpan GetMovieRuntime(DBMovieInfo movie)
        {
            int minutes = 0;
            if (movie == null) return new TimeSpan();

            if (MovingPicturesCore.Settings.DisplayActualRuntime && movie.ActualRuntime > 0)
            {
                // Actual Runtime or (MediaInfo result) is in milliseconds
                // convert to minutes
                minutes = ((movie.ActualRuntime/1000)/60);
            }
            else
                minutes = movie.Runtime;

            return new TimeSpan(0, 0, minutes, 0, 0);
        }
    }
}
*/