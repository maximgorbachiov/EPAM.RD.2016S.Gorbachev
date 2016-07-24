using StorageInterfaces.Entities;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace StorageInterfaces.CommunicationEntities
{
    [Serializable]
    [JsonObject]
    public class UsersCollection
    {
        public List<User> Users { get; private set; }

        public UsersCollection(List<User> users)
        {
            Users = users;
        }
    }
}
