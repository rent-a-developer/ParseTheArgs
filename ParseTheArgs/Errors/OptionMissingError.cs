using System;

namespace ParseTheArgs.Errors
{
    /// <summary>
    /// Represents an error that indicates that a required option is missing.
    /// </summary>
    public class OptionMissingError : OptionError
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="optionName">The name of the option that is missing.</param>
        public OptionMissingError(String optionName) : base(optionName)
        {
        }

        /// <summary>
        /// Gets the error message that describes the error.
        /// </summary>
        /// <returns>The error message that describes the error.</returns>
        public override String GetErrorMessage()
        {
            return $"The option --{this.OptionName} is required.";
        }
    }
}