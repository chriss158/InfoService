using System;
using System.Reflection;

namespace TwitterConnector
{
    public enum TimelineType
    {
        None,

        //Removed from Api
        //[Url("http://twitter.com/statuses/public_timeline.xml")]
        //[PrettyName("Public Timeline")]
        //Public,

        [Url("http://api.twitter.com/1/statuses/home_timeline.xml")]
        [PrettyName("Home Timeline")]
        Home,

        [Url("http://api.twitter.com/1/statuses/user_timeline.xml")]
        [PrettyName("User Timeline")]
        User,

        [Url("http://api.twitter.com/1/statuses/friends_timeline.xml")]
        [PrettyName("Friends Timeline")]
        Friends,

        [Url("http://api.twitter.com/1/statuses/mentions.xml")]
        [PrettyName("Mentions Timeline")]
        Mentions,

        [Url("http://api.twitter.com/1/statuses/retweeted_by_me.xml")]
        [PrettyName("Retweeted by me Timeline")]
        RetweetedByMe,

        [Url("http://api.twitter.com/1/statuses/retweeted_to_me.xml")]
        [PrettyName("Retweeted to me Timeline")]
        RetweetedToMe,

        [Url("http://api.twitter.com/1/statuses/retweets_of_me.xml")]
        [PrettyName("Retweets of me Timeline")]
        RetweetsOfMe,
    }


    public static class TimelineUrl
    {
        public static string GetUrl(this TimelineType value)
        {
            string output = null;
            Type type = value.GetType();
            FieldInfo fi = type.GetField(value.ToString());
            Url[] attrs = fi.GetCustomAttributes(typeof(Url), false) as Url[];
            if (attrs.Length > 0)
                output = attrs[0].Value;
            return output;
        }
        public static string GetPrettyName(this TimelineType value)
        {
            string output = null;
            Type type = value.GetType();
            FieldInfo fi = type.GetField(value.ToString());
            PrettyName[] attrs = fi.GetCustomAttributes(typeof(PrettyName), false) as PrettyName[];
            if (attrs.Length > 0)
                output = attrs[0].Value;
            return output;
        }
    }
    public class Url : System.Attribute
    {
        private string _value;

        public Url(string value)
        {
            _value = value;
        }

        public string Value
        {
            get { return _value; }
        }
    }
    public class PrettyName : System.Attribute
    {
        private string _value;

        public PrettyName(string value)
        {
            _value = value;
        }

        public string Value
        {
            get { return _value; }
        }
    }
}
