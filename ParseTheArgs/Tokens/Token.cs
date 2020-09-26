using System;

namespace ParseTheArgs.Tokens
{
    /// <summary>
    /// Represents a token of a command line.
    /// </summary>
    public abstract class Token
    {
        /// <summary>
        /// Determines if the token has already been parsed.
        /// </summary>
        public Boolean IsParsed { get; set; }
    }
}