#region Usings

using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Xml;
using WeatherConnector.Data;
using WeatherConnector.Expections;
using WeatherConnector.Xml;

#endregion

namespace WeatherConnector
{
    public class Weather
    {
        private const string BaseUrl =
            "http://xoap.weather.com/weather/local/{0}?cc=*&dayf=5&link=xoap&prod=xoap&par={1}&key={2}&unit={3}";

        private static string _licenseKey;
        private static bool _loginSet;
        private static string _partnerId;
        private string _locationId;
        private bool _useFile;
        private string _weatherFile;
        private readonly XmlDocument _weatherXml = new XmlDocument();
        private TemperatureFormat _temperatureFormat = TemperatureFormat.Grad;
        private WeatherData _weatherData;
        private WeatherEntities _weatherEntities;


        public Weather(string locationId, TemperatureFormat temperatureFormat)
        {
            if (!_loginSet)
            {
                throw new WeatherNoPartnerIDOrLicenseKeyExpection("No Partner ID and/or License ID is set");
            }
            if (locationId == String.Empty)
            {
                throw new WeatherNoLocationIDExpection("No LocationID set");
            }
            LogEvents.InvokeOnDebug(new WeatherArgs("Creating WeatherConnector with weather location " + locationId + " and temperature format " + _temperatureFormat));
            _locationId = locationId;
            _temperatureFormat = temperatureFormat;
            _useFile = false;
            //RefreshWeatherData();
        }
        public Weather(string pathToWeatherXML)
        {
            _useFile = true;
            _weatherFile = pathToWeatherXML;
        }
        public Weather()
        {
            
        }

        public WeatherData WeatherData
        {
            get { return _weatherData; }
        }

        public WeatherEntities WeatherEntities
        {
            get { return _weatherEntities; }
        }

        public void SetTemperatureFormat(TemperatureFormat temperatureFormat)
        {
            if (!_useFile)
            {
                LogEvents.InvokeOnDebug(new WeatherArgs("Setting temperature format to " + temperatureFormat));
                _temperatureFormat = temperatureFormat;
            }
        }

        public string Location { get; private set; }

        public string LocationId
        {
            get
            {
                return _locationId;
            }
            set
            {
                LogEvents.InvokeOnDebug(new WeatherArgs("Setting location id to " + value));
                _locationId = value;
            }
        }

        public static void SetLoginCreditials(string partnerId, string licenseKey)
        {
            LogEvents.InvokeOnDebug(new WeatherArgs("Setting login creditials"));
            _partnerId = partnerId;
            _licenseKey = licenseKey;
            _loginSet = true;
        }

        public bool UpdateWeatherData()
        {
            if (!_useFile)
            {
                if (_locationId == String.Empty)
                {
                    LogEvents.InvokeOnError(new WeatherArgs("No location id set"));
                    return false;
                }
                string xmlStr = string.Empty;
                string tempUnitXML = string.Empty;
                switch (_temperatureFormat)
                {
                    case TemperatureFormat.Grad:
                        tempUnitXML = "m";
                        break;
                    case TemperatureFormat.Fahrenheit:
                        tempUnitXML = "s";
                        break;
                }

                LogEvents.InvokeOnInfo(new WeatherArgs("Updating weather data for location id " + _locationId));
                string url = String.Format(BaseUrl, _locationId, _partnerId, _licenseKey, tempUnitXML);
                using (WebClient client = new WebClient())
                {
                    try
                    {
                        LogEvents.InvokeOnDebug(new WeatherArgs("Downloading weather xml for location id " + _locationId));
                        using (StreamReader reader = new StreamReader(client.OpenRead(url)))
                        {
                            xmlStr = reader.ReadToEnd();
                            LogEvents.InvokeOnDebug(
                                new WeatherArgs(
                                    "Downloaded weather xml successful. Now checking whether the file has errors..."));
                        }
                    }
                    catch (Exception ex)
                    {
                        LogEvents.InvokeOnError(new WeatherArgs("Error downloading weather data", ex.Message,
                                                                ex.StackTrace));
                        return false;

                    }
                }
                if (string.IsNullOrEmpty(xmlStr))
                {
                    LogEvents.InvokeOnError(
                        new WeatherArgs("Error downloading weather data. Downloaded weather xml is empty"));
                    return false;
                }
                _weatherXml.LoadXml(xmlStr);
            }
            else
            {
                LogEvents.InvokeOnInfo(new WeatherArgs("Updating weather data from file " + _weatherFile));
                if(File.Exists(_weatherFile))
                {
                    _weatherXml.Load(_weatherFile);
                }
                else
                {
                    LogEvents.InvokeOnError(new WeatherArgs("Updating weather data from file " + _weatherFile + " unsuccessful. File doesn't exist"));
                    return false;
                }
            }
                

            if (_weatherXml.DocumentElement != null)
            {
                if (_weatherXml.DocumentElement.SelectSingleNode("//err[@type='0']") != null)
                {
                    LogEvents.InvokeOnError(
                        new WeatherArgs("Downloading was successful, but weather.com returned an error: " +
                                        _weatherXml.DocumentElement.SelectSingleNode("//err[@type='0']").InnerText));
                }
            }
            else
            {
                LogEvents.InvokeOnError(new WeatherArgs("Downloading was successful, but the file is not valid"));
                return false;
            }
            WeatherXml.WeatherXmlDocument = _weatherXml;
            _locationId = WeatherXml.ParseXmlString("loc/@id");
            LogEvents.InvokeOnInfo(new WeatherArgs("Downloaded/Checked successful the weather data for location id " + _locationId));
            return ParseWeatherData();

        }

