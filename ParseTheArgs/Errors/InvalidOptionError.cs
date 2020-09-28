using System;

namespace ParseTheArgs.Errors
{
    /// <summary>
    /// Represents an error that indicates that an option is invalid.
    /// </summary>
    public class InvalidOptionError : OptionError
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="optionName">The name of the option which is invalid.</param>
        /// <param name="message">The message that describes why the option is invalid.</param>
        public InvalidOptionError(String optionName, String message) : base(optionName)
        {
            this.Message = message;
        }

        /// <summary>
        /// The message that describes why the option is invalid
        /// </summary>
        public String Message { get; }

        /// <summary>
        /// Gets the error message that describes the error.
        /// </summary>
        /// <returns>The error message that describes the error.</returns>
        public override String GetErrorMessage()
        {
            return $"The option --{this.OptionName} is invalid: {this.Message}";
        }
    }
}