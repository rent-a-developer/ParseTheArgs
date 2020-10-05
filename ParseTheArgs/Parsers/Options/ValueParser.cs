using System;
using System.Globalization;

namespace ParseTheArgs.Parsers.Options
{
    /// <summary>
    /// Parses strings to various types (e.g. DateTime).
    /// </summary>
    internal class ValueParser
    {
        /// <summary>
        /// Converts the specified string representation of a date and time to its <see cref="T:System.DateTime" /> equivalent using the specified format, culture-specific format information and formatting style, and returns a value that indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="value">A string containing a date and time to convert.</param>
        /// <param name="format">The required format of <paramref name="value" />. See the Remarks section for more information.</param>
        /// <param name="formatProvider">An object that supplies culture-specific formatting information about <paramref name="value" />.</param>
        /// <param name="styles">A bitwise combination of one or more enumeration values that indicate the permitted format of <paramref name="value" />.</param>
        /// <param name="result">When this method returns, contains the <see cref="T:System.DateTime" /> value equivalent to the date and time contained in <paramref name="value" />, if the conversion succeeded, or <see cref="F:System.DateTime.MinValue" /> if the conversion failed. The conversion fails if <paramref name="value" /> does not contain a date and time that correspond to the pattern specified in <paramref name="format" />. This parameter is passed uninitialized.</param>
        /// <returns><see langword="true" /> if <paramref name="value" /> was converted successfully; otherwise, <see langword="false" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException"><paramref name="value" /> is an empty string or a string consisting only of white-space characters.</exception>
        public virtual Boolean TryParseDateTime(String value, String? format, IFormatProvider formatProvider, DateTimeStyles styles, out DateTime result)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (String.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Value cannot be an empty string or a string consisting only of white-space characters.", nameof(value));
            }

            if (!String.IsNullOrWhiteSpace(format))
            {
                return DateTime.TryParseExact(value, format, formatProvider, styles, out result);
            }
            else
            {
                return DateTime.TryParse(value, formatProvider, styles, out result);
            }
        }

        /// <summary>
        /// Converts the string representation of a number to its <see cref="T:System.Decimal" /> equivalent using the specified style and culture-specific format. A return value indicates whether the conversion succeeded or failed.
        /// </summary>
        /// <param name="value">The string representation of the number to convert.</param>
        /// <param name="style">A bitwise combination of enumeration values that indicates the permitted format of <paramref name="value" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Number" />.</param>
        /// <param name="formatProvider">An object that supplies culture-specific parsing information about <paramref name="value" />.</param>
        /// <param name="result">When this method returns, contains the <see cref="T:System.Decimal" /> number that is equivalent to the numeric value contained in <paramref name="value" />, if the conversion succeeded, or is zero if the conversion failed. The conversion fails if the <paramref name="value" /> parameter is <see langword="null" /> or <see cref="F:System.String.Empty" />, is not in a format compliant with <paramref name="style" />, or represents a number less than <see cref="F:System.Decimal.MinValue" /> or greater than <see cref="F:System.Decimal.MaxValue" />. This parameter is passed uninitialized; any value originally supplied in <paramref name="result" /> will be overwritten.</param>
        /// <returns><see langword="true" /> if <paramref name="value" /> was converted successfully; otherwise, <see langword="false" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException"><paramref name="value" /> is an empty string or a string consisting only of white-space characters.</exception>
        public virtual Boolean TryParseDecimal(String value, NumberStyles style, IFormatProvider formatProvider, out Decimal result)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (String.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Value cannot be an empty string or a string consisting only of white-space characters.", nameof(value));
            }

