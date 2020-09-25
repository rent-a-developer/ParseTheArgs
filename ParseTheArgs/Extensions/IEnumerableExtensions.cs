using System;
using System.Collections.Generic;

namespace ParseTheArgs.Extensions
{
    /// <summary>
    /// Provides extension methods for the <see cref="IEnumerable{T}" /> type.
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Determines if this instance has the same elements, in the same order, as the given other instance.
        /// The equality of the elements is determined using the (overloaded) <see cref="Object.Equals(Object)" /> method of each element.
        /// </summary>
        /// <typeparam name="T">The type of elements this instance contains.</typeparam>
        /// <param name="source">The first instance to compare.</param>
        /// <param name="other">The second instance to compare.</param>
        /// <returns>True if this instance has the same elements, in the same order, as the given other instance; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="source" /> or <paramref name="other" /> is null.</exception>
        public static Boolean HasSameElementsThan<T>(this IEnumerable<T> source, IEnumerable<T> other)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            if (ReferenceEquals(source, other))
            {
                return true;
            }

            var sourceEnumerator = source.GetEnumerator();
            var otherEnumerator = other.GetEnumerator();

            try
            {
                if (sourceEnumerator == null && otherEnumerator == null)
                {
                    return true;
                }

                if (sourceEnumerator == null || otherEnumerator == null)
                {
                    return false;
                }

                while (true)
                {
                    var sourceMovedNext = sourceEnumerator.MoveNext();
                    var otherMovedNext = otherEnumerator.MoveNext();

                    if (sourceMovedNext != otherMovedNext)
                    {
                        return false;
                    }

                    if (!sourceMovedNext)
                    {
                        break;
                    }

                    if (sourceEnumerator.Current == null && otherEnumerator.Current == null)
                    {
                        continue;
                    }

                    if (sourceEnumerator.Current == null || otherEnumerator.Current == null)
                    {
                        return false;
                    }

                    if (ReferenceEquals(sourceEnumerator.Current, otherEnumerator.Current))
                    {
                        continue;
                    }

                    if (!sourceEnumerator.Current.Equals(otherEnumerator.Current))
                    {
                        return false;
                    }
                }
            }
            finally
            {
                sourceEnumerator?.Dispose();
                otherEnumerator?.Dispose();
            }

            return true;
        }
    }
}