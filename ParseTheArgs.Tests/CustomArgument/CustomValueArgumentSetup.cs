using System.Linq.Expressions;
using ParseTheArgs.Parsers.Commands;
using ParseTheArgs.Setup.Arguments;

namespace ParseTheArgs.Tests.CustomArgument
{
    public class CustomValueArgumentSetup<TCommandArguments> : SingleValueArgumentSetup<TCommandArguments, CustomValueArgumentParser<TCommandArguments>, CustomValueArgumentSetup<TCommandArguments>, CustomValue>
    {
        public CustomValueArgumentSetup(CommandParser<TCommandArguments> commandParser, LambdaExpression propertyExpression) : base(commandParser, propertyExpression)
        {
        }
    }
}