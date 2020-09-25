using System;

namespace ParseTheArgs.Tokens
{
    /// <summary>
    /// Represents a command line arguments token.
    /// </summary>
    public abstract class CommandLineArgumentsToken
    {
        /// <summary>
        /// Determines if the token has already been parsed.
        /// </summary>
        public Boolean IsParsed { get; set; }
    }
}