using System;
using System.IO;
using System.Xml.Serialization;
using InfoService.Settings.Data;
using InfoService.Utils;


namespace InfoService.Settings
{
    [XmlRoot("Settings")]
    public class SettingsManager : IXmlDeserializationCallback
    {
        private static readonly Logger Logger = Logger.GetInstance();
        
        private SettingsManager()
        {
            InitDefaultSettings();
        }

        private static volatile SettingsManager _Settings = null;

        public static bool SettingsLoaded { get; set; }

        public static void Initialize() 
        {
            _Settings = new SettingsManager();
        }

        private void InitDefaultSettings() {
            GeneralSettings = new SettingsGeneral();
            WebBrowserSettings = new SettingsWebBrowser();
            FeedSettings = new SettingsFeeds();
            TwitterSettings = new SettingsTwitter();
            WeatherSettings = new SettingsWeather();
            //RecentlyAddedSettings = new SettingsRecentlyAdded();
            FeedItemsFiltersSettings = new SettingsFeedItemsFilters();
        }

        public static SettingsManager Properties
        {
            get
            {
                if (_Settings == null)
                {
                    lock (_lock)
                    {
                        if (_Settings == null)
                        {
                            Initialize();
                        }
                    }
                }
                return _Settings;
            }
        }

        [XmlIgnore]
        private static object _lock = new object();

        public static bool Load(string path)
        {
            try
            {
                Initialize();

                if (!System.IO.File.Exists(path)) 
                {
                    Logger.WriteLog("InfoService is used the first time. Default settings will be used", LogLevel.Debug, InfoServiceModul.InfoService);
                    Properties.FeedSettings.CreateDefaultFeed();
                    Save(path);
                }
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    CustomXmlSerializer xmlSerializer = new CustomXmlSerializer(typeof(SettingsManager));
                    _Settings = (SettingsManager)xmlSerializer.Deserialize(fs);
                    SettingsLoaded = true;
                    return true;
                }
            }
            catch(System.Exception)
            {
                throw;
            }
        }

        public static bool Save(string path)
        {
            try
            {
                if (System.IO.File.Exists(path))
                {
                    string backupPath = path + ".bak";
                    if (System.IO.File.Exists(backupPath))
                        System.IO.File.Delete(backupPath);
                    System.IO.File.Move(path, backupPath);
                }
                using (FileStream fs = new FileStream(path, FileMode.Create))
                {
                    CustomXmlSerializer xmlSerializer = new CustomXmlSerializer(typeof(SettingsManager));
                    xmlSerializer.Serialize(fs, Properties);
                    fs.Close();
                    return true;
                }

            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public SettingsGeneral GeneralSettings;
        public SettingsWebBrowser WebBrowserSettings;
        public SettingsFeeds FeedSettings;
        public SettingsTwitter TwitterSettings;
        public SettingsWeather WeatherSettings;
        //public SettingsRecentlyAdded RecentlyAddedSettings;
        public SettingsFeedItemsFilters FeedItemsFiltersSettings;

        public void OnXmlDeserialization(object sender)
        {
            if (FeedSettings.Feeds.Count <= 0)
            {
                FeedSettings.CreateDefaultFeed();
            }
        }
    
    }
}