            return Decimal.TryParse(value, style, formatProvider, out result);
        }

        /// <summary>
        /// Converts the string representation of the name or numeric value of one or more enumerated constants to an equivalent enumerated object. The operation is case-sensitive. The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enumeration type to which to convert <paramref name="value" />.</typeparam>
        /// <param name="value">The string representation of the enumeration name or underlying value to convert.</param>
        /// <param name="result">When this method returns, <paramref name="result" /> contains an object of type <typeparamref name="TEnum" /> whose value is represented by <paramref name="value" /> if the parse operation succeeds. If the parse operation fails, <paramref name="result" /> contains the default value of the underlying type of <typeparamref name="TEnum" />. Note that this value need not be a member of the <typeparamref name="TEnum" /> enumeration. This parameter is passed uninitialized.</param>
        /// <returns><see langword="true" /> if the <paramref name="value" /> parameter was converted successfully; otherwise, <see langword="false" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException"><paramref name="value" /> is an empty string or a string consisting only of white-space characters.</exception>
        public virtual Boolean TryParseEnum<TEnum>(String value, out TEnum result)
            where TEnum : struct, Enum
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (String.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Value cannot be an empty string or a string consisting only of white-space characters.", nameof(value));
            }

            return Enum.TryParse(value, true, out result);
        }

        /// <summary>
        /// Converts the string representation of a GUID to the equivalent <see cref="T:System.Guid" /> structure, provided that the string is in the specified format.
        /// </summary>
        /// <param name="value">The GUID to convert.</param>
        /// <param name="format">One of the following specifiers that indicates the exact format to use when interpreting <paramref name="value" />: "N", "D", "B", "P", or "X".</param>
        /// <param name="result">The structure that will contain the parsed value. If the method returns <see langword="true" />, <paramref name="result" /> contains a valid <see cref="T:System.Guid" />. If the method returns <see langword="false" />, <paramref name="result" /> equals <see cref="F:System.Guid.Empty" />.</param>
        /// <returns><see langword="true" /> if the parse operation was successful; otherwise, <see langword="false" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException"><paramref name="value" /> is an empty string or a string consisting only of white-space characters.</exception>
        public virtual Boolean TryParseGuid(String value, String? format, out Guid result)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (String.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Value cannot be an empty string or a string consisting only of white-space characters.", nameof(value));
            }

            if (!String.IsNullOrWhiteSpace(format))
            {
                return Guid.TryParseExact(value, format, out result);
            }
            else
            {
                return Guid.TryParse(value, out result);
            }
        }

        /// <summary>
        /// Converts the string representation of a number in a specified style and culture-specific format to its 64-bit signed integer equivalent. A return value indicates whether the conversion succeeded or failed.
        /// </summary>
        /// <param name="value">A string containing a number to convert. The string is interpreted using the style specified by <paramref name="styles" />.</param>
        /// <param name="styles">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="value" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Integer" />.</param>
        /// <param name="formatProvider">An object that supplies culture-specific formatting information about <paramref name="value" />.</param>
        /// <param name="result">When this method returns, contains the 64-bit signed integer value equivalent of the number contained in <paramref name="value" />, if the conversion succeeded, or zero if the conversion failed. The conversion fails if the <paramref name="value" /> parameter is <see langword="null" />  or <see cref="F:System.String.Empty" />, is not in a format compliant with <paramref name="styles" />, or represents a number less than <see cref="F:System.Int64.MinValue" /> or greater than <see cref="F:System.Int64.MaxValue" />. This parameter is passed uninitialized; any value originally supplied in <paramref name="result" /> will be overwritten.</param>
        /// <returns><see langword="true" /> if <paramref name="value" /> was converted successfully; otherwise, <see langword="false" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException"><paramref name="value" /> is an empty string or a string consisting only of white-space characters.</exception>
        public virtual Boolean TryParseInt64(String value, NumberStyles styles, IFormatProvider formatProvider, out Int64 result)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (String.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Value cannot be an empty string or a string consisting only of white-space characters.", nameof(value));
            }

            return Int64.TryParse(value, styles, formatProvider, out result);
        }

        /// <summary>
        /// Converts the string representation of a time interval to its <see cref="T:System.TimeSpan" /> equivalent by using the specified format, culture-specific format information, and styles, and returns a value that indicates whether the conversion succeeded. The format of the string representation must match the specified format exactly.
        /// </summary>
        /// <param name="value">A string that specifies the time interval to convert.</param>
        /// <param name="format">A standard or custom format string that defines the required format of <paramref name="value" />.</param>
        /// <param name="formatProvider">An object that provides culture-specific formatting information.</param>
        /// <param name="styles">One or more enumeration values that indicate the style of <paramref name="value" />.</param>
        /// <param name="result">When this method returns, contains an object that represents the time interval specified by <paramref name="value" />, or <see cref="F:System.TimeSpan.Zero" /> if the conversion failed. This parameter is passed uninitialized.</param>
        /// <returns><see langword="true" /> if <paramref name="value" /> was converted successfully; otherwise, <see langword="false" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException"><paramref name="value" /> is an empty string or a string consisting only of white-space characters.</exception>
        public virtual Boolean TryParseTimeSpan(String value, String? format, IFormatProvider formatProvider, TimeSpanStyles styles, out TimeSpan result)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (String.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Value cannot be an empty string or a string consisting only of white-space characters.", nameof(value));
            }

            if (!String.IsNullOrWhiteSpace(format))
            {
                return TimeSpan.TryParseExact(value, format, formatProvider, styles, out result);
            }
            else
            {
                return TimeSpan.TryParse(value, formatProvider, out result);
            }
        }
    }
}