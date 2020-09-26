using System;
using System.Globalization;
using System.Linq.Expressions;
using ParseTheArgs.Parsers.Arguments;
using ParseTheArgs.Parsers.Commands;

namespace ParseTheArgs.Setup.Arguments
{
    /// <summary>
    /// Represents the configuration of an argument that accepts one or more <see cref="TimeSpan" /> values.
    /// </summary>
    /// <typeparam name="TCommandArguments">The type in which the arguments of the command the argument belongs to will be stored.</typeparam>
    public class TimeSpanListArgumentSetup<TCommandArguments> : MultiValueArgumentSetup<TCommandArguments, TimeSpanListArgumentParser, TimeSpanListArgumentSetup<TCommandArguments>, TimeSpan>
        where TCommandArguments : class
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="commandParser">The parser for the command the argument belongs to.</param>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandArguments" /> type in which the argument value should be stored.</param>
        public TimeSpanListArgumentSetup(CommandParser<TCommandArguments> commandParser, LambdaExpression propertyExpression) : base(commandParser, propertyExpression)
        {
        }

        /// <summary>
        /// Sets the format that is accepted when the argument values are parsed to <see cref="TimeSpan" /> values.
        /// For supported formats see the documentation of <see cref="TimeSpan.Parse(string)" />.
        /// </summary>
        /// <param name="timeSpanFormat">The format that is accepted when the argument values are parsed to <see cref="TimeSpan" /> values.</param>
        /// <returns>A reference to this instance for further configuration.</returns>
        public TimeSpanListArgumentSetup<TCommandArguments> Format(String timeSpanFormat)
        {
            this.ArgumentParser.TimeSpanFormat = timeSpanFormat;
            return this;
        }

        /// <summary>
        /// Sets the format provider to be used to parse the argument values to <see cref="TimeSpan" /> values.
        /// The default is <see cref="CultureInfo.CurrentCulture" />.
        /// </summary>
        /// <param name="formatProvider">The format provider to be used to parse the argument values to <see cref="TimeSpan" /> values.</param>
        /// <returns>A reference to this instance for further configuration.</returns>
        public TimeSpanListArgumentSetup<TCommandArguments> FormatProvider(IFormatProvider formatProvider)
        {
            this.ArgumentParser.FormatProvider = formatProvider;
            return this;
        }

        /// <summary>
        /// Sets the time span styles that are permitted when parsing the argument values to <see cref="TimeSpan" /> values.
        /// The default is <see cref="TimeSpanStyles.None" />.
        /// </summary>
        /// <param name="timeSpanStyles">The time span styles that are permitted when parsing the argument values to <see cref="TimeSpan" /> values.</param>
        /// <returns>A reference to this instance for further configuration.</returns>
        public TimeSpanListArgumentSetup<TCommandArguments> Styles(TimeSpanStyles timeSpanStyles)
        {
            this.ArgumentParser.TimeSpanStyles = timeSpanStyles;
            return this;
        }
    }
}