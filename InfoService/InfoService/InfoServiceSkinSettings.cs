using System.Collections;
using System.Xml;
using InfoService.Settings;
using InfoService.Utils;
using InfoService.Enums;
using MediaPortal.Configuration;
using MediaPortal.Profile;

namespace InfoService
{
    public class InfoServiceSkinSettings
    {
        #region Object Instance
        private static InfoServiceSkinSettings instance = null;
        public static InfoServiceSkinSettings InfoServiceSkinSettingsInstance
        {
            get { return instance ?? (instance = new InfoServiceSkinSettings()); }
            set
            {
            }
        }
        #endregion

        #region Private variables
        private static readonly Logger logger = Logger.GetInstance();
        private IDictionary defines = null;
        #endregion

        #region Feeds
        public bool Feeds_Enabled { get; set; }
        private string feeds_Enabled_Val;
        public string Feeds_Enabled_Val
        {
            get
            {
                return feeds_Enabled_Val;
            }
            set
            {
                bool isSet = false;

                feeds_Enabled_Val = string.Empty;
                if (!string.IsNullOrEmpty(value))
                {
                    logger.WriteLog(string.Format("Loading skin setting #InfoService.SkinSettings.Feeds.Enabled with value '{0}'", value), LogLevel.Debug, InfoServiceModul.SkinSettings);

                    if (value.ToUpper() == "TRUE" || value.ToUpper() == "YES")
                    {
                        //SettingsManager.Properties.FeedSettings.Enabled = true;
                        feeds_Enabled_Val = "TRUE";
                        Feeds_Enabled = true;
                        isSet = true;
                    }
                    else if (value.ToUpper() == "FALSE" || value.ToUpper() == "NO")
                    {
                        //SettingsManager.Properties.FeedSettings.Enabled = false;
                        feeds_Enabled_Val = "FALSE";
                        Feeds_Enabled = false;
                        isSet = true;
                    }
                }

                if (!isSet)
                    Feeds_Enabled = SettingsManager.Properties.FeedSettings.Enabled;
            }
        }

        public decimal Feeds_General_RefreshFeedEvery { get; set; }
        private string feeds_General_RefreshFeedEvery_Val;
        public string Feeds_General_RefreshFeedEvery_Val
        {
            get
            {
                return feeds_General_RefreshFeedEvery_Val;
            }
            set
            {
                bool isSet = false;

                feeds_General_RefreshFeedEvery_Val = string.Empty;
                if (!string.IsNullOrEmpty(value))
                {
                    logger.WriteLog(string.Format("Loading skin setting #InfoService.SkinSettings.Feeds.General.RefreshFeedEvery with value '{0}'", value), LogLevel.Debug, InfoServiceModul.SkinSettings);

                    int valueInt;
                    if (int.TryParse(value, out valueInt))
                    {
                        //SettingsManager.Properties.FeedSettings.RefreshInterval = valueInt;
                        feeds_General_RefreshFeedEvery_Val = valueInt.ToString();
                        Feeds_General_RefreshFeedEvery = valueInt;
                        isSet = true;
                    }
                }

                if (!isSet)
                    Feeds_General_RefreshFeedEvery = SettingsManager.Properties.FeedSettings.RefreshInterval;
            }
        }

        public bool Feeds_Layout_ShowFeedItemPublishTime { get; set; }
        private string feeds_Layout_ShowFeedItemPublishTime_Val;
        public string Feeds_Layout_ShowFeedItemPublishTime_Val
        {
            get
            {
                return feeds_Layout_ShowFeedItemPublishTime_Val;
            }
            set
            {
                bool isSet = false;

                feeds_Layout_ShowFeedItemPublishTime_Val = string.Empty;
                if (!string.IsNullOrEmpty(value))
                {
                    logger.WriteLog(string.Format("Loading skin setting #InfoService.SkinSettings.Feeds.Layout.ShowFeedItemPublishTime with value '{0}'", value), LogLevel.Debug, InfoServiceModul.SkinSettings);

                    if (value.ToUpper() == "TRUE" || value.ToUpper() == "YES")
                    {
                        //SettingsManager.Properties.FeedSettings.ShowItemPublishTime = true;
                        feeds_Layout_ShowFeedItemPublishTime_Val = "TRUE";
                        Feeds_Layout_ShowFeedItemPublishTime = true;
                        isSet = true;
                    }
                    else if (value.ToUpper() == "FALSE" || value.ToUpper() == "NO")
                    {
                        //SettingsManager.Properties.FeedSettings.ShowItemPublishTime = false;
                        feeds_Layout_ShowFeedItemPublishTime_Val = "FALSE";
                        Feeds_Layout_ShowFeedItemPublishTime = false;
                        isSet = true;
                    }
                }

                if (!isSet)
                    Feeds_Layout_ShowFeedItemPublishTime = SettingsManager.Properties.FeedSettings.ShowItemPublishTime;
            }
        }

