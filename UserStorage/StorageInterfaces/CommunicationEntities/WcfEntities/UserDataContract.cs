using System;
using System.Runtime.Serialization;
using StorageInterfaces.Entities;

namespace StorageInterfaces.CommunicationEntities.WcfEntities
{
    [DataContract]
    public class UserDataContract
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string SecondName { get; set; }

        [DataMember]
        public DateTime DateOfBirth { get; set; }

        [DataMember]
        public Gender Gender { get; set; }

        [DataMember]
        public CountryVisa[] Visas { get; set; }
    }
}
