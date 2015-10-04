// Copyright (c) Massive Pixel.  All Rights Reserved.  Licensed under the MIT License (MIT). See License.txt in the project root for license information.

namespace YAWL.Serialization
{
    public interface ISerializable
    {
        void Serialize(ISerializer serializer);
    }
}