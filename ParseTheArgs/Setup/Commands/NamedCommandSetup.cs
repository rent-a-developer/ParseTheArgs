using System;
using System.Linq;
using ParseTheArgs.Extensions;
using ParseTheArgs.Parsers.Commands;

namespace ParseTheArgs.Setup.Commands
{
    /// <summary>
    /// Represents the configuration of a named (non-default) command.
    /// </summary>
    /// <typeparam name="TCommandArguments">The type where the values of the arguments of the command will be stored in.</typeparam>
    public class NamedCommandSetup<TCommandArguments> : CommandSetup<TCommandArguments> where TCommandArguments : class, new()
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="parser">The parser the command belongs to.</param>
        internal NamedCommandSetup(Parser parser) : base(parser, CreateCommandParser)
        {
        }

        /// <summary>
        /// Sets the name of the command.
        /// Initially (until this method is called) the name of the given <typeparamref name="TCommandArguments" /> type (converted to lower camel case, see <see cref="StringExtensions.ToCamelCase" />) will be used as name for the command.
        /// </summary>
        /// <param name="name">The name of the command.</param>
        /// <returns>A reference to this instance for further configuration of the command.</returns>
        /// <exception cref="ArgumentException">Thrown if another command with the same name already exists.</exception>
        public NamedCommandSetup<TCommandArguments> Name(String name)
        {
            if (this.Parser.CommandParsers.Any(a => a != this.CommandParser && a.CommandName == name))
            {
                throw new ArgumentException($"The given command name '{name}' is already in use by another command. Please use a different name.", nameof(name));
            }

            this.CommandParser.CommandName = name;
            return this;
        }

        /// <summary>
        /// Sets a text that describes an example usage of the command.
        /// </summary>
        /// <param name="exampleUsageText">The text that describes an example usage of the command.</param>
        /// <returns>A reference to this instance for further configuration of the command.</returns>
        public NamedCommandSetup<TCommandArguments> ExampleUsage(String exampleUsageText)
        {
            this.CommandParser.CommandExampleUsage = exampleUsageText;
            return this;
        }

        /// <summary>
        /// Sets the help text for the command.
        /// </summary>
        /// <param name="help">The help text for the command.</param>
        /// <returns>A reference to this instance for further configuration of the command.</returns>
        public NamedCommandSetup<TCommandArguments> Help(String help)
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
        public NamedCommandSetup<TCommandArguments> Validate(Action<CommandValidatorContext<TCommandArguments>> validator)
        {
            this.CommandParser.Validator = validator;
            return this;
        }

        private static CommandParser<TCommandArguments> CreateCommandParser(Parser parser)
        {
            var commandParser = parser.CommandParsers.OfType<CommandParser<TCommandArguments>>().FirstOrDefault();

            if (commandParser == null)
            {
                commandParser = new CommandParser<TCommandArguments>(parser)
                {
                    CommandName = typeof(TCommandArguments)
                        .Name
                        .ToCamelCase()
                        .Replace("Arguments", "")
                        .Replace("Args", "")
                };

                parser.CommandParsers.Add(commandParser);
            }

            return commandParser;
        }
    }
}