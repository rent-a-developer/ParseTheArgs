using System;

namespace ParseTheArgs.Errors
{
    /// <summary>
    /// Represents an error that indicates that the value of an option has an invalid format.
    /// </summary>
    public class OptionValueFormatError : OptionError
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="optionName">The name of the option whose value has an invalid format.</param>
        /// <param name="invalidOptionValue">The option value that has an invalid format.</param>
        /// <param name="expectedValueFormat">The format the value was expected to have.</param>
        public OptionValueFormatError(String optionName, String invalidOptionValue, String expectedValueFormat) : base(optionName)
        {
            this.InvalidOptionValue = invalidOptionValue;
            this.ExpectedValueFormat = expectedValueFormat;
        }

        /// <summary>
        /// The format the value was expected to have.
        /// </summary>
        public String ExpectedValueFormat { get; }

        /// <summary>
        /// The option value that has an invalid format.
        /// </summary>
        public String InvalidOptionValue { get; }

        /// <summary>
        /// Gets the error message that describes the error.
        /// </summary>
        /// <returns>The error message that describes the error.</returns>
        public override String GetErrorMessage()
        {
            return $"The value '{this.InvalidOptionValue}' of the option --{this.OptionName} has an invalid format. The expected format is: {this.ExpectedValueFormat}.";
        }
    }
}