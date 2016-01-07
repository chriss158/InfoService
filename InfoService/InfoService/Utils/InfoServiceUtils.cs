using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using InfoService.Enums;
using InfoService.Feeds;
using InfoService.GUIWindows;
using InfoService.Settings;
using InfoService.Twitter;
using MediaPortal.Configuration;
using MediaPortal.Dialogs;
using MediaPortal.GUI.Library;
using MediaPortal.Localisation;

namespace InfoService.Utils
{
    public static class InfoServiceUtils
    {
        private static LocalisationProvider _localizationStrings;

        private static readonly Logger logger = Logger.GetInstance();

        private static bool _localizationLoaded;

        public static void LoadLocalization()
        {
            _localizationLoaded = false;
            //string localizationFile = Config.GetSubFolder(Config.Dir.Language, @"InfoService\strings_en.xml");
            //logger.WriteLog("Try to load localization " + localizationFile, LogLevel.Debug, InfoServiceModul.InfoService);
            string localizationDirectory = Config.GetSubFolder(Config.Dir.Language, "InfoService");
            if (Directory.Exists(localizationDirectory))
            {
                logger.WriteLog("Looking for user language", LogLevel.Debug, InfoServiceModul.InfoService);
                //1.2 = gui, 1.1 = skin
                string language = new MediaPortal.Profile.MPSettings().GetValueAsString("gui", "language", "English");
                string localizationCultureName = string.Empty;
                if (!string.IsNullOrEmpty(language))
                {
                    logger.WriteLog("User language is " + language + ". Load localization...", LogLevel.Debug, InfoServiceModul.InfoService);
                    localizationCultureName = GUILocalizeStrings.GetCultureName(language);
                    if (File.Exists(Config.GetSubFolder(Config.Dir.Language, @"InfoService\strings_" + localizationCultureName + ".xml")))
                    {
                        _localizationStrings = new LocalisationProvider(localizationDirectory, localizationCultureName);
                        _localizationLoaded = true;
                    }
                    else
                    {
                        logger.WriteLog("Couldn't find user language " + Config.GetSubFolder(Config.Dir.Language, @"InfoService\strings_" + localizationCultureName + ".xml") + " for InfoService. Loading default (English) localization...", LogLevel.Error, InfoServiceModul.InfoService);
                        if (File.Exists(Config.GetSubFolder(Config.Dir.Language, @"InfoService\strings_en.xml")))
                        {
                            _localizationStrings = new LocalisationProvider(localizationDirectory, "en");
                            _localizationLoaded = true;
                        }
                    }
                }
                else logger.WriteLog("Couldn't get the user language. Didn't found user language from MediaPortal", LogLevel.Error, InfoServiceModul.InfoService);
            }
            else logger.WriteLog("Localization file doesn't exist. Folder " + localizationDirectory + " doesn't exist", LogLevel.Error, InfoServiceModul.InfoService);
        }

        public static string GetLocalizedLabel(int id)
        {
            return _localizationLoaded ? _localizationStrings.GetString("unmapped", id) : string.Empty;
        }

        public static void ShowDialogOkWindow(string header, string text)
        {
            logger.WriteLog("Show dialog window", LogLevel.Info, InfoServiceModul.InfoService);
            GUIDialogOK window = (GUIDialogOK)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_OK);
            window.SetHeading(header);
            List<string> textCol = new List<string>();
            int lastIndex = 0;
            for (int i = 44; i < text.Length; i += 45)
            {
                if (text[i] != ' ' || text[i] != ',' || text[i] != '.')
                {
                    while (i > 1)
                    {
                        i--;
                        if (text[i] == ' ' || text[i] == ',' || text[i] == '.')
                        {
                            break;
                        }
                    }
                }
                textCol.Add(text.Substring(lastIndex, i - lastIndex));
                lastIndex = i;
            }
            textCol.Add(text.Substring(lastIndex, text.Length - lastIndex));
            for (int i = 0; i < textCol.Count; i++) textCol[i] = textCol[i].Trim();
            for (int i = 1; i <= 4; i++)
            {
                if (i <= textCol.Count) window.SetLine(i, textCol[i - 1]);
            }
            //window.FlowDirection = FlowDirection.RightToLeft;
            window.DoModal(GUIWindowManager.ActiveWindow);

        }

        public static bool AreNotifyBarSkinFilesInstalled()
        {
            bool installed = File.Exists(GUIGraphicsContext.Skin + @"\infoservice.notifybar.xml");
            logger.WriteLog(
                installed
                    ? "InfoService NotifyBar skin files are installed."
                    : "InfoService NotifyBar skin files are NOT installed.", LogLevel.Info, InfoServiceModul.InfoService);
            return installed;
        }

