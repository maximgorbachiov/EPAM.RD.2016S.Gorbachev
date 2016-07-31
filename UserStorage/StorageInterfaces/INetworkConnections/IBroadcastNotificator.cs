using System;

namespace StorageInterfaces.INetworkConnections
{
    public interface IBroadcastNotificator
    {
        [STAThread]
        void UdpSend();
    }
}
