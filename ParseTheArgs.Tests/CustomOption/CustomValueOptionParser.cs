using System;
using System.Reflection;
using ParseTheArgs.Parsers.Options;

namespace ParseTheArgs.Tests.CustomOption
{
    public class CustomValueOptionParser : SingleValueOptionParser<CustomValue>
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public CustomValueOptionParser(PropertyInfo targetProperty, String optionName) : base(targetProperty, optionName)
        {
        }

        protected override Boolean TryParseValue(String optionValue, ParseResult parseResult, out CustomValue resultValue)
        {
            resultValue = new CustomValue(optionValue);
            return true;
        }
    }
}