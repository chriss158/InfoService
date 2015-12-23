using System.ComponentModel;
using System.Threading;
using InfoService.Weather;

namespace InfoService.BackgroundWorkers
{
    public class WeatherWorker : BackgroundWorker
    {
        private volatile string _locationId;

        public WeatherWorker()
        {
            WorkerReportsProgress = true;
            WorkerSupportsCancellation = true;
        }



        public WeatherWorker(string locationId)
            : this()
        {
            this._locationId = locationId;
        }



        protected override void OnDoWork(DoWorkEventArgs e)
        {
            //Set Priority to lowest, so safe some performance for MP
            Thread.CurrentThread.Priority = ThreadPriority.Lowest;

            ReportProgress (0, "Starting weather update...");
            if(!WeatherService.UpdateWeatherData(_locationId))
            {
                e.Cancel = true;
                e.Result = false;
                return;
            }
            ReportProgress (100, "Finished weather update successful");
            e.Result = true;
        }
    }
}