        public ItemPublishTimeAllFeedsType Feeds_Layout_ItemPublishTimeAllFeeds { get; set; }
        private string feeds_Layout_ItemPublishTimeAllFeeds_Val;
        public string Feeds_Layout_ItemPublishTimeAllFeeds_Val
        {
            get
            {
                return feeds_Layout_ItemPublishTimeAllFeeds_Val;
            }
            set
            {
                bool isSet = false;

                feeds_Layout_ItemPublishTimeAllFeeds_Val = string.Empty;
                if (!string.IsNullOrEmpty(value))
                {
                    logger.WriteLog(string.Format("Loading skin setting #InfoService.SkinSettings.Feeds.Layout.ItemPublishTimeAllFeeds with value '{0}'", value), LogLevel.Debug, InfoServiceModul.SkinSettings);

                    if (value.ToUpper() == ItemPublishTimeAllFeedsType.PublishTime.ToString().ToUpper())
                    {
                        //SettingsManager.Properties.FeedSettings.ItemPublishTimeAllFeeds = true;
                        feeds_Layout_ItemPublishTimeAllFeeds_Val = ItemPublishTimeAllFeedsType.PublishTime.ToString();
                        Feeds_Layout_ItemPublishTimeAllFeeds = ItemPublishTimeAllFeedsType.PublishTime;
                        isSet = true;
                    }
                    else if (value.ToUpper() == ItemPublishTimeAllFeedsType.FeedName.ToString().ToUpper())
                    {
                        //SettingsManager.Properties.FeedSettings.ItemPublishTimeAllFeeds = true;
                        feeds_Layout_ItemPublishTimeAllFeeds_Val = ItemPublishTimeAllFeedsType.FeedName.ToString();
                        Feeds_Layout_ItemPublishTimeAllFeeds = ItemPublishTimeAllFeedsType.FeedName;
                        isSet = true;
                    }
                    else if (value.ToUpper() == ItemPublishTimeAllFeedsType.Both.ToString().ToUpper())
                    {
                        //SettingsManager.Properties.FeedSettings.ItemPublishTimeAllFeeds = true;
                        feeds_Layout_ItemPublishTimeAllFeeds_Val = ItemPublishTimeAllFeedsType.Both.ToString();
                        Feeds_Layout_ItemPublishTimeAllFeeds = ItemPublishTimeAllFeedsType.Both;
                        isSet = true;
                    }
                }

                if (!isSet)
                    Feeds_Layout_ItemPublishTimeAllFeeds = SettingsManager.Properties.FeedSettings.ItemPublishTimeAllFeeds;
            }
        }

        public decimal Feeds_Layout_MaxItemsPerFeedForAllFeeds { get; set; }
        private string feeds_Layout_MaxItemsPerFeedForAllFeeds_Val;
        public string Feeds_Layout_MaxItemsPerFeedForAllFeeds_Val
        {
            get
            {
                return feeds_Layout_MaxItemsPerFeedForAllFeeds_Val;
            }
            set
            {
                bool isSet = false;

                feeds_Layout_MaxItemsPerFeedForAllFeeds_Val = string.Empty;
                if (!string.IsNullOrEmpty(value))
                {
                    logger.WriteLog(string.Format("Loading skin setting #InfoService.SkinSettings.Feeds.Layout.MaxItemsPerFeedForAllFeeds with value '{0}'", value), LogLevel.Debug, InfoServiceModul.SkinSettings);

                    int valueInt;
                    if (int.TryParse(value, out valueInt))
                    {
                        //SettingsManager.Properties.FeedSettings.ItemsCountPerFeed = valueInt;
                        feeds_Layout_MaxItemsPerFeedForAllFeeds_Val = valueInt.ToString();
                        Feeds_Layout_MaxItemsPerFeedForAllFeeds = valueInt;
                        isSet = true;
                    }
                }

                if (!isSet)
                    Feeds_Layout_MaxItemsPerFeedForAllFeeds = SettingsManager.Properties.FeedSettings.ItemsCountPerFeed;
            }
        }

