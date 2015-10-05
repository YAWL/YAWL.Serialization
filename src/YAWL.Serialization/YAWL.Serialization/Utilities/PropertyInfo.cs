// Copyright (c) Massive Pixel.  All Rights Reserved.  Licensed under the MIT License (MIT). See License.txt in the project root for license information.

namespace YAWL.Serialization.Utilities
{
    public enum PropertyType
    {
        String,
        Nullable,
        Struct,
        Enum,
        Serializable,
        List
    }

    public class PropertyInfo
    {
        public PropertyType PropertyType { get; }
        public string Name { get; }
        public object Value { get; }

        public PropertyInfo(PropertyType propertyType, string name, object value)
        {
            PropertyType = propertyType;
            Name = name;
            Value = value;
        }

        public override string ToString()
        {
            return $"PropertyType: {PropertyType}, Name: {Name}, Value: {Value}";
        }

        protected bool Equals(PropertyInfo other)
        {
            return PropertyType == other.PropertyType &&
                   string.Equals(Name, other.Name) &&
                   Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((PropertyInfo)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int)PropertyType;
                hashCode = (hashCode * 397) ^ (Name?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (Value?.GetHashCode() ?? 0);
                return hashCode;
            }
        }
    }
}