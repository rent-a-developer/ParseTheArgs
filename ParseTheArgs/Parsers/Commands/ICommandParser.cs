using System;
using System.Collections.Generic;
using ParseTheArgs.Parsers.Arguments;
using ParseTheArgs.Tokens;

namespace ParseTheArgs.Parsers.Commands
{
    /// <summary>
    /// Represents a parser that parses a command line command and its arguments.
    /// </summary>
    public interface ICommandParser
    {
        /// <summary>
        /// Defines the list of argument parsers for the command.
        /// </summary>
        List<ArgumentParser> ArgumentParsers { get; }

        /// <summary>
        /// Defines the help text of the command.
        /// </summary>
        String CommandHelp { get; }

        /// <summary>
        /// Defines the name of the command.
        /// Will be null, if the command is the default command (see <see cref="IsCommandDefault" />).
        /// </summary>
        String? CommandName { get; }

        /// <summary>
        /// Determines if the command is the default (non-named) command.
        /// If the command is the default command <see cref="CommandName" /> will be null.
        /// </summary>
        Boolean IsCommandDefault { get; }

        /// <summary>
        /// Gets the help text of the argument.
        /// </summary>
        /// <returns>The help text of the argument.</returns>
        String GetHelpText();

        /// <summary>
        /// Parses the given tokens and puts the result of the parsing into the given parse result object.
        /// </summary>
        /// <param name="tokens">The tokens to parse.</param>
        /// <param name="parseResult">The parse result to put result of the parsing into.</param>
        void Parse(List<Token> tokens, ParseResult parseResult);

        /// <summary>
        /// Validates the given tokens and puts the result of the validation into the given parse result object.
        /// </summary>
        /// <param name="tokens">The tokens to validate.</param>
        /// <param name="parseResult">The parse result to put result of the validation into.</param>
        void Validate(List<Token> tokens, ParseResult parseResult);
    }
}