using System;
using System.Globalization;
using System.Linq.Expressions;
using ParseTheArgs.Parsers.Arguments;
using ParseTheArgs.Parsers.Commands;

namespace ParseTheArgs.Setup.Arguments
{
    /// <summary>
    /// Represents the configuration of an argument that accepts a single <see cref="DateTime" /> value.
    /// </summary>
    /// <typeparam name="TCommandArguments">The type in which the arguments of the command the argument belongs to will be stored.</typeparam>
    public class DateTimeArgumentSetup<TCommandArguments> : SingleValueArgumentSetup<TCommandArguments, DateTimeArgumentParser, DateTimeArgumentSetup<TCommandArguments>, DateTime>
        where TCommandArguments : class
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="commandParser">The parser for the command the argument belongs to.</param>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandArguments" /> type in which the argument value should be stored.</param>
        public DateTimeArgumentSetup(CommandParser<TCommandArguments> commandParser, LambdaExpression propertyExpression) : base(commandParser, propertyExpression)
        {
        }

        /// <summary>
        /// Sets the format that is accepted when the argument value is parsed to a <see cref="DateTime" />.
        /// For supported formats see the documentation of <see cref="DateTime.Parse(string)" />.
        /// </summary>
        /// <param name="dateTimeFormat">The format that is accepted when the argument value is parsed to a <see cref="DateTime" />.</param>
        /// <returns>A reference to this instance for further configuration.</returns>
        public DateTimeArgumentSetup<TCommandArguments> Format(String dateTimeFormat)
        {
            this.ArgumentParser.DateTimeFormat = dateTimeFormat;
            return this;
        }

        /// <summary>
        /// Sets the format provider to be used to parse the argument value to a <see cref="DateTime" />.
        /// The default is <see cref="CultureInfo.CurrentCulture" />.
        /// </summary>
        /// <param name="formatProvider">The format provider to be used to parse the argument value to a <see cref="DateTime" />.</param>
        /// <returns>A reference to this instance for further configuration.</returns>
        public DateTimeArgumentSetup<TCommandArguments> FormatProvider(IFormatProvider formatProvider)
        {
            this.ArgumentParser.FormatProvider = formatProvider;
            return this;
        }

        /// <summary>
        /// Sets the date time styles that are permitted when parsing the argument value to a <see cref="DateTime" />.
        /// The default is <see cref="DateTimeStyles.None" />.
        /// </summary>
        /// <param name="dateTimeStyles">The date time styles that are permitted when parsing the argument value to a <see cref="DateTime" />.</param>
        /// <returns>A reference to this instance for further configuration.</returns>
        public DateTimeArgumentSetup<TCommandArguments> Styles(DateTimeStyles dateTimeStyles)
        {
            this.ArgumentParser.DateTimeStyles = dateTimeStyles;
            return this;
        }
    }
}