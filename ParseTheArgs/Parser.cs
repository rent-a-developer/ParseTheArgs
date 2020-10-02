using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using ParseTheArgs.Errors;
using ParseTheArgs.Parsers.Commands;
using ParseTheArgs.Setup;
using ParseTheArgs.Tokens;

namespace ParseTheArgs
{
    /// <summary>
    /// Parses command line arguments.
    /// </summary>
    public class Parser
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public Parser()
        {
            this.CommandParsers = new List<ICommandParser>();
            this.Setup = new ParserSetup(this);

            this.Banner = String.Empty;
            this.ProgramName = Process.GetCurrentProcess().ProcessName;

            if (IsConsolePresent())
            {
                this.HelpTextWriter = Console.Out;
                this.ErrorTextWriter = Console.Error;
                this.HelpTextMaxLineLength = Console.WindowWidth;
            }
            else
            {
                this.HelpTextMaxLineLength = Int32.MaxValue;
            }
        }

        /// <summary>
        /// Gets the setup for the parser that allows to configure the parser.
        /// </summary>
        public ParserSetup Setup { get; }

        /// <summary>
        /// Gets the help text of the command with the given name.
        /// </summary>
        /// <param name="commandName">The name of the command to get the help text for.</param>
        /// <param name="includeBanner">Determines if the returned text should contain the banner (which can be set up via (<see cref="ParserSetup.Banner" />) at the beginning.</param>
        /// <returns>The help text of the command with the given name.</returns>
        public String GetCommandHelpText(String commandName, Boolean includeBanner = true)
        {
            var stringBuilder = new StringBuilder();

            if (!String.IsNullOrEmpty(this.Banner) && includeBanner)
            {
                stringBuilder.AppendLine(this.Banner);
                stringBuilder.AppendLine();
            }

            var commandParser = this.CommandParsers.FirstOrDefault(a => a.CommandName == commandName);

            if (commandParser == null)
            {
                stringBuilder.AppendLine($"The command '{commandName}' is unknown.");
                stringBuilder.AppendLine("Try the following command to get a list of valid commands:");
                stringBuilder.AppendLine($"{this.ProgramName} help");
            }
            else
            {
                stringBuilder.Append(commandParser.GetHelpText());
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Gets the error messages for the errors of the given parse result.
        /// If the given parse result does not have errors an empty string is returned.
        /// </summary>
        /// <param name="parseResult">The parse result to get the error messages for.</param>
        /// <param name="includeBanner">Determines if the returned text should contain the banner (which can be set up via (<see cref="ParserSetup.Banner" />) at the beginning.</param>
        /// <returns>The error messages for the errors of the given parse result.</returns>
        public String GetErrorsText(ParseResult parseResult, Boolean includeBanner = true)
        {
            if (!parseResult.HasErrors)
            {
                return "";
            }

            var stringBuilder = new StringBuilder();

            if (!String.IsNullOrEmpty(this.Banner) && includeBanner)
            {
                stringBuilder.AppendLine(this.Banner);
                stringBuilder.AppendLine();
            }

            stringBuilder.AppendLine("Invalid or missing option(s):");

            foreach (var error in parseResult.Errors)
            {
                stringBuilder.AppendLine("- " + error.GetErrorMessage());
            }

            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("Try the following command to get help:");

            stringBuilder.Append($"{this.ProgramName} help");
            if (!String.IsNullOrEmpty(parseResult.CommandName))
            {
                stringBuilder.Append(" ");
                stringBuilder.Append(parseResult.CommandName);
            }
            stringBuilder.AppendLine("");

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Gets the general help text for commands and options.
        /// </summary>
        /// <param name="includeBanner">Determines if the returned text should contain the banner (which can be set up via (<see cref="ParserSetup.Banner" />) at the beginning.</param>
        /// <returns>The general help text for commands and options.</returns>
        public String GetHelpText(Boolean includeBanner = true)
        {
            var stringBuilder = new StringBuilder();

            if (!String.IsNullOrEmpty(this.Banner) && includeBanner)
            {
                stringBuilder.AppendLine(this.Banner);
                stringBuilder.AppendLine();
            }

            var defaultCommandParser = this.CommandParsers.FirstOrDefault(a => a.IsCommandDefault);

            if (defaultCommandParser != null)
            {
                stringBuilder.AppendLine(defaultCommandParser.GetHelpText());
            }

            var nonDefaultCommandParsers = this.CommandParsers.Where(a => !a.IsCommandDefault).ToList();

            if (nonDefaultCommandParsers.Any())
            {
                stringBuilder.AppendLine($"{this.ProgramName} <command> [options]");
                stringBuilder.AppendLine("");
                stringBuilder.AppendLine("Commands:");

                var maxCommandNameLength = nonDefaultCommandParsers.Max(a => a.CommandName!.Length);

                foreach (var commandParser in nonDefaultCommandParsers)
                {
                    stringBuilder.AppendLine($"{commandParser.CommandName!.PadRight(maxCommandNameLength)}\t{commandParser.CommandHelp}");
                }

                stringBuilder.AppendLine("");
            }

            stringBuilder.AppendLine($"{this.ProgramName} help");
            stringBuilder.AppendLine("Prints this help screen.");

            if (nonDefaultCommandParsers.Any())
            {
                stringBuilder.AppendLine("");
                stringBuilder.AppendLine($"{this.ProgramName} help <command>");
                stringBuilder.AppendLine("Prints the help screen for the specified command.");
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Parses the given command line arguments according to the configuration of this instance (see <see cref="Setup" />).
        /// </summary>
        /// <param name="args">The command line arguments to parse.</param>
        /// <returns>The result of the parsing.</returns>
#pragma warning disable S3776 // Cognitive Complexity of methods should not be too high
        public ParseResult Parse(String[] args)
#pragma warning restore S3776 // Cognitive Complexity of methods should not be too high
        {
            if (args.Length == 0)
            {
                // There are no command line arguments specified, so print the help.
                this.PrintHelp();
                return new ParseResult { IsHelpCalled = true };
            }

            if (args.Length == 1 && args[0] == "help")
            {
                // There is only one command line argument and it is 'help', so print the help.
                this.PrintHelp();
                return new ParseResult { IsHelpCalled = true };
            }

            if (args.Length == 2 && args[0] == "help")
            {
                // There are only two command line argument and the first one is 'help', so the second must be a command name, so print the help for that command.
                this.PrintCommandHelp(args[1]);
                return new ParseResult { IsHelpCalled = true };
            }

            var result = new ParseResult();

            var tokens = CommandLineArgumentsTokenizer.Tokenize(args).ToList();

            var commandTokens = tokens.OfType<CommandToken>().ToList();

            if (commandTokens.Count == 0 && !this.CommandParsers.Any(a => a.IsCommandDefault))
            {
                result.AddError(new MissingCommandError());
                this.PrintErrors(result);
            }
            else if (commandTokens.Count > 1)
            {
                result.AddError(new MoreThanOneCommandError());
                this.PrintErrors(result);
            }
            else
            {
                var duplicateOptions = tokens.OfType<OptionToken>().GroupBy(a => a.OptionName).Where(a => a.Count() > 1).ToList();

                if (duplicateOptions.Any())
                {
                    duplicateOptions.ForEach(a => result.AddError(new DuplicateOptionError(a.Key)));
                    this.PrintErrors(result);
                }
                else
                {
                    this.CommandParsers.ForEach(a => a.Parse(tokens, result));
                    this.CommandParsers.ForEach(a => a.Validate(tokens, result));

                    tokens.OfType<CommandToken>().Where(a => !a.IsParsed).ToList().ForEach(a => result.AddError(new UnknownCommandError(a.CommandName)));

                    if (!this.IgnoreUnknownOptions)
                    {
                        tokens.OfType<OptionToken>().Where(a => !a.IsParsed).ToList().ForEach(a => result.AddError(new UnknownOptionError(a.OptionName)));
                    }

                    if (result.HasErrors)
                    {
                        this.PrintErrors(result);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Defines a banner text to display at the beginning of help texts and error texts (e.g. in the return value of <see cref="Parser.GetHelpText" /> or <see cref="Parser.GetErrorsText" />).
        /// </summary>
        internal String Banner { get; set; }

        /// <summary>
        /// Defines a list of parsers for commands.
        /// </summary>
        internal virtual List<ICommandParser> CommandParsers { get; }

        /// <summary>
        /// Defines the text writer to write error messages to.
        /// </summary>
        /// <remarks>The default is <see cref="Console.Error"/>.</remarks>
        internal TextWriter? ErrorTextWriter { get; set; }

        /// <summary>
        /// Defines the text writer to write help messages to.
        /// </summary>
        /// <remarks>The default is <see cref="Console.Out"/>.</remarks>
        internal TextWriter? HelpTextWriter { get; set; }

        /// <summary>
        /// Defines the maximum length a line of a help text can have.
        /// If not explicitly set via <see cref="ParserSetup.HelpTextMaxLineLength" /> the current width of the console width is used or, if no console is available, <see cref="Int32.MaxValue" /> is used.
        /// </summary>
        internal Int32 HelpTextMaxLineLength { get; set; }

        /// <summary>
        /// Determines whether to ignore options that are unknown when options are parsed.
        /// </summary>
        internal Boolean IgnoreUnknownOptions { get; set; }

        /// <summary>
        /// Defines the name of the program to display in help texts.
        /// </summary>
        internal String ProgramName { get; set; }

        private void PrintCommandHelp(String command)
        {
            this.HelpTextWriter?.Write(this.GetCommandHelpText(command));
        }

        private void PrintErrors(ParseResult parseResult)
        {
            this.ErrorTextWriter?.Write(this.GetErrorsText(parseResult));
        }

        private void PrintHelp()
        {
            this.HelpTextWriter?.Write(this.GetHelpText());
        }

        private static Boolean IsConsolePresent()
        {
            try
            {
#pragma warning disable S1481 // Unused local variables should be removed
                var windowHeight = Console.WindowHeight;
#pragma warning restore S1481 // Unused local variables should be removed
                return true;
            }
            catch (IOException)
            {
                return false;
            }
        }
    }
}