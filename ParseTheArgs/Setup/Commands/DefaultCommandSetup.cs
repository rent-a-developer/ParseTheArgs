using System;
using System.Linq;
using ParseTheArgs.Parsers.Commands;

namespace ParseTheArgs.Setup.Commands
{
    /// <summary>
    /// Represents the configuration of the default (unnamed) command.
    /// </summary>
    /// <typeparam name="TCommandArguments">The type where the values of the arguments of the command will be stored in.</typeparam>
    public class DefaultCommandSetup<TCommandArguments> : CommandSetup<TCommandArguments> where TCommandArguments : class, new()
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="parser">The parser the command belongs to.</param>
        internal DefaultCommandSetup(Parser parser) : base(parser, () => CreateCommandParser(parser))
        {
        }

        /// <summary>
        /// Sets a text that describes an example usage of the command.
        /// </summary>
        /// <param name="exampleUsageText">The text that describes an example usage of the command.</param>
        /// <returns>A reference to this instance for further configuration of the command.</returns>
        public DefaultCommandSetup<TCommandArguments> ExampleUsage(String exampleUsageText)
        {
            this.CommandParser.CommandExampleUsage = exampleUsageText;
            return this;
        }

        /// <summary>
        /// Sets the help text for the command.
        /// </summary>
        /// <param name="help">The help text for the command.</param>
        /// <returns>A reference to this instance for further configuration of the command.</returns>
        public DefaultCommandSetup<TCommandArguments> Help(String help)
        {
            this.CommandParser.CommandHelp = help;
            return this;
        }

        /// <summary>
        /// Sets the validator for this command.
        /// The given action is executed after all arguments of the command have been parsed and their values have been stored in <see cref="ParseResult.CommandArguments" />.
        /// </summary>
        /// <param name="validator">An action that validates the command arguments.</param>
        /// <returns>A reference to this instance for further configuration of the command.</returns>
        public DefaultCommandSetup<TCommandArguments> Validate(Action<CommandValidatorContext<TCommandArguments>> validator)
        {
            this.CommandParser.Validator = validator;
            return this;
        }

        private static CommandParser<TCommandArguments> CreateCommandParser(Parser parser)
        {
            var commandParser = parser.CommandParsers.OfType<CommandParser<TCommandArguments>>().FirstOrDefault(a => a.IsCommandDefault);

            if (commandParser == null)
            {
                commandParser = new CommandParser<TCommandArguments>(parser)
                {
                    IsCommandDefault = true
                };

                parser.CommandParsers.Add(commandParser);
            }

            return commandParser;
        }
    }
}