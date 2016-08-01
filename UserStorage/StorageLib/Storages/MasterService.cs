using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using StorageInterfaces.CommunicationEntities;
using StorageInterfaces.CommunicationEntities.WcfEntities;
using StorageInterfaces.Entities;
using StorageInterfaces.IFactories;
using StorageInterfaces.IGenerators;
using StorageInterfaces.INetworkConnections;
using StorageInterfaces.IRepositories;
using StorageInterfaces.IServices;
using StorageInterfaces.IValidators;
using StorageInterfaces.Mappers;
using StorageLib.Services;

namespace StorageLib.Storages
{
    public class MasterService : MarshalByRefObject, IService, ILoader
    {
        private readonly IValidator validator;
        private readonly IGenerator generator;
        private readonly IRepository repository;
        private readonly INetworkNotificator networkNotificator;
        private readonly ReaderWriterLockSlim locker = new ReaderWriterLockSlim();

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

            locker.EnterWriteLock();

            try
            {
                generator.MoveNext();
                user.Id = generator.Current;
                users.Add(user);
                LogService.Service.TraceInfo($"{ AppDomain.CurrentDomain.FriendlyName } add user №{ users.Count }");
            }
            finally
            {
                locker.ExitWriteLock();
            }

            networkNotificator.NotifyServicesAboutDataUpdate(new NetworkData(user, ServiceCommands.ADD_USER));
            return user.Id;
        }

        public void DeleteUser(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException(nameof(id));
            }

            User user = users.FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                LogService.Service.TraceInfo($"{ AppDomain.CurrentDomain.FriendlyName } user №{ id } doesn't exist");
                return;
            }

            locker.EnterWriteLock();

            try
            { 
                users.Remove(user);
                LogService.Service.TraceInfo($"{ AppDomain.CurrentDomain.FriendlyName } delete user №{ id }");
            }
            finally
            {
                locker.ExitWriteLock();
            }

            networkNotificator.NotifyServicesAboutDataUpdate(new NetworkData(user, ServiceCommands.DELETE_USER));
        }

        public List<int> SearchBy(Func<List<User>, List<User>>[] searchFuncs)
        {
            if (searchFuncs == null)
            {
                throw new NullReferenceException(nameof(searchFuncs));
            }

            var tempResults = new List<List<User>>();
            var result = new List<int>();

            locker.EnterReadLock();

            try
            {
                tempResults.AddRange(searchFuncs.Select(func => func(users)));

                foreach (var tempResult in tempResults)
                {
                    result.AddRange(tempResult.Distinct().Select(user => user.Id));
                }
            }
            finally
            {
                locker.ExitReadLock();
            }

            return result.Distinct().ToList();
        }

        void ILoader.Load()
        {
            locker.EnterWriteLock();

            try
            {
                ServiceState lastState = repository.Load();
                users = lastState.Users?.Select(user => user.ToUser()).ToList() ?? new List<User>();
                generator.SetGeneratorState(lastState.LastId);
                LogService.Service.TraceInfo($"{ AppDomain.CurrentDomain.FriendlyName } loaded");
            }
            finally
            {
                locker.ExitWriteLock();
            }
        }

        void ILoader.Save()
        {
            locker.EnterWriteLock();

            try
            {
                repository.Save(new ServiceState { Users = users?.Select(user => user.ToSavedUser()).ToList(), LastId = generator.Current });
                LogService.Service.TraceInfo($"{ AppDomain.CurrentDomain.FriendlyName } saved his state");
            }
            finally
            {
                locker.ExitWriteLock();
            }
        }
    }
}
