using System;
using System.Globalization;
using ParseTheArgs.Errors;

namespace ParseTheArgs.Parsers.Arguments
{
    /// <summary>
    /// Parses a command line argument that accepts a single <see cref="DateTime" /> value.
    /// </summary>
    /// <typeparam name="TCommandArguments">The type in which the value of the argument (of the command the argument belongs to) will be stored.</typeparam>
    public class DateTimeArgumentParser<TCommandArguments> : SingleValueArgumentParser<TCommandArguments, DateTime>
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public DateTimeArgumentParser()
        {
            this.FormatProvider = CultureInfo.CurrentCulture;
            this.DateTimeStyles = DateTimeStyles.None;
        }

        /// <summary>
        /// Defines the format to use when parsing the argument value to a <see cref="DateTime" />.
        /// For supported formats see the documentation of <see cref="DateTime.Parse(string)" />.
        /// </summary>
        public String DateTimeFormat { get; set; }

        /// <summary>
        /// Defines the date time styles that are permitted when parsing the argument value to a <see cref="DateTime" />.
        /// The default is <see cref="System.Globalization.DateTimeStyles.None" />.
        /// </summary>
        public DateTimeStyles DateTimeStyles { get; set; }

        /// <summary>
        /// Defines the format provider to use to parse the argument value to a <see cref="DateTime" />.
        /// The default is <see cref="CultureInfo.CurrentCulture" />.
        /// </summary>
        public IFormatProvider FormatProvider { get; set; }

        /// <summary>
        /// Parses the given value to the desired argument value type of the argument parser.
        /// </summary>
        /// <param name="argumentValue">The argument value to parse.</param>
        /// <param name="parseResult">The parse result to put parse errors in if a parse error occurred.</param>
        /// <param name="resultValue">The parsed value.</param>
        /// <returns>True if the given argument value could be parsed; otherwise false.</returns>
        protected override Boolean TryParseValue(String argumentValue, ParseResult parseResult, out DateTime resultValue)
        {
            if (!String.IsNullOrEmpty(this.DateTimeFormat))
            {
                if (!DateTime.TryParseExact(argumentValue, this.DateTimeFormat, this.FormatProvider, this.DateTimeStyles, out resultValue))
                {
                    parseResult.AddError(new ArgumentValueFormatError(this.ArgumentName, argumentValue, "A valid DateTime"));
                    return false;
                }

                return true;
            }
            else
            {
                if (!DateTime.TryParse(argumentValue, this.FormatProvider, this.DateTimeStyles, out resultValue))
                {
                    parseResult.AddError(new ArgumentValueFormatError(this.ArgumentName, argumentValue, "A valid DateTime"));
                    return false;
                }

                return true;
            }
        }
    }
}