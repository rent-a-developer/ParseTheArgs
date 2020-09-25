using System;
using ParseTheArgs.Parsers.Arguments;

namespace ParseTheArgs.Tests.CustomArgument
{
    public class CustomValueArgumentParser<TCommandArguments> : SingleValueArgumentParser<TCommandArguments, CustomValue>
    {
        protected override Boolean TryParseValue(String argumentValue, ParseResult parseResult, out CustomValue resultValue)
        {
            resultValue = new CustomValue(argumentValue);
            return true;
        }
    }
}