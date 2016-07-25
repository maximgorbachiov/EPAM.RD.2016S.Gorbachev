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

namespace StorageLib.Storages
{
    public class SlaveStorage : MarshalByRefObject, ISlaveStorage
    {
        private readonly IPEndPoint slaveEndPoint;
        private readonly IPEndPoint masterEndPoint;
        private List<User> users = new List<User>();

        public List<User> Users => users;

        public SlaveStorage(IRepository repository, IPEndPoint slaveEndPoint, IPEndPoint masterEndPoint)
        {
            this.slaveEndPoint = slaveEndPoint;
            this.masterEndPoint = masterEndPoint;
            users = repository.Load().Users;
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

            while(true)
            {
                var client = await listener.AcceptTcpClientAsync();

                using (var service = new NetworkClient(client))
                {
                    var data = await service.ReadAsync<NetworkData>(1024);

                    switch (data.Command)
                    {
                        case ServiceCommands.ADD_USER:
                            AddUserUpdate(data.User);
                            break;
                        case ServiceCommands.DELETE_USER:
                            DeleteUserUpdate(data.User.Id);
                            break;
                    }
                }
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
