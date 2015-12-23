using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using MediaPortal.GUI.Library;

namespace InfoService.Utils
{
    public enum LogLevel
    {
        Info = 1,
        Warning = 2,
        Error = 3,
        Debug = 4,
    }

    public enum InfoServiceModul
    {
        InfoService,
        Weather,
        Feed,
        Twitter,
        RecentlyAddedWatched,
        SkinSettings,
    }

    public class TempLogger
    {
        public TempLogger(string message, LogLevel level, InfoServiceModul modul)
        {
            this.message = message;
            this.level = level;
            this.modul = modul;
        }

        public string message { get; set; }
        public LogLevel level { get; set; }
        public InfoServiceModul modul { get; set; }
    }

    /// <summary>
    /// Summary description for Logger.
    /// </summary>
    public class Logger
    {
        private Logger()
        {
        }

        private static volatile Logger _Logger = null;

        public static Logger GetInstance()
        {
            // DoubleLock
            if (_Logger == null)
            {
                lock (m_lock)
                {
                    if (_Logger == null) _Logger = new Logger();
                }
            }
            return _Logger;
        }

        // Hilfsfeld für eine sichere Threadsynchronisierung
        private static object m_lock = new object();

        private static System.IO.StreamWriter _output = null;
        private static System.IO.StreamWriter Output
        {
            get
            {
                if (_output == null && !string.IsNullOrEmpty(LogFile))
                {
                    //_output = new System.IO.StreamWriter(LogFile, true, System.Text.Encoding.Default);
                    _output = File.CreateText(LogFile);
                    _output.Close();
                    _output.Dispose();
                    //GetInstance().FlushTempLog();
                }
                return _output;
            }
            set
            {
                _output = value;
            }
        }
       
        private static List<TempLogger> _tempLogger = new List<TempLogger>();

        public static bool LogDebug { get; set; }   
        public static bool LogError { get; set; }
        public static bool LogWarning { get; set; }

        private static string _logFile = string.Empty;
        public static string LogFile
        {
            get { return _logFile; }
            set
            {
                if (string.IsNullOrEmpty(_logFile))
                {
                    if (System.IO.File.Exists(value))
                    {
                        string dir = System.IO.Path.GetDirectoryName(value) + "\\";
                        string filename = System.IO.Path.GetFileNameWithoutExtension(value) + ".bak";
                        if (System.IO.File.Exists(dir + filename)) System.IO.File.Delete(dir + filename);
                        System.IO.File.Move(value, dir + filename);
                    }
                    _logFile = value;
                }
            }
        }

        public void WriteLog(string message, LogLevel level, InfoServiceModul modul)
        {
            try
            {
                if (Output == null)
                {
                    _tempLogger.Add(new TempLogger(message, level, modul));
                    return;
                }

                if (LogDebug || (level == LogLevel.Error && LogError) || (level == LogLevel.Warning && LogWarning) || level == LogLevel.Info)
                {
                    lock (Output)
                    {
                        if (File.Exists(LogFile))
                            Output = File.AppendText(LogFile);
                        else
                            Output = File.CreateText(LogFile);

                        Output.WriteLine(DateTime.Now + " | " + string.Format("{0:D4}", Thread.CurrentThread.ManagedThreadId) + " | " + modul.ToString().PadLeft("RecentlyAddedWatched".Length, ' ') + " | " + level.ToString().PadLeft("Warning".Length, ' ') + " | " + message);
                        Output.Flush();
                        Output.Close();
                        Output.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void WriteLog(TwitterConnector.TwitterArgs twitterArgs, LogLevel level, InfoServiceModul modul)
        {
            string exMessage = "";
            string stackTrace = "";
            string message = "";
            if (!string.IsNullOrEmpty(twitterArgs.ExMessage)) exMessage = twitterArgs.ExMessage;
            if (!string.IsNullOrEmpty(twitterArgs.StackTrace)) stackTrace = twitterArgs.StackTrace;
            if (!string.IsNullOrEmpty(twitterArgs.Message)) message = twitterArgs.Message;
            string logMessage = message + "\n\t\t\t\t\t\t" + exMessage + "\n\t\t\t\t\t\t" + stackTrace;
            logMessage = logMessage.Trim();
            WriteLog(logMessage, level, modul);
        }

        public void WriteLog(WeatherConnector.WeatherArgs weatherArgs, LogLevel level, InfoServiceModul modul)
        {
            string exMessage = "";
            string stackTrace = "";
            string message = "";
            if (!string.IsNullOrEmpty(weatherArgs.ExMessage)) exMessage = weatherArgs.ExMessage;
            if (!string.IsNullOrEmpty(weatherArgs.StackTrace)) stackTrace = weatherArgs.StackTrace;
            if (!string.IsNullOrEmpty(weatherArgs.Message)) message = weatherArgs.Message;
            string logMessage = message + "\n\t\t\t\t\t\t" + exMessage + "\n\t\t\t\t\t\t" + stackTrace;
            logMessage = logMessage.Trim();
            WriteLog(logMessage, level, modul);
        }

        public void WriteLog(FeedReader.FeedArgs feedArgs, LogLevel level, InfoServiceModul modul)
        {
            string exMessage = "";
            string stackTrace = "";
            string message = "";
            if (!string.IsNullOrEmpty(feedArgs.ExMessage)) exMessage = feedArgs.ExMessage;
            if (!string.IsNullOrEmpty(feedArgs.StackTrace)) stackTrace = feedArgs.StackTrace;
            if (!string.IsNullOrEmpty(feedArgs.Message)) message = feedArgs.Message;
            string logMessage = message + "\n\t\t\t\t\t\t" + exMessage + "\n\t\t\t\t\t\t" + stackTrace;
            logMessage = logMessage.Trim();
            WriteLog(logMessage, level, modul);
        }

        public static void CloseLog()
        {
            try
            {
                if (Output != null)
                {
                    GetInstance().FlushTempLog();
                    Output.Flush();
                    Output.Close();
                    Output = null;
                    LogFile = string.Empty;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void FlushTempLog(string toFile)
        {
            LogFile = toFile;
            LogDebug = true;
            LogError = true;
            LogWarning = true;
            GetInstance().FlushTempLog();
        }
        public void FlushTempLog()
        {
            try
            {
                if (Output != null && _tempLogger != null && _tempLogger.Count > 0)
                {
                    WriteLog("Flushing the memory log", LogLevel.Debug, InfoServiceModul.InfoService);
                    foreach (TempLogger tempLogger in _tempLogger)
                    {
                        WriteLog(tempLogger.message, tempLogger.level, tempLogger.modul);
                    }
                    _tempLogger.Clear();
                    WriteLog("End flushing the memory log", LogLevel.Debug, InfoServiceModul.InfoService);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
