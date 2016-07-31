using StorageInterfaces.Entities;
using StorageInterfaces.IServices;
using System.Collections.Generic;

namespace Storage.Services
{
    public class ServiceManager //: IService
    {
        /* private IMasterStorage masterStorage;
         private List<ISlaveStorage> slaveStorages;
         private int currentServiceSearchIndex;

         public ServiceManager(IMasterStorage masterStorage, List<ISlaveStorage> slaveStorages)
         {
             this.masterStorage = masterStorage;
             this.slaveStorages = slaveStorages;
         }

         public int AddUser(User user)
         {
             return masterStorage.AddUser(user);
         }

         public void DeleteUser(int id)
         {
             masterStorage.DeleteUser(id);
         }

         public List<int> SearchBy(IComparer<User> comparator, User searchingUser)
         {
             List<int> result;

             if (currentServiceSearchIndex == 0)
             {
                 result = masterStorage.SearchBy(comparator, searchingUser);
             }
             result = slaveStorages[currentServiceSearchIndex - 1].SearchBy(comparator, searchingUser);

             if (++currentServiceSearchIndex == slaveStorages.Count)
             {
                 currentServiceSearchIndex = 0;
             }

             return result;
         }*/
    }
}
