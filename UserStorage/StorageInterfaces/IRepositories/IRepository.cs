using StorageInterfaces.Entities;

namespace StorageInterfaces.IRepositories
{
    public interface IRepository
    {
        void Save(ServiceState state);
        ServiceState Load();
    }
}
