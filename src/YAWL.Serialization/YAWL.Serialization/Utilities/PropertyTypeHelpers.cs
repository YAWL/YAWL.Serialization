// Copyright (c) Massive Pixel.  All Rights Reserved.  Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Reflection;

namespace YAWL.Serialization.Utilities
{
    public static class PropertyTypeHelpers
    {
        public static PropertyType GetTypeFromType<T>()
        {
            if (typeof(T) == typeof(string))
                return PropertyType.String;

            var typeInfo = typeof(T).GetTypeInfo();

            if (typeof(ISerializable).GetTypeInfo().IsAssignableFrom(typeInfo))
                return PropertyType.Serializable;

            if (typeInfo.IsGenericType &&
                typeof(List<>).GetTypeInfo().IsAssignableFrom(typeInfo.GetGenericTypeDefinition().GetTypeInfo()))
                return PropertyType.List;

            if (typeInfo.IsEnum) return PropertyType.Enum;
            if (typeInfo.IsClass) return PropertyType.Nullable;
            if (typeInfo.IsGenericType &&
                typeInfo.GetGenericTypeDefinition() == typeof(Nullable<>))
                return PropertyType.Nullable;

            if (typeInfo.IsValueType)
                return PropertyType.Struct;

            throw new InvalidOperationException("Unrecognized type");
        }
    }
}