        public decimal Feeds_Layout_MaxItemsForFeedTicker { get; set; }
        private string feeds_Layout_MaxItemsForFeedTicker_Val;
        public string Feeds_Layout_MaxItemsForFeedTicker_Val
        {
            get
            {
                return feeds_Layout_MaxItemsForFeedTicker_Val;
            }
            set
            {
                bool isSet = false;

                feeds_Layout_MaxItemsForFeedTicker_Val = string.Empty;
                if (!string.IsNullOrEmpty(value))
                {
                    logger.WriteLog(string.Format("Loading skin setting #InfoService.SkinSettings.Feeds.Layout.MaxItemsForFeedTicker with value '{0}'", value), LogLevel.Debug, InfoServiceModul.SkinSettings);

                    int valueInt;
                    if (int.TryParse(value, out valueInt))
                    {
                        //SettingsManager.Properties.FeedSettings.ItemsCount = valueInt;
                        feeds_Layout_MaxItemsForFeedTicker_Val = valueInt.ToString();
                        Feeds_Layout_MaxItemsForFeedTicker = valueInt;
                        isSet = true;
                    }
                }

                if (!isSet)
                    Feeds_Layout_MaxItemsForFeedTicker = SettingsManager.Properties.FeedSettings.ItemsCount;
            }
        }

        public string Feeds_TickerLayout_TickerMask { get; set; }
        private string feeds_TickerLayout_TickerMask_Val;
        public string Feeds_TickerLayout_TickerMask_Val
        {
            get
            {
                return feeds_TickerLayout_TickerMask_Val; 
            }
            set
            {
                bool isSet = false;

                feeds_TickerLayout_TickerMask_Val = string.Empty;
                if (!string.IsNullOrEmpty(value))
                {
                    logger.WriteLog(string.Format("Loading skin setting #InfoService.SkinSettings.Feeds.TickerLayout.TickerMask with value '{0}'", value), LogLevel.Debug, InfoServiceModul.SkinSettings);

                    //SettingsManager.Properties.FeedSettings.TickerMask = value;
                    feeds_TickerLayout_TickerMask_Val = value;
                    Feeds_TickerLayout_TickerMask = value;
                    isSet = true;
                }

                if (!isSet)
                    Feeds_TickerLayout_TickerMask = SettingsManager.Properties.FeedSettings.TickerMask;
            }
        }

        public string Feeds_TickerLayout_TickerAllMask { get; set; }
        private string feeds_TickerLayout_TickerAllMask_Val;
        public string Feeds_TickerLayout_TickerAllMask_Val
        {
            get
            {
                return feeds_TickerLayout_TickerAllMask_Val;
            }
            set
            {
                bool isSet = false;

                feeds_TickerLayout_TickerAllMask_Val = string.Empty;
                if (!string.IsNullOrEmpty(value))
                {
                    logger.WriteLog(string.Format("Loading skin setting #InfoService.SkinSettings.Feeds.TickerLayout.TickerAllMask with value '{0}'", value), LogLevel.Debug, InfoServiceModul.SkinSettings);

                    //SettingsManager.Properties.FeedSettings.TickerMask = value;
                    feeds_TickerLayout_TickerAllMask_Val = value;
                    Feeds_TickerLayout_TickerAllMask = value;
                    isSet = true;
                }

                if (!isSet)
                    Feeds_TickerLayout_TickerAllMask = SettingsManager.Properties.FeedSettings.TickerAllMask;
            }
        }

