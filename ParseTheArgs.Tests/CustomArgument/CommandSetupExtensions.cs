using System;
using System.Linq.Expressions;
using ParseTheArgs.Setup.Commands;

namespace ParseTheArgs.Tests.CustomArgument
{
    public static class CommandSetupExtensions
    {
        public static CustomValueArgumentSetup<TCommandArguments> Argument<TCommandArguments>(this CommandSetup<TCommandArguments> commandSetup, Expression<Func<TCommandArguments, CustomValue>> propertyExpression) where TCommandArguments : class, new()
        {
            return new CustomValueArgumentSetup<TCommandArguments>(commandSetup.CommandParser, propertyExpression);
        }
    }
}