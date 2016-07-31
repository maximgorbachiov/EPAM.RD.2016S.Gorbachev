using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace StorageInterfaces.Entities
{
    [Serializable]
    public class ServiceState : IXmlSerializable
    {
        public List<SavedUser> Users { get; set; }

        public int LastId { get; set; }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            reader.ReadStartElement(nameof(ServiceState));

            LastId = reader.ReadElementContentAsInt();

            reader.MoveToAttribute("count");
            int count = int.Parse(reader.Value);
            reader.ReadStartElement(nameof(Users));
            var userSer = new XmlSerializer(typeof(SavedUser));
            for (int i = 0; i < count; i++)
            {
                var user = (SavedUser)userSer.Deserialize(reader);
                Users.Add(user);
            }

            reader.ReadEndElement();

            reader.ReadEndElement();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString(nameof(LastId), LastId.ToString());

            writer.WriteStartElement(nameof(Users));
            writer.WriteAttributeString("count", Users.Count.ToString());
            foreach (var user in Users)
            {
                user.WriteXml(writer);
            }

            writer.WriteEndElement();
        }
    }
}
