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
    internal class FeedAtomXmlParser : IFeedTypeXmlParser
    {
        private static string _cacheFolder;
        private static bool _useCache;

        Feed rFeed = new Feed();

        public Feed GetParsedFeed()
        {
            if (rFeed != null) return rFeed;
            else return new Feed();
        }

        public bool TryParse(XDocument xmlFeed, string cacheFolder, bool downloadImages, List<FeedItemFilter> itemFilter)
        {
            _cacheFolder = cacheFolder;
            _useCache = true;
            return TryParse(xmlFeed, downloadImages, itemFilter);
        }

        public bool TryParse(XDocument xmlFeed, bool downloadImages, List<FeedItemFilter> itemFilter)
        {
            rFeed = new Feed();
            
            XNamespace atom = "http://www.w3.org/2005/Atom";
            XElement element = xmlFeed.Element(atom + "feed");
            if (element != null)
            {
                IFeedTypeImageXmlParser imageParser = new FeedAtomImageXmlParser();
                LogEvents.InvokeOnDebug(new FeedArgs("This is an ATOM feed. Now parsing the Feed data..."));

                rFeed.Description = Utils.Clean(FeedXmlParser.ParseString(xmlFeed.Descendants(atom + "feed").Elements(atom + "subtitle"), "feed/subtitle"));
                rFeed.Title = Utils.Clean(FeedXmlParser.ParseString(xmlFeed.Descendants(atom + "feed").Elements(atom + "title"), "feed/title"));
                LogEvents.InvokeOnDebug(new FeedArgs("Try to get feed[" + rFeed.Title + "] image..."));
                if (downloadImages)
                {
                    bool parseSuccess = false;
                    if (_useCache) parseSuccess = imageParser.TryParseFeedImageUrl(xmlFeed, _cacheFolder, rFeed.Title);
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
                    foreach (XElement entry in xmlFeed.Descendants(atom + "entry"))
                    {
                        feedItem = new FeedItem();
                        feedItem.Title = Utils.Clean(FeedXmlParser.ParseString(entry.Element(atom + "title"), "entry[" + i + "]/title"), true, false, true, true, itemFilter);
                        
                        feedItem.Author = Utils.Clean(FeedXmlParser.ParseString(entry.Descendants(atom + "author").Elements(atom + "name"), "entry[" + i + "]/author/name"));
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
                        feedItem.Url = FeedXmlParser.ParseString(entry.Element(atom + "link"), "href", "entry[" + i + "]/link/attribute[href]").Trim();
                        feedItem.PublishDate = FeedXmlParser.ParseDateTime(entry.Element(atom + "updated"), "entry[" + i + "]/updated");
                        string description = FeedXmlParser.ParseString(entry.Element(atom + "summary"), "entry[" + i + "]/summary");
                        feedItem.Description = Utils.Clean(description, false, true, false, true, itemFilter);
                        rFeed.Items.Add(feedItem);
                        i++;
                    }
                    /*rFeed.Items = (from entry in xmlFeed.Descendants(atom + "entry")
                                   select new FeedItem
                                              {
                                                  ImagePath = GetImageUrl(entry, "entry/content/attribute[source]"),
                                                  Author =
                                                      FeedXmlParser.ParseString(
                                                      entry.Descendants(atom + "author").Elements(atom + "name"), "entry/author/name"),
                                                  Title =
                                                      Utils.Clean(
                                                      FeedXmlParser.ParseString(entry.Element(atom + "title"),"entry/title")),
                                                  UrlPath = FeedXmlParser.ParseString(entry.Element(atom + "link"), "href", "entry/link/attribute[href]"),
                                                  PublishDate =
                                                      FeedXmlParser.ParseDateTime(entry.Element(atom + "updated"), "entry/updated"),
                                                  Description =
                                                      Utils.Clean(
                                                      FeedXmlParser.ParseString(entry.Element(atom + "summary"), "entry/summary")),
                                              }).OrderByDescending(feedItem => feedItem.PublishDate).ToList();*/

                    rFeed.Items = rFeed.Items.OrderByDescending(fItem => fItem.PublishDate).ToList();
                    LogEvents.InvokeOnDebug(new FeedArgs("Parsing of ATOM feed done. See above for possibly errors or warnings"));
                    return true;
                }
                catch (Exception ex)
                {
                    LogEvents.InvokeOnError(new FeedArgs("Error parsing ATOM feed", ex.Message, ex.StackTrace));
                    //feed = null;
                    return false;
                    //throw new FeedDownloadErrorExpection("Error downloading from URL: " + UrlPath);
                }
            }
            LogEvents.InvokeOnDebug(new FeedArgs("This is not an ATOM feed or the downloaded xml file is not a valid feed"));
            //feed = null;
            return false;
            //throw new FeedDownloadErrorExpection("Error downloading from URL: " + UrlPath);
        }
    }
}