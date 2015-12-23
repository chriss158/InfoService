#region Usings

using System;
using System.IO;
using InfoService.BackgroundWorkers;
using InfoService.Weather;
using MediaPortal.Configuration;
using MediaPortal.GUI.Library;
using MediaPortal.Profile;
using WeatherConnector.Data;

#endregion

namespace InfoService.Utils
{
    public static class WeatherUtils
    {
        private static readonly Logger logger = Logger.GetInstance();

        public static void UpdateWeatherData()
        {
            using (MediaPortal.Profile.Settings settings = new MediaPortal.Profile.MPSettings())
            {
                string newId = settings.GetValueAsString("weather", "location", string.Empty);
                if (WeatherService.CurrentLocationId != newId)
                {
                    if (!string.IsNullOrEmpty(newId))
                    {
                        WeatherService.CurrentLocationId = newId;
                        logger.WriteLog("Location ID has changed to " + newId, LogLevel.Debug, InfoServiceModul.Weather);
                    }
                    else
                    {
                        logger.WriteLog("New Location ID was empty. Using the old Location ID -> " + WeatherService.CurrentLocationId, LogLevel.Debug, InfoServiceModul.Weather);
                    }
                }
            }
            WeatherWorker wWorker = new WeatherWorker(WeatherService.CurrentLocationId);
            wWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(wWorker_RunWorkerCompleted);
            wWorker.RunWorkerAsync();

        }

