using System;
using System.Globalization;
using ParseTheArgs.Parsers.Commands;
using ParseTheArgs.Parsers.Options;

namespace ParseTheArgs.Setup.Options
{
    /// <summary>
    /// Represents the configuration of an option that accepts one or more <see cref="decimal" /> values.
    /// </summary>
    /// <typeparam name="TCommandOptions">The type in which the options of the command the option belongs to will be stored.</typeparam>
    public class DecimalListOptionSetup<TCommandOptions> : MultiValueOptionSetup<TCommandOptions, DecimalListOptionParser, DecimalListOptionSetup<TCommandOptions>, Decimal>
        where TCommandOptions : class
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="commandParser">The parser for the command the option belongs to.</param>
        /// <param name="optionParser">The parser for the option.</param>
        /// <exception cref="ArgumentNullException"><paramref name="commandParser"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="optionParser"/> is null.</exception>
        public DecimalListOptionSetup(CommandParser<TCommandOptions> commandParser, DecimalListOptionParser optionParser) : base(commandParser, optionParser)
        {
        }

        /// <summary>
        /// Sets the format provider to be used to parse the option value to <see cref="Decimal" /> values.
        /// The default is <see cref="CultureInfo.CurrentCulture" />.
        /// </summary>
        /// <param name="formatProvider">The format provider to be used to parse the option value to <see cref="Decimal" /> values.</param>
        /// <returns>A reference to this instance for further configuration.</returns>
        public DecimalListOptionSetup<TCommandOptions> FormatProvider(IFormatProvider formatProvider)
        {
            this.optionParser.FormatProvider = formatProvider;
            return this;
        }

        /// <summary>
        /// Sets the number styles that are permitted when parsing the option value to <see cref="Decimal" /> values.
        /// The default is <see cref="NumberStyles.Any" />.
        /// </summary>
        /// <param name="numberStyles">The number styles that are permitted when parsing the option value to <see cref="Decimal" /> values.</param>
        /// <returns>A reference to this instance for further configuration.</returns>
        public DecimalListOptionSetup<TCommandOptions> Styles(NumberStyles numberStyles)
        {
            this.optionParser.NumberStyles = numberStyles;
            return this;
        }
    }
}