using ParseTheArgs.Parsers.Commands;
using ParseTheArgs.Setup.Options;

namespace ParseTheArgs.Tests.CustomOption
{
    public class CustomValueOptionSetup<TCommandOptions> : SingleValueOptionSetup<TCommandOptions, CustomValueOptionParser, CustomValueOptionSetup<TCommandOptions>, CustomValue>
        where TCommandOptions : class
    {
        public CustomValueOptionSetup(CommandParser<TCommandOptions> commandParser, CustomValueOptionParser optionParser) : base(commandParser, optionParser)
        {
        }
    }
}