#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

#endregion

namespace FeedReader.Xml
{
    internal static class FeedXmlParser
    {
        internal static string ParseString(IEnumerable<XElement> ele, string element)
        {
            /*try
            {
                return (from e in ele select e.Value).Single();
            }
            catch (Exception ex)
            {
                LogEvents.InvokeOnWarning(new FeedArgs("Unable to parse string from element " + element + ". It's possible that the element/s not exists, so you can ingore the warning", ex.Message, ex.StackTrace));
            }*/
            if (ele != null)
            {
                if (ele.Count() > 0)
                {
                    try
                    {
                        return (from e in ele select e.Value).First();
                    }
                    catch (Exception ex)
                    {
                        LogEvents.InvokeOnWarning(new FeedArgs("Unable to parse string from element " + element + ". It's possible that the element/s not exists, so you can ingore the warning", ex.Message, ex.StackTrace));
                        return "";
                    }
                }

            }
            LogEvents.InvokeOnWarning(new FeedArgs("Unable to parse string from element " + element + ". It's possible that the element/s not exists, so you can ingore the warning"));
            return "";
        }

        internal static string ParseString(XElement ele, string element)
        {
            /*try
            {
                return ele.Value;
            }
            catch (Exception ex)
            {
                LogEvents.InvokeOnWarning(new FeedArgs("Unable to parse string from element " + element + ". It's possible that the element/s not exists, so you can ingore the warning", ex.Message, ex.StackTrace));
            }*/
            if (ele != null)
                return ele.Value;
            LogEvents.InvokeOnWarning(new FeedArgs("Unable to parse string from element " + element + ". It's possible that the element/s not exists, so you can ingore the warning"));
            return "";
        }

        internal static string ParseString(IEnumerable<XAttribute> attr, string element)
        {
            /*try
            {
                return (from a in attr select a.Value).Single();
            }
            catch (Exception ex)
            {
                LogEvents.InvokeOnWarning(new FeedArgs("Unable to parse string from element " + element + ". It's possible that the element/s not exists, so you can ingore the warning", ex.Message, ex.StackTrace));
            }*/
            if (attr != null)
            {
                if (attr.Count() > 0)
                {
                    try
                    {
                        return (from a in attr select a.Value).First();
                    }
                    catch (Exception ex)
                    {
                        LogEvents.InvokeOnWarning(new FeedArgs("Unable to parse string from element " + element + ". It's possible that the element/s not exists, so you can ingore the warning", ex.Message, ex.StackTrace));
                        return "";
                    }
                }
            }
            LogEvents.InvokeOnWarning(new FeedArgs("Unable to parse string from element " + element + ". It's possible that the element/s not exists, so you can ingore the warning"));
            return "";
        }

        internal static string ParseString(XElement ele, XName attribute, string element)
        {
            /*try
            {
                return ele.Attribute(attribute).Value;
            }
            catch (Exception ex)
            {
                LogEvents.InvokeOnWarning(new FeedArgs("Unable to parse string from element " + element + ". It's possible that the element/s not exists, so you can ingore the warning", ex.Message, ex.StackTrace));
            }*/
            if (ele != null)
            {
                if (ele.Attribute(attribute) != null)
                {
                    return ele.Attribute(attribute).Value;
                }
            }
            LogEvents.InvokeOnWarning(new FeedArgs("Unable to parse string from element " + element + ". It's possible that the element/s not exists, so you can ingore the warning"));
            return "";
        }

        internal static DateTime ParseDateTime(XElement ele, string element)
        {
            /*try
            {
                DateTime tryTime;
                if (DateTime.TryParse(ele.Value, out tryTime))
                {
                    return DateTime.Parse(ele.Value);
                }
            }
            catch (Exception ex)
            {
                LogEvents.InvokeOnWarning(new FeedArgs("Unable to parse DateTime from element " + element + ". It's possible that the element/s not exists, so you can ingore the warning", ex.Message, ex.StackTrace));
            }*/
            if (ele != null)
            {
                DateTime tryTime;
                if (DateTime.TryParse(ele.Value, out tryTime))
                {
                    return DateTime.Parse(ele.Value);
                }
            }
            LogEvents.InvokeOnWarning(new FeedArgs("Unable to parse DateTime from element " + element + ". It's possible that the element/s not exists, so you can ingore the warning"));
            return DateTime.MinValue;
        }
    }
}