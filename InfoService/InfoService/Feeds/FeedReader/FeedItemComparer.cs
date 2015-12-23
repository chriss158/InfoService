using System;
using System.Collections.Generic;
using FeedReader.Data;

namespace FeedReader
{
    public class FeedItemComparer : IEqualityComparer<FeedItem>
    {
        public bool Equals(FeedItem x, FeedItem y)
        {
            return x.Title == y.Title;
        }

        public int GetHashCode(FeedItem obj)
        {
            if (Object.ReferenceEquals(obj, null)) return 0;

            int hashTitle = obj.Title == null ? 0 : obj.Title.GetHashCode();

            return hashTitle;
        }  
    }
}