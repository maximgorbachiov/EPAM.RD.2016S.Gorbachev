using StorageInterfaces.Entities;
using System;
using Newtonsoft.Json;

namespace StorageInterfaces.CommunicationEntities
{
    [Serializable]
    [JsonObject]
    public class AddUserEntity
    {
        public User AddingUser { get; private set; }

        public AddUserEntity(User user)
        {
            AddingUser = user;
        }
    }
}
