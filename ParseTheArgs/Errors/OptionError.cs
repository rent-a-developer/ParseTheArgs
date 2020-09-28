using System;

namespace ParseTheArgs.Errors
{
    /// <summary>
    /// Represents an error that indicates an issue with an option.
    /// </summary>
    public abstract class OptionError : IParseError
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="optionName">The name of the option which caused the error.</param>
        protected OptionError(String optionName)
        {
            this.OptionName = optionName;
        }

        /// <summary>
        /// The name of the option which caused the error.
        /// </summary>
        public String OptionName { get; }

        /// <summary>
        /// Gets the error message that describes the error.
        /// </summary>
        /// <returns>The error message that describes the error.</returns>
        public abstract String GetErrorMessage();
    }
}