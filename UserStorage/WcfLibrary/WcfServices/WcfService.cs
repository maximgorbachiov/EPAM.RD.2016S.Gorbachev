using System;
using System.Collections.Generic;
using System.ServiceModel;
using StorageInterfaces.CommunicationEntities.WcfEntities;
using StorageInterfaces.INetworkConnections;
using StorageInterfaces.IRepositories;
using StorageInterfaces.IServices;
using StorageInterfaces.IWcfServices;
using WcfLibrary.Interfaces;

namespace WcfLibrary.WcfServices
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class WcfService : MarshalByRefObject, IServiceContract, IWcfLoader, IWcfListener
    {
        private readonly IService service;

        public WcfService(IService service)
        {
            this.service = service;
        }

        public int AddUser(User userData)
        {
            return service.AddUser(userData);
        }

        public void DeleteUser(int id)
        {
            service.DeleteUser(id);
        }

        public List<int> SearchBy(IComparer<User> comparer, User searchingUser)
        {
            return service.SearchBy(comparer, searchingUser);
        }

        void IWcfLoader.Load()
        {
            (service as ILoader)?.Load();
        }

        void IWcfLoader.Save()
        {
            (service as ILoader)?.Save();
        }

        void IWcfListener.UpdateByCommand()
        {
            (service as IListener)?.UpdateByMasterCommand();
        }
    }
}
