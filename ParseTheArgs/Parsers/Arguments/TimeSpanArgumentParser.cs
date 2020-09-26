using System;
using System.Globalization;
using System.Reflection;
using ParseTheArgs.Errors;

namespace ParseTheArgs.Parsers.Arguments
{
    /// <summary>
    /// Parses a command line argument that accepts a single <see cref="TimeSpan" /> value.
    /// </summary>
    public class TimeSpanArgumentParser : SingleValueArgumentParser<TimeSpan>
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="targetProperty">The property where the value of the argument will be stored.</param>
        /// <param name="argumentName">The name of the argument the parser parses.</param>
        public TimeSpanArgumentParser(PropertyInfo targetProperty, ArgumentName argumentName) : base(targetProperty, argumentName)
        {
            this.FormatProvider = CultureInfo.CurrentCulture;
            this.TimeSpanStyles = TimeSpanStyles.None;
        }

        /// <summary>
        /// Defines the format provider to use to parse the argument value to a <see cref="TimeSpan" />.
        /// The default is <see cref="CultureInfo.CurrentCulture" />.
        /// </summary>
        public IFormatProvider FormatProvider { get; set; }

        /// <summary>
        /// Defines the format to use when parsing the argument value to a <see cref="TimeSpan" />.
        /// For supported formats see the documentation of <see cref="TimeSpan.Parse(string)" />.
        /// </summary>
        public String? TimeSpanFormat { get; set; }

        /// <summary>
        /// Defines the date time styles that are permitted when parsing the argument value to a <see cref="TimeSpan" />.
        /// The default is <see cref="System.Globalization.TimeSpanStyles.None" />.
        /// </summary>
        public TimeSpanStyles TimeSpanStyles { get; set; }

        /// <summary>
        /// Parses the given value to the desired argument value type of the argument parser.
        /// </summary>
        /// <param name="argumentValue">The argument value to parse.</param>
        /// <param name="parseResult">The parse result to put parse errors in if a parse error occurred.</param>
        /// <param name="resultValue">The parsed value.</param>
        /// <returns>True if the given argument value could be parsed; otherwise false.</returns>
        protected override Boolean TryParseValue(String argumentValue, ParseResult parseResult, out TimeSpan resultValue)
        {
            if (!String.IsNullOrEmpty(this.TimeSpanFormat))
            {
                if (!TimeSpan.TryParseExact(argumentValue, this.TimeSpanFormat, this.FormatProvider, this.TimeSpanStyles, out resultValue))
                {
                    parseResult.AddError(new ArgumentValueFormatError(this.ArgumentName, argumentValue, "A valid TimeSpan"));
                    return false;
                }

                return true;
            }
            else
            {
                if (!TimeSpan.TryParse(argumentValue, this.FormatProvider, out resultValue))
                {
                    parseResult.AddError(new ArgumentValueFormatError(this.ArgumentName, argumentValue, "A valid TimeSpan"));
                    return false;
                }

                return true;
            }
        }
    }
}