using System;
using System.Reflection;
using ParseTheArgs.Errors;

namespace ParseTheArgs.Parsers.Options
{
    /// <summary>
    /// Parses a command line option that accepts a single <see cref="Guid" /> value.
    /// </summary>
    public class GuidOptionParser : SingleValueOptionParser<Guid>
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="targetProperty">The property where the value of the option will be stored.</param>
        /// <param name="optionName">The name of the option the parser parses.</param>
        public GuidOptionParser(PropertyInfo targetProperty, String optionName) : base(targetProperty, optionName)
        {
        }

        /// <summary>
        /// Defines the format to use when parsing the option value to a <see cref="Guid" />.
        /// For supported formats see the documentation of <see cref="Guid.Parse(string)" />.
        /// </summary>
        public String? GuidFormat { get; set; }

        /// <summary>
        /// Parses the given value to the desired option value type of the option parser.
        /// </summary>
        /// <param name="optionValue">The option value to parse.</param>
        /// <param name="parseResult">The parse result to put parse errors in if a parse error occurred.</param>
        /// <param name="resultValue">The parsed value.</param>
        /// <returns>True if the given option value could be parsed; otherwise false.</returns>
        protected override Boolean TryParseValue(String optionValue, ParseResult parseResult, out Guid resultValue)
        {
            if (!String.IsNullOrEmpty(this.GuidFormat))
            {
                if (!Guid.TryParseExact(optionValue, this.GuidFormat, out resultValue))
                {
                    parseResult.AddError(new OptionValueInvalidFormatError(this.OptionName, optionValue, "A valid Guid"));
                    return false;
                }

                return true;
            }
            else
            {
                if (!Guid.TryParse(optionValue, out resultValue))
                {
                    parseResult.AddError(new OptionValueInvalidFormatError(this.OptionName, optionValue, "A valid Guid"));
                    return false;
                }

                return true;
            }
        }
    }
}