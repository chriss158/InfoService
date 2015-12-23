/*
using System;
using InfoService.Enums;
using InfoService.RecentlyAddedWatched.Data;
using InfoService.RecentlyAddedWatched.Providers;
using InfoService.Utils;
using MediaPortal.GUI.Library;
using Action = MediaPortal.GUI.Library.Action;
using System.Collections.Generic;

namespace InfoService.RecentlyAddedWatched
{
    public static class RecentlyAddedWatchedUpdater
    {
        private static readonly Logger logger = Logger.GetInstance();

        private static Providers.ProviderMovingPictures mopi;
        private static Providers.ProviderMPTVSeries mptv;
        //private static Providers.ProviderRecordings recs;

        public static void SetupUpdater()
        {
            GUIWindowManager.OnNewAction += new OnActionHandler(GUIWindowManager_OnNewAction);

            if (InfoServiceUtils.IsAssemblyAvailable("MovingPictures", new Version(1, 0, 6, 1116)))
            {
                mopi = new ProviderMovingPictures();
                mopi.OnNewItem += new EventHandler<NewItemHandler<RecentlyMovieItem>>(mopi_OnNewItem);
                
                RecentlyAddedWatchedUtils.SetRecentlyAddedWatchedProperties(mopi.GetRecentlyAdded());
                RecentlyAddedWatchedUtils.SetRecentlyAddedWatchedProperties(mopi.GetRecentlyWatched());
            }
            if (InfoServiceUtils.IsAssemblyAvailable("MP-TVSeries", new Version(3, 0, 0, 1389)))
            {
                mptv = new ProviderMPTVSeries();
                mptv.OnNewItem += new EventHandler<NewItemHandler<RecentlySeriesItem>>(mptv_OnNewItem);

                RecentlyAddedWatchedUtils.SetRecentlyAddedWatchedProperties(mptv.GetRecentlyAdded());
                RecentlyAddedWatchedUtils.SetRecentlyAddedWatchedProperties(mptv.GetRecentlyWatched());
            }
           // if (InfoServiceUtils.IsAssemblyAvailable("TvPlugin", new Version(1, 1, 2, 27649)))
            //{
               // recs = new ProviderRecordings();
            //}

        }

        static void mptv_OnNewItem(object sender, NewItemHandler<RecentlySeriesItem> e)
        {
            if (e.ItemDetails.Mode == RecentlyAddedWatchedMode.Added) RecentlyAddedWatchedUtils.SetRecentlyAddedWatchedProperties(mptv.GetRecentlyAdded());
            else if (e.ItemDetails.Mode == RecentlyAddedWatchedMode.Watched) RecentlyAddedWatchedUtils.SetRecentlyAddedWatchedProperties(mptv.GetRecentlyWatched());
        }

        static void mopi_OnNewItem(object sender, NewItemHandler<RecentlyMovieItem> e)
        {
            if (e.ItemDetails.Mode == RecentlyAddedWatchedMode.Added) RecentlyAddedWatchedUtils.SetRecentlyAddedWatchedProperties(mopi.GetRecentlyAdded());
            else if (e.ItemDetails.Mode == RecentlyAddedWatchedMode.Watched) RecentlyAddedWatchedUtils.SetRecentlyAddedWatchedProperties(mopi.GetRecentlyWatched());
        }

        static void GUIWindowManager_OnNewAction(MediaPortal.GUI.Library.Action action)
        {
            if(action.wID == Action.ActionType.ACTION_SELECT_ITEM && GUIWindowManager.ActiveWindow == 35) // 35 = Basichome
            {
                logger.WriteLog("Getting Basichome GUIWindow...", LogLevel.Debug, InfoServiceModul.RecentlyAddedWatched);
                GUIWindow fWindow = GUIWindowManager.GetWindow(GUIWindowManager.ActiveWindow);
                if (fWindow != null)
                {
                    logger.WriteLog("Basichome Window found...", LogLevel.Debug, InfoServiceModul.RecentlyAddedWatched);
                    int id = fWindow.GetFocusControlId();
                    if (id == 50000) //This should be the ID of the first Recently Added Movie control
                    {
                        logger.WriteLog("Recently Added Movie Item 1 selected", LogLevel.Info, InfoServiceModul.RecentlyAddedWatched);
                        PlayRecentlyAddedMovie(0);
                    }
                    else if (id == 50001) //This should be the ID of the second Recently Added Movie control
                    {
                        logger.WriteLog("Recently Added Movie Item 2 selected", LogLevel.Info, InfoServiceModul.RecentlyAddedWatched);
                        PlayRecentlyAddedMovie(1);
                    }
                    else if (id == 50002) //This should be the ID of the third Recently Added Movie control
                    {
                        logger.WriteLog("Recently Added Movie Item 3 selected", LogLevel.Info, InfoServiceModul.RecentlyAddedWatched);
                        PlayRecentlyAddedMovie(2);
                    }
                    else if (id == 60000) //This should be the ID of the third Recently Added Series control
                    {
                        logger.WriteLog("Recently Added Series Item 1 selected", LogLevel.Info, InfoServiceModul.RecentlyAddedWatched);
                        PlayRecentlyAddedEpisode(0);
                    }
                    else if (id == 60001) //This should be the ID of the third Recently Added Series control
                    {
                        logger.WriteLog("Recently Added Series Item 2 selected", LogLevel.Info, InfoServiceModul.RecentlyAddedWatched);
                        PlayRecentlyAddedEpisode(1);
                    }
                    else if (id == 60002) //This should be the ID of the third Recently Added Series control
                    {
                        logger.WriteLog("Recently Added Series Item 3 selected", LogLevel.Info, InfoServiceModul.RecentlyAddedWatched);
                        PlayRecentlyAddedEpisode(2);
                    }
                }
            }
            
        }
        private static void PlayRecentlyAddedEpisode(int id)
        {
            if (mptv != null)
            {
                List<RecentlySeriesItem> recentlySeries = mptv.GetRecentlyAdded();
                if (recentlySeries.Count > id) RecentlyAddedWatchedUtils.PlayEpisode(mptv.GetRecentlyAdded()[id].Episode);
            }
        }
        private static void PlayRecentlyAddedMovie(int id)
        {
            if (mopi != null)
            {
                List<RecentlyMovieItem> recentlyMovies = mopi.GetRecentlyAdded();
                if (recentlyMovies.Count > id) RecentlyAddedWatchedUtils.PlayMovie(mopi.GetRecentlyAdded()[id].Movie);
            }
        }
    }
}
*/