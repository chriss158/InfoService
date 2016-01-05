using System;
using System.Collections.Generic;
using FeedReader.Data;

namespace FeedReader
{
    public class FeedItemComparer : IEqualityComparer<FeedItem>
    {
        public bool Equals(FeedItem x, FeedItem y)
        {
            //return x.Title == y.Title || x.Url == y.Url;
            if (x.Title == y.Title) return true;
            if (x.Url == y.Url) return true;
            return false;
        }

        public int GetHashCode(FeedItem obj)
        {
            if (Object.ReferenceEquals(obj, null)) return 0;

            int hashTitle = 13;
            //hashTitle = (hashTitle * 7) + obj.Title.GetHashCode();
            hashTitle = (hashTitle * 7) + obj.Url.GetHashCode();

            return hashTitle;
        }
    }
}