using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace StorageInterfaces.CommunicationEntities
{
    [Serializable]
    [JsonObject]
    public class ServicesIp
    {
        public IPEndPoint MasterEndPoint { get; set; }

        public ServiceCommands Command { get; set; }

        public List<IPEndPoint> SlavesEndPoints { get; set; }
    }
}
