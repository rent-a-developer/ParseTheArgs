using System;

namespace ParseTheArgs.Errors
{
    /// <summary>
    /// Represents an error that indicates that no value is given for an argument that requires a value.
    /// </summary>
    public class ArgumentValueMissingError : ArgumentError
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="argumentName">The name of the argument for which the value is missing.</param>
        public ArgumentValueMissingError(String argumentName) : base(argumentName)
        {
        }

        /// <summary>
        /// Gets the error message that describes the error.
        /// </summary>
        /// <returns>The error message that describes the error.</returns>
        public override String GetErrorMessage()
        {
            return $"The value for the argument --{this.ArgumentName} is missing.";
        }
    }
}