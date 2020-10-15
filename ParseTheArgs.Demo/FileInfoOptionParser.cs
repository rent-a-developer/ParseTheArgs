using System;
using System.IO;
using System.Reflection;
using ParseTheArgs.Parsers.Options;

namespace ParseTheArgs.Demo
{
    public class FileInfoOptionParser : SingleValueOptionParser<FileInfo>
    {
        public FileInfoOptionParser(PropertyInfo targetProperty, String optionName) : base(targetProperty, optionName)
        {
        }

        protected override Boolean TryParseValue(String optionValue, ParseResult parseResult, out FileInfo resultValue)
        {
            resultValue = new FileInfo(optionValue);
            return true;
        }
    }
}