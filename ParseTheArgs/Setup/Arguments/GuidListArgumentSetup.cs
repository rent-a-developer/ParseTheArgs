using System;
using System.Linq.Expressions;
using ParseTheArgs.Parsers.Arguments;
using ParseTheArgs.Parsers.Commands;

namespace ParseTheArgs.Setup.Arguments
{
    /// <summary>
    /// Represents the configuration of an argument that accepts one or more <see cref="Guid" /> values.
    /// </summary>
    /// <typeparam name="TCommandArguments">The type in which the arguments of the command the argument belongs to will be stored.</typeparam>
    public class GuidListArgumentSetup<TCommandArguments> : MultiValueArgumentSetup<TCommandArguments, GuidListArgumentParser, GuidListArgumentSetup<TCommandArguments>, Guid>
        where TCommandArguments : class
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="commandParser">The parser for the command the argument belongs to.</param>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandArguments" /> type in which the argument value should be stored.</param>
        public GuidListArgumentSetup(CommandParser<TCommandArguments> commandParser, LambdaExpression propertyExpression) : base(commandParser, propertyExpression)
        {
        }

        /// <summary>
        /// Sets the format that is accepted when the argument values are parsed to <see cref="Guid" /> values.
        /// For supported formats see the documentation of <see cref="Guid.Parse(string)" />.
        /// </summary>
        /// <param name="guidFormat">The format that is accepted when the argument values are parsed to <see cref="Guid" /> values.</param>
        /// <returns>A reference to this instance for further configuration.</returns>
        public GuidListArgumentSetup<TCommandArguments> Format(String guidFormat)
        {
            this.ArgumentParser.GuidFormat = guidFormat;
            return this;
        }
    }
}