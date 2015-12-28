using System;
using System.Collections.Generic;
using TwitterConnector.Data;

namespace TwitterConnector
{
    public class TwitterItemComparer : IEqualityComparer<TwitterItem>
    {
        public bool Equals(TwitterItem x, TwitterItem y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(TwitterItem obj)
        {
            if (Object.ReferenceEquals(obj, null)) return 0;

            int hashTitle = obj.Id == null ? 0 : obj.Id.GetHashCode();

            return hashTitle;
        }
    }
}