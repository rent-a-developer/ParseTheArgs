using System;
using System.IO;
using ParseTheArgs.Extensions;
using ParseTheArgs.Setup.Commands;

namespace ParseTheArgs.Setup
{
    /// <summary>
    /// Represents the configuration of a <see cref="Parser" />.
    /// Can be used to configure a parser (e.g. setup commands and options).
    /// </summary>
    public class ParserSetup
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="parser">The parser this instance should set up.</param>
        /// <exception cref="ArgumentNullException"><paramref name="parser"/> is null.</exception>
        internal ParserSetup(Parser parser)
        {
            if (parser == null)
            {
                throw new ArgumentNullException(nameof(parser));
            }

            this.parser = parser;
        }

        /// <summary>
        /// Sets the name of the program to display in help texts.
        /// 
        /// Initially (until a program name is set by calling this method) the name of the current process will be used as the program name.
        /// </summary>
        /// <param name="programName">The name of the program to display in help texts.</param>
        /// <returns>A reference to this instance for further configuration.</returns>
        public ParserSetup ProgramName(String programName)
        {
            this.parser.ProgramName = programName;
            return this;
        }

        /// <summary>
        /// Sets a banner text to display at the beginning of help texts and error texts (e.g. in the return value of <see cref="Parser.GetHelpText" /> or <see cref="Parser.GetErrorsText" />).
        /// </summary>
        /// <param name="banner">The banner text to display in help texts.</param>
        /// <returns>A reference to this instance for further configuration.</returns>
        public ParserSetup Banner(String banner)
        {
            this.parser.Banner = banner;
            return this;
        }

        /// <summary>
        /// Sets the maximum length a line of a help text can have.
        /// When this method is not called, the current width of the console window is used.
        /// </summary>
        /// <param name="maxLineLength">The maximum length a line of a help text can have.</param>
        /// <returns>A reference to this instance for further configuration.</returns>
        public ParserSetup HelpTextMaxLineLength(Int32 maxLineLength)
        {
            this.parser.HelpTextMaxLineLength = maxLineLength;
            return this;
        }

        /// <summary>
        /// Sets up a non-default (named) command.
        /// Initially (until a name is set through <see cref="NamedCommandSetup{TCommandOptions}.Name" />) the name of the given <typeparamref name="TCommandOptions" /> type (converted to lower camel case, see <see cref="StringExtensions.ToCamelCase" />) will be used as name for the command.
        /// 
        /// When the command specified on the command line matches the name of the command an instance of <typeparamref name="TCommandOptions" /> will be instantiated and stored in <see cref="ParseResult.CommandOptions" />.
        /// The values of options specified on the command line for the command will be stored in that instance.
        /// </summary>
        /// <typeparam name="TCommandOptions">The type where the values of the options of the command will be stored in.</typeparam>
        /// <returns>An instance of <see cref="NamedCommandSetup{TCommandOptions}" /> that can be used to configure the command.</returns>
        public NamedCommandSetup<TCommandOptions> Command<TCommandOptions>() where TCommandOptions : class, new()
        {
            var commandName = typeof(TCommandOptions).Name.ToCamelCase().Replace("Options", "");
            var commandParser = this.parser.GetOrCreateCommandParser<TCommandOptions>(commandName);

            return new NamedCommandSetup<TCommandOptions>(this.parser, commandParser);
        }

        /// <summary>
        /// Sets up the default (unnamed) command.
        /// 
        /// When no command is specified on the command line an instance of <typeparamref name="TCommandOptions" /> will be instantiated and stored in <see cref="ParseResult.CommandOptions" />.
        /// The values of options specified on the command line for the command will be stored in that instance.
        /// </summary>
        /// <typeparam name="TCommandOptions">The type where the values of the options of the command will be stored in.</typeparam>
        /// <returns>An instance of <see cref="DefaultCommandSetup{TCommandOptions}" /> that can be used to configure the command.</returns>
        public DefaultCommandSetup<TCommandOptions> DefaultCommand<TCommandOptions>() where TCommandOptions : class, new()
        {
            var commandParser = parser.GetOrCreateCommandParser<TCommandOptions>();

            return new DefaultCommandSetup<TCommandOptions>(this.parser, commandParser);
        }

        /// <summary>
        /// Sets the text writer to write error messages to.
        /// When the arguments parsed cause errors (e.g. missing options) an error message is written to this text writer.
        /// </summary>
        /// <param name="textWriter">The text writer to write error messages to.</param>
        /// <returns>A reference to this instance for further configuration.</returns>
        /// <remarks>The default is <see cref="Console.Error"/>.</remarks>
        public ParserSetup ErrorTextWriter(TextWriter? textWriter)
        {
            this.parser.ErrorTextWriter = textWriter;
            return this;
        }

        /// <summary>
        /// Sets the text writer to write help messages to.
        /// When no options are given and there is no default command set up, the general help text will be written to this text writer (same text as <see cref="Parser.GetHelpText" /> returns).
        /// When exactly one option with the value "help" is given, the general help text will be written to this text writer (same text as <see cref="Parser.GetHelpText" /> returns).
        /// When exactly two options are given and the first has the value "help", the second is assumed to be the name of a command and the help text for the command will be written to this text writer (same text as <see cref="Parser.GetCommandHelpText" /> returns).
        /// </summary>
        /// <param name="textWriter">The text writer to write help messages to.</param>
        /// <returns>A reference to this instance for further configuration.</returns>
        /// <remarks>The default is <see cref="Console.Out"/>.</remarks>
        public ParserSetup HelpTextWriter(TextWriter? textWriter)
        {
            this.parser.HelpTextWriter = textWriter;
            return this;
        }

        /// <summary>
        /// Specifies to ignore options that are unknown when options are parsed.
        /// 
        /// The default behavior is that unknown options will cause parse errors (see <see cref="ParseResult.Errors" />) when they encountered during parsing.
        /// After this method has been called unknown options will be ignored, so they will not cause errors.
        /// </summary>
        /// <returns>A reference to this instance for further configuration.</returns>
        public ParserSetup IgnoreUnknownOptions()
        {
            this.parser.IgnoreUnknownOptions = true;
            return this;
        }

        private readonly Parser parser;
    }
}