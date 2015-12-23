using System;

namespace FeedReader
{
    public static class LogEvents
    {
        public delegate void FeedErrorHandler(FeedArgs feedArguments);
        public static event FeedErrorHandler OnError;
        public static event FeedErrorHandler OnDebug;
        public static event FeedErrorHandler OnInfo;
        public static event FeedErrorHandler OnWarning;

        public static void InvokeOnDebug(FeedArgs feedarguments)
        {
            FeedErrorHandler handler = OnDebug;
            if (handler != null) handler(feedarguments);
        }

        public static void InvokeOnInfo(FeedArgs feedarguments)
        {
            FeedErrorHandler handler = OnInfo;
            if (handler != null) handler(feedarguments);
        }

        public static void InvokeOnWarning(FeedArgs feedarguments)
        {
            FeedErrorHandler handler = OnWarning;
            if (handler != null) handler(feedarguments);
        }

        public static void InvokeOnError(FeedArgs feedarguments)
        {
            FeedErrorHandler handler = OnError;
            if (handler != null) handler(feedarguments);
        }
    }
    public class FeedArgs : EventArgs
    {
        public FeedArgs(string message, string exMessage, string stackTrace)
        {
            Message = message;
            ExMessage = exMessage;
            StackTrace = stackTrace;
        }
        public FeedArgs(string message, string exMessage)
        {
            Message = message;
            ExMessage = exMessage;
        }
        public FeedArgs(string message)
        {
            Message = message;
        }

        public string Message { get; private set; }
        public string ExMessage { get; private set; }
        public string StackTrace { get; private set; }
    }
}
