#region Usings

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using FeedReader.Data;
using FeedReader.Expections;
using FeedReader.Xml;
using Image = System.Drawing.Image;
using FeedReader.Xml.Interfaces;
using System.Net;
using System.Xml;
#endregion

namespace FeedReader
{
    public class Feed : IDisposable
    {
        public static bool CacheEnabled { get; private set; }
        public static bool CacheAutomatic { get; private set; }
        //public static string CacheFolder { get; private set; }
        private bool _isDisposed;
        private static List<FeedItemFilter> _feedItemFilters = new List<FeedItemFilter>();

        public delegate void OnNewItemsEventHandler(Feed feed, List<FeedItem> newItems);
        public event OnNewItemsEventHandler OnNewItems;

        public Feed(string url_path, string cacheFolder, List<FeedItemFilter> itemFilters, bool useCacheAutomatic = false)
        {
            SetCache(cacheFolder);
            LogEvents.InvokeOnDebug(new FeedArgs("Add new feed " + url_path + " using cache folder " + cacheFolder));
            UrlPath = url_path;
            _feedItemFilters = itemFilters;
            LastUpdateSuccessful = false;
            Items = new List<FeedItem>();
            CacheAutomatic = useCacheAutomatic;
            if (!useCacheAutomatic) EnableCache();
            Guid = Guid.NewGuid();
            //Update();
        }
        public Feed(string url_path, string cacheFolder, bool useCacheAutomatic = false)
        {
            SetCache(cacheFolder);
            LogEvents.InvokeOnDebug(new FeedArgs("Add new feed " + url_path + " using cache folder " + cacheFolder));
            UrlPath = url_path;
            CacheEnabled = false;
            LastUpdateSuccessful = false;
            Items = new List<FeedItem>();
            CacheAutomatic = useCacheAutomatic;
            if (!useCacheAutomatic) EnableCache();
            Guid = Guid.NewGuid();
            //Update();
        }
        public Feed(string url_path)
        {
            LogEvents.InvokeOnDebug(new FeedArgs("Add new feed without using cache"));
            UrlPath = url_path;
            CacheEnabled = false;
            LastUpdateSuccessful = false;
            Items = new List<FeedItem>();
            Guid = Guid.NewGuid();
            //Update();
        }

