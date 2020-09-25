using System;
using ParseTheArgs.Errors;

namespace ParseTheArgs.Parsers.Arguments
{
    /// <summary>
    /// Parses a command line argument that accepts one or more <see cref="long" /> values.
    /// </summary>
    /// <typeparam name="TCommandArguments">The type in which the value of the argument (of the command the argument belongs to) will be stored.</typeparam>
    public class Int64ListArgumentParser<TCommandArguments> : NumericListArgumentParser<TCommandArguments, Int64>
    {
        /// <summary>
        /// Parses the given value to the desired argument value type of the argument parser.
        /// </summary>
        /// <param name="argumentValue">The argument value to parse.</param>
        /// <param name="parseResult">The parse result to put parse errors in if a parse error occurred.</param>
        /// <param name="resultValue">The parsed value.</param>
        /// <returns>True if the given argument value could be parsed; otherwise false.</returns>
        protected override Boolean TryParseValue(String argumentValue, ParseResult parseResult, out Int64 resultValue)
        {
            if (!Int64.TryParse(argumentValue, this.NumberStyles, this.FormatProvider, out resultValue))
            {
                parseResult.AddError(new ArgumentValueFormatError(this.ArgumentName, argumentValue, $"An integer in the range from {Int64.MinValue} to {Int64.MaxValue}"));
                return false;
            }

            return true;
        }
    }
}