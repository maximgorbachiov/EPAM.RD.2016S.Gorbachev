using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using StorageInterfaces.CommunicationEntities;
using StorageInterfaces.CommunicationEntities.WcfEntities;
using StorageInterfaces.EventArgs;
using StorageInterfaces.IFactories;
using StorageInterfaces.INetworkConnections;
using StorageInterfaces.IRepositories;
using StorageInterfaces.IServices;
using StorageInterfaces.Mappers;
using StorageLib.Services;

namespace StorageLib.Storages
{
    public class SlaveService : MarshalByRefObject, IService, IListener
    {
        private readonly INetworkUpdater networkUpdater;
        private readonly ReaderWriterLockSlim locker = new ReaderWriterLockSlim();

        private readonly List<User> users;

        public SlaveService(IFactory factory)
        {
            networkUpdater = factory.CreateDependency<INetworkUpdater>();
            networkUpdater.OnMessageReceived += Update;
            users = factory.CreateDependency<IRepository>().Load().Users?.Select(user => user.ToUser()).ToList() ?? new List<User>();
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

        public List<int> SearchBy(Func<List<User>, List<User>>[] searchFuncs)
        {
            if (searchFuncs == null)
            {
                throw new NullReferenceException(nameof(searchFuncs));
            }

            List<List<User>> tempResults = new List<List<User>>();
            List<int> result = new List<int>();

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

        public void UpdateByMasterCommand()
        {
            networkUpdater.UpdateByCommand();
        }

        protected virtual void Update(object sender, ReceiveMessageEventArg e)
        {
            switch (e.Data.Command)
            {
                case ServiceCommands.ADD_USER:
                    AddUserUpdate(e.Data.User);
                    break;
                case ServiceCommands.DELETE_USER:
                    DeleteUserUpdate(e.Data.User);
                    break;
            }
        }

        protected void AddUserUpdate(User user)
        {
            if (user != null)
            {
                locker.EnterWriteLock();

                try
                {
                    users.Add(user);
                    LogService.Service.TraceInfo($"{ AppDomain.CurrentDomain.FriendlyName } is added user №{ users.Count }");
                }
                finally
                {
                    locker.ExitWriteLock();
                }
            }
            else
            {
                LogService.Service.TraceInfo($"{ AppDomain.CurrentDomain.FriendlyName } user №{ users.Count } is null");
            }
        }

        protected void DeleteUserUpdate(User user)
        {
            if (user == null)
            {
                LogService.Service.TraceInfo($"{ AppDomain.CurrentDomain.FriendlyName } user doesn't exist");
                return;
            }

            User tempUser = users.FirstOrDefault(u => u.Id == user.Id);

            if (tempUser == null)
            {
                LogService.Service.TraceInfo($"{ AppDomain.CurrentDomain.FriendlyName } user doesn't exist");
                return;
            }

            locker.EnterWriteLock();

            try
            {
                users.Remove(tempUser);
                LogService.Service.TraceInfo($"{ AppDomain.CurrentDomain.FriendlyName } is deleted user { tempUser.Id }");
            }
            finally
            {
                locker.ExitWriteLock();
            }
        }
    }
}
