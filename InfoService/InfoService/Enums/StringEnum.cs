using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace InfoService.Enums
{
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

    public class PropertyValue : System.Attribute
    {
        private string _value;

        public PropertyValue(string value)
        {
            _value = value;
        }

        public string Value
        {
            get { return _value; }
        }
    }

    public class WindowID : System.Attribute
    {
        private string _value;

        public WindowID(string value)
        {
            _value = value;
        }

        public string Value
        {
            get { return _value; }
        }
    }

    public static class StringEnum
    {
        public static string GetStringValue(Enum value)
        {
            string output = null;

            Type type = value.GetType();

            FieldInfo fi = type.GetField(value.ToString());
            StringValue[] attrs = fi.GetCustomAttributes(typeof(StringValue), false) as StringValue[];
            if (attrs.Length > 0)
            {
                output = attrs[0].Value;
            }

            return output;
        }

        public static string GetPropertyValue(Enum value)
        {
            string output = null;

            Type type = value.GetType();

            FieldInfo fi = type.GetField(value.ToString());
            PropertyValue[] attrs = fi.GetCustomAttributes(typeof(PropertyValue), false) as PropertyValue[];
            if (attrs.Length > 0)
            {
                output = attrs[0].Value;
            }

            return output;
        }

        public static string GetWindowID(Enum value)
        {
            string output = null;

            Type type = value.GetType();

            FieldInfo fi = type.GetField(value.ToString());
            WindowID[] attrs = fi.GetCustomAttributes(typeof(WindowID), false) as WindowID[];
            if (attrs.Length > 0)
            {
                output = attrs[0].Value;
            }

            return output;
        }
    }
}