        public string Feeds_TickerLayout_Separator { get; set; }
        private string feeds_TickerLayout_Separator_Val;
        public string Feeds_TickerLayout_Separator_Val
        {
            get
            {
                return feeds_TickerLayout_Separator_Val;
            }
            set
            {
                bool isSet = false;

                feeds_TickerLayout_Separator_Val = string.Empty;
                if (!string.IsNullOrEmpty(value))
                {
                    logger.WriteLog(string.Format("Loading skin setting #InfoService.SkinSettings.Feeds.TickerLayout.Separator with value '{0}'", value), LogLevel.Debug, InfoServiceModul.SkinSettings);

                    //SettingsManager.Properties.FeedSettings.Separator = value;
                    feeds_TickerLayout_Separator_Val = value;
                    Feeds_TickerLayout_Separator = value;
                    isSet = true;
                }

                if (!isSet)
                    Feeds_TickerLayout_Separator = SettingsManager.Properties.FeedSettings.Separator;
            }
        }

        public string Feeds_TickerLayout_SeparatorAll { get; set; }
        private string feeds_TickerLayout_SeparatorAll_Val;
        public string Feeds_TickerLayout_SeparatorAll_Val
        {
            get
            {
                return feeds_TickerLayout_SeparatorAll_Val;
            }
            set
            {
                bool isSet = false;

                feeds_TickerLayout_SeparatorAll_Val = string.Empty;
                if (!string.IsNullOrEmpty(value))
                {
                    logger.WriteLog(string.Format("Loading skin setting #InfoService.SkinSettings.Feeds.TickerLayout.SeparatorAll with value '{0}'", value), LogLevel.Debug, InfoServiceModul.SkinSettings);

                    //SettingsManager.Properties.FeedSettings.Separator = value;
                    feeds_TickerLayout_SeparatorAll_Val = value;
                    Feeds_TickerLayout_SeparatorAll = value;
                    isSet = true;
                }

                if (!isSet)
                    Feeds_TickerLayout_SeparatorAll = SettingsManager.Properties.FeedSettings.SeparatorAll;
            }
        }
        #endregion

        #region Weather
        public bool Weather_Enabled { get; set; }
        private string weather_Enabled_Val;
        public string Weather_Enabled_Val
        {
            get
            {
                return weather_Enabled_Val;
            }
            set
            {
                bool isSet = false;

                weather_Enabled_Val = string.Empty;
                if (!string.IsNullOrEmpty(value))
                {
                    logger.WriteLog(string.Format("Loading skin setting #InfoService.SkinSettings.Weather.Enabled with value '{0}'", value), LogLevel.Debug, InfoServiceModul.SkinSettings);

                    if (value.ToUpper() == "TRUE" || value.ToUpper() == "YES")
                    {
                        //SettingsManager.Properties.WeatherSettings.Enabled = true;
                        weather_Enabled_Val = "TRUE";
                        Weather_Enabled = true;
                        isSet = true;
                    }
                    else if (value.ToUpper() == "FALSE" || value.ToUpper() == "NO")
                    {
                        //SettingsManager.Properties.WeatherSettings.Enabled = false;
                        weather_Enabled_Val = "FALSE";
                        Weather_Enabled = false;
                        isSet = true;
                    }
                }

                if (!isSet)
                    Weather_Enabled = SettingsManager.Properties.WeatherSettings.Enabled;
            }
        }

        public decimal Weather_General_RefreshWeatherEvery { get; set; }
        private string weather_General_RefreshWeatherEvery_Val;
        public string Weather_General_RefreshWeatherEvery_Val
        {
            get
            {
                return weather_General_RefreshWeatherEvery_Val;
            }
            set
            {
                bool isSet = false;

                weather_General_RefreshWeatherEvery_Val = string.Empty;
                if (!string.IsNullOrEmpty(value))
                {
                    logger.WriteLog(string.Format("Loading skin setting #InfoService.SkinSettings.Weather.General.RefreshWeatherEvery with value '{0}'", value), LogLevel.Debug, InfoServiceModul.SkinSettings);

                    int valueInt;
                    if (int.TryParse(value, out valueInt))
                    {
                        //SettingsManager.Properties.WeatherSettings.RefreshInterval = valueInt;
                        weather_General_RefreshWeatherEvery_Val = valueInt.ToString();
                        Weather_General_RefreshWeatherEvery = valueInt;
                        isSet = true;
                    }
                }

                if (!isSet)
                    Weather_General_RefreshWeatherEvery = SettingsManager.Properties.WeatherSettings.RefreshInterval;
            }
        }
        #endregion

