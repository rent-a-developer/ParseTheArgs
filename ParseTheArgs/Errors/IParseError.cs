using System;

namespace ParseTheArgs.Errors
{
    /// <summary>
    /// Represents an error that indicates that there is an issue with the command line arguments which where parsed.
    /// </summary>
    public interface IParseError
    {
        /// <summary>
        /// Gets the error message that describes the error.
        /// </summary>
        /// <returns>The error message that describes the error.</returns>
        String GetErrorMessage();
    }
}