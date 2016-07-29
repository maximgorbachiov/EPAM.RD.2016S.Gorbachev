using StorageInterfaces.IServices;

namespace StorageInterfaces.IWcfServices
{
    public interface IInitializeServiceContract
    {
        void Initialize(IService service);
    }
}
