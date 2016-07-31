namespace StorageInterfaces.INetworkConnections
{
    public interface IBroadcastReceiver
    {
        void UdpReceive(int port);
    }
}
