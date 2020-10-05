using System;
using System.Collections.Generic;
using System.Reflection;
using ParseTheArgs.Tokens;

namespace ParseTheArgs.Parsers.Options
{
    /// <summary>
    /// Parses a command line option.
    /// </summary>
    public abstract class OptionParser
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="targetProperty">The property where the value of the option will be stored.</param>
        /// <param name="optionName">The name of the option the parser parses.</param>
        protected OptionParser(PropertyInfo targetProperty, String optionName)
        {
            this.TargetProperty = targetProperty;
            this.OptionName = optionName;

            this.OptionHelp = String.Empty;
        }

        /// <summary>
        /// Defines the property where the value of the option will be stored.
        /// </summary>
        public virtual PropertyInfo TargetProperty { get; }

        /// <summary>
        /// The name of the option the parser parses.
        /// </summary>
        public virtual String OptionName { get; set; }

        /// <summary>
        /// The type of the option the parser parses.
        /// </summary>
        public abstract OptionType OptionType { get; }

        /// <summary>
        /// Determines if the option is required.
        /// </summary>
        public virtual Boolean IsOptionRequired { get; set; }

        /// <summary>
        /// Defines the help text of the option.
        /// </summary>
        public String OptionHelp { get; set; }

        /// <summary>
        /// Gets the help text of the option.
        /// </summary>
        /// <returns>The help text of the option.</returns>
        public virtual String GetHelpText()
        {
            return this.OptionHelp;
        }

        /// <summary>
        /// Parses the given tokens and puts the result of the parsing into the given parse result object.
        /// </summary>
        /// <param name="tokens">The tokens to parse.</param>
        /// <param name="parseResult">The object where to put result of the parsing into.</param>
        public abstract void Parse(List<Token> tokens, ParseResult parseResult);

        internal virtual ValueParser ValueParser { get; set; } = new ValueParser();
    }
}