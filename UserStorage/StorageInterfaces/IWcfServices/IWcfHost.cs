using StorageInterfaces.IServices;

namespace StorageInterfaces.IWcfServices
{
    public interface IWcfHost
    {
        void CreateWcfService(IService service, string address);
        void CloseWcfService();
    }
}
