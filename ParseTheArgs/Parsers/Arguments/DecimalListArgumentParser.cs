using System;
using ParseTheArgs.Errors;

namespace ParseTheArgs.Parsers.Arguments
{
    /// <summary>
    /// Parses a command line argument that accepts one or more <see cref="decimal" /> values.
    /// </summary>
    /// <typeparam name="TCommandArguments">The type in which the value of the argument (of the command the argument belongs to) will be stored.</typeparam>
    public class DecimalListArgumentParser<TCommandArguments> : NumericListArgumentParser<TCommandArguments, Decimal>
    {
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