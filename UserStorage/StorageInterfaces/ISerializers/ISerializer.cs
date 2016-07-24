namespace StorageInterfaces.ISerializers
{
    public interface ISerializer<T>
    {
        byte[] Serialize(T obj);
        T Deserialize(byte[] obj);
    }
}
