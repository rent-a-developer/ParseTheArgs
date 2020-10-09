using System.IO;
using ParseTheArgs.Parsers.Commands;
using ParseTheArgs.Setup.Options;

namespace ParseTheArgs.Demo
{
    public class FileInfoOptionSetup<TCommandOptions> : SingleValueOptionSetup<TCommandOptions, FileInfoOptionParser, FileInfoOptionSetup<TCommandOptions>, FileInfo>
        where TCommandOptions : class
    {
        public FileInfoOptionSetup(CommandParser<TCommandOptions> commandParser, FileInfoOptionParser optionParser) : base(commandParser, optionParser)
        {
        }
    }
}