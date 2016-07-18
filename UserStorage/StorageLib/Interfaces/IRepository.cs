using StorageLib.Entities;

namespace StorageLib.Interfaces
{
    public interface IRepository
    {
        void Save(ServiceState state);
        ServiceState Load();
    }
}
