using System;

namespace ParseTheArgs.Errors
{
    /// <summary>
    /// Represents an error that indicates that multiple values are given for an argument that only supports a single value.
    /// </summary>
    public class ArgumentMultipleValuesError : ArgumentError
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="argumentName">The name of the argument for which multiple values are given.</param>
        public ArgumentMultipleValuesError(ArgumentName argumentName) : base(argumentName)
        {
        }

        /// <summary>
        /// Gets the error message that describes the error.
        /// </summary>
        /// <returns>The error message that describes the error.</returns>
        public override String GetErrorMessage()
        {
            return $"Multiple values are given for the argument {this.ArgumentName}, but the argument does not support multiple values.";
        }
    }
}