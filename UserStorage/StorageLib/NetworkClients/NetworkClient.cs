using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using StorageInterfaces.INetworkConnections;
using StorageInterfaces.ISerializers;
using StorageLib.Serializers;
using StorageLib.Services;
using System.IO;

namespace StorageLib.NetworkClients
{
    public class NetworkClient// : INetworkIO
    {
        protected TcpClient client;

        public NetworkClient(TcpClient client)
        {
            this.client = client;
        }

        /*public async Task<T> ReadAsync<T>(int bufferSize)
        {
            return await ReadFromStreamAsync<T>(bufferSize);
        }

        public async void WriteAsync<T>(T data)
        {
            try
            {
                await WriteToStreamAsync(data);
                LogService.Service.TraceInfo($"{ AppDomain.CurrentDomain.FriendlyName } wrote info");
            }
            catch (ObjectDisposedException oDEx)
            {
                LogService.Service.TraceInfo(oDEx.Message);
            }
            catch (NullReferenceException nREx)
            {
                LogService.Service.TraceInfo(nREx.Message);
            }
        }*/

        /*protected virtual async Task<T> ReadFromStreamAsync<T>(int bufferSize)
        {
            ISerializer<T> serializer = new BsonSerializer<T>();
            byte[] data = new byte[bufferSize];
            T message;

            using (var memStream = new MemoryStream())
            {
                int count;
                do
                {
                    count = await stream.ReadAsync(data, 0, data.Length);
                    memStream.Write(data, 0, count);
                }
                while (count == bufferSize);

                LogService.Service.TraceInfo($"{ AppDomain.CurrentDomain.FriendlyName } read info");
                memStream.Position = 0;

                try
                {
                    message = serializer.Deserialize(memStream);
                }
                catch
                {
                    throw new InvalidDataException("Unable to deserialize.");
                }
            }

            return message;
        }

        protected virtual async Task WriteToStreamAsync<T>(T data)
        {
            ISerializer<T> serializer = new BsonSerializer<T>();
            byte[] serializedData = serializer.Serialize(data);

            try
            {
                await stream.WriteAsync(serializedData, 0, serializedData.Length);
            }
            catch (ObjectDisposedException oDEx)
            {
                LogService.Service.TraceInfo(oDEx.Message);
            }
            catch (NullReferenceException nREx)
            {
                LogService.Service.TraceInfo(nREx.Message);
            }
        }*/

        /*protected virtual async Task<T> ReadFromStreamAsync<T>(int bufferSize)
        {
            ISerializer<T> serializer = new BsonSerializer<T>();
            byte[] result = { };

            try
            {
                List<byte[]> list = new List<byte[]>();
                int resultLength = 0, i = 0;

                list.Add(new byte[bufferSize]);
                var countOfReadBytes = await stream.ReadAsync(list[i], 0, bufferSize);
                resultLength += countOfReadBytes;

                while (countOfReadBytes == bufferSize)
                {
                    list.Add(new byte[bufferSize]);
                    countOfReadBytes = await stream.ReadAsync(list[i], 0, bufferSize);
                    resultLength += countOfReadBytes;
                    i++;
                }

                result = new byte[resultLength];

                for (int j = 0; j < list.Count; j++)
                {
                    if (i != j)
                    {
                        int tempResultLength = result.Length;
                        list[j].CopyTo(result, tempResultLength);
                    }
                    else
                    {
                        int temp = i * bufferSize;
                        for (int k = 0; k < resultLength - temp; k++)
                        {
                            result[k + temp] = list[j][k];
                        }
                    }
                }
            }
            catch (ObjectDisposedException oDEx)
            {
                LogService.Service.TraceInfo(oDEx.Message);
            }
            catch (NullReferenceException nREx)
            {
                LogService.Service.TraceInfo(nREx.Message);
            }

            LogService.Service.TraceInfo($"{ AppDomain.CurrentDomain.FriendlyName } read info");
            return serializer.Deserialize(result);
        }*/
    }
}
