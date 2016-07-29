using System;
using System.Collections.Generic;
using StorageInterfaces.CommunicationEntities.WcfEntities;
using StorageInterfaces.Entities;
using StorageInterfaces.IServices;
using StorageInterfaces.IWcfServices;
using StorageInterfaces.Mappers;

namespace StorageLib.Services
{
    [Serializable]
    public class WcfService : IServiceContract
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

        public List<int> SearchBy(IComparer<User> comparer, UserDataContract searchingUser)
        {
            return service.SearchBy(comparer, searchingUser.ToUser());
        }
    }
}
