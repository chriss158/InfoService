#region Usings

using System;

#endregion

namespace FeedReader.Expections
{
    public class FeedCacheFolderNotValid : Exception
    {
        public FeedCacheFolderNotValid()
        {
        }

        public FeedCacheFolderNotValid(string message)
            : base(message)
        {
        }

        public FeedCacheFolderNotValid(string message, Exception inner)
            : base(message, inner)
        {
        }
    }


    public class FeedNoCacheFolderExpection : Exception
    {
        public FeedNoCacheFolderExpection()
        {
        }

        public FeedNoCacheFolderExpection(string message)
            : base(message)
        {
        }

        public FeedNoCacheFolderExpection(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}