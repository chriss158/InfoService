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
        private string _password;
        private string _user;
        private string _pin;
        internal const string CONSUMER_KEY = "7wS9lAEQhFT6nqFX2EhKUw";
        internal const string CONSUMER_SECRET = "AoLW66pilNF2ck4KWHh4MBMZNZehZ1OuwP5FWZlrY";
        private bool _useCache;
        private AuthType _useAuthType;
        private AccessToken _accessToken;
        private static RequestToken _reqToken;

        public string CacheFolder { get; protected set; }
        public Dictionary<string, Timeline> Timelines { get; set; }

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
                _reqToken = c.ObtainUnauthorizedRequestToken("https://api.twitter.com/oauth/request_token", "http://twitter.com/");
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
                //_reqToken = c.ObtainUnauthorizedRequestToken("http://twitter.com/oauth/request_token", "http://twitter.com/");
                accessToken = c.RequestAccessToken(pin, _reqToken, "https://api.twitter.com/oauth/access_token", "http://twitter.com/");
                return true;
            }
            catch (Exception ex)
            {
                throw new TwitterAuthExpection("Couldn't get auth token", ex);
            }

        }

        public Twitter(string user, string password, string pin, AccessToken accessToken, AuthType authType)
        {
            InitTwitter(user, password, pin, authType, accessToken);
        }
        public Twitter(string user, string password, string pin, AccessToken accessToken, AuthType authType, string cacheFolder)
        {
            InitTwitter(user, password, pin, authType, accessToken);
            SetCache(cacheFolder);
        }

        private void InitTwitter(string user, string password, string pin, AuthType authType, AccessToken accessToken)
        {
            if (authType == AuthType.HTTPAuth)
            {
                if (string.IsNullOrEmpty(user))
                {
                    throw new TwitterUserPasswordExpection("No twitter user set. TwitterConnector disabled");
                }
                if (string.IsNullOrEmpty(password))
                {
                    throw new TwitterUserPasswordExpection("No password for twitter user set. TwitterConnector disabled");
                }
                LogEvents.InvokeOnDebug(new TwitterArgs("Create TwitterConnector instance without using cache"));
                _user = user;
                _password = password;
            }
            else if (authType == AuthType.OAuth)
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

            }
            Timelines = new Dictionary<string, Timeline>();
            //Timelines.Add(TimelineType.Public.ToString(), new Timeline(TimelineType.Public, authType, user, password, accessToken));
            Timelines.Add(TimelineType.Friends.ToString(), new Timeline(TimelineType.Friends, authType, user, password, accessToken));
            Timelines.Add(TimelineType.User.ToString(), new Timeline(TimelineType.User, authType, user, password, accessToken));
            Timelines.Add(TimelineType.Home.ToString(), new Timeline(TimelineType.Home, authType, user, password, accessToken));
            Timelines.Add(TimelineType.Mentions.ToString(), new Timeline(TimelineType.Mentions, authType, user, password, accessToken));
            Timelines.Add(TimelineType.RetweetedByMe.ToString(), new Timeline(TimelineType.RetweetedByMe, authType, user, password, accessToken));
            Timelines.Add(TimelineType.RetweetedToMe.ToString(), new Timeline(TimelineType.RetweetedToMe, authType, user, password, accessToken));
            Timelines.Add(TimelineType.RetweetsOfMe.ToString(), new Timeline(TimelineType.RetweetsOfMe, authType, user, password, accessToken));
            _useAuthType = authType;
        }

        private void SetCache(string cacheFolder)
        {
            if (!string.IsNullOrEmpty(cacheFolder))
            {
                if (Utils.IsValidPath(cacheFolder))
                {
                    LogEvents.InvokeOnDebug(new TwitterArgs(cacheFolder + " is a valid path. Now checking if the cache folder already exists"));
                    if (!Directory.Exists(cacheFolder))
                    {
                        LogEvents.InvokeOnDebug(new TwitterArgs(cacheFolder + " doesn't exist. Now create a new folder."));
                        try
                        {
                            Directory.CreateDirectory(cacheFolder);
                        }
                        catch(Exception ex)
                        {
                            _useCache = false;
                            throw new TwitterCacheFolderNotValid("Could not create cache older " + cacheFolder + ". " + ex.Message);
                        }
                    }
                    LogEvents.InvokeOnDebug(new TwitterArgs(cacheFolder + " exist. Caching is now enabled."));
                    if (!cacheFolder.EndsWith(@"\")) cacheFolder += @"\";
                    CacheFolder = cacheFolder;
                    _useCache = true;
                }
                else
                {
                    _useCache = false;
                    throw new TwitterCacheFolderNotValid(cacheFolder + " is not a valid path. Caching disabled");
                }
            }
            else
            {
                _useCache = false;
                throw new TwitterNoCacheFolderExpection("Cache folder path is empty. Caching disabled");
            }
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
            if (_useAuthType == AuthType.HTTPAuth)
            {
                if (StatusUpdate.PostStatus(_user, _password, message))
                {
                    LogEvents.InvokeOnInfo(new TwitterArgs("Posting status update \"" + message + "\" successful"));
                    return true;
                }
                LogEvents.InvokeOnInfo(new TwitterArgs("Posting status update \"" + message + "\" unsuccessful. See above for errors or warnings"));
            }
            else if(_useAuthType == AuthType.OAuth)
            {
                if (StatusUpdate.PostStatus(_accessToken, message))
                {
                    LogEvents.InvokeOnInfo(new TwitterArgs("Posting status update \"" + message + "\" successful"));
                    return true;
                }
                LogEvents.InvokeOnInfo(new TwitterArgs("Posting status update \"" + message + "\" unsuccessful. See above for errors or warnings")); 
            }
            return false;
        }
        public void UpdateAllTimelines()
        {
            foreach (KeyValuePair<string, Timeline> timeline in Timelines)
            {
                if (_useCache) timeline.Value.Update(CacheFolder);
                else timeline.Value.Update();
            }
        }
    }
}
