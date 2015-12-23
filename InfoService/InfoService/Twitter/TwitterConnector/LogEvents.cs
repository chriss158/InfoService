#region Usings

using System;

#endregion

namespace TwitterConnector
{
    public static class LogEvents
    {
        #region Delegates

        public delegate void TwitterErrorHandler(TwitterArgs twitterArguments);

        #endregion

        public static event TwitterErrorHandler OnError;
        public static event TwitterErrorHandler OnDebug;
        public static event TwitterErrorHandler OnWarning;
        public static event TwitterErrorHandler OnInfo;

        public static void InvokeOnDebug(TwitterArgs Twitterarguments)
        {
            TwitterErrorHandler handler = OnDebug;
            if (handler != null) handler(Twitterarguments);
        }

        public static void InvokeOnWarning(TwitterArgs Twitterarguments)
        {
            TwitterErrorHandler handler = OnWarning;
            if (handler != null) handler(Twitterarguments);
        }

        public static void InvokeOnError(TwitterArgs Twitterarguments)
        {
            TwitterErrorHandler handler = OnError;
            if (handler != null) handler(Twitterarguments);
        }
        public static void InvokeOnInfo(TwitterArgs Twitterarguments)
        {
            TwitterErrorHandler handler = OnInfo;
            if (handler != null) handler(Twitterarguments);
        }
    }
    public class TwitterArgs : EventArgs
    {
        public TwitterArgs(string message, string exMessage, string stackTrace)
        {
            Message = message;
            ExMessage = exMessage;
            StackTrace = stackTrace;
        }
        public TwitterArgs(string message, string exMessage)
        {
            Message = message;
            ExMessage = exMessage;
        }
        public TwitterArgs(string message)
        {
            Message = message;
        }

        public string Message { get; private set; }
        public string ExMessage { get; private set; }
        public string StackTrace { get; private set; }
    }
}
