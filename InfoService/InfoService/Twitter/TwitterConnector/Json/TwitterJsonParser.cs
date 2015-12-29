using System;
using System.Globalization;
using System.Xml.Linq;

namespace TwitterConnector.Json
{
    internal class TwitterJsonParser
    {
        internal static string ParseString(dynamic ele, string element)
        {
            //return ele != null ? ele.Value : "";
            try
            {
                return Convert.ToString(ele);
            }
            catch (Exception ex)
            {
                LogEvents.InvokeOnWarning(new TwitterArgs("Error parsing string from element " + element + ". It's possible that the element/s not exists, so you can ingore the warning", ex.Message, ex.StackTrace));
            }
            return "";
        }
        internal static int ParseInteger(dynamic ele, string element)
        {
            //return ele != null ? Convert.ToInt32(ele.Value) : 0;
            try
            {
                return Convert.ToInt32(ele);
            }
            catch (Exception ex)
            {
                LogEvents.InvokeOnWarning(new TwitterArgs("Error parsing string from element " + element + ". It's possible that the element/s not exists, so you can ingore the warning", ex.Message, ex.StackTrace));
            }
            return -1;
        }
        internal static DateTime ParseDateTime(dynamic ele, string element)
        {
            try
            {
                DateTime tryTime;
                string dateTime = Convert.ToString(ele);
                if (DateTime.TryParseExact(dateTime, "ddd MMM dd HH:mm:ss zzzz yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out tryTime))
                {
                    return DateTime.ParseExact(dateTime, "ddd MMM dd HH:mm:ss zzzz yyyy", CultureInfo.InvariantCulture);
                }
            }
            catch (Exception ex)
            {
                LogEvents.InvokeOnWarning(new TwitterArgs("Error parsing DateTime from element " + element + ". It's possible that the element/s not exists, so you can ingore the warning", ex.Message, ex.StackTrace));
            }
            return new DateTime();
        }
    }
}
