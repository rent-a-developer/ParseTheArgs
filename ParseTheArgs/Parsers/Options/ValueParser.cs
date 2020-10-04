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
        /// <param name="result">When this method returns, contains the <see cref="T:System.DateTime" /> value equivalent to the date and time contained in <paramref name="value" />, if the conversion succeeded, or <see cref="F:System.DateTime.MinValue" /> if the conversion failed. The conversion fails if <paramref name="value"/> does not contain a date and time that correspond to the pattern specified in <paramref name="format" />. This parameter is passed uninitialized.</param>
        /// <returns><see langword="true" /> if <paramref name="value" /> was converted successfully; otherwise, <see langword="false" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/> is an empty string or a string consisting only of white-space characters.</exception>
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
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/> is an empty string or a string consisting only of white-space characters.</exception>
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

        public virtual Boolean TryParseGuid(String value, String? guidFormat, out Guid result)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (String.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Value cannot be an empty string or a string consisting only of white-space characters.", nameof(value));
            }

            if (!String.IsNullOrWhiteSpace(guidFormat))
            {
                return Guid.TryParseExact(value, guidFormat, out result);
            }
            else
            {
                return Guid.TryParse(value, out result);
            }
        }

        public virtual Boolean TryParseInt64(String value, NumberStyles numberStyles, IFormatProvider formatProvider, out Int64 result)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (String.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Value cannot be an empty string or a string consisting only of white-space characters.", nameof(value));
            }

            return Int64.TryParse(value, numberStyles, formatProvider, out result);
        }

        public virtual Boolean TryParseTimeSpan(String value, String? timeSpanFormat, IFormatProvider formatProvider, TimeSpanStyles timeSpanStyles, out TimeSpan result)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (String.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Value cannot be an empty string or a string consisting only of white-space characters.", nameof(value));
            }

            if (!String.IsNullOrWhiteSpace(timeSpanFormat))
            {
                return TimeSpan.TryParseExact(value, timeSpanFormat, formatProvider, timeSpanStyles, out result);
            }
            else
            {
                return TimeSpan.TryParse(value, formatProvider, out result);
            }
        }
    }
}