        #region Twitter
        public bool Twitter_Enabled { get; set; }
        private string twitter_Enabled_Val;
        public string Twitter_Enabled_Val
        {
            get
            {
                return twitter_Enabled_Val;
            }
            set
            {
                bool isSet = false;

                twitter_Enabled_Val = string.Empty;
                if (!string.IsNullOrEmpty(value))
                {
                    logger.WriteLog(string.Format("Loading skin setting #InfoService.SkinSettings.Twitter.Enabled with value '{0}'", value), LogLevel.Debug, InfoServiceModul.SkinSettings);

                    if (value.ToUpper() == "TRUE" || value.ToUpper() == "YES")
                    {
                        //SettingsManager.Properties.TwitterSettings.Enabled = true;
                        twitter_Enabled_Val = "TRUE";
                        Twitter_Enabled = true;
                        isSet = true;
                    }
                    else if (value.ToUpper() == "FALSE" || value.ToUpper() == "NO")
                    {
                        //SettingsManager.Properties.TwitterSettings.Enabled = false;
                        twitter_Enabled_Val = "FALSE";
                        Twitter_Enabled = false;
                        isSet = true;
                    }
                }

                if (!isSet)
                    Twitter_Enabled = SettingsManager.Properties.TwitterSettings.Enabled;
            }
        }

        public decimal Twitter_General_RefreshTwitterEvery { get; set; }
        private string twitter_General_RefreshTwitterEvery_Val;
        public string Twitter_General_RefreshTwitterEvery_Val
        {
            get
            {
                return twitter_General_RefreshTwitterEvery_Val;
            }
            set
            {
                bool isSet = false;

                twitter_General_RefreshTwitterEvery_Val = string.Empty;
                if (!string.IsNullOrEmpty(value))
                {
                    logger.WriteLog(string.Format("Loading skin setting #InfoService.SkinSettings.Twitter.General.RefreshTwitterEvery with value '{0}'", value), LogLevel.Debug, InfoServiceModul.SkinSettings);

                    int valueInt;
                    if (int.TryParse(value, out valueInt))
                    {
                        //SettingsManager.Properties.TwitterSettings.RefreshInterval = valueInt;
                        twitter_General_RefreshTwitterEvery_Val = valueInt.ToString();
                        Twitter_General_RefreshTwitterEvery = valueInt;
                        isSet = true;
                    }
                }

                if (!isSet)
                    Twitter_General_RefreshTwitterEvery = SettingsManager.Properties.TwitterSettings.RefreshInterval;
            }
        }

        public decimal Twitter_Layout_MaxItemsForTwitterTicker { get; set; }
        private string twitter_Layout_MaxItemsForTwitterTicker_Val;
        public string Twitter_Layout_MaxItemsForTwitterTicker_Val
        {
            get
            {
                return twitter_Layout_MaxItemsForTwitterTicker_Val;
            }
            set
            {
                bool isSet = false;

                twitter_Layout_MaxItemsForTwitterTicker_Val = string.Empty;
                if (!string.IsNullOrEmpty(value))
                {
                    logger.WriteLog(string.Format("Loading skin setting #InfoService.SkinSettings.Twitter.Layout.MaxItemsForTwitterTicker with value '{0}'", value), LogLevel.Debug, InfoServiceModul.SkinSettings);

                    int valueInt;
                    if (int.TryParse(value, out valueInt))
                    {
                        //SettingsManager.Properties.TwitterSettings.ItemsCount = valueInt;
                        twitter_Layout_MaxItemsForTwitterTicker_Val = valueInt.ToString();
                        Twitter_Layout_MaxItemsForTwitterTicker = valueInt;
                        isSet = true;
                    }
                }

                if (!isSet)
                    Twitter_Layout_MaxItemsForTwitterTicker = SettingsManager.Properties.TwitterSettings.ItemsCount;
            }
        }

