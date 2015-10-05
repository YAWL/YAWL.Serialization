// Copyright (c) Massive Pixel.  All Rights Reserved.  Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System.Collections.Generic;

namespace YAWL.Serialization.Tests
{
    class Related : ISerializable
    {
        public string Name { get; set; }

        public void Serialize(ISerializer serializer)
        {
            serializer.Serialize(() => Name, v => Name = v);
        }
    }

    class Person : ISerializable
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public bool? IsEmployed { get; set; }
        public List<decimal> Paychecks { get; set; }
        public List<Related> Related { get; set; }

        public Person()
        {
            Name = "John";
            Age = 20;
            IsEmployed = true;
            Paychecks = new List<decimal> { 1, 2, 3 };
            Related = new List<Related>
            {
                new Related(),
                new Related()
            };
        }

        public void Serialize(ISerializer serializer)
        {
            serializer.Serialize(() => Name, v => Name = v);
            serializer.Serialize(() => Age, v => Age = v);
            serializer.Serialize(() => IsEmployed, v => IsEmployed = v);
            serializer.Serialize(() => Paychecks, v => Paychecks = v);
            serializer.Serialize(() => Related, v => Related = v);
        }
    }
}
