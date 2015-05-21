using System.IO;
using ProtoBuf;

namespace Core.Messaging.Serilaizers
{
    public class ProtobufSerializer : ISerializer
    {
        public byte[] MessageToBytes<T>(T message)
        {
            if (message == null)
            {
                return null;
            }

            using (MemoryStream ms = new MemoryStream())
            {
                Serializer.Serialize(ms, message);
                var bytes = ms.ToArray();

                return bytes;
            }            
        }

        public T BytesToMessage<T>(byte[] bytes)
        {
            if (bytes == null)
            {
                return default(T);
            }

            using (MemoryStream ms = new MemoryStream(bytes))
            {
                return Serializer.Deserialize<T>(ms);
            }            
        }
    }
}