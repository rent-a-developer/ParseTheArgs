using System.IO;
using System.Linq.Expressions;
using ParseTheArgs.Parsers.Commands;
using ParseTheArgs.Setup.Arguments;

namespace ParseTheArgs.Demo
{
    public class FileInfoArgumentSetup<TCommandArguments> : SingleValueArgumentSetup<TCommandArguments, FileInfoArgumentParser<TCommandArguments>, FileInfoArgumentSetup<TCommandArguments>, FileInfo>
    {
        public FileInfoArgumentSetup(CommandParser<TCommandArguments> commandParser, LambdaExpression propertyExpression) : base(commandParser, propertyExpression)
        {
        }
    }
}