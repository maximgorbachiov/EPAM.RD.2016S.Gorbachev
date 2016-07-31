using System;
using System.Runtime.Serialization;

namespace StorageInterfaces.CommunicationEntities.WcfEntities
{
    [DataContract]
    public struct CountryVisa
    {
        public CountryVisa(string country, DateTime start, DateTime end)
        {
            Country = country;
            Start = start;
            End = end;
        }

        [DataMember]
        public string Country { get; set; }

        [DataMember]
        public DateTime Start { get; set; }

        [DataMember]
        public DateTime End { get; set; }
    }
}
