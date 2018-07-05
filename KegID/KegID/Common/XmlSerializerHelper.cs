using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace KegID.Common
{
    public class XmlSerializerHelper
    {
        private static string ConvertJObjectToXml(JObject jo, string rootElementName)
        {
            XmlDocument doc = JsonConvert.DeserializeXmlNode(jo.ToString(), rootElementName);
            StringBuilder sb = new StringBuilder();
            StringWriter sr = new StringWriter(sb);
            XmlTextWriter xw = new XmlTextWriter(sr);
            xw.Formatting = System.Xml.Formatting.Indented;
            doc.WriteTo(xw);
            return sb.ToString();
        }

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
            try
            {
                if (obj is JObject)
                {
                    return ConvertJObjectToXml((JObject)obj, "root");
                }

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

                var shipDateNameSpace = new XmlSerializerNamespaces();
                manifestNameSpace.Add("d2p1", "http://schemas.datacontract.org/2004/07/System");

                var stringWriter = new StringWriter();
                using (var xmlWriter = XmlWriter.Create(stringWriter, writerSettings))
                {
                    serializer.Serialize(xmlWriter, obj, manifestNameSpace);

                    return stringWriter.ToString();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.InnerException);
                return string.Empty;
            }
        }
    }
}
