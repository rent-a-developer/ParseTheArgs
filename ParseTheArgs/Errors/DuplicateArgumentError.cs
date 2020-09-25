using System;

namespace ParseTheArgs.Errors
{
    /// <summary>
    /// Represents an error that indicates that an argument is given multiple times.
    /// </summary>
    public class DuplicateArgumentError : ArgumentError
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="argumentName">The name of the argument that is given multiple times.</param>
        public DuplicateArgumentError(ArgumentName argumentName) : base(argumentName)
        {
        }

        /// <summary>
        /// Gets the error message that describes the error.
        /// </summary>
        /// <returns>The error message that describes the error.</returns>
        public override String GetErrorMessage()
        {
            return $"The argument {this.ArgumentName} is used more than once. Please only use each argument once.";
        }
    }
}