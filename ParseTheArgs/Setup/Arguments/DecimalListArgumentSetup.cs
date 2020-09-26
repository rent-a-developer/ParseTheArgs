using System;
using System.Globalization;
using System.Linq.Expressions;
using ParseTheArgs.Parsers.Arguments;
using ParseTheArgs.Parsers.Commands;

namespace ParseTheArgs.Setup.Arguments
{
    /// <summary>
    /// Represents the configuration of an argument that accepts one or more <see cref="decimal" /> values.
    /// </summary>
    /// <typeparam name="TCommandArguments">The type in which the arguments of the command the argument belongs to will be stored.</typeparam>
    public class DecimalListArgumentSetup<TCommandArguments> : MultiValueArgumentSetup<TCommandArguments, DecimalListArgumentParser, DecimalListArgumentSetup<TCommandArguments>, Decimal>
        where TCommandArguments : class
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="commandParser">The parser for the command the argument belongs to.</param>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandArguments" /> type in which the argument value should be stored.</param>
        public DecimalListArgumentSetup(CommandParser<TCommandArguments> commandParser, LambdaExpression propertyExpression) : base(commandParser, propertyExpression)
        {
        }

        /// <summary>
        /// Sets the format provider to be used to parse the argument value to <see cref="Decimal" /> values.
        /// The default is <see cref="CultureInfo.CurrentCulture" />.
        /// </summary>
        /// <param name="formatProvider">The format provider to be used to parse the argument value to <see cref="Decimal" /> values.</param>
        /// <returns>A reference to this instance for further configuration.</returns>
        public DecimalListArgumentSetup<TCommandArguments> FormatProvider(IFormatProvider formatProvider)
        {
            this.ArgumentParser.FormatProvider = formatProvider;
            return this;
        }

        /// <summary>
        /// Sets the number styles that are permitted when parsing the argument value to <see cref="Decimal" /> values.
        /// The default is <see cref="NumberStyles.Any" />.
        /// </summary>
        /// <param name="numberStyles">The number styles that are permitted when parsing the argument value to <see cref="Decimal" /> values.</param>
        /// <returns>A reference to this instance for further configuration.</returns>
        public DecimalListArgumentSetup<TCommandArguments> Styles(NumberStyles numberStyles)
        {
            this.ArgumentParser.NumberStyles = numberStyles;
            return this;
        }
    }
}