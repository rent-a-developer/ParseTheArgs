using System;
using System.Linq.Expressions;
using System.Reflection;
using ParseTheArgs.Extensions;

namespace ParseTheArgs
{
    /// <summary>
    /// Provides utility functions to deal with expressions.
    /// </summary>
    public static class ExpressionHelper
    {
        // TODO: Document
        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyExpression"></param>
        /// <returns></returns>
        public static PropertyInfo GetPropertyFromPropertyExpression(LambdaExpression propertyExpression)
        {
            MemberExpression? memberExpression = null;

            if (propertyExpression.Body is MemberExpression expressionBody)
            {
                memberExpression = expressionBody;
            }

            if (
                propertyExpression.Body is UnaryExpression unaryExpression &&
                unaryExpression.NodeType == ExpressionType.Convert &&
                unaryExpression.Operand is MemberExpression operand
            )
            {
                memberExpression = operand;
            }

            PropertyInfo? propertyInfo = null;

            if (memberExpression != null)
            {
                propertyInfo = memberExpression.Member as PropertyInfo;
            }

            if (propertyInfo == null || !propertyInfo.HasPublicSetter())
            {
                throw new ArgumentException("The given property expression is invalid. It must be an expression that returns the value of a property and the property must have a public setter.", nameof(propertyExpression));
            }

            return propertyInfo;
        }
    }
}
