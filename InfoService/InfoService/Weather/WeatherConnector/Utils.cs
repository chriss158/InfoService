using System;
using System.Reflection;
using WeatherConnector;

namespace WeatherConnector
{
    public static class Utils
    {
        public static string GetStringValue(this TemperatureFormat value)
        {
            string output = null;
            Type type = value.GetType();
            FieldInfo fi = type.GetField(value.ToString());
            StringValue[] attrs = fi.GetCustomAttributes(typeof(StringValue), false) as StringValue[];
            if (attrs.Length > 0)
                output = attrs[0].Value;
            return output;
        }

    }
    public class StringValue : System.Attribute
    {
        private string _value;

        public StringValue(string value)
        {
            _value = value;
        }

        public string Value
        {
            get { return _value; }
        }

    }

}
