using System;
using System.Globalization;
using ParseTheArgs.Parsers.Commands;
using ParseTheArgs.Parsers.Options;

namespace ParseTheArgs.Setup.Options
{
    /// <summary>
    /// Represents the configuration of an option that accepts a single <see cref="decimal" /> value.
    /// </summary>
    /// <typeparam name="TCommandOptions">The type in which the options of the command the option belongs to will be stored.</typeparam>
    public class DecimalOptionSetup<TCommandOptions> : SingleValueOptionSetup<TCommandOptions, DecimalOptionParser, DecimalOptionSetup<TCommandOptions>, Decimal>
        where TCommandOptions : class
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="commandParser">The parser for the command the option belongs to.</param>
        /// <param name="optionParser">The parser for the option.</param>
        /// <exception cref="ArgumentNullException"><paramref name="commandParser"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="optionParser"/> is null.</exception>
        public DecimalOptionSetup(CommandParser<TCommandOptions> commandParser, DecimalOptionParser optionParser) : base(commandParser, optionParser)
        {
        }

        /// <summary>
        /// Sets the format provider to be used to parse the option value to a <see cref="Decimal" />.
        /// The default is <see cref="CultureInfo.CurrentCulture" />.
        /// </summary>
        /// <param name="formatProvider">The format provider to be used to parse the option value to a <see cref="Decimal" />.</param>
        /// <returns>A reference to this instance for further configuration.</returns>
        public DecimalOptionSetup<TCommandOptions> FormatProvider(IFormatProvider formatProvider)
        {
            this.optionParser.FormatProvider = formatProvider;
            return this;
        }

        /// <summary>
        /// Sets the number styles that are permitted when parsing the option value to a <see cref="Decimal" />.
        /// The default is <see cref="NumberStyles.Any" />.
        /// </summary>
        /// <param name="numberStyles">The number styles that are permitted when parsing the option value to a <see cref="Decimal" />.</param>
        /// <returns>A reference to this instance for further configuration.</returns>
        public DecimalOptionSetup<TCommandOptions> Styles(NumberStyles numberStyles)
        {
            this.optionParser.NumberStyles = numberStyles;
            return this;
        }
    }
}