using System;
using ParseTheArgs.Errors;

namespace ParseTheArgs.Parsers.Arguments
{
    /// <summary>
    /// Parses a command line argument that accepts a single <see cref="Guid" /> value.
    /// </summary>
    /// <typeparam name="TCommandArguments">The type in which the value of the argument (of the command the argument belongs to) will be stored.</typeparam>
    public class GuidArgumentParser<TCommandArguments> : SingleValueArgumentParser<TCommandArguments, Guid>
    {
        /// <summary>
        /// Defines the format to use when parsing the argument value to a <see cref="Guid" />.
        /// For supported formats see the documentation of <see cref="Guid.Parse(string)" />.
        /// </summary>
        public String GuidFormat { get; set; }

        /// <summary>
        /// Parses the given value to the desired argument value type of the argument parser.
        /// </summary>
        /// <param name="argumentValue">The argument value to parse.</param>
        /// <param name="parseResult">The parse result to put parse errors in if a parse error occurred.</param>
        /// <param name="resultValue">The parsed value.</param>
        /// <returns>True if the given argument value could be parsed; otherwise false.</returns>
        protected override Boolean TryParseValue(String argumentValue, ParseResult parseResult, out Guid resultValue)
        {
            if (!String.IsNullOrEmpty(this.GuidFormat))
            {
                if (!Guid.TryParseExact(argumentValue, this.GuidFormat, out resultValue))
                {
                    parseResult.AddError(new ArgumentValueFormatError(this.ArgumentName, argumentValue, "Value is not a valid Guid"));
                    return false;
                }

                return true;
            }
            else
            {
                if (!Guid.TryParse(argumentValue, out resultValue))
                {
                    parseResult.AddError(new ArgumentValueFormatError(this.ArgumentName, argumentValue, "Value is not a valid Guid"));
                    return false;
                }

                return true;
            }
        }
    }
}