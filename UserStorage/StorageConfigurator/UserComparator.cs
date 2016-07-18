﻿using System.Collections.Generic;
using StorageLib.Entities;

namespace StorageConfigurator
{
    internal class UserComparator : IComparer<User>
    {
        public int Compare(User x, User y)
        {
            return (x.Gender == y.Gender) ? 0 : 1;
        }
    }
}