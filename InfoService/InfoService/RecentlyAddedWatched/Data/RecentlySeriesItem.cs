/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowPlugins.GUITVSeries;

namespace InfoService.RecentlyAddedWatched.Data
{
    public class RecentlySeriesItem : RecentlyItem
    {
        public string SeriesTitle { get; set; }
        public string EpisodeTitle { get; set; }
        public string Season { get; set; }
        public string EpisodeNumber { get; set; }
        public DBEpisode Episode { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(RecentlySeriesItem)) return false;
            RecentlySeriesItem rsi = (RecentlySeriesItem)obj;

            if (rsi.SeriesTitle != this.SeriesTitle) return false;
            if (rsi.EpisodeNumber != this.EpisodeNumber) return false;
            if (rsi.EpisodeTitle != this.EpisodeTitle) return false;
            if (rsi.Season != this.Season) return false;
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
*/