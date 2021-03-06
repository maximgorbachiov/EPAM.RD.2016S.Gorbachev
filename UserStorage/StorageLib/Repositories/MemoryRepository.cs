﻿using StorageInterfaces.Entities;
using StorageInterfaces.IRepositories;

namespace StorageLib.Repositories
{
    public class MemoryRepository : IRepository
    {
        private ServiceState state = new ServiceState();

        public ServiceState Load()
        {
            return state;
        }

        public void Save(ServiceState state)
        {
            this.state = state;
        }
    }
}
