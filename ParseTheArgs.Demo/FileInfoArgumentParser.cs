using System;
using System.IO;
using System.Reflection;
using ParseTheArgs.Parsers.Arguments;

namespace ParseTheArgs.Demo
{
    public class FileInfoArgumentParser : SingleValueArgumentParser<FileInfo>
    {
        public FileInfoArgumentParser(PropertyInfo targetProperty, String argumentName) : base(targetProperty, argumentName)
        {
        }

        protected override Boolean TryParseValue(String argumentValue, ParseResult parseResult, out FileInfo resultValue)
        {
            resultValue = new FileInfo(argumentValue);
            return true;
        }
    }
}