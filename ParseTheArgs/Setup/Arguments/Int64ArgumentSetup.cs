using System;
using System.Globalization;
using System.Linq.Expressions;
using ParseTheArgs.Parsers.Arguments;
using ParseTheArgs.Parsers.Commands;

namespace ParseTheArgs.Setup.Arguments
{
    /// <summary>
    /// Represents the configuration of an argument that accepts a single <see cref="long" /> value.
    /// </summary>
    /// <typeparam name="TCommandArguments">The type in which the arguments of the command the argument belongs to will be stored.</typeparam>
    public class Int64ArgumentSetup<TCommandArguments> : SingleValueArgumentSetup<TCommandArguments, Int64ArgumentParser<TCommandArguments>, Int64ArgumentSetup<TCommandArguments>, Int64>
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="commandParser">The parser for the command the argument belongs to.</param>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandArguments" /> type in which the argument value should be stored.</param>
        public Int64ArgumentSetup(CommandParser<TCommandArguments> commandParser, LambdaExpression propertyExpression) : base(commandParser, propertyExpression)
        {
        }

        /// <summary>
        /// Sets the format provider to be used to parse the argument value to a <see cref="Int64" />.
        /// The default is <see cref="CultureInfo.CurrentCulture" />.
        /// </summary>
        /// <param name="formatProvider">The format provider to be used to parse the argument value to a <see cref="Int64" />.</param>
        /// <returns>A reference to this instance for further configuration.</returns>
        public Int64ArgumentSetup<TCommandArguments> FormatProvider(IFormatProvider formatProvider)
        {
            this.ArgumentParser.FormatProvider = formatProvider;
            return this;
        }

        /// <summary>
        /// Sets the number styles that are permitted when parsing the argument value to a <see cref="Int64" />.
        /// The default is <see cref="NumberStyles.Any" />.
        /// </summary>
        /// <param name="numberStyles">The number styles that are permitted when parsing the argument value to a <see cref="Int64" />.</param>
        /// <returns>A reference to this instance for further configuration.</returns>
        public Int64ArgumentSetup<TCommandArguments> Styles(NumberStyles numberStyles)
        {
            this.ArgumentParser.NumberStyles = numberStyles;
            return this;
        }
    }
}