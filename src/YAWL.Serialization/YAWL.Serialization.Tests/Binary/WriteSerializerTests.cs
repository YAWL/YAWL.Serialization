// Copyright (c) Massive Pixel.  All Rights Reserved.  Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YAWL.Serialization.Binary;
using YAWL.Serialization.Utilities;

namespace YAWL.Serialization.Tests.Binary
{
    [TestClass]
    public class WriteSerializerTests
    {
        public string TestProperty { get; } = "Property";
        public string AnotherProperty { get; } = "Property";
        private readonly string testField = "Field";
        private const string TestConstant = "Constant";
        private static string TestFunction() => "Function";

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullGet()
        {
            TestWriteRead(w => w.Serialize((Expression<Func<string>>)null, null), r => { });
        }

        [TestMethod]
        public void WriteProperty()
        {
            var serializer = new WriteSerializer();
            serializer.Serialize(() => TestProperty, null);

            var properties = serializer.Properties;
            Assert.AreEqual(1, properties.Count);
            Assert.AreEqual(nameof(TestProperty), properties.Keys.First());

            var propertyInfo = properties[properties.Keys.First()];
            Assert.AreEqual(PropertyType.String, propertyInfo.PropertyType);
            Assert.AreEqual(nameof(TestProperty), propertyInfo.Name);
            Assert.AreEqual(TestProperty, propertyInfo.Value);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WriteSamePropertyTwiceThrowsException()
        {
            var serializer = new WriteSerializer();
            serializer.Serialize(() => TestProperty, null);
            serializer.Serialize(() => TestProperty, null);
        }

        [TestMethod]
        public void WriteField()
        {
            var serializer = new WriteSerializer();
            serializer.Serialize(() => testField, null);

            var properties = serializer.Properties;
            Assert.AreEqual(1, properties.Count);
            Assert.AreEqual(nameof(testField), properties.Keys.First());

            var propertyInfo = properties[properties.Keys.First()];
            Assert.AreEqual(PropertyType.String, propertyInfo.PropertyType);
            Assert.AreEqual(nameof(testField), propertyInfo.Name);
            Assert.AreEqual(testField, propertyInfo.Value);
        }

        [TestMethod]
        public void WriteConstant()
        {
            var serializer = new WriteSerializer();
            serializer.Serialize(() => TestConstant, null);

            var properties = serializer.Properties;

            Assert.AreEqual(1, properties.Count);
            Assert.AreEqual(SerializationConstants.ConstantName, properties.Keys.First());

            var propertyInfo = properties[properties.Keys.First()];
            Assert.AreEqual(PropertyType.String, propertyInfo.PropertyType);
            Assert.AreEqual(SerializationConstants.ConstantName, propertyInfo.Name);
            Assert.AreEqual(TestConstant, propertyInfo.Value);
        }

        [TestMethod]
        public void WriteFunction()
        {
            var serializer = new WriteSerializer();
            serializer.Serialize(() => TestFunction(), null);

            var properties = serializer.Properties;

            Assert.AreEqual(1, properties.Count);
            Assert.AreEqual(SerializationConstants.DynamicName, properties.Keys.First());

            var propertyInfo = properties[properties.Keys.First()];
            Assert.AreEqual(PropertyType.String, propertyInfo.PropertyType);
            Assert.AreEqual(SerializationConstants.DynamicName, propertyInfo.Name);
            Assert.AreEqual(TestFunction(), propertyInfo.Value);
        }

        [TestMethod]
        public void WriteExpression()
        {
            var serializer = new WriteSerializer();
            serializer.Serialize(() => string.Empty + "Expression", null);

            var properties = serializer.Properties;

            Assert.AreEqual(1, properties.Count);
            Assert.AreEqual(SerializationConstants.DynamicName, properties.Keys.First());

            var propertyInfo = properties[properties.Keys.First()];
            Assert.AreEqual(PropertyType.String, propertyInfo.PropertyType);
            Assert.AreEqual(SerializationConstants.DynamicName, propertyInfo.Name);
            Assert.AreEqual(string.Empty + "Expression", propertyInfo.Value);
        }

        [TestMethod]
        public void WriteNullString()
        {
            var serializer = new WriteSerializer();
            serializer.Serialize(() => (string)null, null);

            var properties = serializer.Properties;
            Assert.AreEqual(1, properties.Count);
            Assert.AreEqual(SerializationConstants.ConstantName, properties.Keys.First());

            var propertyInfo = properties[properties.Keys.First()];
            Assert.AreEqual(PropertyType.String, propertyInfo.PropertyType);
            Assert.AreEqual(SerializationConstants.ConstantName, propertyInfo.Name);
            Assert.AreEqual(null, propertyInfo.Value);
        }

        [TestMethod]
        public void WriteString()
        {
            const string helloWorld = "Hello world";

            var serializer = new WriteSerializer();
            serializer.Serialize(() => helloWorld, null);

            var properties = serializer.Properties;
            Assert.AreEqual(1, properties.Count);
            Assert.AreEqual(SerializationConstants.ConstantName, properties.Keys.First());

            var propertyInfo = properties[properties.Keys.First()];
            Assert.AreEqual(PropertyType.String, propertyInfo.PropertyType);
            Assert.AreEqual(SerializationConstants.ConstantName, propertyInfo.Name);
            Assert.AreEqual(helloWorld, propertyInfo.Value);
        }

        [TestMethod]
        public void WriteTwoStrings()
        {
            var serializer = new WriteSerializer();
            serializer.Serialize(() => "Hello", null);
            serializer.Serialize(() => "World", null);

            var properties = serializer.Properties;
            Assert.AreEqual(2, properties.Count);
            Assert.IsTrue(properties.ContainsKey(SerializationConstants.ConstantName));
            Assert.IsTrue(properties.ContainsKey(SerializationConstants.ConstantName + "1"));
        }

        private static void TestWriteRead(Action<WriteSerializer> write, Action<BinaryReader> read)
        {
            using (var ms = new MemoryStream())
            {
                var writer = new BinaryWriter(ms);
                var writeSerializer = new WriteSerializer();

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
