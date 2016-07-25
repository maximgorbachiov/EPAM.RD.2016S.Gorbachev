namespace StorageInterfaces.IStorages
{
    public interface IMasterStorage : IStorage
    {
        void Load();
        void Save();

        void InitializeServices();
    }
}
