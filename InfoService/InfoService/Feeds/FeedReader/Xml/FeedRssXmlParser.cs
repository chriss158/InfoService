#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using FeedReader.Data;
using FeedReader.Enums;
using FeedReader.Xml.Interfaces;

#endregion

namespace FeedReader.Xml
{
    internal class FeedRssXmlParser : IFeedTypeXmlParser
    {
        private string _cacheFolder;
        private bool _useCache;

        Feed rFeed = new Feed();

        public Feed GetParsedFeed()
        {
            if (rFeed != null) return rFeed;
            else return new Feed();
        }

        public bool TryParse(XDocument xmlFeed, string cacheFolder, bool downloadImages, List<FeedItemFilter> itemFilters)
        {
            _cacheFolder = cacheFolder;
            _useCache = true;
            return TryParse(xmlFeed, downloadImages, itemFilters);
        }

        public bool TryParse(XDocument xmlFeed, bool downloadImages, List<FeedItemFilter> itemFilters)
        {
            rFeed = new Feed();
            XElement element = xmlFeed.Element("rss");
            IFeedTypeImageXmlParser imageParser = new FeedRssImageXmlParser();
            if (element != null)
            {
                LogEvents.InvokeOnDebug(new FeedArgs("This is an RSS feed. Now parsing the Feed data..."));

                rFeed.Title = Utils.Clean(FeedXmlParser.ParseString(xmlFeed.Descendants("channel").Elements("title"), "channel/title"));
                rFeed.Description = Utils.Clean(FeedXmlParser.ParseString(xmlFeed.Descendants("channel").Elements("description"), "channel/description"));
                LogEvents.InvokeOnDebug(new FeedArgs("Try to get feed[" + rFeed.Title + "] image..."));
                if (downloadImages)
                {
                    bool parseSuccess = false;
                    if(_useCache) parseSuccess = imageParser.TryParseFeedImageUrl(xmlFeed, _cacheFolder, rFeed.Title);
                    else parseSuccess = imageParser.TryParseFeedImageUrl(xmlFeed, rFeed.Title);
                    if (parseSuccess)
                    {
                        rFeed.Image = imageParser.GetParsedImage();
                        rFeed.ImagePath = imageParser.GetImagePath();
                    }
                }


                try
                {
                    LogEvents.InvokeOnDebug(new FeedArgs("Now parsing the Feed item data..."));
                    FeedItem feedItem;
                    int i = 1;
                    int count = xmlFeed.Descendants("item").Count();
                    foreach (XElement item in xmlFeed.Descendants("item"))
                    {
                        feedItem = new FeedItem();
                        feedItem.Author = Utils.Clean(FeedXmlParser.ParseString(item.Element("author"), "item[" + i + "]/author"));
                        feedItem.Title = Utils.Clean(FeedXmlParser.ParseString(item.Element("title"), "item/title"), true, false, true, true, itemFilters);
                        LogEvents.InvokeOnDebug(new FeedArgs("Try to get feed[" + rFeed.Title + "][" + feedItem.Title + "][" + i + "] item image..."));
                        if (downloadImages)
                        {
                            bool parseSuccess = false;
                            if (_useCache) parseSuccess = imageParser.TryParseFeedItemImageUrl(xmlFeed, _cacheFolder, rFeed.Title, feedItem.Title, i - 1);
                            else parseSuccess = imageParser.TryParseFeedItemImageUrl(xmlFeed, rFeed.Title, feedItem.Title, i - 1);
                            if (parseSuccess)
                            {
                                feedItem.Image = imageParser.GetParsedImage();
                                feedItem.ImagePath = imageParser.GetImagePath();
                            }
                        }
                        
                        feedItem.PublishDate = FeedXmlParser.ParseDateTime(item.Element("pubDate"), "item/pubDate");
                        feedItem.Url = FeedXmlParser.ParseString(item.Element("link"), "item/link").Trim();
                        string description = FeedXmlParser.ParseString(item.Element("description"), "item/description");
                        feedItem.Description = Utils.Clean(description, false, true, false, true, itemFilters);

                        rFeed.Items.Add(feedItem);
                        i++;
                    }
                    /*rFeed.Items = (from item in xmlFeed.Descendants("item")
                                   select new FeedItem
                                              {
                                                  Author = FeedXmlParser.ParseString(item.Element("author"), "item/author", item.),
                                                  ImagePath =
                                                      FeedXmlParser.ParseString(item.Element("enclosure"), "url", "item/enclosure/attribute[url]"),
                                                  Title = Utils.Clean(FeedXmlParser.ParseString(item.Element("title"), "item/title")),
                                                  PublishDate = FeedXmlParser.ParseDateTime(item.Element("pubDate"), "item/pubDate"),
                                                  UrlPath = FeedXmlParser.ParseString(item.Element("link"), "item/link"),
                                                  Description =
                                                      Utils.Clean(FeedXmlParser.ParseString(item.Element("description"), "item/description")),
                                              }).OrderByDescending(feedItem => feedItem.PublishDate).ToList();*/
                    rFeed.Items = rFeed.Items.OrderByDescending(fItem => fItem.PublishDate).ToList();

                    LogEvents.InvokeOnDebug(new FeedArgs("Parsing of RSS feed done. See above for possibly errors or warnings"));
                    return true;
                }
                catch (Exception ex)
                {
                    LogEvents.InvokeOnError(new FeedArgs("Error parsing RSS feed", ex.Message, ex.StackTrace));
                    //feed = null;
                    return false;
                }
            }
            LogEvents.InvokeOnDebug(new FeedArgs("This is not an RSS feed or the downloaded xml file is not a valid feed"));
            //feed = null;
            return false;
        }
    }
}