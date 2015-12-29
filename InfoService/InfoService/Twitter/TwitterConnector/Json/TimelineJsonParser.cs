using System.Collections.Generic;
using System.Xml.Linq;
using TwitterConnector.Data;
using TwitterConnector.OAuth;
using System;

namespace TwitterConnector.Json
{
    internal static class TimelineJsonParser
    {
        private const string _retweetsUrl = "https://api.twitter.com/1.1/statuses/retweets/{0}.json";

        internal static bool TryParse(ref List<TwitterItem> twitterItems, TimelineType type, AccessToken accessToken, bool withRetweets, string cacheFolder = "")
        {
            LogEvents.InvokeOnDebug(!string.IsNullOrEmpty(cacheFolder)
                ? new TwitterArgs("Try downloading/parsing the " + type + " timeline using cache")
                : new TwitterArgs("Try downloading/parsing the " + type + " timeline"));
            dynamic jsonTwitter = null;
            jsonTwitter = Utils.DownloadTwitterJson(accessToken, type.GetUrl());
            if (jsonTwitter == null)
            {
                LogEvents.InvokeOnError(new TwitterArgs("There was an error downloading the " + type + " timeline. See above for errors"));
                return false;
            }

            LogEvents.InvokeOnDebug(new TwitterArgs("Download of the " + type + " timeline succesful. Now parsing the json"));
            twitterItems.Clear();

            int i = 0;
            foreach (dynamic tweet in jsonTwitter)
            {
                TwitterItem twit = new TwitterItem
                {
                    Id = TwitterJsonParser.ParseString(tweet.id_str, "tweet[" + i + "]/id_str"),
                    Text = TwitterJsonParser.ParseString(tweet.text, "tweet[" + i + "]/text"),
                    Source = Utils.Clean(TwitterJsonParser.ParseString(tweet.source, "tweet[" + i + "]/source")),
                    PublishDate = TwitterJsonParser.ParseDateTime(tweet.created_at, "tweet[" + i + "]/created_at")
                };
                if(tweet.entities.media != null && tweet.entities.media[0] != null)
                {
                    twit.MediaId = TwitterJsonParser.ParseString(tweet.entities.media[0].id_str, "tweet[" + i + "]/entities/media/id_str");
                    twit.MediaPath = TwitterJsonParser.ParseString(tweet.entities.media[0].media_url, "tweet[" + i + "]/entities/media/media_url");
                }
                if (tweet.user != null)
                {
                    twit.User = new TwitterUser
                    {
                        Description = TwitterJsonParser.ParseString(tweet.user.description, "tweet[" + i + "]/user/description"),
                        Location = TwitterJsonParser.ParseString(tweet.user.location, "tweet[" + i + "]/user/location"),
                        ScreenName = TwitterJsonParser.ParseString(tweet.user.screen_name, "tweet[" + i + "]/user/screen_name"),
                        Name = TwitterJsonParser.ParseString(tweet.user.name, "tweet[" + i + "]/user/name"),
                        FollowersCount = TwitterJsonParser.ParseInteger(tweet.user.followers_count, "tweet[" + i + "]/user/followers_count"),
                        FriendsCount = TwitterJsonParser.ParseInteger(tweet.user.friends_count, "tweet[" + i + "]/user/friends_count"),
                        PicturePath = TwitterJsonParser.ParseString(tweet.user.profile_image_url, "tweet[" + i + "]/user/profile_image_url").Replace("_normal.", ".")
                    };
                }
                else LogEvents.InvokeOnWarning(new TwitterArgs("No user for the tweet \"" + twit.Text + "\" in the " + type + " timeline found."));

                if (withRetweets)
                {
                    LogEvents.InvokeOnDebug(
                        new TwitterArgs("Try downloading/parsing the retweets timeline of tweet \"" + twit.Text +
                                        "\"..."));

                    dynamic jsonRetweetedStatus = Utils.DownloadTwitterJson(accessToken,
                        string.Format(_retweetsUrl, twit.Id));

                    if (jsonRetweetedStatus != null)
                    {
                        LogEvents.InvokeOnDebug(
                            new TwitterArgs("Download of the retweets timeline of tweet \"" + twit.Text +
                                            "\" succesful. Now parsing the json..."));
                        int j = 0;
                        foreach (dynamic retweet in jsonRetweetedStatus)
                        {
                            TwitterItem twiRetweet = new TwitterItem
                            {
                                Id =
                                    TwitterJsonParser.ParseString(tweet.id_str,
                                        "tweet[" + i + "]/retweeted_status[" + j + "]/id_str"),
                                Text =
                                    TwitterJsonParser.ParseString(retweet.text,
                                        "tweet[" + i + "]/retweeted_status[" + j + "]/text"),
                                Source =
                                    Utils.Clean(TwitterJsonParser.ParseString(retweet.source,
                                        "tweet[" + i + "]/retweeted_status[" + j + "]/source")),
                                PublishDate =
                                    TwitterJsonParser.ParseDateTime(retweet.created_at,
                                        "tweet[" + i + "]/retweeted_status[" + j + "]/created_at"),

                            };
                            if (retweet.user != null)
                            {
                                twiRetweet.User = new TwitterUser
                                {
                                    Description =
                                        TwitterJsonParser.ParseString(retweet.user.description,
                                            "tweet[" + i + "]/retweeted_status[" + j + "]/user/description"),
                                    Location =
                                        TwitterJsonParser.ParseString(retweet.user.location,
                                            "tweet[" + i + "]/retweeted_status[" + j + "]/user/location"),
                                    ScreenName =
                                        TwitterJsonParser.ParseString(retweet.user.screen_name,
                                            "tweet[" + i + "]/retweeted_status[" + j + "]/user/screen_name"),
                                    Name =
                                        TwitterJsonParser.ParseString(retweet.user.name,
                                            "tweet[" + i + "]/retweeted_status[" + j + "]/user/name"),
                                    FollowersCount =
                                        TwitterJsonParser.ParseInteger(retweet.user.followers_count,
                                            "tweet[" + i + "]/retweeted_status[" + j + "]/user/followers_count"),
                                    FriendsCount =
                                        TwitterJsonParser.ParseInteger(retweet.user.friends_count,
                                            "tweet[" + i + "]/retweeted_status[" + j + "]/user/friends_count"),
                                    PicturePath =
                                        TwitterJsonParser.ParseString(retweet.user.profile_image_url,
                                            "tweet[" + i + "]/retweeted_status[" + j + "]/user/profile_image_url")
                                            .Replace("_normal.", ".")
                                };
                            }
                            else
                                LogEvents.InvokeOnWarning(
                                    new TwitterArgs("No user for the retweet \"" + twiRetweet.Text + "\" in the " + type +
                                                    " timeline found."));

                            twit.Retweets.Add(twiRetweet);
                            j++;
                        }
                    }
                }

                twitterItems.Add(twit);
                i++;
            }

            LogEvents.InvokeOnInfo(new TwitterArgs("Try to get user pictures from " + type + " timeline either from cache or from download links"));
            twitterItems = Utils.GetImages(twitterItems, cacheFolder);
            LogEvents.InvokeOnDebug(new TwitterArgs("Parsing of " + type + " timeline done. See above for possibly errors or warnings"));
            return true;
        }
    }
}
