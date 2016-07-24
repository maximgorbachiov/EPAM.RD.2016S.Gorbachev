namespace StorageInterfaces.IStorages
{
    public interface ISlaveStorage : IStorage
    {
        void NotifyMasterAboutSlaveCreate();
    }
}
