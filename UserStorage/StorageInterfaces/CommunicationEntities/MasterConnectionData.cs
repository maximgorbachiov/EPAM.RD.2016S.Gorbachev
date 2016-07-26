using System;
using System.Collections.Generic;
using System.Net;

namespace StorageInterfaces.CommunicationEntities
{
    [Serializable]
    public class MasterConnectionData
    {
        public IPEndPoint MasterEndPoint { get; set; }
        public List<IPEndPoint> SlavesEndPoints { get; set; }
    }
}
