// Copyright (c) Massive Pixel.  All Rights Reserved.  Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System;
using System.IO;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YAWL.Serialization.Binary;

namespace YAWL.Serialization.Tests
{
    [TestClass]
    public class TestSimpleBinaryWrite
    {
        public string TestProperty { get; } = "Property";
        private string TestField = "Field";
        private const string TestConstant = "Constant";
        private string TestFunction() => "Function";

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullWriterException()
        {
            var serializer = new WriteSerializer(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullGet()
        {
            Expression<Func<string>> get = null;
            TestWriteRead(w => w.Serialize(get, null), r => { });
        }

        [TestMethod]
        public void WriteProperty()
        {
            TestWriteRead(
                s =>
                {
                    s.Serialize(() => TestProperty, null);
                }, r =>
                {
                    Assert.AreEqual(nameof(TestProperty), r.ReadString());
                    Assert.IsTrue(r.ReadBoolean());
                    Assert.AreEqual(TestProperty, r.ReadString());
                });
        }

        [TestMethod]
        public void WritePropertyTwice()
        {
            TestWriteRead(
                s =>
                {
                    s.Serialize(() => TestProperty, null);
                    s.Serialize(() => TestProperty, null);
                }, r =>
                {
                    for (var i = 0; i < 2; ++i)
                    {
                        Assert.AreEqual(nameof(TestProperty), r.ReadString());
                        Assert.IsTrue(r.ReadBoolean());
                        Assert.AreEqual(TestProperty, r.ReadString());
                    }
                });
        }

        [TestMethod]
        public void WriteField()
        {
            TestWriteRead(
                s =>
                {
                    s.Serialize(() => TestField, null);
                }, r =>
                {
                    Assert.AreEqual(nameof(TestField), r.ReadString());
                    Assert.IsTrue(r.ReadBoolean());
                    Assert.AreEqual(TestField, r.ReadString());
                });
        }

        [TestMethod]
        public void WriteConstant()
        {
            TestWriteRead(
                s =>
                {
                    s.Serialize(() => TestConstant, null);
                }, r =>
                {
                    Assert.AreEqual(SerializationConstants.ConstantName, r.ReadString());
                    Assert.IsTrue(r.ReadBoolean());
                    Assert.AreEqual(TestConstant, r.ReadString());
                });
        }

        [TestMethod]
        public void WriteFunction()
        {
            TestWriteRead(
                s =>
                {
                    s.Serialize(() => TestFunction(), null);
                }, r =>
                {
                    Assert.AreEqual(SerializationConstants.DynamicName, r.ReadString());
                    Assert.IsTrue(r.ReadBoolean());
                    Assert.AreEqual(TestFunction(), r.ReadString());
                });
        }

        [TestMethod]
        public void WriteExpression()
        {
            TestWriteRead(
                s =>
                {
                    s.Serialize(() => string.Empty + "Expression", null);
                }, r =>
                {
                    Assert.AreEqual(SerializationConstants.DynamicName, r.ReadString());
                    Assert.IsTrue(r.ReadBoolean());
                    Assert.AreEqual(string.Empty + "Expression", r.ReadString());
                });
        }

        [TestMethod]
        public void WriteNullString()
        {
            TestWriteRead(
                s => s.Serialize(() => null, null),
                r =>
                {
                    Assert.AreEqual(SerializationConstants.ConstantName, r.ReadString());
                    Assert.IsFalse(r.ReadBoolean());
                });
        }

        [TestMethod]
        public void WriteString()
        {
            const string helloWorld = "Hello world";

            TestWriteRead(
                s => s.Serialize(() => helloWorld, null),
                r =>
                {
                    Assert.AreEqual(SerializationConstants.ConstantName, r.ReadString());
                    Assert.IsTrue(r.ReadBoolean());
                    Assert.AreEqual(helloWorld, r.ReadString());
                });
        }

        [TestMethod]
        public void WriteTwoStrings()
        {
            TestWriteRead(
                s =>
                {
                    s.Serialize(() => "Hello", null);
                    s.Serialize(() => "World", null);
                }, r =>
                {
                    Assert.AreEqual(SerializationConstants.ConstantName, r.ReadString());
                    Assert.AreEqual(true, r.ReadBoolean());
                    Assert.AreEqual("Hello", r.ReadString());
                    Assert.AreEqual(SerializationConstants.ConstantName, r.ReadString());
                    Assert.AreEqual(true, r.ReadBoolean());
                    Assert.AreEqual("World", r.ReadString());
                });
        }

        private static void TestWriteRead(Action<WriteSerializer> write, Action<BinaryReader> read)
        {
            using (var ms = new MemoryStream())
            {
                var writer = new BinaryWriter(ms);
                var writeSerializer = new WriteSerializer(writer);

                write(writeSerializer);

                ms.Seek(0, SeekOrigin.Begin);
                var reader = new BinaryReader(ms);

                read(reader);

                writer.Dispose();
                reader.Dispose();
            }
        }
    }
}