        public string Twitter_TickerLayout_TickerMask { get; set; }
        private string twitter_TickerLayout_TickerMask_Val;
        public string Twitter_TickerLayout_TickerMask_Val
        {
            get
            {
                return twitter_TickerLayout_TickerMask_Val;
            }
            set
            {
                bool isSet = false;

                twitter_TickerLayout_TickerMask_Val = string.Empty;
                if (!string.IsNullOrEmpty(value))
                {
                    logger.WriteLog(string.Format("Loading skin setting #InfoService.SkinSettings.Twitter.TickerLayout.TickerMask with value '{0}'", value), LogLevel.Debug, InfoServiceModul.SkinSettings);

                    //SettingsManager.Properties.TwitterSettings.TickerMask = value;
                    twitter_TickerLayout_TickerMask_Val = value;
                    Twitter_TickerLayout_TickerMask = value;
                    isSet = true;
                }

                if (!isSet)
                    Twitter_TickerLayout_TickerMask = SettingsManager.Properties.TwitterSettings.TickerMask;
            }
        }

        public string Twitter_TickerLayout_Separator { get; set; }
        private string twitter_TickerLayout_Separator_Val;
        public string Twitter_TickerLayout_Separator_Val
        {
            get
            {
                return twitter_TickerLayout_Separator_Val;
            }
            set
            {
                bool isSet = false;

                twitter_TickerLayout_Separator_Val = string.Empty;
                if (!string.IsNullOrEmpty(value))
                {
                    logger.WriteLog(string.Format("Loading skin setting #InfoService.SkinSettings.Twitter.TickerLayout.Separator with value '{0}'", value), LogLevel.Debug, InfoServiceModul.SkinSettings);

                    //SettingsManager.Properties.TwitterSettings.Separator = value;
                    twitter_TickerLayout_Separator_Val = value;
                    Twitter_TickerLayout_Separator = value;
                    isSet = true;
                }

                if (!isSet)
                    Twitter_TickerLayout_Separator = SettingsManager.Properties.TwitterSettings.Separator;
            }
        }
        #endregion

        #region Others
        public bool HasFeedsSkinSettings
        {
            get
            {
                return !string.IsNullOrEmpty(Feeds_Enabled_Val) ||
                       !string.IsNullOrEmpty(Feeds_General_RefreshFeedEvery_Val) ||
                       !string.IsNullOrEmpty(Feeds_Layout_MaxItemsForFeedTicker_Val) ||
                       !string.IsNullOrEmpty(Feeds_Layout_MaxItemsPerFeedForAllFeeds_Val) ||
                       !string.IsNullOrEmpty(Feeds_Layout_ShowFeedItemPublishTime_Val) ||
                       !string.IsNullOrEmpty(Feeds_Layout_ItemPublishTimeAllFeeds_Val) ||
                       !string.IsNullOrEmpty(Feeds_TickerLayout_TickerMask_Val) ||
                       !string.IsNullOrEmpty(Feeds_TickerLayout_TickerAllMask_Val) ||
                       !string.IsNullOrEmpty(Feeds_TickerLayout_Separator_Val) ||
                       !string.IsNullOrEmpty(Feeds_TickerLayout_SeparatorAll_Val);
            }
        }
        public bool HasWeatherSkinSettings
        {
            get
            {
                return !string.IsNullOrEmpty(Weather_Enabled_Val) ||
                       !string.IsNullOrEmpty(Weather_General_RefreshWeatherEvery_Val);
            }
        }
        public bool HasTwitterSkinSettings
        {
            get
            {
                return !string.IsNullOrEmpty(Twitter_Enabled_Val) ||
                       !string.IsNullOrEmpty(Twitter_General_RefreshTwitterEvery_Val) ||
                       !string.IsNullOrEmpty(Twitter_Layout_MaxItemsForTwitterTicker_Val) ||
                       !string.IsNullOrEmpty(Twitter_TickerLayout_Separator_Val) ||
                       !string.IsNullOrEmpty(Twitter_TickerLayout_TickerMask_Val);
            }
        }
        #endregion

