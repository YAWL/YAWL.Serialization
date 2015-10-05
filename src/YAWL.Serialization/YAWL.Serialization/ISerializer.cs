// Copyright (c) Massive Pixel.  All Rights Reserved.  Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System;
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
    }
}
