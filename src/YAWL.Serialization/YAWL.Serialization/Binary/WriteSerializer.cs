using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;

namespace YAWL.Serialization.Binary
{
    public class WriteSerializer : ISerializer
    {
        public BinaryWriter Writer { get; }

        public WriteSerializer(BinaryWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));

            Writer = writer;
        }

        public void Serialize<T>(Expression<Func<T>> get, Action<T> set, T defaultValue = default(T))
        {
            throw new NotImplementedException();
        }

        public void Serialize<T>(Expression<Func<T?>> get, Action<T?> set) where T : struct
        {
            throw new NotImplementedException();
        }

        public void SerializeList<T>(Expression<Func<List<T>>> get, Action<List<T>> set)
        {
            throw new NotImplementedException();
        }

        public void Serialize(Dictionary<string, object> values)
        {
            throw new NotImplementedException();
        }

        public void Serialize(Expression<Func<string>> get, Action<string> set, string defaultValue = default(string))
        {
            var value = get.Compile()();

            string name;

            if (get.Body.NodeType == ExpressionType.Constant)
            {
                name = SerializationConstants.ConstantName;
            }
            else if (get.Body.NodeType == ExpressionType.MemberAccess)
            {
                name = (get.Body as MemberExpression)?.Member.Name;
            }
            else
            {
                name = SerializationConstants.DynamicName;
            }

            Writer.Write(name);
            Writer.Write(value != null);

            if (value != null)
                Writer.Write(value);
        }
    }
}
