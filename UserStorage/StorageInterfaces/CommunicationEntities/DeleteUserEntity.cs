using System;
using Newtonsoft.Json;

namespace StorageInterfaces.CommunicationEntities
{
    [Serializable]
    [JsonObject]
    public class DeleteUserEntity
    {
        public int UserId { get; private set; }

        public DeleteUserEntity(int id)
        {
            UserId = id;
        }
    }
}
