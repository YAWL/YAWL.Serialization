// Copyright (c) Massive Pixel.  All Rights Reserved.  Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using YAWL.Serialization.Utilities;
using PropertyInfo = YAWL.Serialization.Utilities.PropertyInfo;

namespace YAWL.Serialization.Binary
{
    public class WriteSerializer : ISerializer
    {
        private readonly Dictionary<string, PropertyInfo> properties = new Dictionary<string, PropertyInfo>();
        public ReadOnlyDictionary<string, PropertyInfo> Properties { get; }

        public WriteSerializer()
        {
            Properties = new ReadOnlyDictionary<string, PropertyInfo>(properties);
        }

        public void Serialize<T>(Expression<Func<T>> get, Action<T> set, T defaultValue = default(T))
        {
            if (get == null)
                throw new ArgumentNullException();

            AddProperty(PropertyTypeHelpers.GetTypeFromType<T>(), get);
        }

        private void AddProperty<T>(PropertyType propertyType, Expression<Func<T>> getExpression)
        {
            if (getExpression == null)
                throw new ArgumentNullException(nameof(getExpression));

            var name = ExpressionHelpers.GetNameFromExpression(getExpression);
            var value = getExpression.Compile()();

            if ((name == SerializationConstants.ConstantName ||
                name == SerializationConstants.DynamicName) &&
                properties.ContainsKey(name))
            {
                var baseName = name;
                var i = 0;
                while (properties.ContainsKey(baseName + ++i))
                {
                }
                name = baseName + i;
            }

            properties.Add(name, new PropertyInfo(propertyType, name, value));
        }
    }
}
