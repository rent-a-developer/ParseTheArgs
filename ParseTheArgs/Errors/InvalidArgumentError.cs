using System;

namespace ParseTheArgs.Errors
{
    /// <summary>
    /// Represents an error that indicates that an argument is invalid.
    /// </summary>
    public class InvalidArgumentError : ArgumentError
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="argumentName">The name of the argument which is invalid.</param>
        /// <param name="message">The message that describes why the argument is invalid.</param>
        public InvalidArgumentError(String argumentName, String message) : base(argumentName)
        {
            this.Message = message;
        }

        /// <summary>
        /// The message that describes why the argument is invalid
        /// </summary>
        public String Message { get; }

        /// <summary>
        /// Gets the error message that describes the error.
        /// </summary>
        /// <returns>The error message that describes the error.</returns>
        public override String GetErrorMessage()
        {
            return $"The argument --{this.ArgumentName} is invalid: {this.Message}";
        }
    }
}