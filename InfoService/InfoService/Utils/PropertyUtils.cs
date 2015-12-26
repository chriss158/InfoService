using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaPortal.GUI.Library;

namespace InfoService.Utils
{
    public static class PropertyUtils
    {
        private static readonly Logger logger = Logger.GetInstance();
        public struct Properties
        {
            #region Weather
            public struct Weather
            {
                public const string Enabled = "#infoservice.weather.enabled";
                public const string Location = "#infoservice.weather.location";
            }
            #endregion
            #region Weather Today Properties
            public struct TodayWeather
            {
                public const string Temp = "#infoservice.weather.today.temp";
                public const string Feelsliketemp = "#infoservice.weather.today.feelsliketemp";
                public const string Humidity = "#infoservice.weather.today.humidity";
                public const string Uvindex = "#infoservice.weather.today.uvindex";
                public const string Condition = "#infoservice.weather.today.condition";
                public const string Sunrise = "#infoservice.weather.today.sunrise";
                public const string Sunset = "#infoservice.weather.today.sunset";
                public const string Wind = "#infoservice.weather.today.wind";
                //public const string Mintemp = "#infoservice.weather.today.mintemp";
                //public const string Maxtemp = "#infoservice.weather.today.maxtemp";
                public const string ImgSmallFullPath = "#infoservice.weather.today.img.small.fullpath";
                public const string ImgSmallFilenamewithext = "#infoservice.weather.today.img.small.filenamewithext";
                public const string ImgSmallFilenamewithoutext = "#infoservice.weather.today.img.small.filenamewithoutext";
                public const string ImgBigFullPath = "#infoservice.weather.today.img.big.fullpath";
                public const string ImgBigFilenamewithext = "#infoservice.weather.today.img.big.filenamewithext";
                public const string ImgBigFilenamewithoutext = "#infoservice.weather.today.img.big.filenamewithoutext";
                public const string Weekday = "#infoservice.weather.today.weekday";
                public const string LastupdatedMessage = "#infoservice.weather.lastupdated.message";
                public const string LastupdatedDatetime = "#infoservice.weather.lastupdated.datetime";
            }
            #endregion

            #region Weather Forecast Properties
            public struct ForecastWeather
            {
                public const string Mintemp = "#infoservice.weather.forecast{0}.mintemp";
                public const string Maxtemp = "#infoservice.weather.forecast{0}.maxtemp";
                public const string Sunrise = "#infoservice.weather.forecast{0}.sunrise";
                public const string Sunset = "#infoservice.weather.forecast{0}.sunset";
                public const string DayCondition = "#infoservice.weather.forecast{0}.day.condition";
                public const string NightCondition = "#infoservice.weather.forecast{0}.night.condition";
                public const string DayWind = "#infoservice.weather.forecast{0}.day.wind";
                public const string NightWind = "#infoservice.weather.forecast{0}.night.wind";
                public const string DayHumidity = "#infoservice.weather.forecast{0}.day.humidity";
                public const string NightHumidity = "#infoservice.weather.forecast{0}.night.humidity";
                public const string DayImgSmallFullpath = "#infoservice.weather.forecast{0}.day.img.small.fullpath";
                public const string DayImgSmallFilenamewithext = "#infoservice.weather.forecast{0}.day.img.small.filenamewithext";
                public const string DayImgSmallFilenamewithoutext = "#infoservice.weather.forecast{0}.day.img.small.filenamewithoutext";
                public const string DayImgBigFullpath = "#infoservice.weather.forecast{0}.day.img.big.fullpath";
                public const string DayImgBigFilenamewithext = "#infoservice.weather.forecast{0}.day.img.big.filenamewithext";
                public const string DayImgBigFilenamewithoutext = "#infoservice.weather.forecast{0}.day.img.big.filenamewithoutext";
                public const string NightImgSmallFullpath = "#infoservice.weather.forecast{0}.night.img.small.fullpath";
                public const string NightImgSmallFilenamewithext = "#infoservice.weather.forecast{0}.night.img.small.filenamewithext";
                public const string NightImgSmallFilenamewithoutext = "#infoservice.weather.forecast{0}.night.img.small.filenamewithoutext";
                public const string NightImgBigFullpath = "#infoservice.weather.forecast{0}.night.img.big.fullpath";
                public const string NightImgBigFilenamewithext = "#infoservice.weather.forecast{0}.night.img.big.filenamewithext";
                public const string NightImgBigFilenamewithoutext = "#infoservice.weather.forecast{0}.night.img.big.filenamewithoutext";
                public const string Weekday = "#infoservice.weather.forecast{0}.weekday";
            }
            #endregion

