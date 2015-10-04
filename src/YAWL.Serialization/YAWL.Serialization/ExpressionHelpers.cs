using System;
using System.Linq.Expressions;

namespace YAWL.Serialization
{
    public class ExpressionHelpers
    {
        public static string GetNameFromExpression(Expression<Func<string>> expression)
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