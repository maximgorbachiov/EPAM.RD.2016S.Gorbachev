using System;
using System.Collections.Generic;
using System.Linq;
using StorageLib.Entities;
using StorageLib.EventArguments;
using StorageLib.Interfaces;

namespace Storage.Storages
{
    public class SlaveStorage : IStorage
    {
        public List<User> Users { get; }

        public SlaveStorage(IMasterStorage masterStorage)
        {
            Users = masterStorage.Users;
            masterStorage.OnAddUser += AddEventHandler;
            masterStorage.OnDeleteUser += DeleteEventHandler;
        }

        public int AddUser(User user)
        {
            throw new NotSupportedException("This method can't be called from slave. Try to use master for it");
        }

        public void DeleteUser(int id)
        {
            throw new NotSupportedException("This method can't be called from slave. Try to use master for it");
        }

        public List<int> SearchBy(IComparer<User> comparator, User searchingUser)
        {
            if (comparator == null)
            {
                throw new NullReferenceException(nameof(comparator));
            }

            if (searchingUser == null)
            {
                throw new NullReferenceException(nameof(searchingUser));
            }

            return Users.Where(user => comparator.Compare(user, searchingUser) == 0)
                .Select(user => user.Id).ToList();
        }

        protected virtual void AddEventHandler(object sender, AddEventArgs e)
        {
            if (e.User != null)
            {
                Users.Add(e.User);
            }
        }

        protected virtual void DeleteEventHandler(object sender, DeleteEventArgs e)
        {
            User user;
            if ((user = Users.FirstOrDefault(u => u.Id == e.Id)) != null)
            {
                Users.Remove(user);
            }
        }
    }
}
