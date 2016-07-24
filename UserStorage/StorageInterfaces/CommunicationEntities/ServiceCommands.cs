using System;
using Newtonsoft.Json;

namespace StorageInterfaces.CommunicationEntities
{
    [Serializable]
    [JsonObject]
    public enum ServiceCommands
    {
        IS_CREATED,
        TAKE_USERS,
        ADD_USER,
        DELETE_USER,
        SEARCH_USERS
    }
}
