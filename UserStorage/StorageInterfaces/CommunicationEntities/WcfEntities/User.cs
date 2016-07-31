using System;
using System.Runtime.Serialization;
using StorageInterfaces.Entities;

namespace StorageInterfaces.CommunicationEntities.WcfEntities
{
    [DataContract]
    public class User : IEquatable<User>
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

        public bool Equals(User other)
        {
            if (other == null)
            {
                return false;
            }

            return Id == other.Id &&
                Name == other.Name &&
                SecondName == other.SecondName;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            return obj.GetType() == typeof(User) && Equals((User)obj);
        }

        public override int GetHashCode()
        {
            int hash = Id.GetHashCode();
            hash ^= Name?.GetHashCode() ?? 0;
            hash ^= SecondName?.GetHashCode() ?? 0;
            hash ^= DateOfBirth.GetHashCode();
            hash ^= Gender.GetHashCode();

            return hash;
        }

    }
}
