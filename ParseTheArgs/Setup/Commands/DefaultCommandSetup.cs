﻿using System;
using ParseTheArgs.Parsers.Commands;
using ParseTheArgs.Validation;

namespace ParseTheArgs.Setup.Commands
{
    /// <summary>
    /// Represents the configuration of the default (unnamed) command.
    /// </summary>
    /// <typeparam name="TCommandOptions">The type where the values of the options of the command will be stored in.</typeparam>
    public class DefaultCommandSetup<TCommandOptions> : CommandSetup<TCommandOptions> where TCommandOptions : class, new()
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="parser">The parser the command belongs to.</param>
        /// <param name="commandParser">The command parser for the command.</param>
        /// <exception cref="ArgumentException"><paramref name="parser" /> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="commandParser" /> is null.</exception>
        internal DefaultCommandSetup(Parser parser, CommandParser<TCommandOptions> commandParser) : base(parser, commandParser)
        {
        }

        /// <summary>
        /// Sets a text that describes an example usage of the command.
        /// </summary>
        /// <param name="exampleUsageText">The text that describes an example usage of the command.</param>
        /// <returns>A reference to this instance for further configuration of the command.</returns>
        public DefaultCommandSetup<TCommandOptions> ExampleUsage(String exampleUsageText)
        {
            this.CommandParser.CommandExampleUsage = exampleUsageText;
            return this;
        }

        /// <summary>
        /// Sets the help text for the command.
        /// </summary>
        /// <param name="help">The help text for the command.</param>
        /// <returns>A reference to this instance for further configuration of the command.</returns>
        public DefaultCommandSetup<TCommandOptions> Help(String help)
        {
            this.CommandParser.CommandHelp = help;
            return this;
        }

        /// <summary>
        /// Sets the validator for this command.
        /// The given action is executed after all options of the command have been parsed and their values have been stored in <see cref="ParseResult.CommandOptions" />.
        /// </summary>
        /// <param name="validator">An action that validates the command options.</param>
        /// <returns>A reference to this instance for further configuration of the command.</returns>
        public DefaultCommandSetup<TCommandOptions> Validate(Action<CommandValidatorContext<TCommandOptions>> validator)
        {
            this.CommandParser.Validator = validator;
            return this;
        }
    }
}