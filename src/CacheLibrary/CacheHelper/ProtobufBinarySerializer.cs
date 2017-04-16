using System.IO;
using System.Runtime.Serialization;
using ProtoBuf.Meta;

namespace CacheLibrary.CacheHelper
{
    public class ProtobufBinarySerializer : IBinarySerializer
    {
        public byte[] Serialize<T>(T data)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                RuntimeTypeModel.Default.Serialize(ms, data);
                return ms.ToArray();
            }
        }

        public T Deserialize<T>(byte[] serialized)
        {
            try
            {
                using (MemoryStream memStream = new MemoryStream())
                {
                    memStream.Write(serialized, 0, serialized.Length);
                    memStream.Seek(0, SeekOrigin.Begin);
                    var obj = RuntimeTypeModel.Default.Deserialize(memStream, null, typeof(T));
                    return (T)obj;
                }
            }
            catch (SerializationException e)
            {
                // Wrap in a standard .NET exception.
                throw new SerializationException(e.Message, e);
            }
        }
    }
}