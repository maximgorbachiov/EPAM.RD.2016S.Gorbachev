using StorageInterfaces.CommunicationEntities;

namespace StorageInterfaces.INetworkConnections
{
    public interface INetworkNotificator
    {
        void NotifyServicesAboutDataUpdate(NetworkData data);
    }
}
