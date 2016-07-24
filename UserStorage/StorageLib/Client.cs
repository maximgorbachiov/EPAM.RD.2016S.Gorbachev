using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Storage
{
    public class Client : IDisposable
    {
        private NetworkStream stream;

        public Client(TcpClient client)
        {
            stream = client.GetStream();
        }

        public void Dispose()
        {
            stream.Dispose();
        }

        async Task<byte[]> ReadFromStreamAsync(int bufferSize)
        {
            List<byte[]> list = new List<byte[]>();
            int countOfReadBytes = 0, resultLength = 0, i = 0;

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

            return result;
        }

        public async Task<byte[]> ProcessAsync()
        {
            return await ReadFromStreamAsync(1024);
            /*var action = (ActionEnum)BitConverter.ToInt16(actionBuffer, 0);
            switch (action)
            {
                // логика в зависимости от кода команды
            }*/
        }
    }
}
