using System.Collections.Generic;
using StorageLib.Entities;
using StorageLib.Interfaces;

namespace StorageLib
{
    public class Storage : IStorage
    {
        private IStrategy strategy;

        public List<User> Users { get { return strategy.Users; } }

        public Storage(IStrategy strategy)
        {
            this.strategy = strategy;
        }

        public int AddUser(User user)
        {
            return strategy.Add(user);
        }

        public void DeleteUser(int id)
        {
            strategy.Delete(id);
        }

        public List<int> SearchBy(IComparer<User> comparator, User searchingUser)
        {
            return strategy.SearchBy(comparator, searchingUser);
        }
    }
}
