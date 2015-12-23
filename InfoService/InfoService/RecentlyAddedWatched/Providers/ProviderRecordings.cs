/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InfoService.RecentlyAddedWatched.Data;
using InfoService.RecentlyAddedWatched.Interfaces;
using InfoService.Utils;

namespace InfoService.RecentlyAddedWatched.Providers
{
    public class ProviderRecordings : IRecentlyAddedWatchedProvider<RecentlyRecordItem>
    {
        private static readonly Logger logger = Logger.GetInstance();
        private static List<RecentlyRecordItem> RecordingsAdded;
        private const int MaxRecentlyItems = 3;

        public ProviderRecordings()
        {
            logger.WriteLog("TVRecordings: InitTVRecordings RecentlyAddedWatchedProvider", LogLevel.Info, InfoServiceModul.RecentlyAddedWatched);

            RecordingsAdded = RetrieveRecentlyAddedFromPlugin();
            
        }
        public List<RecentlyRecordItem> GetRecentlyAdded()
        {
            return RecordingsAdded;
        }

        public List<RecentlyRecordItem> GetRecentlyWatched()
        {
            logger.WriteLog("TVRecordings: No recently watched for Recordings", LogLevel.Info, InfoServiceModul.RecentlyAddedWatched);
            return new List<RecentlyRecordItem>();
        }


        public List<RecentlyRecordItem> RetrieveRecentlyAddedFromPlugin()
        {
            logger.WriteLog("TVRecordings: Get Most Recent Added Recordings", LogLevel.Info, InfoServiceModul.RecentlyAddedWatched);
            List<RecentlyRecordItem> rri = new List<RecentlyRecordItem>();
            IList<Recording> recs = Recording.ListAll();
            logger.WriteLog(string.Format("TVRecordings: {0} Recordings found in database", recs.Count.ToString()), LogLevel.Info, InfoServiceModul.RecentlyAddedWatched);
            recs = recs.OrderBy(r => r.StartTime).ToList<Recording>();
            int i = 1;
            foreach (Recording rec in recs)
            {
                RecentlyRecordItem record = GetRecentlyRecordItemFromEpisode(rec);
                logger.WriteLog(string.Format("TVRecordings: Recently Added Recordings {0} is {1}", i, record.Title), LogLevel.Debug, InfoServiceModul.RecentlyAddedWatched);
                rri.Add(record);
                i++;
                if (i == MaxRecentlyItems + 1) break;
            }

            return rri;
        }
        public event EventHandler<NewItemHandler<RecentlyRecordItem>> OnNewItem;

        private RecentlyRecordItem GetRecentlyRecordItemFromEpisode(Recording rec)
        {
            RecentlyRecordItem record = new RecentlyRecordItem();
            record.Filename = rec.FileName;
            record.DateAdded = rec.StartTime;
            record.Mode = Enums.RecentlyAddedWatchedMode.Added;
            record.Runtime = rec.EndTime - rec.StartTime;
            record.Title = rec.Title;
            record.Genre = rec.Genre;
            record.Description = rec.Description;
            return record;
        }
    }
}
*/