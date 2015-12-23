#region Usings

using System;

#endregion

namespace WeatherConnector.Data
{
    public class WeatherForecast
    {
        public WeatherForecast(DateTime date, string maxTemperature, string minTemperature, DateTime sunrise,
                               DateTime sunset, WeatherDay day, WeatherDay night)
        {
            Sunrise = sunrise;
            Sunset = sunset;
            MinTemperature = minTemperature;
            MaxTemperature = maxTemperature;
            Date = date;
            Day = new WeatherDay(day.Condition, day.WindDirection, day.WindSpeed, day.Humidity, day.WeatherImage);
            Night = new WeatherDay(night.Condition, night.WindDirection, night.WindSpeed, night.Humidity,
                                   night.WeatherImage);
        }

        public WeatherForecast()
        {
        }

        public DateTime Date { get; set; }
        public DateTime Sunrise { get; set; }
        public DateTime Sunset { get; set; }
        public WeatherDay Day { get; set; }
        public WeatherDay Night { get; set; }
        public string MinTemperature { get; set; }
        public string MaxTemperature { get; set; }
    }
}