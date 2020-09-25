using System;

namespace ParseTheArgs.Errors
{
    /// <summary>
    /// Represents an error that indicates that no command is given.
    /// </summary>
    public class MissingCommandError : IParseError
    {
        /// <summary>
        /// Gets the error message that describes the error.
        /// </summary>
        /// <returns>The error message that describes the error.</returns>
        public String GetErrorMessage()
        {
            return "No command was specified.";
        }
    }
}