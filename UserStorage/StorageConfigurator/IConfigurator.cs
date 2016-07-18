using StorageLib.Interfaces;

namespace StorageConfigurator
{
    public interface IConfigurator
    {
        IStorage Load();
        void Save();
    }
}
