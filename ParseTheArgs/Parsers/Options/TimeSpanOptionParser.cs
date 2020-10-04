using System;
using System.Globalization;
using System.Reflection;
using ParseTheArgs.Errors;

namespace ParseTheArgs.Parsers.Options
{
    /// <summary>
    /// Parses a command line option that accepts a single <see cref="TimeSpan" /> value.
    /// </summary>
    public class TimeSpanOptionParser : SingleValueOptionParser<TimeSpan>
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="targetProperty">The property where the value of the option will be stored.</param>
        /// <param name="optionName">The name of the option the parser parses.</param>
        public TimeSpanOptionParser(PropertyInfo targetProperty, String optionName) : base(targetProperty, optionName)
        {
            this.FormatProvider = CultureInfo.CurrentCulture;
            this.TimeSpanStyles = TimeSpanStyles.None;
        }

        /// <summary>
        /// Defines the format provider to use to parse the option value to a <see cref="TimeSpan" />.
        /// The default is <see cref="CultureInfo.CurrentCulture" />.
        /// </summary>
        public virtual IFormatProvider FormatProvider { get; set; }

        /// <summary>
        /// Defines the format to use when parsing the option value to a <see cref="TimeSpan" />.
        /// For supported formats see the documentation of <see cref="TimeSpan.Parse(string)" />.
        /// </summary>
        public virtual String? TimeSpanFormat { get; set; }

        /// <summary>
        /// Defines the date time styles that are permitted when parsing the option value to a <see cref="TimeSpan" />.
        /// The default is <see cref="System.Globalization.TimeSpanStyles.None" />.
        /// </summary>
        public virtual TimeSpanStyles TimeSpanStyles { get; set; }

        /// <summary>
        /// Parses the given value to the desired option value type of the option parser.
        /// </summary>
        /// <param name="optionValue">The option value to parse.</param>
        /// <param name="parseResult">The parse result to put parse errors in if a parse error occurred.</param>
        /// <param name="resultValue">The parsed value.</param>
        /// <returns>True if the given option value could be parsed; otherwise false.</returns>
        protected override Boolean TryParseValue(String optionValue, ParseResult parseResult, out TimeSpan resultValue)
        {
            if (!this.ValueParser.TryParseTimeSpan(optionValue, this.TimeSpanFormat, this.FormatProvider, this.TimeSpanStyles, out resultValue))
            {
                var errorMessage = !String.IsNullOrEmpty(this.TimeSpanFormat) ? $"A valid time interval in the format '{this.TimeSpanFormat}'" : "A valid time interval";
                parseResult.AddError(new OptionValueInvalidFormatError(this.OptionName, optionValue, errorMessage));
                return false;
            }

            return true;
        }
    }
}