using System;

namespace ParseTheArgs.Errors
{
    /// <summary>
    /// Represents an error that indicates that the value of an argument has an invalid format.
    /// </summary>
    public class ArgumentValueFormatError : ArgumentError
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="argumentName">The name of the argument whose value has an invalid format.</param>
        /// <param name="invalidArgumentValue">The argument value that has an invalid format</param>
        /// <param name="expectedValueFormat">The format the value was expected to have.</param>
        public ArgumentValueFormatError(ArgumentName argumentName, String invalidArgumentValue, String expectedValueFormat) : base(argumentName)
        {
            this.InvalidArgumentValue = invalidArgumentValue;
            this.ExpectedValueFormat = expectedValueFormat;
        }

        /// <summary>
        /// The format the value was expected to have.
        /// </summary>
        public String ExpectedValueFormat { get; }

        /// <summary>
        /// The argument value that has an invalid format.
        /// </summary>
        public String InvalidArgumentValue { get; }

        /// <summary>
        /// Gets the error message that describes the error.
        /// </summary>
        /// <returns>The error message that describes the error.</returns>
        public override String GetErrorMessage()
        {
            return $"The value '{this.InvalidArgumentValue}' of the argument {this.ArgumentName} has an invalid format. The expected format is: {this.ExpectedValueFormat}.";
        }
    }
}