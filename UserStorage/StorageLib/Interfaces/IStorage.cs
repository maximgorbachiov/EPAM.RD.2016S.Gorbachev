using System.Collections.Generic;
using StorageLib.Entities;

namespace StorageLib.Interfaces
{
    public interface IStorage
    {
        List<User> Users { get; }

        int AddUser(User user);
        void DeleteUser(int id);
        List<int> SearchBy(IComparer<User> comparator, User searchingUser);
    }
}
