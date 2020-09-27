using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParseTheArgs.Extensions;
using ParseTheArgs.Parsers.Arguments;
using ParseTheArgs.Tokens;

namespace ParseTheArgs.Parsers.Commands
{
    /// <summary>
    /// Parses a command line command.
    /// </summary>
    /// <typeparam name="TCommandArguments">The type in which the values of the arguments of the command will be stored.</typeparam>
    public class CommandParser<TCommandArguments> : ICommandParser
        where TCommandArguments : class
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="parser">The parser the command parser belongs to.</param>
        public CommandParser(Parser parser)
        {
            this.parser = parser;

            this.ArgumentParsers = new List<ArgumentParser>();
            this.CommandName = String.Empty;
            this.CommandHelp = String.Empty;
            this.CommandExampleUsage = String.Empty;
        }

        /// <summary>
        /// Defines the list of argument parsers for the command.
        /// </summary>
        public virtual List<ArgumentParser> ArgumentParsers { get; }

        /// <summary>
        /// Defines a text that describes an example usage of the command.
        /// </summary>
        public String CommandExampleUsage { get; set; }

        /// <summary>
        /// Defines the help text of the command.
        /// </summary>
        public String CommandHelp { get; set; }

        /// <summary>
        /// Defines the name of the command.
        /// Will be <see cref="String.Empty"/> if the command is the default command (see <see cref="ICommandParser.IsCommandDefault" />).
        /// </summary>
        public String CommandName { get; set; }

        /// <summary>
        /// Determines if the command is the default (non-named) command.
        /// If the command is the default command <see cref="ICommandParser.CommandName" /> will be null.
        /// </summary>
        public Boolean IsCommandDefault { get; set; }

        /// <summary>
        /// Defines the validator to use to validate the command and its arguments.
        /// </summary>
        public Action<CommandValidatorContext<TCommandArguments>>? Validator { get; set; }

        /// <summary>
        /// Gets the help text of the command.
        /// </summary>
        /// <returns>The help text of the command.</returns>
        public String GetHelpText()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append($"{this.parser.ProgramName} ");

            if (!this.IsCommandDefault)
            {
                stringBuilder.Append($"{this.CommandName} ");
            }

            stringBuilder.AppendLine(String.Join(" ", this.ArgumentParsers.Select(a => GetArgumentShortHelpPart(a))));

            stringBuilder.AppendLine();

            if (!String.IsNullOrEmpty(this.CommandHelp))
            {
                stringBuilder.AppendLine(this.CommandHelp);
                stringBuilder.AppendLine();
            }

            stringBuilder.AppendLine("Arguments:");

            var maxArgumentNameLength = this.ArgumentParsers.Count == 0 ? 0 : this.ArgumentParsers.Max(a => GetArgumentLongHelpPart(a).Length);
            var argumentHelpRightPadding = maxArgumentNameLength + 1 + 11;

            foreach (var argumentParser in this.ArgumentParsers)
            {
                var argumentHelpText = argumentParser.GetHelpText();
                var argumentHelpTextWrappedLines = argumentHelpText.WordWrap(this.parser.HelpTextMaxLineLength - argumentHelpRightPadding);

                for (int i = 0; i < argumentHelpTextWrappedLines.Length; i++)
                {
                    if (i == 0)
                    {
                        stringBuilder.Append($"{GetArgumentLongHelpPart(argumentParser).PadRight(maxArgumentNameLength)} ");
                        stringBuilder.Append(argumentParser.IsArgumentRequired ? "(Required) " : "(Optional) ");
                    }
                    else
                    {
                        stringBuilder.Append(new String(' ', argumentHelpRightPadding));
                    }

                    stringBuilder.AppendLine(argumentHelpTextWrappedLines[i]);
                }
            }

            if (!String.IsNullOrEmpty(this.CommandExampleUsage))
            {
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("Example usage:");
                stringBuilder.AppendLine(this.CommandExampleUsage);
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Parses the given tokens and puts the result of the parsing into the given parse result object.
        /// </summary>
        /// <param name="tokens">The tokens to parse.</param>
        /// <param name="parseResult">The parse result to put result of the parsing into.</param>
        public void Parse(List<Token> tokens, ParseResult parseResult)
        {
            var commandToken = tokens.OfType<CommandToken>().FirstOrDefault();

            if ((commandToken == null && this.IsCommandDefault) || (commandToken != null && commandToken.CommandName == this.CommandName))
            {
                if (commandToken != null)
                {
                    commandToken.IsParsed = true;
                }

                parseResult.CommandArguments = Activator.CreateInstance(typeof(TCommandArguments));
                parseResult.CommandName = this.CommandName;

                this.ArgumentParsers.ForEach(a => a.Parse(tokens, parseResult));
            }
        }

        /// <summary>
        /// Validates the given tokens and puts the result of the validation into the given parse result object.
        /// </summary>
        /// <param name="tokens">The tokens to validate.</param>
        /// <param name="parseResult">The parse result to put result of the validation into.</param>
        public void Validate(List<Token> tokens, ParseResult parseResult)
        {
            var commandToken = tokens.OfType<CommandToken>().FirstOrDefault();

            if ((commandToken == null && this.IsCommandDefault) || (commandToken != null && commandToken.CommandName == this.CommandName))
            {
                this.Validator?.Invoke(new CommandValidatorContext<TCommandArguments>(this, parseResult));
            }
        }

        private static String GetArgumentLongHelpPart(ArgumentParser argumentParser)
        {
            var result = "";

            if (argumentParser.ArgumentName.ShortName != null)
            {
                result += $"-{argumentParser.ArgumentName.ShortName}|";
            }

            result += $"--{argumentParser.ArgumentName.Name}";

            switch (argumentParser.ArgumentType)
            {
                case ArgumentType.ValuelessArgument:
                    break;

                case ArgumentType.SingleValueArgument:
                    result += " [value]";
                    break;

                case ArgumentType.MultiValueArgument:
                    result += " [value value ...]";
                    break;

                default:
                    throw new ArgumentOutOfRangeException($"The ArgumentType '{argumentParser.ArgumentType}' is not supproted.");
            }

            return result;
        }

        private static String GetArgumentShortHelpPart(ArgumentParser argumentParser)
        {
            var result = "[";

            if (argumentParser.ArgumentName.ShortName != null)
            {
                result += $"-{argumentParser.ArgumentName.ShortName}|";
            }

            result += $"--{argumentParser.ArgumentName.Name}";

            switch (argumentParser.ArgumentType)
            {
                case ArgumentType.ValuelessArgument:
                    break;

                case ArgumentType.SingleValueArgument:
                    result += " value";
                    break;

                case ArgumentType.MultiValueArgument:
                    result += " value value ...";
                    break;

                default:
                    throw new ArgumentOutOfRangeException($"The ArgumentType '{argumentParser.ArgumentType}' is not supproted.");
            }

            result += "]";

            return result;
        }

        private readonly Parser parser;
    }
}