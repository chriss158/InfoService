#region Usings

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using FeedReader.Data;
using FeedReader.Enums;
using FeedReader.Xml.Interfaces;

#endregion

namespace FeedReader.Xml
{
    internal class FeedRdfXmlParser : IFeedTypeXmlParser
    {

        private static string _cacheFolder;
        private static bool _useCache;
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
            XNamespace rdf = "http://www.w3.org/1999/02/22-rdf-syntax-ns#";
            XElement element = xmlFeed.Element(rdf + "RDF");

            if (element != null)
            {
                xmlFeed = Utils.RemoveNamespace(xmlFeed);
                LogEvents.InvokeOnDebug(new FeedArgs("This is an RDF feed. Now parsing the Feed data..."));

                IFeedTypeImageXmlParser imageParser = new FeedRdfImageXmlParser();
                rFeed.Description = Utils.Clean(FeedXmlParser.ParseString(xmlFeed.Descendants("channel").Elements("description"), "channel/description"));
                rFeed.Title = Utils.Clean(FeedXmlParser.ParseString(xmlFeed.Descendants("channel").Elements("title"), "channel/title"));
                LogEvents.InvokeOnDebug(new FeedArgs("Try to get feed[" + rFeed.Title + "] image..."));
                if (downloadImages)
                {
                    bool parseSuccess = false;
                    if (_useCache) parseSuccess = imageParser.TryParseFeedImageUrl(xmlFeed, _cacheFolder, rFeed.Title);
                    else parseSuccess = imageParser.TryParseFeedImageUrl(xmlFeed, rFeed.Title);
                    if (parseSuccess)
                    {
                        if (!_useCache)
                        {
                            rFeed.Image = imageParser.GetParsedImage();
                            rFeed.ImagePath = string.Empty;
                        }
                        else rFeed.ImagePath = imageParser.GetImagePath();
                    }
                }

                try
                {
                    LogEvents.InvokeOnDebug(new FeedArgs("Now parsing the Feed item data..."));
                    FeedItem feedItem;
                    int i = 1;
                    foreach (XElement item in xmlFeed.Descendants("item"))
                    {
                        feedItem = new FeedItem();
                        feedItem.Author = Utils.Clean(FeedXmlParser.ParseString(item.Element("author"), "item[" + i + "]/author"));
                        feedItem.Title = Utils.Clean(FeedXmlParser.ParseString(item.Element("title"), "item[" + i + "]/title"), true, false, true, true, itemFilters);
                        feedItem.Url = FeedXmlParser.ParseString(item.Element("link"), "item[" + i + "]/link").Trim();
                        feedItem.PublishDate = FeedXmlParser.ParseDateTime(item.Element("date"), "item[" + i + "]/date");
                        string description = FeedXmlParser.ParseString(item.Element("description"), "item[" + i + "]/description");
                        LogEvents.InvokeOnDebug(new FeedArgs("Try to get feed[" + rFeed.Title + "][" + feedItem.Title + "][" + i + "] item image..."));
                        if (downloadImages)
                        {
                            bool parseSuccess = false;
                            if (_useCache) parseSuccess = imageParser.TryParseFeedItemImageUrl(xmlFeed, _cacheFolder, rFeed.Title, feedItem.Title, i - 1);
                            else parseSuccess = imageParser.TryParseFeedItemImageUrl(xmlFeed, rFeed.Title, feedItem.Title, i - 1);
                            if (parseSuccess)
                            {
                                if (!_useCache)
                                {
                                    feedItem.Image = imageParser.GetParsedImage();
                                    feedItem.ImagePath = string.Empty;
                                }
                                else feedItem.ImagePath = imageParser.GetImagePath();
                            }
                        }
  
                        feedItem.Description = Utils.Clean(description, false, true, false, true, itemFilters) == "" ?
                                               Utils.Clean(Utils.GetCdata(FeedXmlParser.ParseString(item.Element("encoded"), "item[" + i + "]/encoded")), false, true, false, true, itemFilters) :
                                               Utils.Clean(description, false, true, false, true, itemFilters);
                        rFeed.Items.Add(feedItem);
                        i++;
                    }

                    rFeed.Items = rFeed.Items.OrderByDescending(fItem => fItem.PublishDate).ToList();
                    LogEvents.InvokeOnDebug(new FeedArgs("Parsing of RDF feed done. See above for possibly errors or warnings"));
                    return true;
                }
                catch (Exception ex)
                {
                    LogEvents.InvokeOnError(new FeedArgs("Error parsing RDF feed", ex.Message, ex.StackTrace));
                    //feed = null;
                    return false;
                }
            }
            LogEvents.InvokeOnDebug(new FeedArgs("This is not a RDF feed or the downloaded xml file is not a valid feed"));
            //feed = null;
            return false;
        }
    }
}