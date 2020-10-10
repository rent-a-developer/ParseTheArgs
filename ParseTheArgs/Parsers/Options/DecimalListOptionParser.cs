using System;
using System.Collections.Generic;
using System.Reflection;
using ParseTheArgs.Errors;

namespace ParseTheArgs.Parsers.Options
{
    /// <summary>
    /// Parses a command line option that accepts one or more <see cref="Decimal" /> values.
    /// </summary>
    public class DecimalListOptionParser : NumericListOptionParser<Decimal>
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="targetProperty">The property where the value of the option will be stored.</param>
        /// <param name="optionName">The name of the option the parser parses.</param>
        /// <exception cref="ArgumentNullException"><paramref name="targetProperty"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="targetProperty"/> does not have the property type <see cref="List{Decimal}"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="optionName"/> is null or an empty string.</exception>
        public DecimalListOptionParser(PropertyInfo targetProperty, String optionName) : base(targetProperty, optionName)
        {
            if (targetProperty.PropertyType != typeof(List<Decimal>))
            {
                throw new ArgumentException($"The given target property has an incompatible property type. Expected type is System.Collections.Generic.List<System.Decimal>, actual type was {targetProperty.PropertyType.FullName}.", nameof(targetProperty));
            }
        }

        /// <summary>
        /// Parses the given value to the desired option value type of the option parser.
        /// </summary>
        /// <param name="optionValue">The option value to parse.</param>
        /// <param name="parseResult">The object where to put parse errors in if a parse error occurred.</param>
        /// <param name="resultValue">The parsed value.</param>
        /// <returns>True if the given option value could be parsed; otherwise false.</returns>
        protected override Boolean TryParseValue(String optionValue, ParseResult parseResult, out Decimal resultValue)
        {
            if (!this.ValueParser.TryParseDecimal(optionValue, this.NumberStyles, this.FormatProvider, out resultValue))
            {
                parseResult.AddError(new OptionValueInvalidFormatError(this.OptionName, optionValue, $"A decimal number in the range from {Decimal.MinValue} to {Decimal.MaxValue}"));
                return false;
            }

            return true;
        }
    }
}