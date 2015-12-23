#region Usings

using System;
using System.Globalization;
using System.Xml;
using WeatherConnector.Expections;

#endregion

namespace WeatherConnector.Xml
{
    internal static class WeatherXml
    {
        public static XmlDocument WeatherXmlDocument { private get; set; }

        public static string ParseXmlString(string xPath)
        {
            try
            {
                return WeatherXmlDocument.DocumentElement.SelectSingleNode(xPath).InnerText;
            }
            catch (Exception ex)
            {
                LogEvents.InvokeOnWarning(new WeatherArgs("Error parsing string from element " + xPath, ex.Message, ex.StackTrace));
                return "N/A";
            }

            //if (WeatherXmlDocument.DocumentElement.SelectSingleNode(xPath) != null)
            //    return WeatherXmlDocument.DocumentElement.SelectSingleNode(xPath).InnerText;
            //return "N/A";
        }

        public static DateTime ParseXmlDateTime(string xPath, string format)
        {
            try
            {
                string date = WeatherXmlDocument.DocumentElement.SelectSingleNode(xPath).InnerText;
                return DateTime.ParseExact(date, format, CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                LogEvents.InvokeOnWarning(new WeatherArgs("Error parsing DateTime from element " + xPath, ex.Message, ex.StackTrace));
                return new DateTime(1900, 1, 1);
            }
            //if (WeatherXmlDocument.DocumentElement.SelectSingleNode(xPath) != null)
            //{
            //    string date = WeatherXmlDocument.DocumentElement.SelectSingleNode(xPath).InnerText;
            //    int indexAMPM = date.IndexOf("AM", 0) + 2;
            //    if (indexAMPM <= 2) indexAMPM = date.IndexOf("PM", 0) + 2;
            //    return DateTime.ParseExact(date.Substring(0, indexAMPM), format, CultureInfo.InvariantCulture);
            //}
            //return new DateTime();
        }
    }
}