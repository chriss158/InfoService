using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FeedReader.Data;
using FeedReader.Enums;

namespace FeedReader.Xml.Interfaces
{
    interface IFeedTypeXmlParser
    {
        Feed GetParsedFeed();
        bool TryParse(XDocument xmlFeed, string cacheFolder, bool downloadImages, List<FeedItemFilter> itemFilters);
        bool TryParse(XDocument xmlFeed, bool downloadImages, List<FeedItemFilter> itemFilters);
    }
}
