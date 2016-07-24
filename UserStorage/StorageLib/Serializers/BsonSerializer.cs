using StorageInterfaces.ISerializers;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace StorageLib.Serializers
{
    public class BsonSerializer<T> : ISerializer<T>
    {
        public T Deserialize(byte[] obj)
        {
            T result;

            MemoryStream stream = new MemoryStream(obj);
            using (BsonReader reader = new BsonReader(stream))
            {
                JsonSerializer serializer = new JsonSerializer();
                result = serializer.Deserialize<T>(reader);
            }

            return result;
        }

        public byte[] Serialize(T obj)
        {
            MemoryStream stream = new MemoryStream();
            using (BsonWriter writer = new BsonWriter(stream))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(writer, obj);
            }
            return stream.ToArray();
        }
    }
}
