using StorageInterfaces.CommunicationEntities.WcfEntities;

namespace StorageInterfaces.EventArgs
{
    public class AddEventArg : System.EventArgs
    {
        public User User { get; set; }
    }
}
