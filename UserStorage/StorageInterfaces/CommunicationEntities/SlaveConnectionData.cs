using System;
using System.Net;

namespace StorageInterfaces.CommunicationEntities
{
    [Serializable]
    public class SlaveConnectionData
    {
        public IPEndPoint MasterEndPoint { get; set; }

        public IPEndPoint SlaveEndPoint { get; set; }
    }
}
