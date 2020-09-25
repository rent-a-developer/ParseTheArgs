using System;
using System.Collections.Generic;
using System.Reflection;
using ParseTheArgs.Parsers.Commands;
using ParseTheArgs.Tokens;

namespace ParseTheArgs.Parsers.Arguments
{
    /// <summary>
    /// Parses a command line argument.
    /// </summary>
    /// <typeparam name="TCommandArguments">The type in which the value of the argument (of the command the argument belongs to) will be stored.</typeparam>
    public abstract class ArgumentParser<TCommandArguments> : IArgumentParser
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        protected ArgumentParser()
        {
            this.ArgumentName = new ArgumentName();
        }

        /// <summary>
        /// Defines the help text of the argument.
        /// </summary>
        public String ArgumentHelp { get; set; }

        /// <summary>
        /// The name of the argument the parser parses.
        /// </summary>
        public ArgumentName ArgumentName { get; }

        /// <summary>
        /// The type of the argument the parser parses.
        /// </summary>
        public virtual ArgumentType ArgumentType => ArgumentType.ValuelessArgument;

        /// <summary>
        /// Defines the command parser of the command the argument this this parser parses belongs to.
        /// </summary>
        public CommandParser<TCommandArguments> CommandParser { get; set; }

        /// <summary>
        /// Determines if the argument is required.
        /// </summary>
        public virtual Boolean IsArgumentRequired { get; set; }

        /// <summary>
        /// Defines the property (of the <typeparamref name="TCommandArguments" /> type) where the value of the argument will be stored.
        /// </summary>
        public PropertyInfo TargetProperty { get; set; }

        /// <summary>
        /// Gets the help text of the argument.
        /// </summary>
        /// <returns>The help text of the argument.</returns>
        public virtual String GetHelpText()
        {
            return this.ArgumentHelp;
        }

        /// <summary>
        /// Parses the given tokens and puts the result of the parsing into the given parse result object.
        /// </summary>
        /// <param name="tokens">The tokens to parse.</param>
        /// <param name="parseResult">The parse result to put result of the parsing into.</param>
        public abstract void Parse(List<CommandLineArgumentsToken> tokens, ParseResult parseResult);

        /// <summary>
        /// Validates the given tokens and puts the result of the validation into the given parse result object.
        /// </summary>
        /// <param name="tokens">The tokens to validate.</param>
        /// <param name="parseResult">The parse result to put result of the validation into.</param>
        public virtual void Validate(List<CommandLineArgumentsToken> tokens, ParseResult parseResult)
        {
        }
    }
}