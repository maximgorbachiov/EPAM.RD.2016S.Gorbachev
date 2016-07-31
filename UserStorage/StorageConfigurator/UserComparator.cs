using System;
using System.Collections.Generic;
using StorageInterfaces.Entities;

namespace StorageConfigurator
{
    [Serializable]
    internal class UserComparator : IComparer<SavedUser>
    {
        public int Compare(SavedUser x, SavedUser y)
        {
            return (x.Gender == y.Gender) ? 0 : 1;
        }
    }
}