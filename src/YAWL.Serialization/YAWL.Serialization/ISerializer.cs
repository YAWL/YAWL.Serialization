using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace YAWL.Serialization
{
    public interface ISerializer
    {
        /// <summary>
        /// Serialize any value. If T implements IBinarySerializable, it will recursively
        /// serialize subobjects.
        /// </summary>
        void Serialize<T>(Expression<Func<T>> get, Action<T> set, T defaultValue = default(T));

        /// <summary>
        /// Special handling for nullable types.
        /// </summary>
        void Serialize<T>(Expression<Func<T?>> get, Action<T?> set) where T : struct;

        /// <summary>
        /// List serialization has different name due to ambiguity.
        /// </summary>
        void SerializeList<T>(Expression<Func<List<T>>> get, Action<List<T>> set);

        /// <summary>
        /// Serialize dictionary of values.
        /// </summary>
        /// <param name="values"></param>
        void Serialize(Dictionary<string, object> values);

        /// <summary>
        /// Serialize string.
        /// </summary>
        void Serialize(Expression<Func<string>> get, Action<string> set, string defaultValue = default(string));
    }
}
