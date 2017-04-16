using CacheLibrary.CacheHelper;
using NUnit.Framework;

namespace CacheLibrary.Tests
{
    [TestFixture]
    public class ProtobufBinarySerializerBasicTests
    {
        private IBinarySerializer binarySerializer;

        [SetUp]
        public void Setup()
        {
            binarySerializer = new ProtobufBinarySerializer();
        }

        [Test]
        public void serialize_primitive_reference_type()
        {
            var data = binarySerializer.Serialize("primitive.type");
            Assert.IsNotNull(data);
        }

        [Test]
        public void deserialize_primitive_reference_type()
        {
            var data = binarySerializer.Serialize("primitive.type");
            var obj = binarySerializer.Deserialize<string>(data);
            Assert.AreEqual(obj, "primitive.type");
        }

        [Test]
        public void serialize_primitive_value_type()
        {
            var data = binarySerializer.Serialize(10);
            Assert.IsNotNull(data);
        }

        [Test]
        public void deserialize_primitive_value_type()
        {
            var data = binarySerializer.Serialize(10);
            var obj = binarySerializer.Deserialize<int>(data);
            Assert.AreEqual(obj, 10);
        }



    }
}