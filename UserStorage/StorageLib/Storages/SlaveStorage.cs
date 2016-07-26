using System;
using System.Collections.Generic;
using System.Linq;
using StorageInterfaces.IStorages;
using StorageInterfaces.Entities;
using System.Net;
using StorageInterfaces.CommunicationEntities;
using System.Net.Sockets;
using Storage.NetworkClients;
using StorageInterfaces.IRepositories;
using StorageLib.Services;

namespace StorageLib.Storages
{
    public class SlaveStorage : MarshalByRefObject, ISlaveStorage
    {
        private readonly IPEndPoint slaveEndPoint;
        private readonly IPEndPoint masterEndPoint;
        private List<User> users = new List<User>();

        public List<User> Users => users;
        public IPEndPoint SlavEndPoint => slaveEndPoint;

        public SlaveStorage(IRepository repository, SlaveConnectionData data)
        {
            slaveEndPoint = data.SlaveEndPoint;
            masterEndPoint = data.MasterEndPoint;
            users = repository.Load().Users;
            LogService.Service.TraceInfo($"{ AppDomain.CurrentDomain.FriendlyName } is created");
            LogService.Service.TraceInfo($"{ AppDomain.CurrentDomain.FriendlyName } is load");
        }

        public int AddUser(User user)
        {
            throw new NotSupportedException("This method can't be called from slave. Try to use master for it");
        }

        public void DeleteUser(int id)
        {
            throw new NotSupportedException("This method can't be called from slave. Try to use master for it");
        }

        public List<int> SearchBy(IComparer<User> comparator, User searchingUser)
        {
            if (comparator == null)
            {
                throw new NullReferenceException(nameof(comparator));
            }

            if (searchingUser == null)
            {
                throw new NullReferenceException(nameof(searchingUser));
            }

            return Users.Where(user => comparator.Compare(user, searchingUser) == 0)
                .Select(user => user.Id).ToList();
        }

        public async void NotifyMasterAboutSlaveCreate()
        {
            using (var slave = new TcpClient())
            {
                slave.Connect(masterEndPoint.Address, masterEndPoint.Port);

                using (var client = new NetworkClient(slave))
                {
                    client.WriteAsync(new NetworkData(null, ServiceCommands.IS_CREATED));
                    users = (await client.ReadAsync<UsersCollection>(1024)).Users;
                }
            }
        }

        public async void UpdateByMasterCommand()
        {
            var listener = new TcpListener(slaveEndPoint);
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
                                AddUserUpdate(data.User);
                                LogService.Service.TraceInfo($"{ AppDomain.CurrentDomain.FriendlyName } is added user { data.User.Id }");
                                break;
                            case ServiceCommands.DELETE_USER:
                                DeleteUserUpdate(data.User.Id);
                                LogService.Service.TraceInfo($"{ AppDomain.CurrentDomain.FriendlyName } is deleted user { data.User.Id }");
                                break;
                        }
                    }
                }
            }
            catch (ObjectDisposedException oDEx)
            {
                LogService.Service.TraceInfo(oDEx.Message);
                LogService.Service.TraceInfo(oDEx.InnerException.Message);
                LogService.Service.TraceInfo(oDEx.ObjectName);
                LogService.Service.TraceInfo(oDEx.StackTrace);
                throw;
            }
            catch (NullReferenceException nREx)
            {
                LogService.Service.TraceInfo(nREx.Message);
                LogService.Service.TraceInfo(nREx.StackTrace);
                throw;
            }
        }

        protected virtual void AddUserUpdate(User user)
        {
            if (user != null)
            {
                Users.Add(user);
            }
        }

        protected virtual void DeleteUserUpdate(int id)
        {
            User user;
            if ((user = Users.FirstOrDefault(u => u.Id == id)) != null)
            {
                Users.Remove(user);
            }
        }
    }
}
