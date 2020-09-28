using System;
using System.Linq.Expressions;
using ParseTheArgs.Setup.Commands;

namespace ParseTheArgs.Tests.CustomOption
{
    public static class CommandSetupExtensions
    {
        public static CustomValueOptionSetup<TCommandOptions> Option<TCommandOptions>(this CommandSetup<TCommandOptions> commandSetup, Expression<Func<TCommandOptions, CustomValue>> propertyExpression) where TCommandOptions : class, new()
        {
            return new CustomValueOptionSetup<TCommandOptions>(commandSetup.CommandParser, propertyExpression);
        }
    }
}