        static void wWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled && (bool)e.Result)
            {
                PropertyUtils.SetProperty(PropertyUtils.Properties.Weather.Location, WeatherService.Weather.Location);
                PropertyUtils.SetProperty(PropertyUtils.Properties.TodayWeather.Temp, WeatherService.Weather.WeatherData.TodayTemperature);
                //PropertyUtils.SetProperty(PropertyUtils.Properties.TodayWeather.Maxtemp, WeatherService.Weather.WeatherData.TodayMaxTemperature);
                //PropertyUtils.SetProperty(PropertyUtils.Properties.TodayWeather.Mintemp, WeatherService.Weather.WeatherData.TodayMinTemperature);
                PropertyUtils.SetProperty(PropertyUtils.Properties.TodayWeather.Feelsliketemp, WeatherService.Weather.WeatherData.TodayFeelsLikeTemperature);
                PropertyUtils.SetProperty(PropertyUtils.Properties.TodayWeather.Humidity, WeatherService.Weather.WeatherData.TodayHumidity);
                PropertyUtils.SetProperty(PropertyUtils.Properties.TodayWeather.Sunrise, WeatherService.Weather.WeatherData.TodaySunrise.ToShortTimeString());
                PropertyUtils.SetProperty(PropertyUtils.Properties.TodayWeather.Sunset, WeatherService.Weather.WeatherData.TodaySunset.ToShortTimeString());
                PropertyUtils.SetProperty(PropertyUtils.Properties.TodayWeather.Uvindex, WeatherService.Weather.WeatherData.TodayUvIndex);
                PropertyUtils.SetProperty(PropertyUtils.Properties.TodayWeather.Wind,
                                             string.Format(GUILocalizeStrings.Get(555),
                                                           WeatherService.Weather.WeatherData.TodayWindDirection,
                                                           WeatherService.Weather.WeatherData.TodayWindSpeed,
                                                           WeatherService.Weather.WeatherEntities.WindSpeed));
                PropertyUtils.SetProperty(PropertyUtils.Properties.TodayWeather.Condition, WeatherService.Weather.WeatherData.TodayCondition);
                PropertyUtils.SetProperty(PropertyUtils.Properties.TodayWeather.ImgSmallFullPath, Config.GetFile(Config.Dir.Weather, string.Format(@"64x64\{0}.png", WeatherService.Weather.WeatherData.TodayImageIcon)));
                PropertyUtils.SetProperty(PropertyUtils.Properties.TodayWeather.ImgSmallFilenamewithext, Path.GetFileName(Config.GetFile(Config.Dir.Weather, string.Format(@"64x64\{0}.png", WeatherService.Weather.WeatherData.TodayImageIcon))));
                PropertyUtils.SetProperty(PropertyUtils.Properties.TodayWeather.ImgSmallFilenamewithoutext, Path.GetFileNameWithoutExtension(Config.GetFile(Config.Dir.Weather, string.Format(@"64x64\{0}.png", WeatherService.Weather.WeatherData.TodayImageIcon))));
                PropertyUtils.SetProperty(PropertyUtils.Properties.TodayWeather.ImgBigFullPath, Config.GetFile(Config.Dir.Weather, string.Format(@"128x128\{0}.png", WeatherService.Weather.WeatherData.TodayImageIcon)));
                PropertyUtils.SetProperty(PropertyUtils.Properties.TodayWeather.ImgBigFilenamewithext, Path.GetFileName(Config.GetFile(Config.Dir.Weather, string.Format(@"128x128\{0}.png", WeatherService.Weather.WeatherData.TodayImageIcon))));
                PropertyUtils.SetProperty(PropertyUtils.Properties.TodayWeather.ImgBigFilenamewithoutext, Path.GetFileNameWithoutExtension(Config.GetFile(Config.Dir.Weather, string.Format(@"128x128\{0}.png", WeatherService.Weather.WeatherData.TodayImageIcon))));
                PropertyUtils.SetProperty(PropertyUtils.Properties.TodayWeather.Weekday, GetLocalizedDate(WeatherService.Weather.WeatherData.TodayDate));
                PropertyUtils.SetProperty(PropertyUtils.Properties.TodayWeather.LastupdatedMessage, string.Format(InfoServiceUtils.GetLocalizedLabel(31), WeatherService.LastRefresh));
                PropertyUtils.SetProperty(PropertyUtils.Properties.TodayWeather.LastupdatedDatetime, WeatherService.LastRefresh.ToString());

                int daynum = 1;
                foreach (WeatherForecast wfc in WeatherService.Weather.WeatherData.WeatherForecast)
                {
                    PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.ForecastWeather.Mintemp, daynum), wfc.MinTemperature);
                    PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.ForecastWeather.Maxtemp, daynum), wfc.MaxTemperature);
                    PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.ForecastWeather.Sunrise, daynum), wfc.Sunrise.ToShortTimeString());
                    PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.ForecastWeather.Sunset, daynum), wfc.Sunset.ToShortTimeString());
                    PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.ForecastWeather.DayCondition, daynum), wfc.Day.Condition); // GUILocalizeStrings.Get(0x17a) + ": " + wfc.Day.Condition
                    PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.ForecastWeather.NightCondition, daynum), wfc.Night.Condition);
                    PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.ForecastWeather.DayWind, daynum),
                                                string.Format(GUILocalizeStrings.Get(555),
                                                               LocalizeWeather(wfc.Day.WindDirection).Trim(),
                                                               wfc.Day.WindSpeed,
                                                               WeatherService.Weather.WeatherEntities.WindSpeed));
                    PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.ForecastWeather.NightWind, daynum),
                                                 string.Format(GUILocalizeStrings.Get(555),
                                                               LocalizeWeather(wfc.Night.WindDirection).Trim(),
                                                               wfc.Night.WindSpeed,
                                                               WeatherService.Weather.WeatherEntities.WindSpeed));
                    PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.ForecastWeather.DayHumidity, daynum), wfc.Day.Humidity);
                    PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.ForecastWeather.NightHumidity, daynum), wfc.Night.Humidity);
                    PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.ForecastWeather.DayImgSmallFullpath, daynum), Config.GetFile(Config.Dir.Weather, string.Format(@"64x64\{0}.png", wfc.Day.WeatherImage)));
                    PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.ForecastWeather.DayImgSmallFilenamewithext, daynum), Path.GetFileName(Config.GetFile(Config.Dir.Weather, string.Format(@"64x64\{0}.png", wfc.Day.WeatherImage))));
                    PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.ForecastWeather.DayImgSmallFilenamewithoutext, daynum), Path.GetFileNameWithoutExtension(Config.GetFile(Config.Dir.Weather, string.Format(@"64x64\{0}.png", wfc.Day.WeatherImage))));
                    PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.ForecastWeather.DayImgBigFullpath, daynum), Config.GetFile(Config.Dir.Weather, string.Format(@"128x128\{0}.png", wfc.Day.WeatherImage)));
                    PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.ForecastWeather.DayImgBigFilenamewithext, daynum), Path.GetFileName(Config.GetFile(Config.Dir.Weather, string.Format(@"128x128\{0}.png", wfc.Day.WeatherImage))));
                    PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.ForecastWeather.DayImgBigFilenamewithoutext, daynum), Path.GetFileNameWithoutExtension(Config.GetFile(Config.Dir.Weather, string.Format(@"128x128\{0}.png", wfc.Day.WeatherImage))));
                    PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.ForecastWeather.NightImgSmallFullpath, daynum), Config.GetFile(Config.Dir.Weather, string.Format(@"64x64\{0}.png", wfc.Night.WeatherImage)));
                    PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.ForecastWeather.NightImgSmallFilenamewithext, daynum), Path.GetFileName(Config.GetFile(Config.Dir.Weather, string.Format(@"64x64\{0}.png", wfc.Night.WeatherImage))));
                    PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.ForecastWeather.NightImgSmallFilenamewithoutext, daynum), Path.GetFileNameWithoutExtension(Config.GetFile(Config.Dir.Weather, string.Format(@"64x64\{0}.png", wfc.Night.WeatherImage))));
                    PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.ForecastWeather.NightImgBigFullpath, daynum), Config.GetFile(Config.Dir.Weather, string.Format(@"128x128\{0}.png", wfc.Night.WeatherImage)));
                    PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.ForecastWeather.NightImgBigFilenamewithext, daynum), Path.GetFileName(Config.GetFile(Config.Dir.Weather, string.Format(@"128x128\{0}.png", wfc.Night.WeatherImage))));
                    PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.ForecastWeather.NightImgBigFilenamewithoutext, daynum), Path.GetFileNameWithoutExtension(Config.GetFile(Config.Dir.Weather, string.Format(@"128x128\{0}.png", wfc.Night.WeatherImage))));
                    PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.ForecastWeather.Weekday, daynum), GetLocalizedDate(wfc.Date));
                    daynum++;
                }

                //Fill up the weather forecast up to 5 days with N/A values, if there are less then 5 forcast days
                while (daynum <= 5)
                {
                    PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.ForecastWeather.Mintemp, daynum), "N/A");
                    PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.ForecastWeather.Maxtemp, daynum), "N/A");
                    PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.ForecastWeather.Sunrise, daynum), "N/A");
                    PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.ForecastWeather.Sunset, daynum), "N/A");
                    PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.ForecastWeather.DayCondition, daynum), "N/A"); // GUILocalizeStrings.Get(0x17a) + ": " + wfc.Day.Condition
                    PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.ForecastWeather.NightCondition, daynum), "N/A");
                    PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.ForecastWeather.DayWind, daynum), "N/A");
                    PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.ForecastWeather.NightWind, daynum), "N/A");
                    PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.ForecastWeather.DayHumidity, daynum), "N/A");
                    PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.ForecastWeather.NightHumidity, daynum), "N/A");
                    PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.ForecastWeather.DayImgSmallFullpath, daynum), string.Empty);
                    PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.ForecastWeather.DayImgSmallFilenamewithext, daynum), string.Empty);
                    PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.ForecastWeather.DayImgSmallFilenamewithoutext, daynum), string.Empty);
                    PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.ForecastWeather.DayImgBigFullpath, daynum), string.Empty);
                    PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.ForecastWeather.DayImgBigFilenamewithext, daynum), string.Empty);
                    PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.ForecastWeather.DayImgBigFilenamewithoutext, daynum), string.Empty);
                    PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.ForecastWeather.NightImgSmallFullpath, daynum), string.Empty);
                    PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.ForecastWeather.NightImgSmallFilenamewithext, daynum), string.Empty);
                    PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.ForecastWeather.NightImgSmallFilenamewithoutext, daynum), string.Empty);
                    PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.ForecastWeather.NightImgBigFullpath, daynum), string.Empty);
                    PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.ForecastWeather.NightImgBigFilenamewithext, daynum), string.Empty);
                    PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.ForecastWeather.NightImgBigFilenamewithoutext, daynum), string.Empty);
                    PropertyUtils.SetProperty(string.Format(PropertyUtils.Properties.ForecastWeather.Weekday, daynum), GetLocalizedDate(WeatherService.Weather.WeatherData.TodayDate.AddDays(daynum)));
                    daynum++;
                }
            }
        }

        private static string GetLocalizedDate(DateTime date)
        {
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    return GUILocalizeStrings.Get(11);
                case DayOfWeek.Tuesday:
                    return GUILocalizeStrings.Get(12);
                case DayOfWeek.Wednesday:
                    return GUILocalizeStrings.Get(13);
                case DayOfWeek.Thursday:
                    return GUILocalizeStrings.Get(14);
                case DayOfWeek.Friday:
                    return GUILocalizeStrings.Get(15);
                case DayOfWeek.Saturday:
                    return GUILocalizeStrings.Get(16);
                case DayOfWeek.Sunday:
                    return GUILocalizeStrings.Get(17);
                default:
                    return "";
            }
        }
        public static string LocalizeWeather(string ustring)
        {
            string localizedString;
            string returnString = String.Empty;
            foreach (string estring in ustring.Split(new[] { " " }, StringSplitOptions.None))
            {
                localizedString = String.Empty;
                if ((String.Compare(estring, "T-Storms", true) == 0) || (String.Compare(estring, "T-Storm", true) == 0))
                {
                    localizedString = GUILocalizeStrings.Get(370);
                }
                else if (String.Compare(estring, "Partly", true) == 0)
                {
                    localizedString = GUILocalizeStrings.Get(0x173);
                }
                else if (String.Compare(estring, "Mostly", true) == 0)
                {
                    localizedString = GUILocalizeStrings.Get(0x174);
                }
                else if ((String.Compare(estring, "Sunny", true) == 0) || (String.Compare(estring, "Sun", true) == 0))
                {
                    localizedString = GUILocalizeStrings.Get(0x175);
                }
                else if ((String.Compare(estring, "Cloudy", true) == 0) || (String.Compare(estring, "Clouds", true) == 0))
                {
                    localizedString = GUILocalizeStrings.Get(0x176);
                }
                else if (String.Compare(estring, "Snow", true) == 0)
                {
                    localizedString = GUILocalizeStrings.Get(0x177);
                }
                else if (String.Compare(estring, "Rain", true) == 0)
                {
                    localizedString = GUILocalizeStrings.Get(0x178);
                }
                else if (String.Compare(estring, "Light", true) == 0)
                {
                    localizedString = GUILocalizeStrings.Get(0x179);
                }
                else if (String.Compare(estring, "AM", true) == 0)
                {
                    localizedString = GUILocalizeStrings.Get(0x17a);
                }
                else if (String.Compare(estring, "PM", true) == 0)
                {
                    localizedString = GUILocalizeStrings.Get(0x17b);
                }
                else if (((String.Compare(estring, "Showers", true) == 0) ||
                          (String.Compare(estring, "Shower", true) == 0)) ||
                         (String.Compare(estring, "T-Showers", true) == 0))
                {
                    localizedString = GUILocalizeStrings.Get(380);
                }
                else if (String.Compare(estring, "Few", true) == 0)
                {
                    localizedString = GUILocalizeStrings.Get(0x17d);
                }
                else if ((String.Compare(estring, "Scattered", true) == 0) ||
                         (String.Compare(estring, "Isolated", true) == 0))
                {
                    localizedString = GUILocalizeStrings.Get(0x17e);
                }
                else if (String.Compare(estring, "Wind", true) == 0)
                {
                    localizedString = GUILocalizeStrings.Get(0x17f);
                }
                else if (String.Compare(estring, "Strong", true) == 0)
                {
                    localizedString = GUILocalizeStrings.Get(0x180);
                }
                else if (String.Compare(estring, "Fair", true) == 0)
                {
                    localizedString = GUILocalizeStrings.Get(0x181);
                }
                else if (String.Compare(estring, "Clear", true) == 0)
                {
                    localizedString = GUILocalizeStrings.Get(0x182);
                }
                else if (String.Compare(estring, "Early", true) == 0)
                {
                    localizedString = GUILocalizeStrings.Get(0x183);
                }
                else if (String.Compare(estring, "and", true) == 0)
                {
                    localizedString = GUILocalizeStrings.Get(0x184);
                }
                else if (String.Compare(estring, "Fog", true) == 0 ||
                         (String.Compare(estring, "Foggy", true) == 0))
                {
                    localizedString = GUILocalizeStrings.Get(0x185);
                }
                else if (String.Compare(estring, "Haze", true) == 0)
                {
                    localizedString = GUILocalizeStrings.Get(390);
                }
                else if (String.Compare(estring, "Windy", true) == 0)
                {
                    localizedString = GUILocalizeStrings.Get(0x187);
                }
                else if (String.Compare(estring, "Drizzle", true) == 0)
                {
                    localizedString = GUILocalizeStrings.Get(0x188);
                }
                else if (String.Compare(estring, "Freezing", true) == 0)
                {
                    localizedString = GUILocalizeStrings.Get(0x189);
                }
                else if (String.Compare(estring, "N/A", true) == 0)
                {
                    localizedString = GUILocalizeStrings.Get(0x18a);
                }
                else if (String.Compare(estring, "Mist", true) == 0)
                {
                    localizedString = GUILocalizeStrings.Get(0x18b);
                }
                else if (String.Compare(estring, "High", true) == 0)
                {
                    localizedString = GUILocalizeStrings.Get(0x31f);
                }
                else if (String.Compare(estring, "Low", true) == 0)
                {
                    localizedString = GUILocalizeStrings.Get(0x31e);
                }
                else if (String.Compare(estring, "Moderate", true) == 0)
                {
                    localizedString = GUILocalizeStrings.Get(0x216);
                }
                else if (String.Compare(estring, "Late", true) == 0)
                {
                    localizedString = GUILocalizeStrings.Get(0x229);
                }
                else if (String.Compare(estring, "Very", true) == 0)
                {
                    localizedString = GUILocalizeStrings.Get(0x22a);
                }
                else if (String.Compare(estring, "Heavy", true) == 0)
                {
                    localizedString = GUILocalizeStrings.Get(0x197);
                }
                else if (String.Compare(estring, "N", true) == 0)
                {
                    localizedString = GUILocalizeStrings.Get(0x217);
                }
                else if (String.Compare(estring, "E", true) == 0)
                {
                    localizedString = GUILocalizeStrings.Get(0x218);
                }
                else if (String.Compare(estring, "S", true) == 0)
                {
                    localizedString = GUILocalizeStrings.Get(0x219);
                }
                else if (String.Compare(estring, "W", true) == 0)
                {
                    localizedString = GUILocalizeStrings.Get(0x21a);
                }
                else if (String.Compare(estring, "NE", true) == 0)
                {
                    localizedString = GUILocalizeStrings.Get(0x21b);
                }
                else if (String.Compare(estring, "SE", true) == 0)
                {
                    localizedString = GUILocalizeStrings.Get(540);
                }
                else if (String.Compare(estring, "SW", true) == 0)
                {
                    localizedString = GUILocalizeStrings.Get(0x21d);
                }
                else if (String.Compare(estring, "NW", true) == 0)
                {
                    localizedString = GUILocalizeStrings.Get(0x21e);
                }
                else if (String.Compare(estring, "Thunder", true) == 0)
                {
                    localizedString = GUILocalizeStrings.Get(0x21f);
                }
                else if (String.Compare(estring, "NNE", true) == 0)
                {
                    localizedString = GUILocalizeStrings.Get(0x220);
                }
                else if (String.Compare(estring, "ENE", true) == 0)
                {
                    localizedString = GUILocalizeStrings.Get(0x221);
                }
                else if (String.Compare(estring, "ESE", true) == 0)
                {
                    localizedString = GUILocalizeStrings.Get(0x222);
                }
                else if (String.Compare(estring, "SSE", true) == 0)
                {
                    localizedString = GUILocalizeStrings.Get(0x223);
                }
                else if (String.Compare(estring, "SSW", true) == 0)
                {
                    localizedString = GUILocalizeStrings.Get(0x224);
                }
                else if (String.Compare(estring, "WSW", true) == 0)
                {
                    localizedString = GUILocalizeStrings.Get(0x225);
                }
                else if (String.Compare(estring, "WNW", true) == 0)
                {
                    localizedString = GUILocalizeStrings.Get(0x227);
                }
                else if (String.Compare(estring, "NNW", true) == 0)
                {
                    localizedString = GUILocalizeStrings.Get(0x228);
                }
                else if (String.Compare(estring, "VAR", true) == 0)
                {
                    localizedString = GUILocalizeStrings.Get(0x22c);
                }
                else if (String.Compare(estring, "CALM", true) == 0)
                {
                    localizedString = GUILocalizeStrings.Get(0x22d);
                }
                else if (((String.Compare(estring, "Storm", true) == 0) ||
                          (String.Compare(estring, "Gale", true) == 0)) ||
                         (String.Compare(estring, "Tempest", true) == 0))
                {
                    localizedString = GUILocalizeStrings.Get(0x257);
                }
                else if (String.Compare(estring, "in the Vicinity", true) == 0)
                {
                    localizedString = GUILocalizeStrings.Get(0x22f);
                }
                else if (String.Compare(estring, "Clearing", true) == 0)
                {
                    localizedString = GUILocalizeStrings.Get(560);
                }
                if (localizedString == String.Empty)
                {
                    localizedString = estring;
                }
                returnString = returnString + localizedString + " ";
            }
            return returnString;
        }
    }
}
