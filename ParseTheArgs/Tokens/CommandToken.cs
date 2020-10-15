using System;

namespace ParseTheArgs.Tokens
{
    /// <summary>
    /// A token that represents a command line command.
    /// </summary>
    public sealed class CommandToken : Token
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="commandName">The name of the command.</param>
        public CommandToken(String commandName)
        {
            this.CommandName = commandName;
        }

        /// <summary>
        /// The name of the command.
        /// </summary>
        public String CommandName { get; }
    }
}