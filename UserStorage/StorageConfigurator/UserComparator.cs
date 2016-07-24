using System.Collections.Generic;
using StorageInterfaces.Entities;
using System;

namespace StorageConfigurator
{
    [Serializable]
    internal class UserComparator : IComparer<User>
    {
        public int Compare(User x, User y)
        {
            return (x.Gender == y.Gender) ? 0 : 1;
        }
    }
}