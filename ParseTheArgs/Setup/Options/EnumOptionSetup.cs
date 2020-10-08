using System;
using System.Linq.Expressions;
using ParseTheArgs.Parsers.Commands;
using ParseTheArgs.Parsers.Options;

namespace ParseTheArgs.Setup.Options
{
    /// <summary>
    /// Represents the configuration of an option that accepts a single enum member of the enum <typeparamref name="TEnum" />.
    /// </summary>
    /// <typeparam name="TCommandOptions">The type in which the options of the command the option belongs to will be stored.</typeparam>
    /// <typeparam name="TEnum">The type of the enum the option accepts as option value.</typeparam>
    public class EnumOptionSetup<TCommandOptions, TEnum> : SingleValueOptionSetup<TCommandOptions, EnumOptionParser<TEnum>, EnumOptionSetup<TCommandOptions, TEnum>, TEnum>
        where TCommandOptions : class
        where TEnum : struct, Enum
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="commandParser">The parser for the command the option belongs to.</param>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandOptions" /> type in which the option value should be stored.</param>
        /// <exception cref="ArgumentNullException"><paramref name="commandParser"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="propertyExpression"/> is null.</exception>
        public EnumOptionSetup(CommandParser<TCommandOptions> commandParser, LambdaExpression propertyExpression) : base(commandParser, propertyExpression)
        {
        }

        /// <summary>
        /// Sets the help text for the given enum member of the enum type <typeparamref name="TEnum" />.
        /// </summary>
        /// <param name="value">The enum member to set the help text for.</param>
        /// <param name="help">The help text for the enum member.</param>
        /// <returns>A reference to this instance for further configuration.</returns>
        public EnumOptionSetup<TCommandOptions, TEnum> OptionHelp(TEnum value, String help)
        {
            this.OptionParser.EnumValuesHelps.Add(value, help);
            return this;
        }
    }
}