        public bool ParseWeatherData()
        {
            
            LogEvents.InvokeOnDebug(new WeatherArgs("Parsing weather data for location id " + _locationId));
            Location = WeatherXml.ParseXmlString("loc/dnam");
            LogEvents.InvokeOnDebug(new WeatherArgs("Location id " + _locationId + " seems to be " + Location));
            _weatherEntities = new WeatherEntities();
            _weatherEntities.TemperatureFormat = WeatherXml.ParseXmlString("head/ut") == "C"
                                                     ? TemperatureFormat.Grad
                                                     : TemperatureFormat.Fahrenheit;
            _weatherEntities.WindSpeed = WeatherXml.ParseXmlString("head/us");
            _weatherData = new WeatherData();

            _weatherData.TodayImageIcon = WeatherXml.ParseXmlString("cc/icon");
            _weatherData.TodayCondition = WeatherXml.ParseXmlString("cc/t");
            _weatherData.TodayDate = DateTime.Now;
            _weatherData.TodayFeelsLikeTemperature = WeatherXml.ParseXmlString("cc/flik");
            _weatherData.TodayHumidity = WeatherXml.ParseXmlString("cc/hmid");
            _weatherData.TodaySunrise = WeatherXml.ParseXmlDateTime("loc/sunr", "h:mm tt");
            _weatherData.TodaySunset = WeatherXml.ParseXmlDateTime("loc/suns", "h:mm tt");
            _weatherData.TodayTemperature = WeatherXml.ParseXmlString("cc/tmp");
            //_weatherData.TodayMaxTemperature = WeatherXml.ParseXmlString("dayf/day[@d='0']/hi");
            //_weatherData.TodayMinTemperature = WeatherXml.ParseXmlString("dayf/day[@d='0']/low");
            _weatherData.TodayUvIndex = string.Format("{0} {1}", WeatherXml.ParseXmlString("cc/uv/i"),
                                                      WeatherXml.ParseXmlString("cc/uv/t"));
            _weatherData.TodayWindDirection = WeatherXml.ParseXmlString("cc/wind/t");
            _weatherData.TodayWindSpeed = WeatherXml.ParseXmlString("cc/wind/s");


            bool continueWithParsing = true;
            try
            {
                LogEvents.InvokeOnDebug(new WeatherArgs("Parsing the weather forecast data"));
                _weatherXml.DocumentElement.SelectSingleNode("dayf").RemoveChild(_weatherXml.DocumentElement.SelectSingleNode("dayf/lsup"));
            }
            catch (Exception ex)
            {
                LogEvents.InvokeOnWarning(new WeatherArgs("Parsing the weather forecast data unsuccesful. It seems that the xml is not valid", ex.Message, ex.StackTrace));
                continueWithParsing = false;
            }

            if (continueWithParsing)
            {
                WeatherForecast wfc = new WeatherForecast();
                int i = 0;
                _weatherData.WeatherForecast.Clear();

                foreach (XmlNode day in _weatherXml.DocumentElement.SelectSingleNode("dayf"))
                {
                    DateTime dateFXml = WeatherXml.ParseXmlDateTime("dayf/day[@d='" + i + "']/@dt", "MMM d");
                    dateFXml = new DateTime(DateTime.Now.Year, dateFXml.Month, dateFXml.Day);
   
                    wfc = new WeatherForecast();
                    wfc.Date = new DateTime(DateTime.Now.Year, dateFXml.Month, dateFXml.Day);
                    wfc.MaxTemperature = WeatherXml.ParseXmlString("dayf/day[@d='" + i + "']/hi");
                    wfc.MinTemperature = WeatherXml.ParseXmlString("dayf/day[@d='" + i + "']/low");
                    wfc.Sunrise = WeatherXml.ParseXmlDateTime("dayf/day[@d='" + i + "']/sunr", "h:mm tt");
                    wfc.Sunset = WeatherXml.ParseXmlDateTime("dayf/day[@d='" + i + "']/suns", "h:mm tt");
                    wfc.Day = new WeatherDay(WeatherXml.ParseXmlString("dayf/day[@d='" + i + "']/part[@p='d']/t"),
                                             WeatherXml.ParseXmlString("dayf/day[@d='" + i + "']/part[@p='d']/wind/t"),
                                             WeatherXml.ParseXmlString("dayf/day[@d='" + i + "']/part[@p='d']/wind/s"),
                                             WeatherXml.ParseXmlString("dayf/day[@d='" + i + "']/part[@p='d']/hmid"),
                                             WeatherXml.ParseXmlString("dayf/day[@d='" + i + "']/part[@p='d']/icon"));
                    wfc.Night = new WeatherDay(WeatherXml.ParseXmlString("dayf/day[@d='" + i + "']/part[@p='n']/t"),
                                               WeatherXml.ParseXmlString("dayf/day[@d='" + i + "']/part[@p='n']/wind/t"),
                                               WeatherXml.ParseXmlString("dayf/day[@d='" + i + "']/part[@p='n']/wind/s"),
                                               WeatherXml.ParseXmlString("dayf/day[@d='" + i + "']/part[@p='n']/hmid"),
                                               WeatherXml.ParseXmlString("dayf/day[@d='" + i + "']/part[@p='n']/icon"));
                    _weatherData.WeatherForecast.Add(wfc);
                    i++;
                }
            }
            LogEvents.InvokeOnDebug(new WeatherArgs("Parsing of weather data done. See above for possibly errors or warnings"));
            return true;
        }
    }
}