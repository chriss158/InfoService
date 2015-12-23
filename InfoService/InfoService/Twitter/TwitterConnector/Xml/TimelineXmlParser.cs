#region Usings

using System.Collections.Generic;
using System.Xml.Linq;
using TwitterConnector.Data;
using TwitterConnector.OAuth;

#endregion

namespace TwitterConnector.Xml
{
    internal static class TimelineXmlParser
    {
        private static string _cacheFolder;
        private static bool _useCache;

        internal static bool TryParse(ref List<TwitterItem> twitterItems, TimelineType type, string user, string password, AccessToken accessToken, AuthType authType, string cacheFolder)
        {
            LogEvents.InvokeOnDebug(new TwitterArgs("Try downloading/parsing the " + type + " timeline using cache"));
            _cacheFolder = cacheFolder;
            _useCache = true;
            return TryParse(ref twitterItems, type, user, password, accessToken, authType);
        }
        internal static bool TryParse(ref List<TwitterItem> twitterItems, TimelineType type, string user, string password, AccessToken accessToken, AuthType authType)
        {
            if (!_useCache) LogEvents.InvokeOnDebug(new TwitterArgs("Try downloading/parsing the " + type + " timeline"));
            XDocument xmlTwitter = new XDocument();
            if (authType == AuthType.HTTPAuth) xmlTwitter = Utils.DownloadTwitterXml(user, password, type.GetUrl());
            else if (authType == AuthType.OAuth) xmlTwitter = Utils.DownloadTwitterXml(accessToken, type.GetUrl());
            else if (authType == AuthType.None) xmlTwitter = Utils.DownloadTwitterXml(type.GetUrl());
            if (xmlTwitter == null)
            {
                LogEvents.InvokeOnError(new TwitterArgs("There was an error downloading the " + type + " timeline. See above for errors"));
                return false;
            }


            XElement element = xmlTwitter.Element("statuses");
            if (element != null)
            {
                LogEvents.InvokeOnDebug(new TwitterArgs("Download of the " + type + " timeline succesful. Now parsing the xml"));
                int i = 0;
                twitterItems.Clear();
                foreach (XElement node in element.Elements("status"))
                {
                    TwitterItem twit = new TwitterItem
                    {
                        Text = TwitterXmlParser.ParseString(node.Element("text"), "status[" + i + "]/text"),
                        Source = Utils.Clean(TwitterXmlParser.ParseString(node.Element("source"), "status[" + i + "]/source")),
                        PublishDate = TwitterXmlParser.ParseDateTime(node.Element("created_at"), "status[" + i + "]/publishdate")
                    };
                    if (node.Element("user") != null)
                    {
                        twit.User = new TwitterUser
                        {
                            Description = TwitterXmlParser.ParseString(node.Element("user").Element("description"), "status[" + i + "]/user/description"),
                            Location = TwitterXmlParser.ParseString(node.Element("user").Element("location"), "status[" + i + "]/user/location"),
                            ScreenName = TwitterXmlParser.ParseString(node.Element("user").Element("screen_name"), "status[" + i + "]/user/screenname"),
                            Name = TwitterXmlParser.ParseString(node.Element("user").Element("name"), "status[" + i + "]/user/name"),
                            FollowersCount = TwitterXmlParser.ParseInteger(node.Element("user").Element("followers_count"), "status[" + i + "]/user/followerscount"),
                            FriendsCount = TwitterXmlParser.ParseInteger(node.Element("user").Element("friends_count"), "status[" + i + "]/user/friendscount"),
                            PicturePath = TwitterXmlParser.ParseString(node.Element("user").Element("profile_image_url"), "status[" + i + "]/user/picturepath").Replace("_normal.", ".")
                        };
                    }
                    else LogEvents.InvokeOnWarning(new TwitterArgs("No user for the tweet \"" + twit.Text + "\" in the " + type + " timeline found."));
                    if (node.Element("retweeted_status") != null)
                    {
                        twit.Retweet = new TwitterItem
                        {
                            Text = TwitterXmlParser.ParseString(node.Element("text"), "status[" + i + "]/retweeted_status/text"),
                            Source = Utils.Clean(TwitterXmlParser.ParseString(node.Element("source"), "status[" + i + "]/retweeted_status/source")),
                            PublishDate = TwitterXmlParser.ParseDateTime(node.Element("created_at"), "status[" + i + "]/retweeted_status/publishdate")
                        };
                        if (node.Element("retweeted_status").Element("user") != null)
                        {
                            twit.Retweet.User = new TwitterUser
                            {
                                Description = TwitterXmlParser.ParseString(node.Element("user").Element("description"), "status[" + i + "]/retweeted_status/user/description"),
                                Location = TwitterXmlParser.ParseString(node.Element("user").Element("location"), "status[" + i + "]/retweeted_status/user/location"),
                                ScreenName = TwitterXmlParser.ParseString(node.Element("user").Element("screen_name"), "status[" + i + "]/retweeted_status/user/screenname"),
                                Name = TwitterXmlParser.ParseString(node.Element("user").Element("name"), "status[" + i + "]/retweeted_status/user/name"),
                                FollowersCount = TwitterXmlParser.ParseInteger(node.Element("user").Element("followers_count"), "status[" + i + "]/retweeted_status/user/followerscount"),
                                FriendsCount = TwitterXmlParser.ParseInteger(node.Element("user").Element("friends_count"), "status[" + i + "]/retweeted_status/user/friendscount"),
                                PicturePath = TwitterXmlParser.ParseString(node.Element("user").Element("profile_image_url"), "status[" + i + "]/retweeted_status/user/picturepath").Replace("_normal.", ".")

                            };
                        }
                        else LogEvents.InvokeOnWarning(new TwitterArgs("No user for the retweet \"" + twit.Retweet.Text + "\" in the " + type + " timeline found."));
                    }
                    twitterItems.Add(twit);
                    i++;
                }
                LogEvents.InvokeOnInfo(new TwitterArgs("Try to get user pictures from " + type + " timeline either from cache or from download links"));
                twitterItems = _useCache ? Utils.GetUserPictures(twitterItems, _cacheFolder) : Utils.GetUserPictures(twitterItems);
                LogEvents.InvokeOnDebug(new TwitterArgs("Parsing of " + type + " timeline done. See above for possibly errors or warnings"));
            }
            else
            {
                LogEvents.InvokeOnError(new TwitterArgs("Downloading the " + type + " timeline from twitter was ok, but there seems to be an error in the file itself. The element \"statuses\" doesn't exist. It could be that there are no \"" + type + "\" tweets."));
                return false;
            }
            return true;
        }
    }
}