        public static void ShowDialogNotifyWindow(string header, string text, string imagePath, System.Drawing.Size imageSize, int timeout)
        {
            ShowDialogNotifyWindow(header, text, imagePath, imageSize, timeout, null);
        }

        public static void ShowDialogNotifyWindow(string header, string text, string imagePath, System.Drawing.Size imageSize, int timeout, System.Action action)
        {
            logger.WriteLog("Show notify window with image", LogLevel.Info, InfoServiceModul.InfoService);
            object window = null;
            bool notifyBarFound;
            if (!AreNotifyBarSkinFilesInstalled())
            {
                window = (GUIDialogNotify) GUIWindowManager.GetWindow((int) GUIWindow.Window.WINDOW_DIALOG_NOTIFY);
                notifyBarFound = false;
            }
            else
            {
                window = (GUINotifyBar)GUIWindowManager.GetWindow(GUINotifyBar.ID);
                notifyBarFound = true;
            }

            if (window != null)
            {
                if (notifyBarFound)
                {
                    if(action != null) ((GUINotifyBar) window).OkAction = action;
                    ((GUINotifyBar)window).SetHeading(header);
                    ((GUINotifyBar)window).SetText(text);
                    ((GUINotifyBar)window).SetImage(imagePath);
                    ((GUINotifyBar)window).TimeOut = timeout;
                    ((GUINotifyBar)window).SetImageDimensions(imageSize, true, true);
                    ((GUINotifyBar)window).DoModal(GUIWindowManager.ActiveWindow);
                }
                else
                {
                    ((GUIDialogNotify)window).SetHeading(header);
                    ((GUIDialogNotify)window).SetText(text);
                    ((GUIDialogNotify)window).SetImage(imagePath);
                    ((GUIDialogNotify)window).TimeOut = timeout;
                    ((GUIDialogNotify)window).SetImageDimensions(imageSize, true, true);
                    ((GUIDialogNotify)window).DoModal(GUIWindowManager.ActiveWindow);
                }
            }
        }
        public static void ShowDialogNotifyWindow(string header, string text, int timeout)
        {
            logger.WriteLog("Show notify window", LogLevel.Info, InfoServiceModul.InfoService);
            //GUIDialogNotify window = (GUIDialogNotify)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_NOTIFY);
            GUINotifyBar window = (GUINotifyBar)GUIWindowManager.GetWindow(GUINotifyBar.ID);
            window.SetHeading(header);
            window.SetText(text);
            window.TimeOut = timeout;
            window.DoModal(GUIWindowManager.ActiveWindow);
        }



        public static string GetTimeDifferenceToNow(DateTime datetime)
        {
            //logger.WriteLog("Calc time difference", LogLevel.Debug, InfoServiceModul.InfoService);
            if (datetime.Date != new DateTime())
            {
                TimeSpan diff = (DateTime.Now - datetime);
                if (diff.TotalSeconds >= 1)
                {
                    if (diff.TotalSeconds >= 1 && diff.TotalSeconds < 60)
                    {
                        return string.Format(GetLocalizedLabel(20), (int)diff.TotalSeconds);
                    }
                    if (diff.TotalMinutes < 60 && diff.TotalMinutes >= 1)
                    {
                        return string.Format(GetLocalizedLabel(21), (int)diff.TotalMinutes);
                    }
                    if (diff.TotalHours < 24 && diff.TotalHours >= 1)
                    {
                        return string.Format(GetLocalizedLabel(22), (int)diff.TotalHours, diff.Minutes);
                    }
                    if (diff.TotalDays >= 1)
                    {
                        return string.Format(GetLocalizedLabel(23), (int)diff.TotalDays, diff.Hours, diff.Minutes);
                    }
                }
            }
            logger.WriteLog("Cannot calc time difference. Feed item has no Date/Time", LogLevel.Warning, InfoServiceModul.InfoService);
            return "";
        }

