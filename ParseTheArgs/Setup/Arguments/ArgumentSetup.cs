using System;
using System.Linq;
using System.Linq.Expressions;
using ParseTheArgs.Extensions;
using ParseTheArgs.Parsers.Arguments;
using ParseTheArgs.Parsers.Commands;

namespace ParseTheArgs.Setup.Arguments
{
    /// <summary>
    /// Represents the configuration of an argument.
    /// </summary>
    /// <typeparam name="TCommandArguments">The type in which the values of the arguments (of the command the argument belongs to) will be stored.</typeparam>
    /// <typeparam name="TArgumentParser">The type of parser for the argument.</typeparam>
    /// <typeparam name="TArgumentSetup">The type of setup for the argument.</typeparam>
#pragma warning disable S2436 // Types and methods should not have too many generic parameters
    public abstract class ArgumentSetup<TCommandArguments, TArgumentParser, TArgumentSetup>
#pragma warning restore S2436 // Types and methods should not have too many generic parameters
        where TCommandArguments : class
        where TArgumentParser : ArgumentParser
        where TArgumentSetup : ArgumentSetup<TCommandArguments, TArgumentParser, TArgumentSetup>
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="commandParser">The parser for the command the argument belongs to.</param>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandArguments" /> type in which the argument value should be stored.</param>
        protected ArgumentSetup(CommandParser<TCommandArguments> commandParser, LambdaExpression propertyExpression)
        {
            this.commandParser = commandParser;

            var targetProperty = ExpressionHelper.GetPropertyFromPropertyExpression(propertyExpression);

            this.ArgumentParser = commandParser.ArgumentParsers.OfType<TArgumentParser>().FirstOrDefault(a => a.TargetProperty == targetProperty);

            if (this.ArgumentParser == null)
            {
                this.ArgumentParser = (TArgumentParser) Activator.CreateInstance(typeof(TArgumentParser), new Object[] { targetProperty, new ArgumentName(targetProperty.Name.ToCamelCase()) });

                commandParser.ArgumentParsers.Add(this.ArgumentParser);
            }
        }

        /// <summary>
        /// Sets the help text for the argument.
        /// </summary>
        /// <param name="help">The help text for the argument.</param>
        /// <returns>A reference to this instance for further configuration.</returns>
        public TArgumentSetup Help(String help)
        {
            this.ArgumentParser.ArgumentHelp = help;
            return (TArgumentSetup) this;
        }

        /// <summary>
        /// Sets the name for the argument.
        /// On the command line the argument can be passed by writing two dashes followed by the given name (e.g. --argument).
        /// </summary>
        /// <param name="name">The name for the argument.</param>
        /// <returns>A reference to this instance for further configuration.</returns>
        /// <exception cref="ArgumentException">Throw if another argument with the same name as the given one already exists for the command the argument belongs to.</exception>
        public TArgumentSetup Name(String name)
        {
            if (this.commandParser.ArgumentParsers.Any(a => a != this.ArgumentParser && a.ArgumentName.Name == name))
            {
                throw new ArgumentException($"The given argument name '{name}' is already in use by another argument. Please use a different name.", nameof(name));
            }

            this.ArgumentParser.ArgumentName = new ArgumentName(name, this.ArgumentParser.ArgumentName.ShortName);
            return (TArgumentSetup) this;
        }

        /// <summary>
        /// Sets the short name for the argument.
        /// On the command line the argument can be passed by writing one dash followed by the given (single character) name (e.g. -a).
        /// </summary>
        /// <param name="shortName">The short name (a single character) for the argument.</param>
        /// <returns>A reference to this instance for further configuration.</returns>
        /// <exception cref="ArgumentException">Throw if another argument with the same short name as the given one already exists for the command the argument belongs to.</exception>
        public TArgumentSetup ShortName(Char shortName)
        {
            if (this.commandParser.ArgumentParsers.Any(a => a != this.ArgumentParser && a.ArgumentName.ShortName != null && a.ArgumentName.ShortName.Value == shortName))
            {
                throw new ArgumentException($"The given argument short name '{shortName}' is already in use by another argument. Please use a different short name.", nameof(shortName));
            }

            this.ArgumentParser.ArgumentName = new ArgumentName(this.ArgumentParser.ArgumentName.Name, shortName);
            return (TArgumentSetup) this;
        }

        /// <summary>
        /// Defines the parser for the argument.
        /// </summary>
        protected readonly TArgumentParser ArgumentParser;

        /// <summary>
        /// Defines the parser for the command the argument belongs to.
        /// </summary>
        private readonly CommandParser<TCommandArguments> commandParser;
    }
}