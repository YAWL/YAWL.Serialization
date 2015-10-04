// Copyright (c) Massive Pixel.  All Rights Reserved.  Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace YAWL.Serialization.Tests
{
    [TestClass]
    public class TestExpressionName
    {
        public string PropName { get; set; }
        private string field;
        private const string constant = "";

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullExpressionThrowsException()
        {
            ExpressionHelpers.GetNameFromExpression(null);
        }

        [TestMethod]
        public void ConstantExpressionReturnsConstant()
        {
            Assert.AreEqual(SerializationConstants.ConstantName,
                ExpressionHelpers.GetNameFromExpression(() => "Const"));
        }

        [TestMethod]
        public void PropertyExpressionReturnsName()
        {
            Assert.AreEqual(nameof(PropName),
                ExpressionHelpers.GetNameFromExpression(() => PropName));
        }

        [TestMethod]
        public void StaticMemberReturnsName()
        {
            Assert.AreEqual(nameof(string.Empty),
                ExpressionHelpers.GetNameFromExpression(() => string.Empty));
        }

        [TestMethod]
        public void FieldReturnsName()
        {
            Assert.AreEqual(nameof(field),
                ExpressionHelpers.GetNameFromExpression(() => field));
        }

        [TestMethod]
        public void ConstantReturnsConstant()
        {
            Assert.AreEqual(SerializationConstants.ConstantName,
                ExpressionHelpers.GetNameFromExpression(() => constant));
        }

        [TestMethod]
        public void ExpressionReturnsDynamic()
        {
            Assert.AreEqual(SerializationConstants.DynamicName,
                ExpressionHelpers.GetNameFromExpression(() => "" + this));
        }
    }
}
