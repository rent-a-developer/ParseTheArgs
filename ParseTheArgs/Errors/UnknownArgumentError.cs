using System;

namespace ParseTheArgs.Errors
{
    /// <summary>
    /// Represents an error that indicates that an unknown argument is given.
    /// </summary>
    public class UnknownArgumentError : ArgumentError
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="argumentName">The name of the argument that is unknown.</param>
        public UnknownArgumentError(ArgumentName argumentName) : base(argumentName)
        {
        }

        /// <summary>
        /// Gets the error message that describes the error.
        /// </summary>
        /// <returns>The error message that describes the error.</returns>
        public override String GetErrorMessage()
        {
            return $"The argument {this.ArgumentName} is unknown.";
        }
    }
}