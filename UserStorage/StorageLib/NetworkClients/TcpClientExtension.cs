using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using StorageInterfaces.ISerializers;
using StorageLib.Serializers;
using StorageLib.Services;

namespace Storage.NetworkClients
{
    public static class TcpClientExtension
    {
        public static async Task<T> ReadAsync<T>(this TcpClient client, int bufferSize)
        {
            ISerializer<T> serializer = new BsonSerializer<T>();
            byte[] result;

            try
            {
                var stream = client.GetStream();
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
                LogService.Service.TraceInfo(oDEx.InnerException.Message);
                LogService.Service.TraceInfo(oDEx.ObjectName);
                LogService.Service.TraceInfo(oDEx.StackTrace);
                throw;
            }
            catch (NullReferenceException nREx)
            {
                LogService.Service.TraceInfo(nREx.Message);
                LogService.Service.TraceInfo(nREx.StackTrace);
                throw;
            }

            LogService.Service.TraceInfo($"{ AppDomain.CurrentDomain.FriendlyName } read info");
            return serializer.Deserialize(result);
        }

        public static async Task WriteAsync<T>(this TcpClient client, T data)
        {
            ISerializer<T> serializer = new BsonSerializer<T>();
            byte[] serializedData = serializer.Serialize(data);

            try
            {
                var stream = client.GetStream();
                await stream.WriteAsync(serializedData, 0, serializedData.Length);
            }
            catch (ObjectDisposedException oDEx)
            {
                LogService.Service.TraceInfo(oDEx.Message);
                LogService.Service.TraceInfo(oDEx.InnerException.Message);
                LogService.Service.TraceInfo(oDEx.ObjectName);
                LogService.Service.TraceInfo(oDEx.StackTrace);
                throw;
            }
            catch (NullReferenceException nREx)
            {
                LogService.Service.TraceInfo(nREx.Message);
                LogService.Service.TraceInfo(nREx.StackTrace);
                throw;
            }
        }
    }
}