        public Feed(string url_path, List<FeedItemFilter> itemFilters)
        {
            LogEvents.InvokeOnDebug(new FeedArgs("Add new feed without using cache"));
            UrlPath = url_path;
            _feedItemFilters = itemFilters;
            CacheEnabled = false;
            LastUpdateSuccessful = false;
            Items = new List<FeedItem>();
            Guid = Guid.NewGuid();
            //Update();
        }
        public static void DisableCache()
        {
            LogEvents.InvokeOnDebug(new FeedArgs("Cache disabled."));
            CacheEnabled = false;

        }
        public static void SetCache(string cacheFolder, bool autoEnableCache)
        {
            LogEvents.InvokeOnDebug(new FeedArgs("Set cache folder to \"" + cacheFolder + "\""));
            DisableCache();
            CacheFolder = cacheFolder;
            if (!CacheFolder.EndsWith(@"\")) CacheFolder += @"\";
            if (autoEnableCache) EnableCache();
        }
        public static void SetCache(string cacheFolder)
        {
            SetCache(cacheFolder, false);
        }
        public static bool CheckCache()
        {
            LogEvents.InvokeOnDebug(new FeedArgs("Checking cache folder \"" + CacheFolder + "\"..."));
            if (!string.IsNullOrEmpty(CacheFolder))
            {
                if (Utils.IsValidPath(CacheFolder))
                {
                    LogEvents.InvokeOnDebug(new FeedArgs(CacheFolder + " is a valid path. Now checking if the cache folder already exists..."));
                    if (Utils.IsCacheFolderAvailable(CacheFolder))
                    {

                        if (!Utils.DoesCacheFolderExists(CacheFolder))
                        {
                            LogEvents.InvokeOnDebug(new FeedArgs(CacheFolder + " doesn't exist. Now create a new folder."));
                            try
                            {
                                Directory.CreateDirectory(CacheFolder);
                            }
                            catch (Exception ex)
                            {
                                LogEvents.InvokeOnWarning(new FeedArgs("Could not create cache older " + CacheFolder, ex.Message));
                                DisableCache();
                                return false;
                                //throw new FeedCacheFolderNotValid("Could not create cache older " + cacheFolder + ". Feed disabled." + ex.Message);
                            }
                        }
                    }
                    else
                    {
                        CacheEnabled = false;
                        LogEvents.InvokeOnWarning(new FeedArgs("Cache folder " + CacheFolder + " is not available."));
                        DisableCache();
                        return false;
                        //throw new FeedCacheFolderNotValid(cacheFolder + " is not available in the network. Feed disabled.");
                    }
                    LogEvents.InvokeOnDebug(new FeedArgs(CacheFolder + " exist. Caching for feeds is now enabled."));

                    //CacheFolder = cacheFolder;
                    CacheEnabled = true;
                    return true;

                }
                else
                {
                    LogEvents.InvokeOnWarning(new FeedArgs("Cache folder " + CacheFolder + " is not valid path."));
                    DisableCache();
                    return false;
                    //throw new FeedCacheFolderNotValid(cacheFolder + " is not a valid path. Caching disabled");
                }
            }
            else
            {
                LogEvents.InvokeOnWarning(new FeedArgs("Cache folder path is empty."));
                DisableCache();
                return false;
                //throw new FeedNoCacheFolderExpection("Cache folder path is empty. Caching disabled");
            }
        }
        public static bool EnableCache()
        {
            LogEvents.InvokeOnDebug(new FeedArgs("Try enabling cache..."));
            if (CacheEnabled)
            {
                LogEvents.InvokeOnDebug(new FeedArgs("Cache already enabled with cache folder \"" + CacheFolder + "\""));
                return true;
            }
            return CheckCache();
        }


        public Feed()
        {
            Items = new List<FeedItem>();
            Guid = Guid.NewGuid();
        }

        public static bool DeleteCache()
        {
            LogEvents.InvokeOnDebug(new FeedArgs("Try to delete cache"));
            if (Directory.Exists(CacheFolder))
            {
                try
                {
                    Directory.Delete(CacheFolder, true);
                    LogEvents.InvokeOnInfo(new FeedArgs("Deleted cache successful"));
                    return true;
                }
                catch (Exception ex)
                {
                    LogEvents.InvokeOnError(new FeedArgs("Error deleting cache", ex.Message, ex.StackTrace));
                }

            }
            LogEvents.InvokeOnInfo(new FeedArgs("Error deleting cache. Directory " + CacheFolder + " doesn't exist"));
            return false;
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public static string CacheFolder { get; protected set; }
        public List<FeedItem> Items { get; set; }
        public FeedType Type { get; set; }
        public Image Image { get; set; }
        public string ImagePath { get; set; }
        public string UrlPath { get; set; }
        public DateTime LastUpdate { get; set; }
        public bool LastUpdateSuccessful { get; set; }
        public Guid Guid { get; private set; }

        public virtual bool Update(bool downloadImages)
        {
            bool oldLastUpdateSuccessful = LastUpdateSuccessful;
            LastUpdateSuccessful = false;

            LogEvents.InvokeOnDebug(new FeedArgs("Try downloading/loading feed from " + UrlPath));
            XDocument xmlFeed;
            try
            {
                WebClient wc = new WebClient();
                wc.Proxy.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;

                MemoryStream ms = new MemoryStream(wc.DownloadData(UrlPath));
                XmlTextReader rdr = new XmlTextReader(ms);
                xmlFeed = XDocument.Load(rdr);
                LogEvents.InvokeOnDebug(new FeedArgs("Download/Loading of feed " + UrlPath + " successful"));
            }
            catch (Exception ex)
            {
                LogEvents.InvokeOnError(new FeedArgs("Error downloading/loading feed from url/path " + UrlPath, ex.Message, ex.StackTrace));
                return false;
            }

            List<FeedItem> oldItems = new List<FeedItem>();
            bool parseSuccess = false;
            List<IFeedTypeXmlParser> feedParsers = new List<IFeedTypeXmlParser>();
            feedParsers.Add(new FeedRdfXmlParser());
            feedParsers.Add(new FeedRssXmlParser());
            feedParsers.Add(new FeedAtomXmlParser());

            bool cacheAvailable = false;
            if (CacheAutomatic)
            {
                CheckCache();
            }

            if (CacheEnabled)
            {
                if (!CacheAutomatic)
                {
                    cacheAvailable = Utils.IsCacheFolderAvailable(CacheFolder);
                }
                else cacheAvailable = true;
            }


            foreach (IFeedTypeXmlParser feedParser in feedParsers)
            {
                if (!CacheEnabled)
                {
                    LogEvents.InvokeOnInfo(new FeedArgs("Parsing feed from url/path " + UrlPath + " without using cache"));
                    parseSuccess = feedParser.TryParse(xmlFeed, downloadImages, _feedItemFilters);
                }
                else
                {
                    if (cacheAvailable)
                    {
                        LogEvents.InvokeOnInfo(new FeedArgs("Parsing feed from url/path " + UrlPath + " into cache folder " + CacheFolder));
                        parseSuccess = feedParser.TryParse(xmlFeed, CacheFolder, downloadImages, _feedItemFilters);
                    }
                    else
                    {
                        if (CacheAutomatic)
                        {
                            LogEvents.InvokeOnInfo(new FeedArgs("Parsing feed from url/path " + UrlPath + " without using cache"));
                            parseSuccess = feedParser.TryParse(xmlFeed, CacheFolder, downloadImages, _feedItemFilters);
                        }
                        else
                        {
                            LogEvents.InvokeOnError(new FeedArgs("Error parsing feed into cache folder " + CacheFolder + ". Cache folder is not available..."));
                        }
                        
                    }
                }
                if (parseSuccess)
                {
                    Description = feedParser.GetParsedFeed().Description;
                    LastUpdate = DateTime.Now;
                    LastUpdateSuccessful = true;
                    Image = feedParser.GetParsedFeed().Image;
                    oldItems = Items.CloneList<FeedItem>();
                    Items = feedParser.GetParsedFeed().Items;
                    Title = feedParser.GetParsedFeed().Title;
                    ImagePath = feedParser.GetParsedFeed().ImagePath;
                    if (feedParser is FeedRdfXmlParser) Type = FeedType.RDF;
                    if (feedParser is FeedRssXmlParser) Type = FeedType.RSS;
                    if (feedParser is FeedAtomXmlParser) Type = FeedType.ATOM;
                    LogEvents.InvokeOnInfo(new FeedArgs("Parsing " + Type.ToString() + " feed from url/path " + UrlPath + " successfull"));
                    break;
                }
            }

            if (!parseSuccess)
            {
                LastUpdateSuccessful = false;
                LogEvents.InvokeOnInfo(new FeedArgs("Parsing feed from url/path " + UrlPath + " unsuccessful. See above for errors or warnings"));
                return false;
            }
            else
            {
                if (oldLastUpdateSuccessful) CheckForNewItems(oldItems, Items);
                return true;
            }
        }

        private void CheckForNewItems(List<FeedItem> oldItems, List<FeedItem> items)
        {
            if (oldItems.Count >= 1 && items.Count >= 1 && OnNewItems != null)
            {
                List<FeedItem> newItems = items.Except(oldItems, new FeedItemComparer()).ToList();
                if (newItems.Count > 0)
                {
                    OnNewItems((Feed)this.Clone(), newItems.CloneList<FeedItem>());
                }
            }
        }

        public virtual object Clone()
        {
            Feed feed = new Feed();
            feed.Title = this.Title;
            feed.Description = this.Description;
            feed.Items = this.Items.CloneList<FeedItem>();
            feed.Type = this.Type;
            if(this.Image != null) feed.Image = this.Image.Clone() as Image;
            feed.ImagePath = this.ImagePath;
            feed.UrlPath = this.UrlPath;
            feed.LastUpdate = this.LastUpdate;
            feed.LastUpdateSuccessful = this.LastUpdateSuccessful;
            feed.Guid = this.Guid;
            return feed;
        }

        #region IDisposable Members

        /// <summary>
        /// Performs the disposal.
        /// </summary>
        private void Dispose(bool disposing)
        {
            if (disposing && !_isDisposed)
            {
                this.Title = null;
                this.Description = null;
                this.Items.Clear();
                this.Items = null;
                if(this.Image != null) this.Image.Dispose();
                this.Image = null;
                this.ImagePath = null;
                this.UrlPath = null;
                this.Guid = Guid.Empty;
            }

            _isDisposed = true;
        }

        /// <summary>
        /// Releases the object to the garbage collector
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion


    }
}
