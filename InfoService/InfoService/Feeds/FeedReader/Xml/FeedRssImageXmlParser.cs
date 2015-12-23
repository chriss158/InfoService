using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Drawing;
using FeedReader;
using FeedReader.Xml;
using FeedReader.Data;
using FeedReader.Enums;
using FeedReader.Xml.Interfaces;

namespace FeedReader.Xml
{
    internal class FeedRssImageXmlParser : IFeedTypeImageXmlParser
    {
        private Image _feedImage;
        private string _feedImagePath = string.Empty;
        public bool TryParseFeedImageUrl(XDocument xmlFeed, string cacheFolder, string feedTitle)
        {
            bool returnValue = false;
            _feedImage = null;
            _feedImagePath = string.Empty;

            //1. Try
            string url = FeedXmlParser.ParseString(xmlFeed.Descendants("channel").Descendants("image").Elements("url"), "channel/image/url");
            returnValue = DownloadAndCheckFeedImageValid(cacheFolder, url, feedTitle);
            if (returnValue) return true;

            //2. Try
            url = FeedXmlParser.ParseString(xmlFeed.Descendants("channel").Descendants("image").Elements("link"), "channel/image/link");
            url += FeedXmlParser.ParseString(xmlFeed.Descendants("channel").Descendants("image").Elements("url"), "channel/image/url");
            returnValue = DownloadAndCheckFeedImageValid(cacheFolder, url, feedTitle);
            if (returnValue) return true;


            LogEvents.InvokeOnDebug(new FeedArgs("No feed image could be parsed"));
            return false;
        }

        public bool TryParseFeedItemImageUrl(XDocument xmlFeed, string cacheFolder, string feedTitle, string feedItemTitle, int itemNumber)
        {
            bool returnValue = false;
            XElement item = xmlFeed.Descendants("item").ElementAt(itemNumber);
            _feedImage = null;
            _feedImagePath = string.Empty;
            if (item != null)
            {

                //1. Try
                string url = FeedXmlParser.ParseString(item.Descendants("image").Elements("url"), "item/image/url");
                returnValue = DownloadAndCheckFeedItemImageValid(cacheFolder, url, feedTitle, feedItemTitle);
                if (returnValue) return true;

                //2. Try
                url = FeedXmlParser.ParseString(item.Descendants("image").Elements("link"), "item/image/link");
                url += FeedXmlParser.ParseString(item.Descendants("image").Elements("url"), "item/image/url");
                returnValue = DownloadAndCheckFeedItemImageValid(cacheFolder, url, feedTitle, feedItemTitle);
                if (returnValue) return true;

                //3. Try
                string enclosureType = FeedXmlParser.ParseString(item.Element("enclosure"), "type", "item/enclosure/attribute[type]");
                if (Utils.IsEnclosureTypeImage(enclosureType))
                {
                    //LogEvents.InvokeOnDebug(new FeedArgs("Enclosure of feed[" + rFeed.Title + "][" + feedItem.Title + "][" + i + "] item is an image. Parse image url..."));
                    url = FeedXmlParser.ParseString(item.Element("enclosure"), "url", "item/enclosure/attribute[url]");
                    returnValue = DownloadAndCheckFeedItemImageValid(cacheFolder, url, feedTitle, feedItemTitle);
                    if (returnValue) return true;
                }




                //Last Try...
                LogEvents.InvokeOnDebug(new FeedArgs("Try to get feed item image out of description field..."));
                string description = FeedXmlParser.ParseString(item.Element("description"), "item/description");
                url = Utils.GetImageOutOfDescription(Utils.ReplaceHTMLSpecialChars(description));
                returnValue = DownloadAndCheckFeedItemImageValid(cacheFolder, url, feedTitle, feedItemTitle);
                if (returnValue) return true;

            }
            LogEvents.InvokeOnDebug(new FeedArgs("No feed item image could be parsed"));
            return returnValue;
        }

        public Image GetParsedImage()
        {
            return _feedImage;
        }

        public string GetImagePath()
        {
            return _feedImagePath;
        }

        public bool DownloadAndCheckFeedImageValid(string cacheFolder, string url, string feedTitle)
        {
            if (Utils.IsValidUrl(ref url))
            {
                LogEvents.InvokeOnDebug(new FeedArgs("Parsed feed image \"" + url + "\" successfull. Downloading image/Load image from cache..."));
                if (string.IsNullOrEmpty(cacheFolder))
                {
                    if (Utils.LoadFeedImage(url, feedTitle, out _feedImage)) return true;
                }
                else
                {
                    if (Utils.LoadFeedImage(url, feedTitle, cacheFolder, out _feedImage, out _feedImagePath)) return true;
                }
            }
            return false;
        }



        public bool DownloadAndCheckFeedItemImageValid(string cacheFolder, string url, string feedTitle, string feedItemTitle)
        {
            if (Utils.IsValidUrl(ref url))
            {
                LogEvents.InvokeOnDebug(new FeedArgs("Parsed feed item image \"" + url + "\" successfull. Downloading image/Load image from cache..."));
                if (string.IsNullOrEmpty(cacheFolder))
                {
                    if (Utils.LoadFeedItemImage(url, feedTitle, feedItemTitle, out _feedImage)) return true;
                }
                else
                {
                    if (Utils.LoadFeedItemImage(url, feedTitle, feedItemTitle, cacheFolder, out _feedImage, out _feedImagePath)) return true;
                }
            }
            return false;
        }


        public bool TryParseFeedImageUrl(XDocument xmlFeed, string feedTitle)
        {
            return TryParseFeedImageUrl(xmlFeed, string.Empty, feedTitle);
        }

        public bool TryParseFeedItemImageUrl(XDocument xmlFeed, string feedTitle, string feedItemTitle, int itemNumber)
        {
            return TryParseFeedItemImageUrl(xmlFeed, string.Empty, feedTitle, feedItemTitle, itemNumber);
        }
    }
}
