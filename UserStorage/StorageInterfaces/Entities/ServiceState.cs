using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace StorageInterfaces.Entities
{
    [Serializable]
    [DataContract]
    public class ServiceState
    {
        [DataMember]
        public List<User> Users { get; set; }

        [DataMember]
        public int LastId { get; set; }
    }
}