            #region Feed Properties
            public struct Feed
            {
                public const string Enabled = "#infoservice.feed.enabled";
                public const string Isalltitles = "#infoservice.feed.isalltitles";
                public const string SelectedItemimg = "#infoservice.feed.selected.itemimg";
                public const string SelectedTitle = "#infoservice.feed.selected.title";
                public const string SelectedSourcefeed = "#infoservice.feed.selected.sourcefeed";
                public const string SelectedItemage = "#infoservice.feed.selected.itemage";
                public const string Titles = "#infoservice.feed.titles";
                public const string Alltitles = "#infoservice.feed.alltitles";
                public const string SelectedFeed = "#infoservice.feed.selected.feed";
                public const string SelectedType = "#infoservice.feed.selected.type";
                public const string SelectedIndex = "#infoservice.feed.selected.index";
                public const string SelectedDescription = "#infoservice.feed.selected.description";
                public const string ItemCount = "#infoservice.feed.itemcount";
                public const string ItemType = "#infoservice.feed.itemtype";
                public const string Img = "#infoservice.feed.img";
                public const string Separator = "#infoservice.feed.separator";
                public const string SeparatorAll = "#infoservice.feed.separatorall";
                public const string LastupdatedMessage = "#infoservice.feed.lastupdated.message";
                public const string LastupdatedDatetime = "#infoservice.feed.lastupdated.datetime";
                public const string TitlesOfFeedIndex = "#infoservice.feed.{0}.titles";
                public const string ImgOfFeedIndex = "#infoservice.feed.{0}.img";
            }
            #endregion

            #region Twitter Properties
            public struct Twitter
            {
                public const string Enabled = "#infoservice.twitter.enabled";
                public const string Messages = "#infoservice.twitter.messages";
                public const string Separator = "#infoservice.twitter.separator";
                public const string ItemCount = "#infoservice.twitter.itemcount";
                public const string ItemType = "#infoservice.twitter.itemtype";
                public const string LastupdatedMessage = "#infoservice.twitter.lastupdated.message";
                public const string LastupdatedDatetime = "#infoservice.twitter.lastupdated.datetime";
                public const string SelectedUsername = "#infoservice.twitter.selected.username";
                public const string SelectedUserpicture = "#infoservice.twitter.selected.userpicture";
                public const string SelectedMediaImage = "#infoservice.twitter.selected.mediaimage";
                public const string SelectedTimeline = "#infoservice.twitter.selected.timeline";
                public const string SelectedIndex = "#infoservice.twitter.selected.index";
            }
            #endregion

            #region Recently Added/Wachted Properties
            public struct RecentlyAddedWatched
            {
                public const string Title = "#infoservice.recently.{0}.{1}{2}.title";
                public const string Season = "#infoservice.recently.{0}.{1}{2}.season";
                public const string Episodenumber = "#infoservice.recently.{0}.{1}{2}.episodenumber";
                public const string Episodetitle = "#infoservice.recently.{0}.{1}{2}.episodetitle";
                public const string Thumb = "#infoservice.recently.{0}.{1}{2}.thumb";
                public const string Fanart = "#infoservice.recently.{0}.{1}{2}.fanart";
                public const string DateAdded = "#infoservice.recently.{0}.{1}{2}.dateadded";
                public const string Runtime = "#infoservice.recently.{0}.{1}{2}.runtime";
                public const string Certification = "#infoservice.recently.{0}.{1}{2}.certification";
                public const string Score = "#infoservice.recently.{0}.{1}{2}.score";
                public const string RoundedScore = "#infoservice.recently.{0}.{1}{2}.roundedscore";
                public const string WatchedCount = "#infoservice.recently.{0}.{1}{2}.watchedcount";
                public const string DateWatched = "#infoservice.recently.{0}.{1}{2}.datewatched";
            }
            #endregion
        }

