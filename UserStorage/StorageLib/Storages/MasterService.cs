using StorageInterfaces.Entities;
using StorageInterfaces.IGenerators;
using StorageInterfaces.IRepositories;
using StorageInterfaces.IValidators;
using System;
using System.Collections.Generic;
using System.Linq;
using StorageInterfaces.CommunicationEntities;
using StorageInterfaces.IFactories;
using StorageInterfaces.INetworkConnections;
using StorageInterfaces.IServices;
using StorageLib.Services;

namespace StorageLib.Storages
{
    public class MasterService : MarshalByRefObject, IService, ILoader
    {
        private readonly IValidator validator;
        private readonly IGenerator generator;
        private readonly IRepository repository;
        private readonly INetworkNotificator networkNotificator;

        private List<User> users = new List<User>();

        public MasterService(IFactory factory)
        {
            validator = factory.CreateDependency<IValidator>();
            generator = factory.CreateDependency<IGenerator>();
            repository = factory.CreateDependency<IRepository>();
            networkNotificator = factory.CreateDependency<INetworkNotificator>();
            LogService.Service.TraceInfo($"{ AppDomain.CurrentDomain.FriendlyName } created");
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
            LogService.Service.TraceInfo($"{ AppDomain.CurrentDomain.FriendlyName } add user №{ users.Count }");
            networkNotificator.NotifyServicesAboutDataUpdate(new NetworkData(user, ServiceCommands.ADD_USER));
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
            LogService.Service.TraceInfo($"{ AppDomain.CurrentDomain.FriendlyName } delete user №{ id }");
            networkNotificator.NotifyServicesAboutDataUpdate(new NetworkData(user, ServiceCommands.DELETE_USER));
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
            
            var usersId = users.Where(user => comparator.Compare(user, searchingUser) == 0)
                .Select(user => user.Id).ToList();
            
            LogService.Service.TraceInfo($"{ AppDomain.CurrentDomain.FriendlyName } find users");

            return usersId;
        }

        public void Load()
        {
            ServiceState lastState = repository.Load();
            users = lastState.Users ?? new List<User>();
            generator.SetGeneratorState(lastState.LastId);
            LogService.Service.TraceInfo($"{ AppDomain.CurrentDomain.FriendlyName } loaded");
        }

        public void Save()
        {
            repository.Save(new ServiceState { Users = users, LastId = generator.Current });
            LogService.Service.TraceInfo($"{ AppDomain.CurrentDomain.FriendlyName } saved his state");
        }

        /*public async void InitializeServices()
        {
            var listener = new TcpListener(masterEndPoint);
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
        }*/
    }
}
