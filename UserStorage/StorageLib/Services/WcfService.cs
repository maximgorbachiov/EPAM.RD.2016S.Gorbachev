using System.Collections.Generic;
using StorageInterfaces.CommunicationEntities.WcfEntities;
using StorageInterfaces.Entities;
using StorageInterfaces.IServices;
using StorageInterfaces.Mappers;

namespace StorageLib.Services
{
    public class WcfService : IServiceContract, IInitializeServiceContract
    {
        private IService service;

        public WcfService() { }

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

        public void Initialize(IService service)
        {
            this.service = service;
        }
    }
}