        #region Helper Functions
        public void loadSkinSettings(string skinXMLFile)
        {
            initializeSkinSettings();
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(skinXMLFile);
                if (doc.DocumentElement != null)
                {
                    defines = LoadDefines(doc);
                    loadSettingsFromSkinSettings();
                }
            }
            catch
            {
                logger.WriteLog("Error loading skin settings from infoservice.xml: error in XML file", LogLevel.Error, InfoServiceModul.SkinSettings);
            }
        }

        private IDictionary LoadDefines(XmlDocument document)
        {
            Hashtable table = new Hashtable();
            try
            {
                foreach (XmlNode node in document.SelectNodes("/window/define"))
                {
                    if (node.InnerText.Contains("#InfoService.SkinSettings."))
                    {
                        logger.WriteLog("Found skin settings define in infoservice.xml: " + node.InnerText, LogLevel.Debug, InfoServiceModul.SkinSettings);

                        string[] tokens = node.InnerText.Split(new char[] { ':' }, 2);
                        if (tokens.Length < 2)
                        {
                            continue;
                        }
                        table[tokens[0]] = tokens[1];
                    }
                }
            }
            catch
            {
                logger.WriteLog("Error loading skin settings from infoservice.xml", LogLevel.Error, InfoServiceModul.SkinSettings);
            }
            return table;
        }

        private void loadSettingsFromSkinSettings()
        {
            if (defines != null)
            {
                #region Feeds

                string value = string.Empty;
                if (defines.Contains("#InfoService.SkinSettings.Feeds.Enabled"))
                    value = (string) defines["#InfoService.SkinSettings.Feeds.Enabled"];
                InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Feeds_Enabled_Val = value.Trim();

                value = string.Empty;
                if (defines.Contains("#InfoService.SkinSettings.Feeds.General.RefreshFeedEvery"))
                    value = (string) defines["#InfoService.SkinSettings.Feeds.General.RefreshFeedEvery"];
                InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Feeds_General_RefreshFeedEvery_Val = value.Trim();

                value = string.Empty;
                if (defines.Contains("#InfoService.SkinSettings.Feeds.Layout.ShowFeedItemPublishTime"))
                    value = (string) defines["#InfoService.SkinSettings.Feeds.Layout.ShowFeedItemPublishTime"];
                InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Feeds_Layout_ShowFeedItemPublishTime_Val = value.Trim();

                value = string.Empty;
                if (defines.Contains("#InfoService.SkinSettings.Feeds.Layout.ItemPublishTimeAllFeeds"))
                    value = (string)defines["#InfoService.SkinSettings.Feeds.Layout.ItemPublishTimeAllFeeds"];
                InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Feeds_Layout_ItemPublishTimeAllFeeds_Val = value.Trim();

                value = string.Empty;
                if (defines.Contains("#InfoService.SkinSettings.Feeds.Layout.MaxItemsPerFeedForAllFeeds"))
                    value = (string) defines["#InfoService.SkinSettings.Feeds.Layout.MaxItemsPerFeedForAllFeeds"];
                InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Feeds_Layout_MaxItemsPerFeedForAllFeeds_Val = value.Trim();

                value = string.Empty;
                if (defines.Contains("#InfoService.SkinSettings.Feeds.Layout.MaxItemsForFeedTicker"))
                    value = (string) defines["#InfoService.SkinSettings.Feeds.Layout.MaxItemsForFeedTicker"];
                InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Feeds_Layout_MaxItemsForFeedTicker_Val = value.Trim();

                value = string.Empty;
                if (defines.Contains("#InfoService.SkinSettings.Feeds.TickerLayout.TickerMask"))
                    value = (string) defines["#InfoService.SkinSettings.Feeds.TickerLayout.TickerMask"];
                InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Feeds_TickerLayout_TickerMask_Val = value;

                value = string.Empty;
                if (defines.Contains("#InfoService.SkinSettings.Feeds.TickerLayout.TickerAllMask"))
                    value = (string)defines["#InfoService.SkinSettings.Feeds.TickerLayout.TickerAllMask"];
                InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Feeds_TickerLayout_TickerAllMask_Val = value;

                value = string.Empty;
                if (defines.Contains("#InfoService.SkinSettings.Feeds.TickerLayout.Separator"))
                    value = (string) defines["#InfoService.SkinSettings.Feeds.TickerLayout.Separator"];
                InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Feeds_TickerLayout_Separator_Val = value;

                value = string.Empty;
                if (defines.Contains("#InfoService.SkinSettings.Feeds.TickerLayout.SeparatorAll"))
                    value = (string)defines["#InfoService.SkinSettings.Feeds.TickerLayout.SeparatorAll"];
                InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Feeds_TickerLayout_SeparatorAll_Val = value;

                #endregion

                #region Weather

                value = string.Empty;
                if (defines.Contains("#InfoService.SkinSettings.Weather.Enabled"))
                    value = (string) defines["#InfoService.SkinSettings.Weather.Enabled"];
                InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Weather_Enabled_Val = value.Trim();

                value = string.Empty;
                if (defines.Contains("#InfoService.SkinSettings.Weather.General.RefreshWeatherEvery"))
                    value = (string) defines["#InfoService.SkinSettings.Weather.General.RefreshWeatherEvery"];
                InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Weather_General_RefreshWeatherEvery_Val = value.Trim();

                #endregion

                #region Twitter

                value = string.Empty;
                if (defines.Contains("#InfoService.SkinSettings.Twitter.Enabled"))
                    value = (string) defines["#InfoService.SkinSettings.Twitter.Enabled"];
                InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Twitter_Enabled_Val = value.Trim();

                value = string.Empty;
                if (defines.Contains("#InfoService.SkinSettings.Twitter.General.RefreshTwitterEvery"))
                    value = (string) defines["#InfoService.SkinSettings.Twitter.General.RefreshTwitterEvery"];
                InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Twitter_General_RefreshTwitterEvery_Val = value.Trim();

                value = string.Empty;
                if (defines.Contains("#InfoService.SkinSettings.Twitter.Layout.MaxItemsForTwitterTicker"))
                    value = (string) defines["#InfoService.SkinSettings.Twitter.Layout.MaxItemsForTwitterTicker"];
                InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Twitter_Layout_MaxItemsForTwitterTicker_Val = value.Trim();

                value = string.Empty;
                if (defines.Contains("#InfoService.SkinSettings.Twitter.TickerLayout.TickerMask"))
                    value = (string) defines["#InfoService.SkinSettings.Twitter.TickerLayout.TickerMask"];
                InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Twitter_TickerLayout_TickerMask_Val = value;

                value = string.Empty;
                if (defines.Contains("#InfoService.SkinSettings.Twitter.TickerLayout.Separator"))
                    value = (string) defines["#InfoService.SkinSettings.Twitter.TickerLayout.Separator"];
                InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Twitter_TickerLayout_Separator_Val = value;

                #endregion
            }
        }

