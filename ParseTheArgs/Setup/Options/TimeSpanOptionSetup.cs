using System;
using System.Globalization;
using ParseTheArgs.Parsers.Commands;
using ParseTheArgs.Parsers.Options;

namespace ParseTheArgs.Setup.Options
{
    /// <summary>
    /// Represents the configuration of an option that accepts a single <see cref="TimeSpan" /> value.
    /// </summary>
    /// <typeparam name="TCommandOptions">The type in which the options of the command the option belongs to will be stored.</typeparam>
    public class TimeSpanOptionSetup<TCommandOptions> : SingleValueOptionSetup<TCommandOptions, TimeSpanOptionParser, TimeSpanOptionSetup<TCommandOptions>, TimeSpan>
        where TCommandOptions : class
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="commandParser">The parser for the command the option belongs to.</param>
        /// <param name="optionParser">The parser for the option.</param>
        /// <exception cref="ArgumentNullException"><paramref name="commandParser"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="optionParser"/> is null.</exception>
        public TimeSpanOptionSetup(CommandParser<TCommandOptions> commandParser, TimeSpanOptionParser optionParser) : base(commandParser, optionParser)
        {
        }

        /// <summary>
        /// Sets the format that is accepted when the option value is parsed to a <see cref="TimeSpan" />.
        /// For supported formats see the documentation of <see cref="TimeSpan.Parse(string)" />.
        /// </summary>
        /// <param name="timeSpanFormat">The format that is accepted when the option value is parsed to a <see cref="TimeSpan" />.</param>
        /// <returns>A reference to this instance for further configuration.</returns>
        public TimeSpanOptionSetup<TCommandOptions> Format(String timeSpanFormat)
        {
            this.optionParser.TimeSpanFormat = timeSpanFormat;
            return this;
        }

        /// <summary>
        /// Sets the format provider to be used to parse the option value to a <see cref="TimeSpan" />.
        /// The default is <see cref="CultureInfo.CurrentCulture" />.
        /// </summary>
        /// <param name="formatProvider">The format provider to be used to parse the option value to a <see cref="TimeSpan" />.</param>
        /// <returns>A reference to this instance for further configuration.</returns>
        public TimeSpanOptionSetup<TCommandOptions> FormatProvider(IFormatProvider formatProvider)
        {
            this.optionParser.FormatProvider = formatProvider;
            return this;
        }

        /// <summary>
        /// Sets the time span styles that are permitted when parsing the option value to a <see cref="TimeSpan" />.
        /// The default is <see cref="TimeSpanStyles.None" />.
        /// </summary>
        /// <param name="timeSpanStyles">The time span styles that are permitted when parsing the option value to a <see cref="TimeSpan" />.</param>
        /// <returns>A reference to this instance for further configuration.</returns>
        public TimeSpanOptionSetup<TCommandOptions> Styles(TimeSpanStyles timeSpanStyles)
        {
            this.optionParser.TimeSpanStyles = timeSpanStyles;
            return this;
        }
    }
}