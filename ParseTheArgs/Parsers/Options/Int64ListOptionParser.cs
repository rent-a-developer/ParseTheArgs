using System;
using System.Collections.Generic;
using System.Reflection;
using ParseTheArgs.Errors;

namespace ParseTheArgs.Parsers.Options
{
    /// <summary>
    /// Parses a command line option that accepts one or more <see cref="Int64" /> values.
    /// </summary>
    public class Int64ListOptionParser : NumericListOptionParser<Int64>
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="targetProperty">The property where the value of the option will be stored.</param>
        /// <param name="optionName">The name of the option the parser parses.</param>
        /// <exception cref="ArgumentNullException"><paramref name="targetProperty"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="targetProperty"/> does not have the property type <see cref="List{Int64}"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="optionName"/> is null or an empty string.</exception>
        public Int64ListOptionParser(PropertyInfo targetProperty, String optionName) : base(targetProperty, optionName)
        {
            if (targetProperty == null)
            {
                throw new ArgumentNullException(nameof(targetProperty));
            }

            if (String.IsNullOrEmpty(optionName))
            {
                throw new ArgumentException("Value cannot be null or an empty string.", nameof(optionName));
            }

            if (targetProperty.PropertyType != typeof(List<Int64>))
            {
                throw new ArgumentException($"The given target property has an incompatible property type. Expected type is System.Collections.Generic.List<System.Int64>, actual type was {targetProperty.PropertyType.FullName}.", nameof(targetProperty));
            }
        }

        /// <summary>
        /// Parses the given value to the desired option value type of the option parser.
        /// </summary>
        /// <param name="optionValue">The option value to parse.</param>
        /// <param name="parseResult">The parse result to put parse errors in if a parse error occurred.</param>
        /// <param name="resultValue">The parsed value.</param>
        /// <returns>True if the given option value could be parsed; otherwise false.</returns>
        protected override Boolean TryParseValue(String optionValue, ParseResult parseResult, out Int64 resultValue)
        {
            if (!this.ValueParser.TryParseInt64(optionValue, this.NumberStyles, this.FormatProvider, out resultValue))
            {
                parseResult.AddError(new OptionValueInvalidFormatError(this.OptionName, optionValue, $"An integer in the range from {Int64.MinValue} to {Int64.MaxValue}"));
                return false;
            }

            return true;
        }
    }
}