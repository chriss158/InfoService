#region Usings

using System;
using System.Collections.Generic;
using System.IO;
using TwitterConnector.Expections;
using TwitterConnector.OAuth;

#endregion

namespace TwitterConnector
{
    public class Twitter
    {
        private string _pin;
        internal const string CONSUMER_KEY = "v7y1GwRj47OYZiHuFOz7Pwh2u";
        internal const string CONSUMER_SECRET = "m7RL45DMq1dqDpoGFCYohzvdktQue5Oyx0qzhizGRo3ZODv24s";
        private bool _useCache;
        private AccessToken _accessToken;
        private static RequestToken _reqToken;

        public static string CacheFolder { get; protected set; }
        public Dictionary<string, Timeline> Timelines { get; set; }
        public static bool CacheEnabled { get; private set; }
        public static bool CacheAutomatic { get; private set; }

        public static string GetAuthUrl()
        {
            if (_reqToken != null)
            {
                try
                {
                    return Consumer.BuildUserAuthorizationURL("https://api.twitter.com/oauth/authorize", _reqToken);
                }
                catch (Exception ex)
                {
                    throw new TwitterAuthExpection("Couldn't get auth url", ex);
                }
                
            }
            throw new TwitterAuthExpection("Couldn't get auth url. RequestToken is empty");
        }
        public static bool GetRequestToken(out RequestToken requestToken)
        {
            try
            {
                Consumer c = new Consumer(CONSUMER_KEY, CONSUMER_SECRET);
                _reqToken = c.ObtainUnauthorizedRequestToken("https://api.twitter.com/oauth/request_token", "https://twitter.com/");
                requestToken = _reqToken;
                return true;
            }
            catch (Exception ex)
            {
                throw new TwitterAuthExpection("Couldn't get request token", ex);
            }

        }
        public static bool GetAuthToken(string pin, out AccessToken accessToken)
        {
            try
            {
                Consumer c = new Consumer(CONSUMER_KEY, CONSUMER_SECRET);
                //_reqToken = c.ObtainUnauthorizedRequestToken("http://twitter.com/oauth/request_token", "https://twitter.com/");
                accessToken = c.RequestAccessToken(pin, _reqToken, "https://api.twitter.com/oauth/access_token", "https://twitter.com/");
                return true;
            }
            catch (Exception ex)
            {
                throw new TwitterAuthExpection("Couldn't get auth token", ex);
            }

        }

        public Twitter(string user, string password, string pin, AccessToken accessToken)
        {
            InitTwitter(user, password, pin, accessToken);
        }
        public Twitter(string user, string password, string pin, AccessToken accessToken, string cacheFolder, bool useCacheAutomatic = false)
        {
            InitTwitter(user, password, pin, accessToken);
            SetCache(cacheFolder);
            CacheAutomatic = useCacheAutomatic;
            if (!useCacheAutomatic) EnableCache();
        }

