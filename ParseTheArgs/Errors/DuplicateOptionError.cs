using System;

namespace ParseTheArgs.Errors
{
    /// <summary>
    /// Represents an error that indicates that an option is given multiple times.
    /// </summary>
    public class DuplicateOptionError : OptionError
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="optionName">The name of the option that is given multiple times.</param>
        public DuplicateOptionError(String optionName) : base(optionName)
        {
        }

        /// <summary>
        /// Gets the error message that describes the error.
        /// </summary>
        /// <returns>The error message that describes the error.</returns>
        public override String GetErrorMessage()
        {
            return $"The option --{this.OptionName} is used more than once. Please only use each option once.";
        }
    }
}