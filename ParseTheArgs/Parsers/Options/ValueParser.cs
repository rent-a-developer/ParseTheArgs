using System;
using System.Globalization;

namespace ParseTheArgs.Parsers.Options
{
    internal class ValueParser
    {
        public Boolean TryParseDateTime(String value, String? dateTimeFormat, IFormatProvider formatProvider, DateTimeStyles dateTimeStyles, out DateTime result)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (String.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Value cannot be an empty string or a string consisting only of white-space characters.", nameof(value));
            }

            if (!String.IsNullOrWhiteSpace(dateTimeFormat))
            {
                return DateTime.TryParseExact(value, dateTimeFormat, formatProvider, dateTimeStyles, out result);
            }
            else
            {
                return DateTime.TryParse(value, formatProvider, dateTimeStyles, out result);
            }
        }

        public Boolean TryParseDecimal(String value, NumberStyles numberStyles, IFormatProvider formatProvider, out Decimal result)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (String.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Value cannot be an empty string or a string consisting only of white-space characters.", nameof(value));
            }

            return Decimal.TryParse(value, numberStyles, formatProvider, out result);
        }

        public Boolean TryParseEnum<TEnum>(String value, out TEnum result)
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

        public Boolean TryParseGuid(String value, String? guidFormat, out Guid result)
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

        public Boolean TryParseInt64(String value, NumberStyles numberStyles, IFormatProvider formatProvider, out Int64 result)
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

        public Boolean TryParseTimeSpan(String value, String? timeSpanFormat, IFormatProvider formatProvider, TimeSpanStyles timeSpanStyles, out TimeSpan result)
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
