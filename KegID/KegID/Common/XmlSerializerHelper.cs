using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace KegID.Common
{
    public class XmlSerializerHelper
    {
        public string GetSerializedString<T>(T objectToSerialize)
        {
            var serializer = new XmlSerializer(typeof(T));
            TextWriter textWriter = new StringWriter();

            var xmlWriter = XmlWriter.Create(textWriter);
            serializer.Serialize(xmlWriter, objectToSerialize);

            string result = textWriter.ToString();
            return result;
        }

        public string Serialize(object obj)
        {
            // process obData as normal using XmlSerializer
            var serializer = new XmlSerializer(obj.GetType());

            var writerSettings =
                new XmlWriterSettings
                {
                    OmitXmlDeclaration = true,
                    Indent = true
                };

            var manifestNameSpace = new XmlSerializerNamespaces();
            manifestNameSpace.Add("i", "http://www.w3.org/2001/XMLSchema-instance");

            manifestNameSpace.Add("d2p1", "http://schemas.datacontract.org/2004/07/System");

            var stringWriter = new StringWriter();
            using (var xmlWriter = XmlWriter.Create(stringWriter, writerSettings))
            {
                serializer.Serialize(xmlWriter, obj, manifestNameSpace);

                return stringWriter.ToString();
            }
        }
    }
}
