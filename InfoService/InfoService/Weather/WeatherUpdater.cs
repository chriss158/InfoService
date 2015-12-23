using System;
using InfoService.Utils;
using MediaPortal.GUI.Library;
using Action = MediaPortal.GUI.Library.Action;
using Window = MediaPortal.GUI.Library.GUIWindow.Window;

namespace InfoService.Weather
{
    public static class WeatherUpdater
    {
        private static System.Threading.TimerCallback timerDelegate;
        private static System.Threading.Timer timer;
        private static readonly Logger logger = Logger.GetInstance();
        public static void SetupUpdater()
        {
            UpdateTimer.OnTimeForUpdateProperties += new UpdateTimer.UpdatePropertiesHandler(UpdateTimer_UpdateProperties);
            GUIWindowManager.OnDeActivateWindow += new GUIWindowManager.WindowActivationHandler(GUIWindowManager_OnDeActivateWindow);
            //GUIGraphicsContext.OnNewAction += new OnActionHandler(GUIGraphicsContext_OnNewAction);
        }

        private static void GUIWindowManager_OnDeActivateWindow(int windowId)
        {
            if (windowId == (int)Window.WINDOW_WEATHER)
            {
                logger.WriteLog("MPWeatherGUI has been closed. Save mediaportal.xml to get new city code...", LogLevel.Debug, InfoServiceModul.Weather);
                timerDelegate = saveSettings;
                timer = new System.Threading.Timer(timerDelegate, null, 1000, System.Threading.Timeout.Infinite);
            }
        }

        private static void GUIGraphicsContext_OnNewAction(Action action)
        {
            int actwin = GUIWindowManager.ActiveWindowEx;
            if (actwin == 2600) //Weather screen
            {
                if (action.wID == Action.ActionType.ACTION_PREVIOUS_MENU)
                {
                    logger.WriteLog("MPWeatherGUI has been closed. Save mediaportal.xml to get new city code...", LogLevel.Debug, InfoServiceModul.Weather);
                    timerDelegate = saveSettings;
                    timer = new System.Threading.Timer(timerDelegate, null, 1000, System.Threading.Timeout.Infinite);
                }
            }
        }
        private static void saveSettings(Object stateInfo)
        {
            if (WeatherService.Enabled)
            {
                logger.WriteLog("Saving mediaportal.xml and updating weather with new city code...", LogLevel.Debug, InfoServiceModul.Weather);
                MediaPortal.Profile.Settings.SaveCache();
                WeatherUtils.UpdateWeatherData();
            }
        }

        static void UpdateTimer_UpdateProperties()
        {
            if (!WeatherService.UpdateOnStartup)
            {
                WeatherService.LastRefresh = DateTime.Now;
                WeatherService.UpdateOnStartup = true;
            }
            TimeSpan spanWeather = DateTime.Now - WeatherService.LastRefresh;

            if (InfoServiceCore.InitDone && WeatherService.Enabled)
            {
                if (((int)spanWeather.TotalMinutes) >= WeatherService.RefreshInterval)
                {
                    WeatherUtils.UpdateWeatherData();
                }

            }
        }
    }
}
