using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Storage.NetworkClients;
using StorageInterfaces.CommunicationEntities;
using StorageInterfaces.INetworkConnections;
using StorageLib.Services;

namespace StorageLib.NetworkClients
{
    public class NetworkNotificator : INetworkNotificator
    {
        private readonly List<IPEndPoint> slavesEndPoints;
        private IPEndPoint endPoint;

        public NetworkNotificator(ServicesIp masterConnectionData)
        {
            endPoint = masterConnectionData.MasterEndPoint;
            slavesEndPoints = masterConnectionData.SlavesEndPoints;
        }

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
