namespace WeatherConnector.Data
{
    public class WeatherDay
    {
        public WeatherDay(string condition, string windDirection, string windSpeed, string humidity, string weatherImage)
        {
            Humidity = humidity;
            Condition = condition;
            WindDirection = windDirection;
            WeatherImage = weatherImage;
            WindSpeed = windSpeed;
        }

        public string Humidity { get; set; }
        public string WindDirection { get; set; }
        public string WindSpeed { get; set; }
        public string Condition { get; set; }
        public string WeatherImage { get; set; }
    }
}