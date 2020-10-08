using System;
using System.Linq.Expressions;
using ParseTheArgs.Parsers.Commands;
using ParseTheArgs.Parsers.Options;

namespace ParseTheArgs.Setup.Options
{
    /// <summary>
    /// Represents the configuration of an option that accepts one or more <see cref="Guid" /> values.
    /// </summary>
    /// <typeparam name="TCommandOptions">The type in which the options of the command the option belongs to will be stored.</typeparam>
    public class GuidListOptionSetup<TCommandOptions> : MultiValueOptionSetup<TCommandOptions, GuidListOptionParser, GuidListOptionSetup<TCommandOptions>, Guid>
        where TCommandOptions : class
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="commandParser">The parser for the command the option belongs to.</param>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandOptions" /> type in which the option value should be stored.</param>
        /// <exception cref="ArgumentNullException"><paramref name="commandParser"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="propertyExpression"/> is null.</exception>
        public GuidListOptionSetup(CommandParser<TCommandOptions> commandParser, LambdaExpression propertyExpression) : base(commandParser, propertyExpression)
        {
        }

        /// <summary>
        /// Sets the format that is accepted when the option values are parsed to <see cref="Guid" /> values.
        /// For supported formats see the documentation of <see cref="Guid.Parse(string)" />.
        /// </summary>
        /// <param name="guidFormat">The format that is accepted when the option values are parsed to <see cref="Guid" /> values.</param>
        /// <returns>A reference to this instance for further configuration.</returns>
        public GuidListOptionSetup<TCommandOptions> Format(String guidFormat)
        {
            this.OptionParser.GuidFormat = guidFormat;
            return this;
        }
    }
}