using StorageInterfaces.CommunicationEntities.WcfEntities;
using StorageInterfaces.Entities;
using StorageInterfaces.IRepositories;
using StorageInterfaces.IServices;
using StorageInterfaces.IWcfServices;
using StorageInterfaces.Mappers;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using WcfLibrary.Interfaces;

namespace WcfLibrary.WcfServices
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class WcfService : MarshalByRefObject, IServiceContract, IWcfLoader
    {
        private readonly IService service;

        public WcfService(IService service)
        {
            this.service = service;
        }

        public int AddUser(UserDataContract userData)
        {
            return service.AddUser(userData.ToUser());
        }

        public void DeleteUser(int id)
        {
            service.DeleteUser(id);
        }

        public List<int> SearchBy(/*IComparer<User> comparer, */UserDataContract searchingUser)
        {
            return null;//service.SearchBy(comparer, searchingUser.ToUser());
        }

        void IWcfLoader.Load()
        {
            (service as ILoader)?.Load();
        }

        void IWcfLoader.Save()
        {
            (service as ILoader)?.Save();
        }
    }
}
