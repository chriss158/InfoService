using System.ComponentModel;
using System.Threading;
using InfoService.Feeds;
using InfoService.Utils;

namespace InfoService.BackgroundWorkers
{
    class FeedWorker : BackgroundWorker
    {
        public FeedWorker()
        {
            WorkerReportsProgress = true;
            WorkerSupportsCancellation = true;
            
        }
        protected override void OnDoWork(DoWorkEventArgs e)
        {
            //Set Priority to lowest, so safe some performance for MP
            Thread.CurrentThread.Priority = ThreadPriority.Lowest;

            ReportProgress(0, "Starting feed update...");
            if (!FeedService.UpdateAllFeeds())
            {
                e.Cancel = true;
                e.Result = false;
                return;
            }
            ReportProgress(100, "Finished feed update successful");
            e.Result = true;
        }
    }
}


