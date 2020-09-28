using System;
using System.Globalization;
using System.Linq.Expressions;
using ParseTheArgs.Parsers.Commands;
using ParseTheArgs.Parsers.Options;

namespace ParseTheArgs.Setup.Options
{
    /// <summary>
    /// Represents the configuration of an option that accepts one or more <see cref="DateTime" /> values.
    /// </summary>
    /// <typeparam name="TCommandOptions">The type in which the options of the command the option belongs to will be stored.</typeparam>
    public class DateTimeListOptionSetup<TCommandOptions> : MultiValueOptionSetup<TCommandOptions, DateTimeListOptionParser, DateTimeListOptionSetup<TCommandOptions>, DateTime>
        where TCommandOptions : class
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="commandParser">The parser for the command the option belongs to.</param>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandOptions" /> type in which the option value should be stored.</param>
        public DateTimeListOptionSetup(CommandParser<TCommandOptions> commandParser, LambdaExpression propertyExpression) : base(commandParser, propertyExpression)
        {
        }

        /// <summary>
        /// Sets the format that is accepted when the option values are parsed to <see cref="DateTime" /> values.
        /// For supported formats see the documentation of <see cref="DateTime.Parse(string)" />.
        /// </summary>
        /// <param name="dateTimeFormat">The format that is accepted when the option values are parsed to <see cref="DateTime" /> values.</param>
        /// <returns>A reference to this instance for further configuration.</returns>
        public DateTimeListOptionSetup<TCommandOptions> Format(String dateTimeFormat)
        {
            this.OptionParser.DateTimeFormat = dateTimeFormat;
            return this;
        }

        /// <summary>
        /// Sets the format provider to be used to parse the option values to <see cref="DateTime" /> values.
        /// The default is <see cref="CultureInfo.CurrentCulture" />.
        /// </summary>
        /// <param name="formatProvider">The format provider to be used to parse the option values to <see cref="DateTime" /> values.</param>
        /// <returns>A reference to this instance for further configuration.</returns>
        public DateTimeListOptionSetup<TCommandOptions> FormatProvider(IFormatProvider formatProvider)
        {
            this.OptionParser.FormatProvider = formatProvider;
            return this;
        }

        /// <summary>
        /// Sets the date time styles that are permitted when parsing the option values to <see cref="DateTime" /> values.
        /// The default is <see cref="DateTimeStyles.None" />.
        /// </summary>
        /// <param name="dateTimeStyles">The date time styles that are permitted when parsing the option values to <see cref="DateTime" /> values.</param>
        /// <returns>A reference to this instance for further configuration.</returns>
        public DateTimeListOptionSetup<TCommandOptions> Styles(DateTimeStyles dateTimeStyles)
        {
            this.OptionParser.DateTimeStyles = dateTimeStyles;
            return this;
        }
    }
}