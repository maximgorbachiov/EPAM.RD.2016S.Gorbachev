using System.Net;
using System.Net.Sockets;
using StorageInterfaces.CommunicationEntities;
using StorageInterfaces.INetworkConnections;
using StorageLib.Serializers;

namespace Storage.NetworkClients
{
    public class BroadcastReceiver : IBroadcastReceiver
    {
        private readonly int port;
        private readonly IPEndPoint endPoint;

        public BroadcastReceiver(int port, IPEndPoint endPoint)
        {
            this.port = port;
            this.endPoint = endPoint;
        }

        public async void ReceiveBroadcast()
        {
            var serializer = new BsonSerializer<ServiceEndPoint>();
            var udpClient = new UdpClient(port);

            var message = await udpClient.ReceiveAsync();

            udpClient.Close();

            var masterIp = serializer.Deserialize(message.Buffer);

            using (var client = new TcpClient())
            {
                await client.ConnectAsync(masterIp.EndPoint.Address, masterIp.EndPoint.Port);

                byte[] data = serializer.Serialize(new ServiceEndPoint { EndPoint = endPoint });

                await client.WriteAsync(data);
            }
        }
    }
}
