using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
            networkUpdater.OnAdd += AddUserUpdate;
            networkUpdater.OnDelete += DeleteUserUpdate;
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

            List<int> result;
            locker.EnterReadLock();

            try
            {
                result = users.Where(user => comparator.Compare(user, searchingUser) == 0)
                    .Select(user => user.Id).ToList();
            }
            finally
            {
                locker.ExitReadLock();
            }

            return result;
        }

        public void UpdateByMasterCommand()
        {
            networkUpdater.UpdateByCommand();
        }

        protected virtual void AddUserUpdate(object sender, AddEventArg e)
        {
            if (e.User != null)
            {
                locker.EnterWriteLock();

                try
                {
                    users.Add(e.User);
                    LogService.Service.TraceInfo($"{ AppDomain.CurrentDomain.FriendlyName } is added user №{ users.Count }");
                }
                finally
                {
                    locker.ExitWriteLock();
                }
            }
        }

        protected virtual void DeleteUserUpdate(object sender, DeleteEventArg e)
        {
            User user;
            if ((user = users.FirstOrDefault(u => u.Id == e.Id)) != null)
            {
                locker.EnterWriteLock();

                try
                {
                    users.Remove(user);
                    LogService.Service.TraceInfo($"{ AppDomain.CurrentDomain.FriendlyName } is deleted user { user.Id }");
                }
                finally
                {
                    locker.ExitWriteLock();
                }
            }
        }

        /*public async void NotifyMasterAboutSlaveCreate()
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
        }*/
    }
}
