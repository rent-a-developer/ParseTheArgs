using System;

namespace ParseTheArgs.Errors
{
    /// <summary>
    /// Represents an error that indicates that multiple values are given for an option that only supports a single value.
    /// </summary>
    public class OptionMultipleValuesError : OptionError
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="optionName">The name of the option for which multiple values are given.</param>
        public OptionMultipleValuesError(String optionName) : base(optionName)
        {
        }

        /// <summary>
        /// Gets the error message that describes the error.
        /// </summary>
        /// <returns>The error message that describes the error.</returns>
        public override String GetErrorMessage()
        {
            return $"Multiple values are given for the option --{this.OptionName}, but the option expects a single value.";
        }
    }
}