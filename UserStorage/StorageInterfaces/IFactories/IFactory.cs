namespace StorageInterfaces.IFactories
{
    public interface IFactory
    {
        T CreateDependency<T>();
    }
}
