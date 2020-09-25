using System;
using System.Reflection;

namespace ParseTheArgs.Extensions
{
    /// <summary>
    /// Provides extension methods for the <see cref="PropertyInfo" /> type.
    /// </summary>
    public static class PropertyInfoExtensions
    {
        /// <summary>
        /// Determines if the property the given PropertyInfo represents has a public setter.
        /// </summary>
        /// <param name="propertyInfo">The PropertyInfo to check.</param>
        /// <returns>True if the property the given PropertyInfo represents has a public setter; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="propertyInfo" /> is null.</exception>
        public static Boolean HasPublicSetter(this PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
            {
                throw new ArgumentNullException(nameof(propertyInfo));
            }

            return propertyInfo.SetMethod != null && propertyInfo.SetMethod.IsPublic;
        }
    }
}