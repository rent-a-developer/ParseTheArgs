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
            var targetProperty = ExpressionHelper.GetPropertyFromPropertyExpression(propertyExpression);
            var optionParser = commandSetup.commandParser.GetOrCreateOptionParser<FileInfoOptionParser>(targetProperty);

            return new FileInfoOptionSetup<TCommandOptions>(commandSetup.commandParser, optionParser);
        }
    }
}
