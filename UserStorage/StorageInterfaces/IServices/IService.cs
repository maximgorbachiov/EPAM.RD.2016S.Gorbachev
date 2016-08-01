using System;
using System.Collections.Generic;
using StorageInterfaces.CommunicationEntities.WcfEntities;

namespace StorageInterfaces.IServices
{
    public interface IService
    {
        int AddUser(User user);

        void DeleteUser(int id);

        List<int> SearchBy(Func<List<User>, List<User>>[] searchFuncs);
    }
}
