using Microsoft.VisualStudio.TestTools.UnitTesting;
using YAWL.Serialization.Binary;

namespace YAWL.Serialization.Tests
{
    [TestClass]
    public class PersonTests
    {
        [TestMethod]
        public void TestPersonCollector()
        {
            var collector = new WriteSerializer();
            new Person().Serialize(collector);

            Assert.AreEqual(collector.Properties.Count, 5);
        }
    }
}
