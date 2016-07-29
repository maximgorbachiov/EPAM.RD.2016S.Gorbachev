using System;
using System.Net;
using System.Net.Sockets;
using StorageInterfaces.CommunicationEntities;
using StorageInterfaces.EventArgs;
using StorageInterfaces.INetworkConnections;
using StorageLib.Services;

namespace Storage.NetworkClients
{
    public class NetworkUpdater : INetworkUpdater
    {
        private readonly IPEndPoint endPoint;

        public EventHandler<AddEventArg> OnAdd { get; set; }
        public EventHandler<DeleteEventArg> OnDelete { get; set; }

        public NetworkUpdater(IPEndPoint endPoint)
        {
            this.endPoint = endPoint;
        }

        public async void UpdateByCommand()
        {
            var listener = new TcpListener(endPoint);
            listener.Start();

            try
            {
                while (true)
                {
                    /*LogService.Service.TraceInfo($"{ AppDomain.CurrentDomain.FriendlyName } wait for active connections");
                    var client = await listener.AcceptTcpClientAsync();
                    LogService.Service.TraceInfo($"{AppDomain.CurrentDomain.FriendlyName} accept client with port {((IPEndPoint)client.Client.RemoteEndPoint).Port}");

                    using (var service = new NetworkClient(client))
                    {
                        LogService.Service.TraceInfo($"{ AppDomain.CurrentDomain.FriendlyName } try to read info from client with port {((IPEndPoint)client.Client.RemoteEndPoint).Port}");
                        var data = await service.ReadAsync<NetworkData>(1024);
                        LogService.Service.TraceInfo($"{ AppDomain.CurrentDomain.FriendlyName } read info from client with port {((IPEndPoint)client.Client.RemoteEndPoint).Port}");
                        switch (data.Command)
                        {
                            case ServiceCommands.ADD_USER:
                                AddUserUpdate(data.User);
                                LogService.Service.TraceInfo($"{ AppDomain.CurrentDomain.FriendlyName } is added user { data.User.Id }");
                                break;
                            case ServiceCommands.DELETE_USER:
                                DeleteUserUpdate(data.User.Id);
                                LogService.Service.TraceInfo($"{ AppDomain.CurrentDomain.FriendlyName } is deleted user { data.User.Id }");
                                break;
                        }
                    }*/
                    using (var client = await listener.AcceptTcpClientAsync())
                    {
                        LogService.Service.TraceInfo($"{AppDomain.CurrentDomain.FriendlyName} accept client with port {((IPEndPoint)client.Client.RemoteEndPoint).Port}");
                        LogService.Service.TraceInfo($"{ AppDomain.CurrentDomain.FriendlyName } try to read info from client with port {((IPEndPoint)client.Client.RemoteEndPoint).Port}");
                        var data = await client.ReadAsync<NetworkData>(1024);
                        LogService.Service.TraceInfo($"{ AppDomain.CurrentDomain.FriendlyName } read info from client with port {((IPEndPoint)client.Client.RemoteEndPoint).Port}");
                        switch (data.Command)
                        {
                            case ServiceCommands.ADD_USER:
                                OnAdd(this, new AddEventArg { user = data.User });
                                LogService.Service.TraceInfo($"{ AppDomain.CurrentDomain.FriendlyName } is added user { data.User.Id }");
                                break;
                            case ServiceCommands.DELETE_USER:
                                OnDelete(this, new DeleteEventArg { Id = data.User.Id });
                                LogService.Service.TraceInfo($"{ AppDomain.CurrentDomain.FriendlyName } is deleted user { data.User.Id }");
                                break;
                        }
                    }
                }
            }
            catch (ObjectDisposedException oDEx)
            {
                LogService.Service.TraceInfo(oDEx.Message);
            }
            catch (NullReferenceException nREx)
            {
                LogService.Service.TraceInfo(nREx.Message);
            }
        }
    }
}
