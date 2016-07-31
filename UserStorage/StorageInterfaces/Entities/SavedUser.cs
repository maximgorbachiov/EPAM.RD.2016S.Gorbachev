using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace StorageInterfaces.Entities
{
    [Serializable]
    public class SavedUser : IXmlSerializable
    { 
        public int Id { get; set; }

        public string Name { get; set; }

        public string SecondName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public Gender Gender { get; set; }

        public SavedCountryVisa[] Visas { get; set; }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            reader.ReadStartElement(nameof(SavedUser));

            Id = reader.ReadElementContentAsInt();
            Name = reader.ReadElementContentAsString();
            SecondName = reader.ReadElementContentAsString();
            Gender = (Gender)reader.ReadElementContentAsInt();
            DateOfBirth = reader.ReadElementContentAsDateTime();

            reader.MoveToAttribute("count");
            int count = int.Parse(reader.Value);
            Visas = new SavedCountryVisa[count];

            reader.ReadStartElement(nameof(Visas));
            var visaSer = new XmlSerializer(typeof(SavedCountryVisa));
            for (int i = 0; i < count; i++)
            {
                var visa = (SavedCountryVisa)visaSer.Deserialize(reader);
                Visas[i] = visa;
            }

            reader.ReadEndElement();

            reader.ReadEndElement();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement(nameof(SavedUser));

            writer.WriteElementString(nameof(Id), Id.ToString());
            writer.WriteElementString(nameof(Name), Name);
            writer.WriteElementString(nameof(SecondName), SecondName);
            writer.WriteElementString(nameof(Gender), ((int)Gender).ToString());
            writer.WriteElementString(nameof(DateOfBirth), DateOfBirth.ToString("yyyy-MM-dd"));

            writer.WriteStartElement(nameof(Visas));
            writer.WriteAttributeString("count", Visas.Length.ToString());
            for (int i = 0; i < Visas.Length; i++)
            {
                Visas[i].WriteXml(writer);
            }

            writer.WriteEndElement();

            writer.WriteEndElement();
        }
    }
}
