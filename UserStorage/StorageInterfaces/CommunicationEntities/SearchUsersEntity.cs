using StorageInterfaces.Entities;
using System;
using Newtonsoft.Json;

namespace StorageInterfaces.CommunicationEntities
{
    [Serializable]
    [JsonObject]
    public class SearchUsersEntity
    {
        public User SearchingUser { get; private set; }

        public SearchUsersEntity(User user)
        {
            SearchingUser = user;
        }
    }
}
