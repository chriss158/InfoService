#region Usings

using System;
using System.Globalization;
using System.Xml.Linq;

#endregion

namespace TwitterConnector.Xml
{
    internal class TwitterXmlParser
    {
        internal static string ParseString(XElement ele, string element)
        {
            //return ele != null ? ele.Value : "";
            try
            {
                return ele.Value;
            }
            catch (Exception ex)
            {
                LogEvents.InvokeOnWarning(new TwitterArgs("Error parsing string from element " + element + ". It's possible that the element/s not exists, so you can ingore the warning", ex.Message, ex.StackTrace));
            }
            return "";
        }
        internal static int ParseInteger(XElement ele, string element)
        {
            //return ele != null ? Convert.ToInt32(ele.Value) : 0;
            try
            {
                return Convert.ToInt32(ele.Value);
            }
            catch (Exception ex)
            {
                LogEvents.InvokeOnWarning(new TwitterArgs("Error parsing string from element " + element + ". It's possible that the element/s not exists, so you can ingore the warning", ex.Message, ex.StackTrace));
            }
            return -1;
        }
        internal static DateTime ParseDateTime(XElement ele, string element)
        {
            try
            {
                DateTime tryTime;
                if (DateTime.TryParseExact(ele.Value, "ddd MMM dd HH:mm:ss zzzz yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out tryTime))
                {
                    return DateTime.ParseExact(ele.Value, "ddd MMM dd HH:mm:ss zzzz yyyy", CultureInfo.InvariantCulture);
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
