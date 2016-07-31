using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using StorageInterfaces.ISerializers;
using StorageLib.Serializers;
using StorageLib.Services;
using System.IO;

namespace Storage.NetworkClients
{
    public static class TcpClientExtension
    {
        public static async Task<T> ReadAsync<T>(this TcpClient client, int bufferSize)
        {
            ISerializer<T> serializer = new BsonSerializer<T>();
            byte[] data = new byte[bufferSize];
            T message;
            var stream = client.GetStream();

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
            }
            catch (NullReferenceException nREx)
            {
                LogService.Service.TraceInfo(nREx.Message);
            }
        }
    }
}
