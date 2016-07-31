using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using StorageInterfaces.CommunicationEntities;
using StorageInterfaces.INetworkConnections;
using StorageLib.Services;
using Storage.NetworkClients;

namespace StorageLib.NetworkClients
{
    public class NetworkNotificator : INetworkNotificator
    {
        private IPEndPoint endPoint;
        private readonly List<IPEndPoint> slavesEndPoints; 

        public NetworkNotificator(MasterConnectionData masterConnectionData)
        {
            endPoint = masterConnectionData.MasterEndPoint;
            slavesEndPoints = masterConnectionData.SlavesEndPoints;
        }

        /*public async void NotifyServicesAboutDataUpdate(NetworkData data)
        {
            foreach (var slave in slavesEndPoints)
            {
                using (var tcpClient = new TcpClient())
                {
                    LogService.Service.TraceInfo($"{ AppDomain.CurrentDomain.FriendlyName } try to connect to slave with port { slave.Port }");
                    await tcpClient.ConnectAsync(slave.Address, slave.Port);
                    LogService.Service.TraceInfo($"{ AppDomain.CurrentDomain.FriendlyName } connected to slave with port { slave.Port }");

                    var host = new NetworkClient(tcpClient);
                    LogService.Service.TraceInfo($"{ AppDomain.CurrentDomain.FriendlyName } try to notify slave with port { slave.Port }");
                    host.WriteAsync(data);
                    LogService.Service.TraceInfo($"{ AppDomain.CurrentDomain.FriendlyName } notify slave with port { slave.Port }");
                }
            }
        }*/

        public async void NotifyServicesAboutDataUpdate(NetworkData data)
        {
            foreach (var slave in slavesEndPoints)
            {
                using (var tcpClient = new TcpClient())
                {
                    LogService.Service.TraceInfo($"{ AppDomain.CurrentDomain.FriendlyName } try to connect to slave with port { slave.Port }");
                    await tcpClient.ConnectAsync(slave.Address, slave.Port);
                    LogService.Service.TraceInfo($"{ AppDomain.CurrentDomain.FriendlyName } connected to slave with port { slave.Port }");

                    LogService.Service.TraceInfo($"{ AppDomain.CurrentDomain.FriendlyName } try to notify slave with port { slave.Port }");
                    await tcpClient.WriteAsync(data);
                    LogService.Service.TraceInfo($"{ AppDomain.CurrentDomain.FriendlyName } notify slave with port { slave.Port }");
                }
            }
        }
    }
}
