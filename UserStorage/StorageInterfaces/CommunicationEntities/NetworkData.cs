using System;
using Newtonsoft.Json;
using StorageInterfaces.CommunicationEntities.WcfEntities;

namespace StorageInterfaces.CommunicationEntities
{
    [Serializable]
    [JsonObject]
    public class NetworkData
    {
        public NetworkData(User user, ServiceCommands command)
        {
            User = user;
            Command = command;
        }

        public User User { get; private set; }

        public ServiceCommands Command { get; private set; }
    }
}
