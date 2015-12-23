#region Usings

using System;

#endregion

namespace WeatherConnector
{
    public static class LogEvents
    {
        #region Delegates

        public delegate void WeatherErrorHandler(WeatherArgs weatherArguments);

        #endregion

        public static event WeatherErrorHandler OnError;
        public static event WeatherErrorHandler OnDebug;
        public static event WeatherErrorHandler OnWarning;
        public static event WeatherErrorHandler OnInfo;

        public static void InvokeOnDebug(WeatherArgs Weatherarguments)
        {
            WeatherErrorHandler handler = OnDebug;
            if (handler != null) handler(Weatherarguments);
        }

        public static void InvokeOnWarning(WeatherArgs Weatherarguments)
        {
            WeatherErrorHandler handler = OnWarning;
            if (handler != null) handler(Weatherarguments);
        }

        public static void InvokeOnError(WeatherArgs Weatherarguments)
        {
            WeatherErrorHandler handler = OnError;
            if (handler != null) handler(Weatherarguments);
        }
        public static void InvokeOnInfo(WeatherArgs Weatherarguments)
        {
            WeatherErrorHandler handler = OnInfo;
            if (handler != null) handler(Weatherarguments);
        }
    }
    public class WeatherArgs : EventArgs
    {
        public WeatherArgs(string message, string exMessage, string stackTrace)
        {
            Message = message;
            ExMessage = exMessage;
            StackTrace = stackTrace;
        }
        public WeatherArgs(string message, string exMessage)
        {
            Message = message;
            ExMessage = exMessage;
        }
        public WeatherArgs(string message)
        {
            Message = message;
        }

        public string Message { get; private set; }
        public string ExMessage { get; private set; }
        public string StackTrace { get; private set; }
    }
}
