using System;
using System.Linq;
using System.Collections.Generic;
using StorageLib.Entities;
using StorageLib.Interfaces;

namespace StorageLib.Strategies
{
    public abstract class Strategy : IStrategy
    {
        protected List<User> users = new List<User>();

        public List<User> Users
        {
            get
            {
                List<User> newUsers = new List<User>();

                foreach (var user in users)
                {
                    newUsers.Add((User)user.Clone());
                }

                return newUsers;
            }
        }

        public abstract int Add(User user);
        public abstract void Delete(int id);

        public Strategy(List<User> users)
        {
            if (users != null)
            {
                this.users = (users.Select(user => (User)(user.Clone()))).ToList();
            }
        }

        public virtual List<int> SearchBy(IComparer<User> comparer, User searchingUser)
        {
            if (comparer == null)
            {
                throw new NullReferenceException(nameof(comparer));
            }

            if (searchingUser == null)
            {
                throw new NullReferenceException(nameof(searchingUser));
            }

            List<int> resultUsers = new List<int>();

            foreach(var user in users)
            {
                if (comparer.Compare(user, searchingUser) == 0)
                {
                    resultUsers.Add(user.Id);
                }
            }

            return resultUsers;
        }
    }
}
