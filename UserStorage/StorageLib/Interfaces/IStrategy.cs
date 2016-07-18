using System.Collections.Generic;
using StorageLib.Entities;

namespace StorageLib.Interfaces
{
    public interface IStrategy
    {
        List<User> Users { get; }

        int Add(User user);
        void Delete(int id);
        List<int> SearchBy(IComparer<User> comparer, User searchingUser);
    }
}
