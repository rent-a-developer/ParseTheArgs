using System.Linq.Expressions;
using ParseTheArgs.Parsers.Commands;
using ParseTheArgs.Setup.Arguments;

namespace ParseTheArgs.Tests.CustomArgument
{
    public class CustomValueArgumentSetup<TCommandArguments> : SingleValueArgumentSetup<TCommandArguments, CustomValueArgumentParser, CustomValueArgumentSetup<TCommandArguments>, CustomValue>
        where TCommandArguments : class
    {
        public CustomValueArgumentSetup(CommandParser<TCommandArguments> commandParser, LambdaExpression propertyExpression) : base(commandParser, propertyExpression)
        {
        }
    }
}