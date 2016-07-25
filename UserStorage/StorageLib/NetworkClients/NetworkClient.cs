using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using StorageInterfaces.ISerializers;
using StorageLib.Serializers;

namespace Storage.NetworkClients
{
    public class NetworkClient : IDisposable
    {
        protected NetworkStream stream;

        public NetworkClient(TcpClient client)
        {
            stream = client.GetStream();
        }

        public void Dispose()
        {
            stream.Dispose();
        }

        protected virtual async Task<T> ReadFromStreamAsync<T>(int bufferSize)
        {
            ISerializer<T> serializer = new BsonSerializer<T>();
            List<byte[]> list = new List<byte[]>();
            int resultLength = 0, i = 0;

            list.Add(new byte[bufferSize]);
            var countOfReadBytes = await stream.ReadAsync(list[i], 0, bufferSize);
            resultLength += countOfReadBytes;
            i++;

            while (countOfReadBytes == bufferSize)
            {
                list.Add(new byte[bufferSize]);
                countOfReadBytes = await stream.ReadAsync(list[i], 0, bufferSize);
                resultLength += countOfReadBytes;
                i++;
            }

            byte[] result = new byte[resultLength];

            for (int j = 0; j < list.Count; j++)
            {
                list[i].CopyTo(result, j * bufferSize);
            }

            return serializer.Deserialize(result);
        }

        protected virtual async Task WriteToStreamAsync<T>(T data)
        {
            ISerializer<T> serializer = new BsonSerializer<T>();
            byte[] serializedData = serializer.Serialize(data);

            await stream.WriteAsync(serializedData, 0, serializedData.Length);
        }

        public async Task<T> ReadAsync<T>(int bufferSize)
        {
            return await ReadFromStreamAsync<T>(bufferSize);
        }

        public async void WriteAsync<T>(T data)
        {
            await WriteToStreamAsync(data);
        }
    }
}
