using System;

namespace StorageInterfaces.CommunicationEntities
{
    [Serializable]
    public enum ServiceCommands
    {
        IS_CREATED,
        TAKE_USERS,
        ADD_USER,
        DELETE_USER,
        SEARCH_USERS
    }
}
