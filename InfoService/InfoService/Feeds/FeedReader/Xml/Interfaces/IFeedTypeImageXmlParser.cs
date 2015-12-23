using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Drawing;

namespace FeedReader.Xml.Interfaces
{
    interface IFeedTypeImageXmlParser
    {
        Image GetParsedImage();
        string GetImagePath();
        bool TryParseFeedImageUrl(XDocument xmlFeed, string cacheFolder, string feedTitle);
        bool TryParseFeedItemImageUrl(XDocument xmlFeed, string cacheFolder,string feedTitle, string feedItemTitle, int itemNumber);
        bool TryParseFeedImageUrl(XDocument xmlFeed, string feedTitle);
        bool TryParseFeedItemImageUrl(XDocument xmlFeed, string feedTitle, string feedItemTitle, int itemNumber);
        bool DownloadAndCheckFeedImageValid(string cacheFolder, string url, string feedTitle);
        bool DownloadAndCheckFeedItemImageValid(string cacheFolder, string url, string feedTitle, string feedItemTitle);
    }
}
