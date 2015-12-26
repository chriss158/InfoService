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

        [Url("https://api.twitter.com/1.1/statuses/home_timeline.json")]
        [PrettyName("Home Timeline")]
        Home,

        [Url("https://api.twitter.com/1.1/statuses/user_timeline.json")]
        [PrettyName("User Timeline")]
        User,

        //[Url("https://api.twitter.com/1.1/statuses/friends_timeline.json")]
        //[PrettyName("Friends Timeline")]
        //Friends,

        [Url("https://api.twitter.com/1.1/statuses/mentions_timeline.json")]
        [PrettyName("Mentions Timeline")]
        Mentions,

        //[Url("https://api.twitter.com/1.1/statuses/retweeted_by_me.json")]
        //[PrettyName("Retweeted by me Timeline")]
        //RetweetedByMe,

        //[Url("https://api.twitter.com/1.1/statuses/retweeted_to_me.json")]
        //[PrettyName("Retweeted to me Timeline")]
        //RetweetedToMe,

        [Url("https://api.twitter.com/1.1/statuses/retweets_of_me.json")]
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
            if (attrs != null && attrs.Length > 0)
                output = attrs[0].Value;
            return output;
        }
        public static string GetPrettyName(this TimelineType value)
        {
            string output = null;
            Type type = value.GetType();
            FieldInfo fi = type.GetField(value.ToString());
            PrettyName[] attrs = fi.GetCustomAttributes(typeof(PrettyName), false) as PrettyName[];
            if (attrs != null && attrs.Length > 0)
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
