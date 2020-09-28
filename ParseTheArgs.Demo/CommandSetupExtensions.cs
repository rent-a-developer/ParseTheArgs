using System;
using System.IO;
using System.Linq.Expressions;
using ParseTheArgs.Setup.Commands;

namespace ParseTheArgs.Demo
{
    public static class CommandSetupExtensions
    {
        public static FileInfoOptionSetup<TCommandOptions> Option<TCommandOptions>(this CommandSetup<TCommandOptions> commandSetup, Expression<Func<TCommandOptions, FileInfo>> propertyExpression) where TCommandOptions : class, new()
        {
            return new FileInfoOptionSetup<TCommandOptions>(commandSetup.CommandParser, propertyExpression);
        }
    }
}
