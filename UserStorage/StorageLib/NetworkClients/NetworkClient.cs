using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using StorageInterfaces.INetworkConnections;
using StorageInterfaces.ISerializers;
using StorageLib.Serializers;
using StorageLib.Services;

namespace Storage.NetworkClients
{
    public class NetworkClient : INetworkIO, IDisposable
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

        public async Task<T> ReadAsync<T>(int bufferSize)
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
        }

        protected virtual async Task<T> ReadFromStreamAsync<T>(int bufferSize)
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
        }
    }
}
