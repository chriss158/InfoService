using System;
using System.IO;
using System.Xml.Serialization;

namespace InfoService.Settings
{
    public interface IXmlDeserializationCallback
    {
        void OnXmlDeserialization(object sender);
    }

    public class CustomXmlSerializer : XmlSerializer
    {
        public CustomXmlSerializer(Type type) : base(type) {
        }

        public new object Deserialize(Stream stream)
        {
            var result = base.Deserialize(stream);

            var deserializedCallback = result as IXmlDeserializationCallback;
            if (deserializedCallback != null)
            {
                deserializedCallback.OnXmlDeserialization(this);
            }

            return result;
        }
    }
}
