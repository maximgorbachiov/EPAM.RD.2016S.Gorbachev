using System.Threading.Tasks;

namespace StorageInterfaces.INetworkConnections
{
    public interface INetworkIO
    {
        Task<T> ReadAsync<T>(int bufferSize);
        void WriteAsync<T>(T data);
    }
}
