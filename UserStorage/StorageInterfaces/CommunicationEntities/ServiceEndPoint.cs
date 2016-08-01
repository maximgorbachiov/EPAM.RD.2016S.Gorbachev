using System;
using System.Net;
using Newtonsoft.Json;

namespace StorageInterfaces.CommunicationEntities
{
    [Serializable]
    [JsonObject]
    public class ServiceEndPoint
    {
        public IPEndPoint EndPoint { get; set; }
    }
}
