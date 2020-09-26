using System;
using System.Linq.Expressions;
using ParseTheArgs.Parsers.Arguments;
using ParseTheArgs.Parsers.Commands;

namespace ParseTheArgs.Setup.Arguments
{
    /// <summary>
    /// Represents the configuration of an argument that accepts one or more enum members of the enum <typeparamref name="TEnum" />.
    /// </summary>
    /// <typeparam name="TCommandArguments">The type in which the arguments of the command the argument belongs to will be stored.</typeparam>
    /// <typeparam name="TEnum">The type of the enum the argument accepts as argument values.</typeparam>
    public class EnumListArgumentSetup<TCommandArguments, TEnum> : MultiValueArgumentSetup<TCommandArguments, EnumListArgumentParser<TEnum>, EnumListArgumentSetup<TCommandArguments, TEnum>, TEnum>
        where TCommandArguments : class
        where TEnum : struct, Enum
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="commandParser">The parser for the command the argument belongs to.</param>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandArguments" /> type in which the argument value should be stored.</param>
        public EnumListArgumentSetup(CommandParser<TCommandArguments> commandParser, LambdaExpression propertyExpression) : base(commandParser, propertyExpression)
        {
        }

        /// <summary>
        /// Sets the help text for the given enum member of the enum type <typeparamref name="TEnum" />.
        /// </summary>
        /// <param name="value">The enum member to set the help text for.</param>
        /// <param name="help">The help text for the enum member.</param>
        /// <returns>A reference to this instance for further configuration.</returns>
        public EnumListArgumentSetup<TCommandArguments, TEnum> OptionHelp(TEnum value, String help)
        {
            this.ArgumentParser.EnumValuesHelps.Add(value, help);
            return this;
        }
    }
}