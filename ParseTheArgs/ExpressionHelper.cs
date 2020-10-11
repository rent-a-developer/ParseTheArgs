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
        /// <summary>
        /// Extracts the property from a linq expression that accesses a property.
        /// </summary>
        /// <param name="propertyExpression">The expression to extract the property from.</param>
        /// <returns>The property that was extracted from the given expression.</returns>
        /// <exception cref="ArgumentException"><paramref name="propertyExpression"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="propertyExpression"/> is not a access call to a property.</exception>
        /// <exception cref="ArgumentException"><paramref name="propertyExpression"/> is accessing a property that does not have a public setter.</exception>
        /// <example>
        /// <code>
        /// class Item
        /// {
        ///     public Int32 PropertyA { get; set; }
        /// }
        ///
        /// Expression{Func{Item, Object}} expression = ((item) => item.PropertyA)
        /// ExpressionHelper.GetPropertyFromPropertyExpression(expression);  // returns same as typeof(Item).GetProperty("PropertyA") would.
        /// </code>
        /// </example>
        public static PropertyInfo GetPropertyFromPropertyExpression(LambdaExpression propertyExpression)
        {
            if (propertyExpression == null)
            {
                throw new ArgumentNullException(nameof(propertyExpression));
            }

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
