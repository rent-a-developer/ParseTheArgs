using System;

namespace ParseTheArgs.Errors
{
    /// <summary>
    /// Represents an error that indicates that an unknown command is given.
    /// </summary>
    public class UnknownCommandError : IParseError
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="commandName">The name of the command that is unknown.</param>
        public UnknownCommandError(String commandName)
        {
            this.CommandName = commandName;
        }

        /// <summary>
        /// The name of the command that is unknown.
        /// </summary>
        public String CommandName { get; }

        /// <summary>
        /// Gets the error message that describes the error.
        /// </summary>
        /// <returns>The error message that describes the error.</returns>
        public String GetErrorMessage()
        {
            return $"The command '{this.CommandName}' is unknown.";
        }
    }
}