        public static bool ShowKeyboard(int window, ref string sString)
        {
            logger.WriteLog("Show keyboard", LogLevel.Info, InfoServiceModul.InfoService);
            VirtualKeyboard keyboard = (VirtualKeyboard)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_VIRTUAL_KEYBOARD);
            if (null == keyboard)
            {
                return false;
            }
            keyboard.IsSearchKeyboard = false;
            keyboard.Reset();
            keyboard.Text = sString;
            keyboard.DoModal(window);
            if (keyboard.IsConfirmed)
            {
                sString = keyboard.Text;
            }
            return keyboard.IsConfirmed;
        }

        public static void DeleteTwitterCache()
        {
            logger.WriteLog("Try to delete twitter cache", LogLevel.Info, InfoServiceModul.Twitter);

            DateTime lastdeletion = SettingsManager.Properties.TwitterSettings.LastCacheDeletion;
            if (lastdeletion.Ticks == 0)
            {
                SettingsManager.Properties.TwitterSettings.LastCacheDeletion = DateTime.Now;
                logger.WriteLog("Couldn't parse last twitter cache deletion date. Set last twitter cache deletion to now", LogLevel.Warning, InfoServiceModul.Twitter);
            }
            else
            {
                if (TwitterService.DeletionInterval == DeleteCache.Day &&
                    DateTime.Today.Subtract(lastdeletion).Days >= 1)
                {
                    logger.WriteLog("Time to delete twitter cache after 1 day", LogLevel.Debug, InfoServiceModul.Twitter);
                    TwitterService.DeleteCache();
                    SettingsManager.Properties.TwitterSettings.LastCacheDeletion = DateTime.Today;
                }
                else if (TwitterService.DeletionInterval == DeleteCache.Week &&
                         DateTime.Today.Subtract(lastdeletion).Days >= 7)
                {
                    logger.WriteLog("Time to delete twitter cache after 1 week", LogLevel.Debug, InfoServiceModul.Twitter);
                    TwitterService.DeleteCache();
                    SettingsManager.Properties.TwitterSettings.LastCacheDeletion = DateTime.Today;
                }
                else if (TwitterService.DeletionInterval == DeleteCache.Month &&
                         DateTime.Today.Subtract(lastdeletion).Days >= 30)
                {
                    logger.WriteLog("Time to delete twitter cache after 1 month", LogLevel.Debug, InfoServiceModul.Twitter);
                    TwitterService.DeleteCache();
                    SettingsManager.Properties.TwitterSettings.LastCacheDeletion = DateTime.Today;
                }
                else
                {
                    logger.WriteLog("It's not the time to delete twitter cache", LogLevel.Info, InfoServiceModul.Twitter);
                    logger.WriteLog(string.Format("Last deletion time: {0}, Deletion interval: {1}, Days passed: {2}", lastdeletion, TwitterService.DeletionInterval, DateTime.Today.Subtract(lastdeletion).Days), LogLevel.Info, InfoServiceModul.Twitter);
                }
            }

        }

        public static void DeleteFeedCache()
        {
            logger.WriteLog("Try to delete feed cache", LogLevel.Info, InfoServiceModul.Feed);
            DateTime lastdeletion = SettingsManager.Properties.FeedSettings.LastCacheDeletion;
            if (lastdeletion.Ticks == 0)
            {
                logger.WriteLog("Couldn't parse last feed cache deletion date. Set last feed cache deletion to now", LogLevel.Warning, InfoServiceModul.Feed);
                SettingsManager.Properties.FeedSettings.LastCacheDeletion = DateTime.Now;
            }
            else
            {
                if (FeedService.DeletionInterval == DeleteCache.Day &&
                    DateTime.Today.Subtract(lastdeletion).Days >= 1)
                {
                    logger.WriteLog("Time to delete feed cache after 1 day", LogLevel.Debug, InfoServiceModul.Feed);
                    FeedService.DeleteCache();
                    SettingsManager.Properties.FeedSettings.LastCacheDeletion = DateTime.Today;
                }
                else if (FeedService.DeletionInterval == DeleteCache.Week &&
                         DateTime.Today.Subtract(lastdeletion).Days >= 7)
                {
                    logger.WriteLog("Time to delete feed cache after 1 week", LogLevel.Debug, InfoServiceModul.Feed);
                    FeedService.DeleteCache();
                    SettingsManager.Properties.FeedSettings.LastCacheDeletion = DateTime.Today;
                }
                else if (FeedService.DeletionInterval == DeleteCache.Month &&
                         DateTime.Today.Subtract(lastdeletion).Days >= 30)
                {
                    logger.WriteLog("Time to delete feed cache after 1 month", LogLevel.Debug, InfoServiceModul.Feed);
                    FeedService.DeleteCache();
                    SettingsManager.Properties.FeedSettings.LastCacheDeletion = DateTime.Today;
                }
                else
                {
                    logger.WriteLog("It's not the time to delete feed cache", LogLevel.Info, InfoServiceModul.Feed);
                    logger.WriteLog(string.Format("Last deletion time: {0}, Deletion interval: {1}, Days passed: {2}", lastdeletion, FeedService.DeletionInterval, DateTime.Today.Subtract(lastdeletion).Days), LogLevel.Info, InfoServiceModul.Feed);
                }
            }
        }

        public static void InitLog()
        {
            if (SettingsManager.SettingsLoaded)
            {
                Logger.LogFile = Config.GetFile(Config.Dir.Log, "InfoService.log");
                Logger.LogDebug = SettingsManager.Properties.GeneralSettings.LogDebug;
                Logger.LogWarning = SettingsManager.Properties.GeneralSettings.LogWarning;
                Logger.LogError = SettingsManager.Properties.GeneralSettings.LogError;
            }
            else
                logger.WriteLog("Settings not yet loaded, skipping log initialization", LogLevel.Debug, InfoServiceModul.InfoService);

        }

        public static string GetWindowIDFromEnum(this WebBrowserType wbType)
        {
            return StringEnum.GetWindowID(wbType) ?? SettingsManager.Properties.WebBrowserSettings.WindowID ?? string.Empty;
        }

        public static List<InfoService.Settings.Data.SettingsWebBrowserGUIProperty> GetPropertyValueListFromEnum(this WebBrowserType wbType)
        {
            List<InfoService.Settings.Data.SettingsWebBrowserGUIProperty> resultList = new List<InfoService.Settings.Data.SettingsWebBrowserGUIProperty>();

            string propertyValue = StringEnum.GetPropertyValue(wbType);

            if (propertyValue != null)
            {
                foreach (string line in propertyValue.Split(new[] { "\n" }, StringSplitOptions.None))
                {
                    string[] tokens = line.Split(new char[] { ':' }, 2);
                    if (tokens.Length < 2)
                    {
                        continue;
                    }

                    InfoService.Settings.Data.SettingsWebBrowserGUIProperty guiProperty = new InfoService.Settings.Data.SettingsWebBrowserGUIProperty();
                    guiProperty.Property = tokens[0].Trim();
                    guiProperty.Value = tokens[1].Trim();

                    resultList.Add(guiProperty);
                }
            }
            else
            {
                resultList = SettingsManager.Properties.WebBrowserSettings.GUIProperties;
            }

            return resultList;
        }

        public static int GetWebBrowserWindowId(string url, string zoom)
        {
            int windowID;
            if (!int.TryParse(GetWindowIDFromEnum(SettingsManager.Properties.WebBrowserSettings.BrowserType), out windowID) || windowID < 1)
            {
                logger.WriteLog("Window ID for Web browser is invalid, please use InfoService advanced configuration to reconfigure it!", LogLevel.Warning, InfoServiceModul.InfoService);
                return -1;
            }

            foreach (InfoService.Settings.Data.SettingsWebBrowserGUIProperty guiProperty in GetPropertyValueListFromEnum(SettingsManager.Properties.WebBrowserSettings.BrowserType))
            {
                string value = guiProperty.Value;
                value = value.Replace("%link%", url);
                value = value.Replace("%zoom%", zoom);

                PropertyUtils.SetProperty(guiProperty.Property, value);
            }

            return windowID;

        }
        public static bool IsValidUrl(String Url)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(Url, @"((https?):((//)|(\\\\))+[\w\d:#@%/;$()~_?\+-=\\\.&]*)");
        }
        public static bool IsValidPath(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                Regex r = new Regex(@"^(([a-zA-Z]\:)|(\\))(\\{1}|((\\{1})[^\\]([^/:*?<>""|]*))+)$");
                return r.IsMatch(path);
            }
            return false;
        }
        public static bool IsAssemblyAvailable(string name, Version ver)
        {
            bool result = false;

            logger.WriteLog(string.Format("Checking whether assembly {0} is available and loaded...", name), LogLevel.Info, InfoServiceModul.InfoService);

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly a in assemblies)
            {
                try
                {
                    if (a.GetName().Name == name && a.GetName().Version >= ver)
                    {
                        logger.WriteLog(string.Format("Assembly {0} v{1} is available and loaded.", name, a.GetName().Version.ToString()), LogLevel.Info, InfoServiceModul.InfoService);
                        result = true;
                        break;
                    }
                }
                catch (Exception e)
                {
                    logger.WriteLog(string.Format("Assembly.GetName() call failed for '{0}'!\nException: {1}", a.Location, e), LogLevel.Warning, InfoServiceModul.InfoService);
                }
            }

            if (!result)
            {
                logger.WriteLog(string.Format("Assembly {0} is not loaded (not available?), trying to load it manually...", name), LogLevel.Info, InfoServiceModul.InfoService);
                try
                {
                    Assembly assembly = Assembly.ReflectionOnlyLoad(name);
                    logger.WriteLog(string.Format("Assembly {0} v{1} is available and loaded successfully.", name, assembly.GetName().Version.ToString()), LogLevel.Info, InfoServiceModul.InfoService);
                    result = true;
                }
                catch (Exception e)
                {
                    logger.WriteLog(string.Format("Assembly {0} is unavailable, load unsuccessful: {1}:{2}", name, e.GetType(), e.Message), LogLevel.Info, InfoServiceModul.InfoService);
                }
            }

            return result;
        }



    }
}
