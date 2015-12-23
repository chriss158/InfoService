using System.ComponentModel;
using System.Threading;
using InfoService.Twitter;

namespace InfoService.BackgroundWorkers
{
    class TwitterWorker : BackgroundWorker
    {
        public TwitterWorker()
        {
            WorkerReportsProgress = true;
            WorkerSupportsCancellation = true;
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            //Set Priority to lowest, so safe some performance for MP
            Thread.CurrentThread.Priority = ThreadPriority.Lowest;

            ReportProgress(0, "Starting twitter update...");
            if (!TwitterService.UpdateTwitter())
            {
                e.Cancel = true;
                e.Result = false;
                return;
            }
            ReportProgress(100, "Finished twitter update successful");
            e.Result = true;
        }
    }
}


