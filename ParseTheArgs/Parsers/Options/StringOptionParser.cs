using System;
using System.Reflection;

namespace ParseTheArgs.Parsers.Options
{
    /// <summary>
    /// Parses a command line option that accepts a single <see cref="String" /> value.
    /// </summary>
    public class StringOptionParser : SingleValueOptionParser<String>
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="targetProperty">The property where the value of the option will be stored.</param>
        /// <param name="optionName">The name of the option the parser parses.</param>
        public StringOptionParser(PropertyInfo targetProperty, String optionName) : base(targetProperty, optionName)
        {
        }

        /// <summary>
        /// Parses the given value to the desired option value type of the option parser.
        /// </summary>
        /// <param name="optionValue">The option value to parse.</param>
        /// <param name="parseResult">The parse result to put parse errors in if a parse error occurred.</param>
        /// <param name="resultValue">The parsed value.</param>
        /// <returns>True if the given option value could be parsed; otherwise false.</returns>
        protected override Boolean TryParseValue(String optionValue, ParseResult parseResult, out String resultValue)
        {
            resultValue = optionValue;
            return true;
        }
    }
}