        private void initializeSkinSettings()
        {
            string value = string.Empty;

            #region Feeds
            InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Feeds_Enabled_Val = value.Trim();
            InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Feeds_General_RefreshFeedEvery_Val = value.Trim();
            InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Feeds_Layout_ShowFeedItemPublishTime_Val = value.Trim();
            InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Feeds_Layout_ItemPublishTimeAllFeeds_Val = value.Trim();
            InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Feeds_Layout_MaxItemsPerFeedForAllFeeds_Val = value.Trim();
            InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Feeds_Layout_MaxItemsForFeedTicker_Val = value.Trim();
            InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Feeds_TickerLayout_TickerMask_Val = value;
            InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Feeds_TickerLayout_TickerAllMask_Val = value;
            InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Feeds_TickerLayout_Separator_Val = value;
            InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Feeds_TickerLayout_SeparatorAll_Val = value;
            #endregion

            #region Weather
            InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Weather_Enabled_Val = value.Trim();
            InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Weather_General_RefreshWeatherEvery_Val = value.Trim();
            #endregion

            #region Twitter
            InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Twitter_Enabled_Val = value.Trim();
            InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Twitter_General_RefreshTwitterEvery_Val = value.Trim();
            InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Twitter_Layout_MaxItemsForTwitterTicker_Val = value.Trim();
            InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Twitter_TickerLayout_TickerMask_Val = value;
            InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Twitter_TickerLayout_Separator_Val = value;
            #endregion
        }

        #endregion
    }
}
