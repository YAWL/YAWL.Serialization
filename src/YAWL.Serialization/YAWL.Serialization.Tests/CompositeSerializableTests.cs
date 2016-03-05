using Microsoft.VisualStudio.TestTools.UnitTesting;
using YAWL.Serialization.Binary;

namespace YAWL.Serialization.Tests
{
    public class Serializable1 : ISerializable
    {
        public string Name { get; set; }

        public void Serialize(ISerializer serializer)
        {
            serializer.Serialize(() => Name, v => Name = v);
        }
    }

    public class Serializable2 : ISerializable
    {
        public Serializable1 Serializable1 { get; set; }

        public void Serialize(ISerializer serializer)
        {
            serializer.Serialize(() => Serializable1, v => Serializable1 = v);
        }
    }

    [TestClass]
    public class CompositeSerializableTests
    {
        [TestMethod]
        public void TestCompositeSerialization()
        {
            var collector = new WriteSerializer();
            new Serializable2
            {
                Serializable1 = new Serializable1
                {
                    Name = "John"
                }
            }.Serialize(collector);

            Assert.AreEqual(1, collector.Properties.Count);
        }
    }
}
