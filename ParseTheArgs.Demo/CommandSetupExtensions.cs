using System;
using System.IO;
using System.Linq.Expressions;
using ParseTheArgs.Setup.Commands;

namespace ParseTheArgs.Demo
{
    public static class CommandSetupExtensions
    {
        public static FileInfoArgumentSetup<TCommandArguments> Argument<TCommandArguments>(this CommandSetup<TCommandArguments> commandSetup, Expression<Func<TCommandArguments, FileInfo>> propertyExpression) where TCommandArguments : class, new()
        {
            return new FileInfoArgumentSetup<TCommandArguments>(commandSetup.CommandParser, propertyExpression);
        }
    }
}
