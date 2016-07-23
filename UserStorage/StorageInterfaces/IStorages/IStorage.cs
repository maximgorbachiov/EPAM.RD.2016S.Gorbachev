using StorageInterfaces.Entities;
using System.Collections.Generic;

namespace StorageInterfaces.IStorages
{
    public interface IStorage
    {
        List<User> Users { get; }

        int AddUser(User user);
        void DeleteUser(int id);
        List<int> SearchBy(IComparer<User> comparator, User searchingUser);
    }
}
