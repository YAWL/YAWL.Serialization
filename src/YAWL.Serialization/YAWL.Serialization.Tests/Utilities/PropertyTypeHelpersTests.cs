using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YAWL.Serialization.Utilities;

namespace YAWL.Serialization.Tests.Utilities
{
    [TestClass]
    public class PropertyTypeHelpersTests
    {
        [TestMethod]
        public void StringForString()
        {
            Assert.AreEqual(PropertyType.String, PropertyTypeHelpers.GetTypeFromType<string>());
        }

        [TestMethod]
        public void StructForInt()
        {
            Assert.AreEqual(PropertyType.Struct, PropertyTypeHelpers.GetTypeFromType<int>());
        }

        [TestMethod]
        public void NullableForNullable()
        {
            Assert.AreEqual(PropertyType.Nullable, PropertyTypeHelpers.GetTypeFromType<int?>());
        }

        [TestMethod]
        public void EnumForEnum()
        {
            Assert.AreEqual(PropertyType.Enum, PropertyTypeHelpers.GetTypeFromType<PropertyType>());
        }

        [TestMethod]
        public void SerializableForSerializable()
        {
            Assert.AreEqual(PropertyType.Serializable, PropertyTypeHelpers.GetTypeFromType<Person>());
        }

        [TestMethod]
        public void ListForList()
        {
            Assert.AreEqual(PropertyType.List, PropertyTypeHelpers.GetTypeFromType<List<int>>());
        }

        [TestMethod]
        public void NullableForClass()
        {
            Assert.AreEqual(PropertyType.Nullable, PropertyTypeHelpers.GetTypeFromType<PropertyTypeHelpersTests>());
        }
    }
}
