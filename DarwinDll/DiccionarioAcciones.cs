using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace DarwinDLL
{
    public class DiccionarioAcciones : Dictionary<String, Accion>, IXmlSerializable
    {
        #region IXmlSerializable Members
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(String));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(Accion));

            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();

            if (wasEmpty)
                return;

            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
                String key = (String)reader["key"];
                reader.ReadStartElement("item");

//                reader.ReadStartElement("value");
                Accion value = (Accion)valueSerializer.Deserialize(reader);
  //              reader.ReadEndElement();

                this.Add(key, value);

                reader.ReadEndElement();
                reader.MoveToContent();
            }
            reader.ReadEndElement();
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(String));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(Accion));

            foreach (String key in this.Keys)
            {
                writer.WriteStartElement("item");

                writer.WriteAttributeString ("key", key);

    //            writer.WriteStartElement("value");
                Accion value = this[key];
                valueSerializer.Serialize(writer, value);
      //          writer.WriteEndElement();

                writer.WriteEndElement();
            }
        }
        #endregion
    }
}
