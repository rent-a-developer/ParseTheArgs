using System;
using System.Collections.Generic;

namespace ParseTheArgs.Tokens
{
    /// <summary>
    /// A token that represents a command line option.
    /// </summary>
    public sealed class OptionToken : Token
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="optionName">The name of the option.</param>
        public OptionToken(String optionName)
        {
            this.OptionName = optionName;
            this.OptionValues = new List<String>();
        }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="optionName">The name of the option.</param>
        /// <param name="optionValues">The values of the option.</param>
        public OptionToken(String optionName, List<String> optionValues) : this(optionName)
        {
            this.OptionValues = optionValues;
        }

        /// <summary>
        /// The name of the option.
        /// </summary>
        public String OptionName { get; }

        /// <summary>
        /// The values of the option.
        /// </summary>
        public List<String> OptionValues { get; }
    }
}