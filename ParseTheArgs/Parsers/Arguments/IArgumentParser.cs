using System;
using System.Collections.Generic;
using System.Reflection;
using ParseTheArgs.Tokens;

namespace ParseTheArgs.Parsers.Arguments
{
    /// <summary>
    /// Represents a parser that parses a command line argument.
    /// </summary>
    public interface IArgumentParser
    {
        /// <summary>
        /// The name of the argument the parser parses.
        /// </summary>
        ArgumentName ArgumentName { get; }

        /// <summary>
        /// The type of the argument the parser parses.
        /// </summary>
        ArgumentType ArgumentType { get; }

        /// <summary>
        /// Determines if the argument is required.
        /// </summary>
        Boolean IsArgumentRequired { get; }

        /// <summary>
        /// Defines the property where the value of the argument will be stored.
        /// </summary>
        PropertyInfo TargetProperty { get; set; }

        /// <summary>
        /// Gets the help text of the argument.
        /// </summary>
        /// <returns></returns>
        String GetHelpText();

        /// <summary>
        /// Parses the given tokens and puts the result of the parsing into the given parse result object.
        /// </summary>
        /// <param name="tokens">The tokens to parse.</param>
        /// <param name="parseResult">The parse result to put result of the parsing into.</param>
        void Parse(List<CommandLineArgumentsToken> tokens, ParseResult parseResult);

        /// <summary>
        /// Validates the given tokens and puts the result of the validation into the given parse result object.
        /// </summary>
        /// <param name="tokens">The tokens to validate.</param>
        /// <param name="parseResult">The parse result to put result of the validation into.</param>
        void Validate(List<CommandLineArgumentsToken> tokens, ParseResult parseResult);
    }
}