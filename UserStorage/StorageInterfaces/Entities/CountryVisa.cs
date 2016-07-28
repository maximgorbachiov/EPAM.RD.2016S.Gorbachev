using System;
using System.Runtime.Serialization;

namespace StorageInterfaces.Entities
{
    [DataContract]
    public struct CountryVisa
    {
        [DataMember]
        public string country;

        [DataMember]
        public DateTime start;

        [DataMember]
        public DateTime end;

        public CountryVisa(string country, DateTime start, DateTime end)
        {
            this.country = country;
            this.start = start;
            this.end = end;
        }
    }
}
