using System.Collections.Generic;
using StorageInterfaces.CommunicationEntities.WcfEntities;

namespace StorageInterfaces.IServices
{
    public interface IService
    {
        int AddUser(User user);

        void DeleteUser(int id);

        List<int> SearchBy(IComparer<User> comparator, User searchingUser);
    }
}
