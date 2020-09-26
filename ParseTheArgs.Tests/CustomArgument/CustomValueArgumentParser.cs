using System;
using System.Reflection;
using ParseTheArgs.Parsers.Arguments;

namespace ParseTheArgs.Tests.CustomArgument
{
    public class CustomValueArgumentParser : SingleValueArgumentParser<CustomValue>
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public CustomValueArgumentParser(PropertyInfo targetProperty, ArgumentName argumentName) : base(targetProperty, argumentName)
        {
        }

        protected override Boolean TryParseValue(String argumentValue, ParseResult parseResult, out CustomValue resultValue)
        {
            resultValue = new CustomValue(argumentValue);
            return true;
        }
    }
}