        private void InitTwitter(string user, string password, string pin, AccessToken accessToken)
        {
                if (string.IsNullOrEmpty(pin))
                {
                    throw new TwitterNoPinExpection("No pin for OAuth is set. TwitterConnector disabled");
                }
                if (accessToken == null)
                {
                    throw new TwitterNoRequestTokenExpection("No token for OAuth is set. TwitterConnector disabled");
                }
                if (string.IsNullOrEmpty(accessToken.TokenValue))
                {
                    throw new TwitterNoRequestTokenExpection("No token value for OAuth is set. TwitterConnector disabled");
                }
                if (string.IsNullOrEmpty(accessToken.TokenSecret))
                {
                    throw new TwitterNoRequestTokenExpection("No token secret for OAuth is set. TwitterConnector disabled");
                }
                try
                {
                    _accessToken = accessToken;
                    _pin = pin;
                    LogEvents.InvokeOnDebug(new TwitterArgs("Create TwitterConnector(using OAuth) instance using cache"));
                }
                catch (Exception ex)
                {
                    throw new TwitterAuthExpection("Authorization unsuccessful", ex);
                }

            Timelines = new Dictionary<string, Timeline>();
            //Timelines.Add(TimelineType.Public.ToString(), new Timeline(TimelineType.Public, authType, user, password, accessToken));
            //Timelines.Add(TimelineType.Friends.ToString(), new Timeline(TimelineType.Friends, authType, user, password, accessToken));
            Timelines.Add(TimelineType.User.ToString(), new Timeline(TimelineType.User, user, password, accessToken));
            Timelines.Add(TimelineType.Home.ToString(), new Timeline(TimelineType.Home, user, password, accessToken));
            Timelines.Add(TimelineType.Mentions.ToString(), new Timeline(TimelineType.Mentions, user, password, accessToken));
            //Timelines.Add(TimelineType.RetweetedByMe.ToString(), new Timeline(TimelineType.RetweetedByMe, authType, user, password, accessToken));
            //Timelines.Add(TimelineType.RetweetedToMe.ToString(), new Timeline(TimelineType.RetweetedToMe, authType, user, password, accessToken));
            Timelines.Add(TimelineType.RetweetsOfMe.ToString(), new Timeline(TimelineType.RetweetsOfMe, user, password, accessToken));
        }
        public static void DisableCache()
        {
            LogEvents.InvokeOnDebug(new TwitterArgs("Cache disabled."));
            CacheEnabled = false;

        }
        public static bool EnableCache()
        {
            LogEvents.InvokeOnDebug(new TwitterArgs("Try enabling cache..."));
            if (CacheEnabled)
            {
                LogEvents.InvokeOnDebug(new TwitterArgs("Cache already enabled with cache folder \"" + CacheFolder + "\""));
                return true;
            }
            return CheckCache();
        }
        public static bool CheckCache()
        {
            LogEvents.InvokeOnDebug(new TwitterArgs("Checking cache folder \"" + CacheFolder + "\"..."));
            if (!string.IsNullOrEmpty(CacheFolder))
            {
                if (Utils.IsValidPath(CacheFolder))
                {
                    LogEvents.InvokeOnDebug(new TwitterArgs(CacheFolder + " is a valid path. Now checking if the cache folder already exists..."));
                    if (Utils.IsCacheFolderAvailable(CacheFolder))
                    {

                        if (!Utils.DoesCacheFolderExists(CacheFolder))
                        {
                            LogEvents.InvokeOnDebug(new TwitterArgs(CacheFolder + " doesn't exist. Now create a new folder."));
                            try
                            {
                                Directory.CreateDirectory(CacheFolder);
                            }
                            catch (Exception ex)
                            {
                                LogEvents.InvokeOnWarning(new TwitterArgs("Could not create cache older " + CacheFolder, ex.Message));
                                DisableCache();
                                return false;
                                //throw new FeedCacheFolderNotValid("Could not create cache older " + cacheFolder + ". Feed disabled." + ex.Message);
                            }
                        }
                    }
                    else
                    {
                        CacheEnabled = false;
                        LogEvents.InvokeOnWarning(new TwitterArgs("Cache folder " + CacheFolder + " is not available."));
                        DisableCache();
                        return false;
                        //throw new FeedCacheFolderNotValid(cacheFolder + " is not available in the network. Feed disabled.");
                    }
                    LogEvents.InvokeOnDebug(new TwitterArgs(CacheFolder + " exist. Caching for feeds is now enabled."));

                    //CacheFolder = cacheFolder;
                    CacheEnabled = true;
                    return true;

                }
                else
                {
                    LogEvents.InvokeOnWarning(new TwitterArgs("Cache folder " + CacheFolder + " is not valid path."));
                    DisableCache();
                    return false;
                    //throw new FeedCacheFolderNotValid(cacheFolder + " is not a valid path. Caching disabled");
                }
            }
            else
            {
                LogEvents.InvokeOnWarning(new TwitterArgs("Cache folder path is empty."));
                DisableCache();
                return false;
                //throw new FeedNoCacheFolderExpection("Cache folder path is empty. Caching disabled");
            }
        }
        public static void SetCache(string cacheFolder, bool autoEnableCache)
        {
            LogEvents.InvokeOnDebug(new TwitterArgs("Set cache folder to \"" + cacheFolder + "\""));
            DisableCache();
            CacheFolder = cacheFolder;
            if (!CacheFolder.EndsWith(@"\")) CacheFolder += @"\";
            if (autoEnableCache) EnableCache();
        }
        public static void SetCache(string cacheFolder)
        {
            SetCache(cacheFolder, false);
        }
       
        public bool DeleteCache()
        {
            LogEvents.InvokeOnDebug(new TwitterArgs("Try to delete cache from " + CacheFolder));
            if (Directory.Exists(CacheFolder))
            {
                try
                {
                    Directory.Delete(CacheFolder, true);
                    LogEvents.InvokeOnInfo(new TwitterArgs("Deleted cache successful"));
                    return true;
                }
                catch (Exception ex)
                {
                    LogEvents.InvokeOnError(new TwitterArgs("Error deleting cache", ex.Message, ex.StackTrace));
                }

            }
            LogEvents.InvokeOnInfo(new TwitterArgs("Error deleting cache. Directory " + CacheFolder + " doesn't exist"));
            return false;
        }

        public bool PostStatus(string message)
        {
            if (StatusUpdate.PostStatus(_accessToken, message))
            {
                LogEvents.InvokeOnInfo(new TwitterArgs("Posting status update \"" + message + "\" successful"));
                return true;
            }
            LogEvents.InvokeOnInfo(new TwitterArgs("Posting status update \"" + message + "\" unsuccessful. See above for errors or warnings"));
            return false;
        }
        public bool UpdateTimelines(List<TimelineType> timeLines)
        {
            int successCount = timeLines.Count;
            int count = 0;
            foreach (KeyValuePair<string, Timeline> timeline in Timelines)
            {
                if (timeLines.Contains(timeline.Value.Type))
                {
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

                    if (!CacheEnabled)
                    {
                        if(timeline.Value.Update())
                        {
                            count++;
                        }
                    }
                    else
                    {
                        if (cacheAvailable)
                        {
                            if(timeline.Value.Update(CacheFolder))
                            {
                                count++;
                            }
                        }
                        else
                        {
                            LogEvents.InvokeOnError(new TwitterArgs("Error parsing timeline into cache folder " + CacheFolder + ". Cache folder is not available...", ""));
                        }
                    }
                }
            }
            if (successCount == count) return true;
            else return false;
        }
        public bool UpdateAllTimelines()
        {
            return UpdateTimelines(new List<TimelineType>() { TimelineType.Home, TimelineType.Mentions, TimelineType.RetweetsOfMe, TimelineType.User });
        }
    }
}
