#region Usings

using System;
using InfoService.Utils;
using MediaPortal.Configuration;
using MediaPortal.GUI.Library;
using WeatherConnector;
using WeatherConnector.Data;
using WeatherConnector.Expections;

#endregion

namespace InfoService.Weather
{
    public static class WeatherService
    {
        #region Config Properties
        private static decimal _refreshInterval;
        public static decimal RefreshInterval
        {
            get { return _refreshInterval; }
            set
            {
                logger.WriteLog("Set WeatherRefreshInterval to " + value, LogLevel.Debug, InfoServiceModul.InfoService); 
                _refreshInterval = value;
            }
        }

        private static bool _updateOnStartup;
        public static bool UpdateOnStartup
        {
            get { return _updateOnStartup; }
            set
            {
                logger.WriteLog("Set WeatherUpdateOnStartup to " + value, LogLevel.Debug, InfoServiceModul.InfoService); 
                _updateOnStartup = value;
            }
        }
        #endregion

        #region Fields
        private static readonly Logger logger = Logger.GetInstance();
        private static WeatherConnector.Weather _weather;
        private static TemperatureFormat _tempFormat;
        private static string _locationCode;
        #endregion

        #region Properties
        public static bool Enabled { get; set; }
        public static WeatherConnector.Weather Weather
        {
            get
            {
                if (_weather.WeatherData != null)
                {

                    return _weather;
                }
                return new WeatherConnector.Weather();
            }
        }
        public static DateTime LastRefresh { get; set; }

        public static string CurrentLocationId { get; set; }

        #endregion

        #region Methods
        public static bool SetupWeather(string locationCode, TemperatureFormat tempFormat, string partnerId, string licenseKey)
        {
            WeatherConnector.Weather.SetLoginCreditials(partnerId, licenseKey);
            try
            {
                LogEvents.OnDebug += LogEvents_OnDebug;
                LogEvents.OnError += LogEvents_OnError;
                LogEvents.OnWarning += LogEvents_OnWarning;
                LogEvents.OnInfo += LogEvents_OnInfo;
                _weather = new WeatherConnector.Weather(locationCode, tempFormat);
                //_weather = new WeatherConnector.Weather(_weatherFileXml);
                _locationCode = locationCode;
                _tempFormat = tempFormat;
                Enabled = true;
                return true;
            }
            catch (WeatherNoPartnerIDOrLicenseKeyExpection ex)
            {
                logger.WriteLog(ex.Message, LogLevel.Error, InfoServiceModul.Weather);
                return false;

            }
            catch (WeatherNoLocationIDExpection ex)
            {
                logger.WriteLog(ex.Message, LogLevel.Error, InfoServiceModul.Weather);
                return false;
            }

        }

        //public static void UpdateWeatherData(string locationId)
        //{
        //    _weather = new WeatherConnector.Weather(_weatherFileXml);
        //    if (_weather.UpdateWeatherData())
        //    {
        //        SetProperties();
        //    }
        //    else
        //    {
        //        logger.WriteLog("Update weather from file " + _weatherFileXml + " unsuccessful. Now trying to update weather directly from weather.com", LogLevel.Warning, InfoServiceModul.Weather);
        //        _weather = new WeatherConnector.Weather(locationId, _tempFormat);
        //        if (_weather.UpdateWeatherData())
        //        {
        //            SetProperties();
        //        }
        //        else
        //        {
        //            logger.WriteLog("Update of weather unsuccesfull. Check the errors and warnings above!", LogLevel.Error, InfoServiceModul.Weather);
        //        }
        //    }

        //}

        public static bool UpdateWeatherData(string locationId)
        {
            _weather.LocationId = locationId;
            if (_weather.UpdateWeatherData())
            {
                LastRefresh = DateTime.Now;
                logger.WriteLog("Update of weather successful. Now setting up properties...", LogLevel.Info,
                                InfoServiceModul.Weather);
                SetProperties();
                return true;
            }
            else
            {
                logger.WriteLog("Update of weather unsuccesfull. Check the errors and warnings above!", LogLevel.Error,
                                InfoServiceModul.Weather);
                return false;
            }

        }

        private static void SetProperties()
        {
            _weather.WeatherData.TodayCondition = WeatherUtils.LocalizeWeather(_weather.WeatherData.TodayCondition);
            string tmp = _weather.WeatherData.TodayFeelsLikeTemperature;
            _weather.WeatherData.TodayFeelsLikeTemperature = tmp +
                                                             _weather.WeatherEntities.TemperatureFormat.
                                                                 GetStringValue();
            tmp = _weather.WeatherData.TodayHumidity;
            _weather.WeatherData.TodayHumidity = tmp + "%";
            tmp = _weather.WeatherData.TodayTemperature;
            _weather.WeatherData.TodayTemperature = tmp +
                                                    _weather.WeatherEntities.TemperatureFormat.GetStringValue();
            //tmp = _weather.WeatherData.TodayMaxTemperature;
            //_weather.WeatherData.TodayMaxTemperature = tmp +
            //                                           _weather.WeatherEntities.TemperatureFormat.GetStringValue();
            //tmp = _weather.WeatherData.TodayMinTemperature;
            //_weather.WeatherData.TodayMinTemperature = tmp +
            //                                           _weather.WeatherEntities.TemperatureFormat.GetStringValue();
            tmp = _weather.WeatherData.TodayUvIndex;
            _weather.WeatherData.TodayUvIndex = WeatherUtils.LocalizeWeather(tmp);
            for (int i = 0; i < _weather.WeatherData.WeatherForecast.Count; i++)
            {
                tmp = _weather.WeatherData.WeatherForecast[i].MaxTemperature;
                _weather.WeatherData.WeatherForecast[i].MaxTemperature = tmp +
                                                                         _weather.WeatherEntities.TemperatureFormat.
                                                                             GetStringValue();
                tmp = _weather.WeatherData.WeatherForecast[i].MinTemperature;
                _weather.WeatherData.WeatherForecast[i].MinTemperature = tmp +
                                                                         _weather.WeatherEntities.TemperatureFormat.
                                                                             GetStringValue();
                tmp = _weather.WeatherData.WeatherForecast[i].Day.Humidity;
                _weather.WeatherData.WeatherForecast[i].Day.Humidity = tmp + "%";
                tmp = _weather.WeatherData.WeatherForecast[i].Night.Humidity;
                _weather.WeatherData.WeatherForecast[i].Night.Humidity = tmp + "%";
                _weather.WeatherData.WeatherForecast[i].Day.Condition =
                    WeatherUtils.LocalizeWeather(_weather.WeatherData.WeatherForecast[i].Day.Condition);
                _weather.WeatherData.WeatherForecast[i].Night.Condition =
                    WeatherUtils.LocalizeWeather(_weather.WeatherData.WeatherForecast[i].Night.Condition);

            }

        }


        
#endregion

        #region Log Events

        static void LogEvents_OnInfo(WeatherArgs weatherArguments)
        {
            logger.WriteLog(weatherArguments, LogLevel.Info, InfoServiceModul.Weather);
        }

        static void LogEvents_OnWarning(WeatherArgs weatherArguments)
        {
            logger.WriteLog(weatherArguments, LogLevel.Warning, InfoServiceModul.Weather);
        }

        static void LogEvents_OnError(WeatherArgs weatherArguments)
        {
            logger.WriteLog(weatherArguments, LogLevel.Error, InfoServiceModul.Weather);
        }

        static void LogEvents_OnDebug(WeatherArgs weatherArguments)
        {
            logger.WriteLog(weatherArguments, LogLevel.Debug, InfoServiceModul.Weather);
        }
        #endregion
    }
}