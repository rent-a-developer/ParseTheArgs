using System;
using System.Reflection;
using ParseTheArgs.Errors;

namespace ParseTheArgs.Parsers.Arguments
{
    /// <summary>
    /// Parses a command line argument that accepts one or more <see cref="Decimal" /> values.
    /// </summary>
    public class DecimalListArgumentParser : NumericListArgumentParser<Decimal>
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="targetProperty">The property where the value of the argument will be stored.</param>
        /// <param name="argumentName">The name of the argument the parser parses.</param>
        public DecimalListArgumentParser(PropertyInfo targetProperty, ArgumentName argumentName) : base(targetProperty, argumentName)
        {
        }

        /// <summary>
        /// Parses the given value to the desired argument value type of the argument parser.
        /// </summary>
        /// <param name="argumentValue">The argument value to parse.</param>
        /// <param name="parseResult">The parse result to put parse errors in if a parse error occurred.</param>
        /// <param name="resultValue">The parsed value.</param>
        /// <returns>True if the given argument value could be parsed; otherwise false.</returns>
        protected override Boolean TryParseValue(String argumentValue, ParseResult parseResult, out Decimal resultValue)
        {
            if (!Decimal.TryParse(argumentValue, this.NumberStyles, this.FormatProvider, out resultValue))
            {
                parseResult.AddError(new ArgumentValueFormatError(this.ArgumentName, argumentValue, $"A decimal number in the range from {Decimal.MinValue} to {Decimal.MaxValue}"));
                return false;
            }

            return true;
        }
    }
}