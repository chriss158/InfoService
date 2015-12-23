#region Usings

using System;

#endregion

namespace TwitterConnector.Expections
{
    public class TwitterUserPasswordExpection : Exception
    {
        public TwitterUserPasswordExpection()
        {
        }
        public TwitterUserPasswordExpection(string message)
            : base(message)
        {
        }
        public TwitterUserPasswordExpection(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
    public class TwitterNoCacheFolderExpection : Exception
    {
        public TwitterNoCacheFolderExpection()
        {
        }
        public TwitterNoCacheFolderExpection(string message)
            : base(message)
        {
        }
        public TwitterNoCacheFolderExpection(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
    public class TwitterCacheFolderNotValid : Exception
    {
        public TwitterCacheFolderNotValid()
        {
        }
        public TwitterCacheFolderNotValid(string message)
            : base(message)
        {
        }
        public TwitterCacheFolderNotValid(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    public class TwitterNoPinExpection : Exception
    {
        public TwitterNoPinExpection()
        {
        }
        public TwitterNoPinExpection(string message)
            : base(message)
        {
        }
        public TwitterNoPinExpection(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
    public class TwitterAuthExpection : Exception
    {
        public TwitterAuthExpection()
        {
        }
        public TwitterAuthExpection(string message)
            : base(message)
        {
        }
        public TwitterAuthExpection(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    public class TwitterNoRequestTokenExpection : Exception
    {
        public TwitterNoRequestTokenExpection()
        {
        }
        public TwitterNoRequestTokenExpection(string message)
            : base(message)
        {
        }
        public TwitterNoRequestTokenExpection(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

}