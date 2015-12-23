#region Usings

using System;
using System.Collections.Generic;

#endregion

namespace WeatherConnector.Data
{
    public class WeatherData
    {
        public WeatherData()
        {
            WeatherForecast = new List<WeatherForecast>();
        }

        public List<WeatherForecast> WeatherForecast { get; set; }
        public DateTime TodaySunrise { get; set; }
        public DateTime TodaySunset { get; set; }
        public string TodayCondition { get; set; }
        public string TodayTemperature { get; set; }
        //public string TodayMaxTemperature { get; set; }
        //public string TodayMinTemperature { get; set; }
        public string TodayFeelsLikeTemperature { get; set; }
        public string TodayHumidity { get; set; }
        public string TodayUvIndex { get; set; }
        public string TodayWindDirection { get; set; }
        public string TodayWindSpeed { get; set; }
        public string TodayImageIcon { get; set; }
        public DateTime TodayDate { get; set; }
    }
}