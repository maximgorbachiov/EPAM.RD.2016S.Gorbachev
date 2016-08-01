using System.Net;
using System.Net.Sockets;
using StorageInterfaces.CommunicationEntities;
using StorageInterfaces.INetworkConnections;
using StorageLib.Serializers;

namespace Storage.NetworkClients
{
    public class BroadcastSender : IBroadcastSender
    {
        private readonly int port;
        private readonly IPEndPoint endPoint;

        public BroadcastSender(int port, IPEndPoint endPoint)
        {
            this.port = port;
            this.endPoint = endPoint;
        }

        public void SendBroadcast()
        {
            var serializer = new BsonSerializer<ServiceEndPoint>();

            using (var udpClient = new UdpClient())
            {
                byte[] message = serializer.Serialize(new ServiceEndPoint { EndPoint = endPoint });

                udpClient.Send(message, message.Length, new IPEndPoint(IPAddress.Broadcast, port));
            }
        }
    }
}