        public static void SetProperty(string property, string value)
        {
            if (string.IsNullOrEmpty(value))
                value = " ";
            GUIPropertyManager.SetProperty(property, value);
            if (GUIPropertyManager.Changed) logger.WriteLog("Set property \"" + property + "\" to \"" + value + "\" successful", LogLevel.Debug, InfoServiceModul.InfoService);
            else logger.WriteLog("Set property \"" + property + "\" to \"" + value + "\" failed", LogLevel.Warning, InfoServiceModul.InfoService);
        }

        public static void InitAllProperties()
        {
            logger.WriteLog("Init all properties", LogLevel.Debug, InfoServiceModul.InfoService);
            SetProperty(Properties.Weather.Enabled, "false");
            SetProperty(Properties.Weather.Location, string.Empty);
            SetProperty(Properties.TodayWeather.Temp, string.Empty);
            SetProperty(Properties.TodayWeather.Feelsliketemp, string.Empty);
            SetProperty(Properties.TodayWeather.Humidity, string.Empty);
            SetProperty(Properties.TodayWeather.Uvindex, string.Empty);
            SetProperty(Properties.TodayWeather.Condition, string.Empty);
            SetProperty(Properties.TodayWeather.Sunrise, string.Empty);
            SetProperty(Properties.TodayWeather.Sunset, string.Empty);
            SetProperty(Properties.TodayWeather.Wind, string.Empty);
            //SetProperty(Properties.TodayWeather.Mintemp, string.Empty);
            //SetProperty(Properties.TodayWeather.Maxtemp, string.Empty);
            SetProperty(Properties.TodayWeather.ImgSmallFullPath, string.Empty);
            SetProperty(Properties.TodayWeather.ImgSmallFilenamewithext, string.Empty);
            SetProperty(Properties.TodayWeather.ImgSmallFilenamewithoutext, string.Empty);
            SetProperty(Properties.TodayWeather.ImgBigFullPath, string.Empty);
            SetProperty(Properties.TodayWeather.ImgBigFilenamewithext, string.Empty);
            SetProperty(Properties.TodayWeather.ImgBigFilenamewithoutext, string.Empty);
            SetProperty(Properties.TodayWeather.Weekday, string.Empty);
            SetProperty(Properties.TodayWeather.LastupdatedMessage, string.Empty);
            SetProperty(Properties.TodayWeather.LastupdatedDatetime, string.Empty);

            for (int daynum = 1; daynum <= 5; daynum++)
            {
                SetProperty(string.Format(Properties.ForecastWeather.Mintemp, daynum), string.Empty);
                SetProperty(string.Format(Properties.ForecastWeather.Maxtemp, daynum), string.Empty);
                SetProperty(string.Format(Properties.ForecastWeather.Sunrise, daynum), string.Empty);
                SetProperty(string.Format(Properties.ForecastWeather.Mintemp, daynum), string.Empty);
                SetProperty(string.Format(Properties.ForecastWeather.DayCondition, daynum), string.Empty);
                SetProperty(string.Format(Properties.ForecastWeather.NightCondition, daynum), string.Empty);
                SetProperty(string.Format(Properties.ForecastWeather.DayWind, daynum), string.Empty);
                SetProperty(string.Format(Properties.ForecastWeather.NightWind, daynum), string.Empty);
                SetProperty(string.Format(Properties.ForecastWeather.DayHumidity, daynum), string.Empty);
                SetProperty(string.Format(Properties.ForecastWeather.NightCondition, daynum), string.Empty);
                SetProperty(string.Format(Properties.ForecastWeather.DayImgSmallFullpath, daynum), string.Empty);
                SetProperty(string.Format(Properties.ForecastWeather.DayImgSmallFilenamewithext, daynum), string.Empty);
                SetProperty(string.Format(Properties.ForecastWeather.DayImgSmallFilenamewithoutext, daynum), string.Empty);
                SetProperty(string.Format(Properties.ForecastWeather.DayImgBigFullpath, daynum), string.Empty);
                SetProperty(string.Format(Properties.ForecastWeather.DayImgBigFilenamewithext, daynum), string.Empty);
                SetProperty(string.Format(Properties.ForecastWeather.DayImgBigFilenamewithoutext, daynum), string.Empty);
                SetProperty(string.Format(Properties.ForecastWeather.NightImgSmallFullpath, daynum), string.Empty);
                SetProperty(string.Format(Properties.ForecastWeather.NightImgSmallFilenamewithext, daynum), string.Empty);
                SetProperty(string.Format(Properties.ForecastWeather.NightImgSmallFilenamewithoutext, daynum), string.Empty);
                SetProperty(string.Format(Properties.ForecastWeather.NightImgBigFullpath, daynum), string.Empty);
                SetProperty(string.Format(Properties.ForecastWeather.NightImgBigFilenamewithext, daynum), string.Empty);
                SetProperty(string.Format(Properties.ForecastWeather.NightImgBigFilenamewithoutext, daynum), string.Empty);
                SetProperty(string.Format(Properties.ForecastWeather.Weekday, daynum), string.Empty);
            }
            SetProperty(Properties.Feed.Enabled, "false");
            SetProperty(Properties.Feed.Isalltitles, "false");
            SetProperty(Properties.Feed.SelectedItemimg, " ");
            SetProperty(Properties.Feed.SelectedTitle, " ");
            SetProperty(Properties.Feed.SelectedSourcefeed, " ");
            SetProperty(Properties.Feed.Titles, InfoServiceUtils.GetLocalizedLabel(36));
            SetProperty(Properties.Feed.Alltitles, InfoServiceUtils.GetLocalizedLabel(36));
            SetProperty(Properties.Feed.SelectedFeed, " ");
            SetProperty(Properties.Feed.SelectedType, " ");
            SetProperty(Properties.Feed.SelectedIndex, " ");
            SetProperty(Properties.Feed.SelectedItemage, " ");
            SetProperty(Properties.Feed.SelectedDescription, " ");
            SetProperty(Properties.Feed.ItemCount, " ");
            SetProperty(Properties.Feed.ItemType, " ");
            SetProperty(Properties.Feed.Img, GUIGraphicsContext.Skin + @"\media\InfoService\defaultFeedAll.png");
            SetProperty(Properties.Feed.Separator, " ");
            SetProperty(Properties.Feed.SeparatorAll, " ");
            SetProperty(Properties.Feed.LastupdatedMessage, " ");
            SetProperty(Properties.Feed.LastupdatedDatetime, " ");
            SetProperty(Properties.Twitter.Enabled, "false");
            SetProperty(Properties.Twitter.Messages, InfoServiceUtils.GetLocalizedLabel(37));
            SetProperty(Properties.Twitter.Separator, " ");
            SetProperty(Properties.Twitter.ItemCount, " ");
            SetProperty(Properties.Twitter.ItemType, " ");
            SetProperty(Properties.Twitter.LastupdatedMessage, " ");
            SetProperty(Properties.Twitter.LastupdatedDatetime, " ");
            SetProperty(Properties.Twitter.SelectedUsername, " ");
            SetProperty(Properties.Twitter.SelectedUserpicture, " ");
            SetProperty(Properties.Twitter.SelectedTimeline, " ");
            SetProperty(Properties.Twitter.SelectedIndex, " ");

        }
    }
}
