﻿using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace StorageInterfaces.CommunicationEntities
{
    [Serializable]
    [JsonObject]
    public class ServicesIp
    {
        public List<IPEndPoint> SlavesEndPoints { get; set; }
    }
}
