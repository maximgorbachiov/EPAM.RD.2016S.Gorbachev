using StorageInterfaces.Entities;
using StorageInterfaces.IServices;
using System.Collections.Generic;

namespace StorageInterfaces.IStorages
{
    public interface IStorage : IService
    {
        List<User> Users { get; }
    }
}
