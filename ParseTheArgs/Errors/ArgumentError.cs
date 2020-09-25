using System;

namespace ParseTheArgs.Errors
{
    /// <summary>
    /// Represents an error that indicates an issue with an argument.
    /// </summary>
    public abstract class ArgumentError : IParseError
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="argumentName">The name of the argument which caused the error.</param>
        protected ArgumentError(ArgumentName argumentName)
        {
            this.ArgumentName = argumentName;
        }

        /// <summary>
        /// The name of the argument which caused the error.
        /// </summary>
        public ArgumentName ArgumentName { get; }

        /// <summary>
        /// Gets the error message that describes the error.
        /// </summary>
        /// <returns>The error message that describes the error.</returns>
        public abstract String GetErrorMessage();
    }
}