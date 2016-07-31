﻿using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace StorageInterfaces.Entities
{
    [Serializable]
    public struct SavedCountryVisa : IXmlSerializable
    {
        public SavedCountryVisa(string country, DateTime start, DateTime end)
        {
            Country = country;
            Start = start;
            End = end;
        }

        public string Country { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            reader.ReadStartElement(nameof(SavedCountryVisa));

            Country = reader.ReadElementContentAsString();
            Start = reader.ReadElementContentAsDateTime();
            End = reader.ReadElementContentAsDateTime();

            reader.ReadEndElement();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement(nameof(SavedCountryVisa));

            writer.WriteElementString(nameof(Country), Country);
            writer.WriteElementString(nameof(Start), Start.ToString("yyyy-MM-dd"));
            writer.WriteElementString(nameof(End), End.ToString("yyyy-MM-dd"));

            writer.WriteEndElement();
        }
    }
}
