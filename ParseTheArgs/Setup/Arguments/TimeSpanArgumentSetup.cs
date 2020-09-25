using System;
using System.Globalization;
using System.Linq.Expressions;
using ParseTheArgs.Parsers.Arguments;
using ParseTheArgs.Parsers.Commands;

namespace ParseTheArgs.Setup.Arguments
{
    /// <summary>
    /// Represents the configuration of an argument that accepts a single <see cref="TimeSpan" /> value.
    /// </summary>
    /// <typeparam name="TCommandArguments">The type in which the arguments of the command the argument belongs to will be stored.</typeparam>
    public class TimeSpanArgumentSetup<TCommandArguments> : SingleValueArgumentSetup<TCommandArguments, TimeSpanArgumentParser<TCommandArguments>, TimeSpanArgumentSetup<TCommandArguments>, TimeSpan>
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="commandParser">The parser for the command the argument belongs to.</param>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandArguments" /> type in which the argument value should be stored.</param>
        public TimeSpanArgumentSetup(CommandParser<TCommandArguments> commandParser, LambdaExpression propertyExpression) : base(commandParser, propertyExpression)
        {
        }

        /// <summary>
        /// Sets the format that is accepted when the argument value is parsed to a <see cref="TimeSpan" />.
        /// For supported formats see the documentation of <see cref="TimeSpan.Parse(string)" />.
        /// </summary>
        /// <param name="timeSpanFormat">The format that is accepted when the argument value is parsed to a <see cref="TimeSpan" />.</param>
        /// <returns>A reference to this instance for further configuration.</returns>
        public TimeSpanArgumentSetup<TCommandArguments> Format(String timeSpanFormat)
        {
            this.ArgumentParser.TimeSpanFormat = timeSpanFormat;
            return this;
        }

        /// <summary>
        /// Sets the format provider to be used to parse the argument value to a <see cref="TimeSpan" />.
        /// The default is <see cref="CultureInfo.CurrentCulture" />.
        /// </summary>
        /// <param name="formatProvider">The format provider to be used to parse the argument value to a <see cref="TimeSpan" />.</param>
        /// <returns>A reference to this instance for further configuration.</returns>
        public TimeSpanArgumentSetup<TCommandArguments> FormatProvider(IFormatProvider formatProvider)
        {
            this.ArgumentParser.FormatProvider = formatProvider;
            return this;
        }

        /// <summary>
        /// Sets the time span styles that are permitted when parsing the argument value to a <see cref="TimeSpan" />.
        /// The default is <see cref="TimeSpanStyles.None" />.
        /// </summary>
        /// <param name="timeSpanStyles">The time span styles that are permitted when parsing the argument value to a <see cref="TimeSpan" />.</param>
        /// <returns>A reference to this instance for further configuration.</returns>
        public TimeSpanArgumentSetup<TCommandArguments> Styles(TimeSpanStyles timeSpanStyles)
        {
            this.ArgumentParser.TimeSpanStyles = timeSpanStyles;
            return this;
        }
    }
}