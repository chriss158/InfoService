#region Usings

using System;
using System.Threading;
using InfoService.Feeds;
using InfoService.Twitter;
using InfoService.Weather;

#endregion

namespace InfoService.Utils
{
    public static class UpdateTimer
    {
        #region Delegates

        public delegate void UpdatePropertiesHandler();

        #endregion

        private static readonly Logger logger = Logger.GetInstance();

        private static bool _updateInProgress;
        private static TimerCallback _timerDelegate;
        private static Timer _timer;
        private static readonly UpdatePropertiesHandler[] UpdateDelegate = new UpdatePropertiesHandler[4];
        private static event UpdatePropertiesHandler UpdateProperties;
        public static int PeriodMs { get; private set; }
        public static bool StartupFastTimer { get; private set; }

        public static event UpdatePropertiesHandler OnTimeForUpdateProperties
        {
            add
            {
                if (value.Method.DeclaringType.Name == "FeedUpdater")
                {
                    logger.WriteLog("FeedUpdater registered for update timer", LogLevel.Debug, InfoServiceModul.InfoService);
                    UpdateDelegate[1] = value;
                }
                if (value.Method.DeclaringType.Name == "TwitterUpdater")
                {
                    logger.WriteLog("TwitterUpdater registered for update timer", LogLevel.Debug, InfoServiceModul.InfoService);
                    UpdateDelegate[2] = value;
                }
                if (value.Method.DeclaringType.Name == "WeatherUpdater")
                {
                    logger.WriteLog("WeatherUpdater registered for update timer", LogLevel.Debug, InfoServiceModul.InfoService);
                    UpdateDelegate[0] = value;
                }
                if (value.Method.DeclaringType.Name == "RecentlyAddedUpdater")
                {
                    logger.WriteLog("RecentlyAddedUpdater registered for update timer", LogLevel.Debug, InfoServiceModul.InfoService);
                    UpdateDelegate[3] = value;
                }
            }
            remove { UpdateProperties -= value; }
        }
        private static void ChangeTimerSettings(bool startupFastTimer)
        {
            if (startupFastTimer)
            {
                PeriodMs = 1000;
                StartupFastTimer = true;
            }
            else
            {
                PeriodMs = 60000;
                StartupFastTimer = false;
            }
            logger.WriteLog("Set Timer callback to " + PeriodMs + " seconds", LogLevel.Debug, InfoServiceModul.InfoService);
        }
        private static void InfoTimerCallback(Object stateInfo)
        {
            if (InfoServiceCore.InitDone)
            {
                if (_updateInProgress) return;
                if (UpdateProperties == null) return;
                _updateInProgress = true;
                UpdateProperties();
                _updateInProgress = false;
            }
        }

        public static void SetTimer(bool startupFastTimer)
        {
            //Set timer only if min. 1 Service is enabled
            //Not needed since Recently added is always enabled
            //if (FeedService.Enabled || TwitterService.Enabled || WeatherService.Enabled)
            //{
            
            UpdateProperties = (UpdatePropertiesHandler)Delegate.Combine(UpdateDelegate);
            ChangeTimerSettings(startupFastTimer);
            _timerDelegate = InfoTimerCallback;
            _timer = new Timer(_timerDelegate, null, 0, PeriodMs);
            //}
        }
    }
}
