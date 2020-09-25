using System;
using System.IO;
using ParseTheArgs.Parsers.Arguments;

namespace ParseTheArgs.Demo
{
    public class FileInfoArgumentParser<TCommandArguments> : SingleValueArgumentParser<TCommandArguments, FileInfo>
    {
        protected override Boolean TryParseValue(String argumentValue, ParseResult parseResult, out FileInfo resultValue)
        {
            resultValue = new FileInfo(argumentValue);
            return true;
        }
    }
}