using StorageInterfaces.Entities;
using StorageInterfaces.IGenerators;
using StorageInterfaces.IRepositories;
using StorageInterfaces.IStorages;
using StorageInterfaces.IValidators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Storage.NetworkClients;
using StorageInterfaces.CommunicationEntities;

namespace StorageLib.Storages
{
    public class MasterStorage : MarshalByRefObject, IMasterStorage
    {
        private readonly IValidator validator;
        private readonly IGenerator generator;
        private readonly IRepository repository;

        private List<User> users = new List<User>();
        private readonly List<IPEndPoint> slavesEndPoints = new List<IPEndPoint>(); 

        public List<User> Users => users;
        public IPEndPoint MasterEndPoint { get; set; }

        public MasterStorage(IValidator validator, IGenerator generator, IRepository repository)
        {
            this.validator = validator;
            this.generator = generator;
            this.repository = repository;
        }

        public int AddUser(User user)
        {
            if (generator == null)
            {
                throw new NullReferenceException(nameof(generator));
            }

            if (user == null)
            {
                throw new NullReferenceException(nameof(user));
            }

            if (!validator.Validate(user))
            {
                throw new ArgumentException(nameof(user));
            }

            user.Id = generator.Current;
            users.Add(user);
            NotifyServicesAboutDataUpdate(new NetworkData(user, ServiceCommands.ADD_USER));
            return user.Id;
        }

        public void DeleteUser(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException(nameof(id));
            }

            var user = users.FirstOrDefault(u => u.Id == id);
            users.Remove(user);
            NotifyServicesAboutDataUpdate(new NetworkData(user, ServiceCommands.DELETE_USER));
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

            return users.Where(user => comparator.Compare(user, searchingUser) == 0)
                .Select(user => user.Id).ToList();
        }

        public void Load()
        {
            ServiceState lastState = repository.Load();
            users = lastState.Users;
            generator.SetGeneratorState(lastState.LastId);
        }

        public void Save()
        {
            repository.Save(new ServiceState { Users = users, LastId = generator.Current });
        }

        public async void InitializeServices()
        {
            var listener = new TcpListener(MasterEndPoint);
            listener.Start();

            while (true)
            {
                var client = await listener.AcceptTcpClientAsync();

                using (var host = new NetworkClient(client))
                {
                    var command = (await host.ReadAsync<NetworkData>(1024)).Command;

                    switch (command)
                    {
                        case ServiceCommands.IS_CREATED:
                            host.WriteAsync(new UsersCollection(users));
                            break;
                    }

                    slavesEndPoints.Add((IPEndPoint)client.Client.RemoteEndPoint);
                }
            }
        }

        protected virtual async void NotifyServicesAboutDataUpdate(NetworkData data)
        {
            foreach (var slave in slavesEndPoints)
            {
                using (var tcpClient = new TcpClient())
                {
                    await tcpClient.ConnectAsync(slave.Address, slave.Port);
                    using (var host = new NetworkClient(tcpClient))
                    {
                        host.WriteAsync(data);
                    }
                }
            }
        }
    }
}
