using System;
using System.Reflection;
using ParseTheArgs.Errors;

namespace ParseTheArgs.Parsers.Options
{
    /// <summary>
    /// Parses a command line option that accepts a single <see cref="Int64" /> value.
    /// </summary>
    public class Int64OptionParser : NumericOptionParser<Int64>
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="targetProperty">The property where the value of the option will be stored.</param>
        /// <param name="optionName">The name of the option the parser parses.</param>
        public Int64OptionParser(PropertyInfo targetProperty, String optionName) : base(targetProperty, optionName)
        {
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
            if (!Int64.TryParse(optionValue, this.NumberStyles, this.FormatProvider, out resultValue))
            {
                parseResult.AddError(new OptionValueFormatError(this.OptionName, optionValue, $"An integer in the range from {Int64.MinValue} to {Int64.MaxValue}"));
                return false;
            }

            return true;
        }
    }
}