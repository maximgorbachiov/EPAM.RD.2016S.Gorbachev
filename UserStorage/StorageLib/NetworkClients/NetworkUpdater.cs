using System;
using System.Net;
using System.Net.Sockets;
using Storage.NetworkClients;
using StorageInterfaces.CommunicationEntities;
using StorageInterfaces.EventArgs;
using StorageInterfaces.INetworkConnections;
using StorageLib.Services;

namespace StorageLib.NetworkClients
{
    public class NetworkUpdater : INetworkUpdater
    {
        private readonly IPEndPoint endPoint;

        public NetworkUpdater(IPEndPoint endPoint)
        {
            this.endPoint = endPoint;
        }

        public EventHandler<AddEventArg> OnAdd { get; set; }

        public EventHandler<DeleteEventArg> OnDelete { get; set; }

        public async void UpdateByCommand()
        {
            var listener = new TcpListener(endPoint);
            listener.Start();

            try
            {
                while (true)
                {
                    using (var client = await listener.AcceptTcpClientAsync())
                    {
                        var networkClient = new NetworkClient(client);
                        LogService.Service.TraceInfo($"{AppDomain.CurrentDomain.FriendlyName} accept client with port {((IPEndPoint)client.Client.RemoteEndPoint).Port}");
                        LogService.Service.TraceInfo($"{ AppDomain.CurrentDomain.FriendlyName } try to read info from client with port {((IPEndPoint)client.Client.RemoteEndPoint).Port}");
                        var data = await client.ReadAsync<NetworkData>(1024);
                        LogService.Service.TraceInfo($"{ AppDomain.CurrentDomain.FriendlyName } read info from client with port {((IPEndPoint)client.Client.RemoteEndPoint).Port}");
                        switch (data.Command)
                        {
                            case ServiceCommands.ADD_USER:
                                OnAdd(this, new AddEventArg { User = data.User });
                                break;
                            case ServiceCommands.DELETE_USER:
                                OnDelete(this, new DeleteEventArg { Id = data.User.Id });
                                break;
                        }
                    }
                }
            }
            catch (ObjectDisposedException objectDisposedException)
            {
                LogService.Service.TraceInfo(objectDisposedException.Message);
            }
            catch (NullReferenceException nullReferenceException)
            {
                LogService.Service.TraceInfo(nullReferenceException.Message);
            }
        }
    }
}
