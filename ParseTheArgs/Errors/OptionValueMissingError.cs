using System;

namespace ParseTheArgs.Errors
{
    /// <summary>
    /// Represents an error that indicates that no value is given for an option that requires a value.
    /// </summary>
    public class OptionValueMissingError : OptionError
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="optionName">The name of the option for which the value is missing.</param>
        public OptionValueMissingError(String optionName) : base(optionName)
        {
        }

        /// <summary>
        /// Gets the error message that describes the error.
        /// </summary>
        /// <returns>The error message that describes the error.</returns>
        public override String GetErrorMessage()
        {
            return $"The option --{this.OptionName} requires a value, but no value was specified.";
        }
    }
}