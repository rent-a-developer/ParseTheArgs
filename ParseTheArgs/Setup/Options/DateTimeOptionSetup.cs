using System;
using System.Globalization;
using ParseTheArgs.Parsers.Commands;
using ParseTheArgs.Parsers.Options;

namespace ParseTheArgs.Setup.Options
{
    /// <summary>
    /// Represents the configuration of an option that accepts a single <see cref="DateTime" /> value.
    /// </summary>
    /// <typeparam name="TCommandOptions">The type in which the options of the command the option belongs to will be stored.</typeparam>
    public class DateTimeOptionSetup<TCommandOptions> : SingleValueOptionSetup<TCommandOptions, DateTimeOptionParser, DateTimeOptionSetup<TCommandOptions>, DateTime>
        where TCommandOptions : class
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="commandParser">The parser for the command the option belongs to.</param>
        /// <param name="optionParser">The parser for the option.</param>
        /// <exception cref="ArgumentNullException"><paramref name="commandParser" /> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="optionParser" /> is null.</exception>
        public DateTimeOptionSetup(CommandParser<TCommandOptions> commandParser, DateTimeOptionParser optionParser) : base(commandParser, optionParser)
        {
        }

        /// <summary>
        /// Sets the format that is accepted when the option value is parsed to a <see cref="DateTime" />.
        /// For supported formats see the documentation of <see cref="DateTime.Parse(string)" />.
        /// </summary>
        /// <param name="dateTimeFormat">The format that is accepted when the option value is parsed to a <see cref="DateTime" />.</param>
        /// <returns>A reference to this instance for further configuration.</returns>
        public DateTimeOptionSetup<TCommandOptions> Format(String dateTimeFormat)
        {
            this.optionParser.DateTimeFormat = dateTimeFormat;
            return this;
        }

        /// <summary>
        /// Sets the format provider to be used to parse the option value to a <see cref="DateTime" />.
        /// The default is <see cref="CultureInfo.CurrentCulture" />.
        /// </summary>
        /// <param name="formatProvider">The format provider to be used to parse the option value to a <see cref="DateTime" />.</param>
        /// <returns>A reference to this instance for further configuration.</returns>
        public DateTimeOptionSetup<TCommandOptions> FormatProvider(IFormatProvider formatProvider)
        {
            this.optionParser.FormatProvider = formatProvider;
            return this;
        }

        /// <summary>
        /// Sets the date time styles that are permitted when parsing the option value to a <see cref="DateTime" />.
        /// The default is <see cref="DateTimeStyles.None" />.
        /// </summary>
        /// <param name="dateTimeStyles">The date time styles that are permitted when parsing the option value to a <see cref="DateTime" />.</param>
        /// <returns>A reference to this instance for further configuration.</returns>
        public DateTimeOptionSetup<TCommandOptions> Styles(DateTimeStyles dateTimeStyles)
        {
            this.optionParser.DateTimeStyles = dateTimeStyles;
            return this;
        }
    }
}