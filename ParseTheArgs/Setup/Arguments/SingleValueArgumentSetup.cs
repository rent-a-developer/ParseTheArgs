using System.Linq.Expressions;
using ParseTheArgs.Parsers.Arguments;
using ParseTheArgs.Parsers.Commands;

namespace ParseTheArgs.Setup.Arguments
{
    /// <summary>
    /// Represents the configuration of an argument that accepts a single value of the type <typeparamref name="TArgumentValue" />.
    /// </summary>
    /// <typeparam name="TCommandArguments">The type in which the arguments of the command the argument belongs to will be stored.</typeparam>
    /// <typeparam name="TArgumentParser">The type of parser for the argument.</typeparam>
    /// <typeparam name="TArgumentSetup">The type of setup for the argument.</typeparam>
    /// <typeparam name="TArgumentValue">The type of the argument value.</typeparam>
#pragma warning disable S2436 // Types and methods should not have too many generic parameters
    public abstract class SingleValueArgumentSetup<TCommandArguments, TArgumentParser, TArgumentSetup, TArgumentValue> : ArgumentSetup<TCommandArguments, TArgumentParser, TArgumentSetup>
#pragma warning restore S2436 // Types and methods should not have too many generic parameters
        where TCommandArguments : class
        where TArgumentParser : SingleValueArgumentParser<TArgumentValue>
        where TArgumentSetup : SingleValueArgumentSetup<TCommandArguments, TArgumentParser, TArgumentSetup, TArgumentValue>
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="commandParser">The parser for the command the argument belongs to.</param>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandArguments" /> type in which the argument value should be stored.</param>
        protected SingleValueArgumentSetup(CommandParser<TCommandArguments> commandParser, LambdaExpression propertyExpression) : base(commandParser, propertyExpression)
        {
        }

        /// <summary>
        /// Sets the default value for the argument.
        /// If the argument is not specified on the command line the target property will be set to the given value.
        /// </summary>
        /// <param name="defaultValue">The default value for the argument.</param>
        /// <returns>A reference to this instance for further configuration.</returns>
        public TArgumentSetup DefaultValue(TArgumentValue defaultValue)
        {
            this.ArgumentParser.ArgumentDefaultValue = defaultValue;
            return (TArgumentSetup) this;
        }

        /// <summary>
        /// Marks that the argument as required.
        /// If the argument is marked as required and is not specified on the command line a parse error will be caused (<see cref="ParseResult.Errors" />).
        /// </summary>
        /// <returns>A reference to this instance for further configuration.</returns>
        public TArgumentSetup IsRequired()
        {
            this.ArgumentParser.IsArgumentRequired = true;
            return (TArgumentSetup) this;
        }
    }
}