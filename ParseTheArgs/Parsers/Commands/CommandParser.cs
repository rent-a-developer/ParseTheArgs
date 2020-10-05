using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ParseTheArgs.Extensions;
using ParseTheArgs.Parsers.Options;
using ParseTheArgs.Tokens;
using ParseTheArgs.Validation;

namespace ParseTheArgs.Parsers.Commands
{
    /// <summary>
    /// Parses a command line command.
    /// </summary>
    /// <typeparam name="TCommandOptions">The type in which the values of the options of the command will be stored.</typeparam>
    public class CommandParser<TCommandOptions> : ICommandParser
        where TCommandOptions : class
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="parser">The parser the command parser belongs to.</param>
        public CommandParser(Parser parser)
        {
            this.parser = parser;

            this.OptionParsers = new List<OptionParser>();
            this.CommandName = String.Empty;
            this.CommandHelp = String.Empty;
            this.CommandExampleUsage = String.Empty;
        }

        /// <summary>
        /// Defines the list of option parsers for the command.
        /// </summary>
        public virtual List<OptionParser> OptionParsers { get; }

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
        /// Defines the validator to use to validate the command and its options.
        /// </summary>
        public Action<CommandValidatorContext<TCommandOptions>>? Validator { get; set; }

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

            stringBuilder.AppendLine(String.Join(" ", this.OptionParsers.Select(a => GetOptionShortHelpPart(a))));

            stringBuilder.AppendLine();

            if (!String.IsNullOrEmpty(this.CommandHelp))
            {
                stringBuilder.AppendLine(this.CommandHelp);
                stringBuilder.AppendLine();
            }

            stringBuilder.AppendLine("Options:");

            var maxOptionNameLength = this.OptionParsers.Count == 0 ? 0 : this.OptionParsers.Max(a => GetOptionLongHelpPart(a).Length);
            var optionHelpRightPadding = maxOptionNameLength + 1 + 11;

            foreach (var optionParser in this.OptionParsers)
            {
                var optionHelpText = optionParser.GetHelpText();
                var optionHelpTextWrappedLines = optionHelpText.WordWrap(this.parser.HelpTextMaxLineLength - optionHelpRightPadding);

                for (int i = 0; i < optionHelpTextWrappedLines.Length; i++)
                {
                    if (i == 0)
                    {
                        stringBuilder.Append($"{GetOptionLongHelpPart(optionParser).PadRight(maxOptionNameLength)} ");
                        stringBuilder.Append(optionParser.IsOptionRequired ? "(Required) " : "(Optional) ");
                    }
                    else
                    {
                        stringBuilder.Append(new String(' ', optionHelpRightPadding));
                    }

                    stringBuilder.AppendLine(optionHelpTextWrappedLines[i]);
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
        /// <param name="parseResult">The object where to put the result of the parsing into.</param>
        public void Parse(List<Token> tokens, ParseResult parseResult)
        {
            var commandToken = tokens.OfType<CommandToken>().FirstOrDefault();

            if ((commandToken == null && this.IsCommandDefault) || (commandToken != null && commandToken.CommandName == this.CommandName))
            {
                if (commandToken != null)
                {
                    commandToken.IsParsed = true;
                }

                parseResult.CommandOptions = Activator.CreateInstance(typeof(TCommandOptions));
                parseResult.CommandName = this.CommandName;

                this.OptionParsers.ForEach(a => a.Parse(tokens, parseResult));
            }
        }

        /// <summary>
        /// Validates the given tokens and puts the result of the validation into the given parse result object.
        /// </summary>
        /// <param name="tokens">The tokens to validate.</param>
        /// <param name="parseResult">The object where to put the result of the validation into.</param>
        public void Validate(List<Token> tokens, ParseResult parseResult)
        {
            var commandToken = tokens.OfType<CommandToken>().FirstOrDefault();

            if ((commandToken == null && this.IsCommandDefault) || (commandToken != null && commandToken.CommandName == this.CommandName))
            {
                this.Validator?.Invoke(new CommandValidatorContext<TCommandOptions>(this, parseResult));
            }
        }

        internal virtual TOptionParser GetOrCreateOptionParser<TOptionParser>(PropertyInfo targetProperty)
            where TOptionParser : OptionParser
        {
            var optionParser = this.OptionParsers.OfType<TOptionParser>().FirstOrDefault(a => a.TargetProperty == targetProperty);

            if (optionParser == null)
            {
                optionParser = (TOptionParser) Activator.CreateInstance(typeof(TOptionParser), new Object[] { targetProperty, targetProperty.Name.ToCamelCase() });

                this.OptionParsers.Add(optionParser);
            }

            return optionParser;
        }

        private static String GetOptionLongHelpPart(OptionParser optionParser)
        {
            var result = "";

            result += $"--{optionParser.OptionName}";

            switch (optionParser.OptionType)
            {
                case OptionType.ValuelessOption:
                    break;

                case OptionType.SingleValueOption:
                    result += " [value]";
                    break;

                case OptionType.MultiValueOption:
                    result += " [value value ...]";
                    break;

                default:
                    throw new ArgumentOutOfRangeException($"The OptionType '{optionParser.OptionType}' is not supproted.");
            }

            return result;
        }

        private static String GetOptionShortHelpPart(OptionParser optionParser)
        {
            var result = "[";

            result += $"--{optionParser.OptionName}";

            switch (optionParser.OptionType)
            {
                case OptionType.ValuelessOption:
                    break;

                case OptionType.SingleValueOption:
                    result += " value";
                    break;

                case OptionType.MultiValueOption:
                    result += " value value ...";
                    break;

                default:
                    throw new ArgumentOutOfRangeException($"The OptionType '{optionParser.OptionType}' is not supproted.");
            }

            result += "]";

            return result;
        }

        private readonly Parser parser;
    }
}