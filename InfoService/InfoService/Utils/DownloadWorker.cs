using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MediaPortal.GUI.Library;

namespace InfoService.Utils
{
    public static class DownloadWorker
    {
        public delegate void StartDownloadHandler(bool updateWeather, bool updateTwitter, bool updateFeeds);
        public static event StartDownloadHandler StartDownload;

        private static bool _workerCompleted;
        private static bool _updateWeather;
        private static bool _updateFeeds;
        private static bool _updateTwitter;
        private static readonly object DownloadLock;

        static DownloadWorker()
        {
            DownloadLock = new object();
        }

        private static void ThreadDownloadWorker()
        {
            _workerCompleted = false;
            lock (DownloadLock)
            {
                if (StartDownload != null)
                {
                    StartDownload(_updateWeather, _updateTwitter, _updateFeeds);
                }
            }
            _workerCompleted = true;
        }

        public static void StartDownloadWorker(bool updateWeather, bool updateTwitter, bool updateFeeds)
        {
            _updateTwitter = updateTwitter;
            _updateWeather = updateWeather;
            _updateFeeds = updateFeeds;

            Thread updateThread = new Thread(ThreadDownloadWorker) { IsBackground = true, Name = "Infoservice updater" };

            updateThread.Start();

            while (_workerCompleted == false)
            {
                GUIWindowManager.Process();
            }
        }
    }
}
