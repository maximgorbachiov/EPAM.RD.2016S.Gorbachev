using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using StorageInterfaces.CommunicationEntities.WcfEntities;

namespace StorageInterfaces.CommunicationEntities
{
    [Serializable]
    [JsonObject]
    public class UsersCollection
    {
        public UsersCollection(List<User> users)
        {
            Users = users;
        }

        public List<User> Users { get; private set; }
    }
}
