using System;

namespace ParseTheArgs.Errors
{
    /// <summary>
    /// Represents an error that indicates that more than one command is given.
    /// </summary>
    public class MoreThanOneCommandError : IParseError
    {
        /// <summary>
        /// Gets the error message that describes the error.
        /// </summary>
        /// <returns>The error message that describes the error.</returns>
        public String GetErrorMessage()
        {
            return "More than one command specified. Please only specify one command.";
        }
    }
}