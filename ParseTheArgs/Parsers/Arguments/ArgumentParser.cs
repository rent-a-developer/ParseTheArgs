using System;
using System.Collections.Generic;
using System.Reflection;
using ParseTheArgs.Tokens;

namespace ParseTheArgs.Parsers.Arguments
{
    /// <summary>
    /// Parses a command line argument.
    /// </summary>
    public abstract class ArgumentParser
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="targetProperty">The property where the value of the argument will be stored.</param>
        /// <param name="argumentName">The name of the argument the parser parses.</param>
        protected ArgumentParser(PropertyInfo targetProperty, ArgumentName argumentName)
        {
            this.TargetProperty = targetProperty;
            this.ArgumentName = argumentName;

            this.ArgumentHelp = String.Empty;
        }

        /// <summary>
        /// Defines the property where the value of the argument will be stored.
        /// </summary>
        public virtual PropertyInfo TargetProperty { get; }

        /// <summary>
        /// The name of the argument the parser parses.
        /// </summary>
        public virtual ArgumentName ArgumentName { get; set; }

        /// <summary>
        /// The type of the argument the parser parses.
        /// </summary>
        public virtual ArgumentType ArgumentType => ArgumentType.ValuelessArgument;

        /// <summary>
        /// Determines if the argument is required.
        /// </summary>
        public virtual Boolean IsArgumentRequired { get; set; }

        /// <summary>
        /// Defines the help text of the argument.
        /// </summary>
        public String ArgumentHelp { get; set; }

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
        public abstract void Parse(List<Token> tokens, ParseResult parseResult);
    }
}