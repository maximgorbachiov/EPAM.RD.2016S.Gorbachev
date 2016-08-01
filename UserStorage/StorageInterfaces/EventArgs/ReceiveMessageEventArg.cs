using StorageInterfaces.CommunicationEntities;

namespace StorageInterfaces.EventArgs
{
    public class ReceiveMessageEventArg : System.EventArgs
    {
        public NetworkData Data { get; set; }
    }
}
