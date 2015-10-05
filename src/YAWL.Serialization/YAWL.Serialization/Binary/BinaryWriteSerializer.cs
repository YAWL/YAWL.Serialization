// Copyright (c) Massive Pixel.  All Rights Reserved.  Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System;
using System.IO;
using YAWL.Serialization.Utilities;

namespace YAWL.Serialization.Binary
{
    public class BinaryWriteSerializer
    {
        public void Serialize(ISerializable serializable, BinaryWriter writer)
        {
            if (serializable == null)
                throw new ArgumentNullException(nameof(serializable));
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));

            var collector = new WriteSerializer();
            serializable.Serialize(collector);

            writer.Write(collector.Properties.Count);
            foreach (var property in collector.Properties)
                WriteProperty(writer, property.Value);

            writer.Flush();
        }

        private void WriteProperty(BinaryWriter writer, PropertyInfo property)
        {
            writer.Write((int)property.PropertyType);

            switch (property.PropertyType)
            {
                case PropertyType.String:
                    writer.Write(property.Name);
                    var s = (string)property.Value;
                    writer.Write(s != null);
                    if (s != null)
                        writer.Write(s);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        public T Deserialize<T>(ISerializable serializable, BinaryReader reader)
            where T : ISerializable, new()
        {
            if (serializable == null)
                throw new ArgumentNullException(nameof(ISerializable));
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            var t = new T();
            return t;
        }
    }
}
