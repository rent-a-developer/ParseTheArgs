using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using ParseTheArgs.Errors;

namespace ParseTheArgs.Parsers.Options
{
    /// <summary>
    /// Parses a command line option that accepts one or more <see cref="DateTime" /> values.
    /// </summary>
    public class DateTimeListOptionParser : MultiValueOptionParser<DateTime>
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="targetProperty">The property where the value of the option will be stored.</param>
        /// <param name="optionName">The name of the option the parser parses.</param>
        /// <exception cref="ArgumentNullException"><paramref name="targetProperty"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="targetProperty"/> does not have the property type <see cref="List{DateTime}"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="optionName"/> is null or an empty string.</exception>
        public DateTimeListOptionParser(PropertyInfo targetProperty, String optionName) : base(targetProperty, optionName)
        {
            if (targetProperty == null)
            {
                throw new ArgumentNullException(nameof(targetProperty));
            }

            if (String.IsNullOrEmpty(optionName))
            {
                throw new ArgumentException("Value cannot be null or an empty string.", nameof(optionName));
            }

            if (targetProperty.PropertyType != typeof(List<DateTime>))
            {
                throw new ArgumentException($"The given target property has an incompatible property type. Expected type is System.Collections.Generic.List<DateTime>, actual type was {targetProperty.PropertyType.FullName}.", nameof(targetProperty));
            }

            this.FormatProvider = CultureInfo.CurrentCulture;
            this.DateTimeStyles = DateTimeStyles.None;
        }

        /// <summary>
        /// Defines the format to use when parsing the option value to a <see cref="DateTime" />.
        /// For supported formats see the documentation of <see cref="DateTime.Parse(string)" />.
        /// </summary>
        public virtual String? DateTimeFormat { get; set; }

        /// <summary>
        /// Defines the date time styles that are permitted when parsing the option value to a <see cref="DateTime" />.
        /// The default is <see cref="System.Globalization.DateTimeStyles.None" />.
        /// </summary>
        public virtual DateTimeStyles DateTimeStyles { get; set; }

        /// <summary>
        /// Defines the format provider to use to parse the option value to a <see cref="DateTime" />.
        /// The default is <see cref="CultureInfo.CurrentCulture" />.
        /// </summary>
        public virtual IFormatProvider FormatProvider { get; set; }

        /// <summary>
        /// Parses the given value to the desired option value type of the option parser.
        /// </summary>
        /// <param name="optionValue">The option value to parse.</param>
        /// <param name="parseResult">The parse result to put parse errors in if a parse error occurred.</param>
        /// <param name="resultValue">The parsed value.</param>
        /// <returns>True if the given option value could be parsed; otherwise false.</returns>
        protected override Boolean TryParseValue(String optionValue, ParseResult parseResult, out DateTime resultValue)
        {
            if (!String.IsNullOrEmpty(this.DateTimeFormat))
            {
                if (!DateTime.TryParseExact(optionValue, this.DateTimeFormat, this.FormatProvider, this.DateTimeStyles, out resultValue))
                {
                    parseResult.AddError(new OptionValueInvalidFormatError(this.OptionName, optionValue, "A valid DateTime"));
                    return false;
                }

                return true;
            }
            else
            {
                if (!DateTime.TryParse(optionValue, this.FormatProvider, this.DateTimeStyles, out resultValue))
                {
                    parseResult.AddError(new OptionValueInvalidFormatError(this.OptionName, optionValue, "A valid DateTime"));
                    return false;
                }

                return true;
            }
        }
    }
}