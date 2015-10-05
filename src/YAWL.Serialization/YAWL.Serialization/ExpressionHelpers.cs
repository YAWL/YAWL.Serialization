// Copyright (c) Massive Pixel.  All Rights Reserved.  Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System;
using System.Linq.Expressions;

namespace YAWL.Serialization
{
    public class ExpressionHelpers
    {
        public static string GetNameFromExpression<T>(Expression<Func<T>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (expression.Body.NodeType == ExpressionType.Constant)
            {
                return SerializationConstants.ConstantName;
            }

            if (expression.Body.NodeType == ExpressionType.MemberAccess)
            {
                return (expression.Body as MemberExpression)?.Member.Name;
            }

            return SerializationConstants.DynamicName;
        }
    }
}