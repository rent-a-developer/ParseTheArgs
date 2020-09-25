using System;

namespace ParseTheArgs.Errors
{
    /// <summary>
    /// Represents an error that indicates that a required argument is missing.
    /// </summary>
    public class ArgumentMissingError : ArgumentError
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="argumentName">The name of the argument that is missing.</param>
        public ArgumentMissingError(ArgumentName argumentName) : base(argumentName)
        {
        }

        /// <summary>
        /// Gets the error message that describes the error.
        /// </summary>
        /// <returns>The error message that describes the error.</returns>
        public override String GetErrorMessage()
        {
            return $"The argument {this.ArgumentName} is missing.";
        }
    }
}