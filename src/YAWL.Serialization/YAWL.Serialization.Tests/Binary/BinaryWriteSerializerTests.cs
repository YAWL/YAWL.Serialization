// Copyright (c) Massive Pixel.  All Rights Reserved.  Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YAWL.Serialization.Binary;
using YAWL.Serialization.Utilities;

namespace YAWL.Serialization.Tests.Binary
{
    public class StringClass : ISerializable
    {
        public string Name { get; set; }

        public void Serialize(ISerializer serializer)
        {
            serializer.Serialize(() => Name, v => Name = v);
        }
    }

    [TestClass]
    public class BinaryWriteSerializerTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullSerializerArgumentThrowsException()
        {
            new BinaryWriteSerializer().Serialize(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullBinaryWriterArgumentThrowsException()
        {
            new BinaryWriteSerializer().Serialize(new Person(), null);
        }

        [TestMethod]
        public void NullStringShouldWritePropertyNameAndFalse()
        {
            TestWrite(new StringClass(), reader =>
            {
                Assert.AreEqual(1, reader.ReadInt32());
                Assert.AreEqual((int)PropertyType.String, reader.ReadInt32());
                Assert.AreEqual(nameof(StringClass.Name), reader.ReadString());
                Assert.IsFalse(reader.ReadBoolean());
            });
        }

        [TestMethod]
        public void NullStringShouldWritePropertyNameTrueAndValue()
        {
            TestWrite(new StringClass
            {
                Name = "John"
            }, reader =>
            {
                Assert.AreEqual(1, reader.ReadInt32());
                Assert.AreEqual((int)PropertyType.String, reader.ReadInt32());
                Assert.AreEqual(nameof(StringClass.Name), reader.ReadString());
                Assert.IsTrue(reader.ReadBoolean());
                Assert.AreEqual("John", reader.ReadString());
            });
        }

        private void TestWrite(ISerializable serializable, Action<BinaryReader> check)
        {
            using (var ms = new MemoryStream())
            using (var writer = new BinaryWriter(ms))
            {
                var serializer = new BinaryWriteSerializer();
                serializer.Serialize(serializable, writer);

                ms.Seek(0, SeekOrigin.Begin);

                using (var reader = new BinaryReader(ms))
                {
                    check(reader);
                }
            }
        }
    }
}
