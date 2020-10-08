using System;
using System.Linq;
using System.Linq.Expressions;
using ParseTheArgs.Parsers.Commands;
using ParseTheArgs.Parsers.Options;

namespace ParseTheArgs.Setup.Options
{
    /// <summary>
    /// Represents the configuration of an option.
    /// </summary>
    /// <typeparam name="TCommandOptions">The type in which the values of the options (of the command the option belongs to) will be stored.</typeparam>
    /// <typeparam name="TOptionParser">The type of parser for the option.</typeparam>
    /// <typeparam name="TOptionSetup">The type of setup for the option.</typeparam>
#pragma warning disable S2436 // Types and methods should not have too many generic parameters
    public abstract class OptionSetup<TCommandOptions, TOptionParser, TOptionSetup>
#pragma warning restore S2436 // Types and methods should not have too many generic parameters
        where TCommandOptions : class
        where TOptionParser : OptionParser
        where TOptionSetup : OptionSetup<TCommandOptions, TOptionParser, TOptionSetup>
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="commandParser">The parser for the command the option belongs to.</param>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandOptions" /> type in which the option value should be stored.</param>
        /// <exception cref="ArgumentNullException"><paramref name="commandParser"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="propertyExpression"/> is null.</exception>
        protected OptionSetup(CommandParser<TCommandOptions> commandParser, LambdaExpression propertyExpression)
        {
            if (commandParser == null)
            {
                throw new ArgumentNullException(nameof(commandParser));
            }

            if (propertyExpression == null)
            {
                throw new ArgumentNullException(nameof(propertyExpression));
            }

            this.commandParser = commandParser;

            var targetProperty = ExpressionHelper.GetPropertyFromPropertyExpression(propertyExpression);

            this.OptionParser = commandParser.GetOrCreateOptionParser<TOptionParser>(targetProperty);
        }

        /// <summary>
        /// Sets the help text for the option.
        /// </summary>
        /// <param name="help">The help text for the option.</param>
        /// <returns>A reference to this instance for further configuration.</returns>
        public TOptionSetup Help(String help)
        {
            this.OptionParser.OptionHelp = help;
            return (TOptionSetup) this;
        }

        /// <summary>
        /// Sets the name for the option.
        /// On the command line the option can be passed by writing two dashes followed by the given name (e.g. --option).
        /// </summary>
        /// <param name="name">The name for the option.</param>
        /// <returns>A reference to this instance for further configuration.</returns>
        /// <exception cref="ArgumentException">Throw if another option with the same name as the given one already exists for the command the option belongs to.</exception>
        public TOptionSetup Name(String name)
        {
            if (this.commandParser.OptionParsers.Any(a => a != this.OptionParser && a.OptionName == name))
            {
                throw new ArgumentException($"The given option name '{name}' is already in use by another option. Please use a different name.", nameof(name));
            }

            this.OptionParser.OptionName = name;
            return (TOptionSetup) this;
        }

        /// <summary>
        /// Defines the parser for the option.
        /// </summary>
        internal readonly TOptionParser OptionParser;

        /// <summary>
        /// Defines the parser for the command the option belongs to.
        /// </summary>
        private readonly CommandParser<TCommandOptions> commandParser;
    }
}