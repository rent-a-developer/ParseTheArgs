using System;

namespace ParseTheArgs.Demo
{
    public class Program
    {
        public static Int32 Main(string[] args)
        {
            var parser = new Parser();

            parser.Setup
                .ProgramName("Toolbox")
                .Banner("Provides various helpful tools.");

            QueryWebCommand.SetupCommand(parser.Setup);
            DateCommand.SetupCommand(parser.Setup);
            ConvertGuidCommand.SetupCommand(parser.Setup);
            LongestWordCommand.SetupCommand(parser.Setup);
            FileReplaceCommand.SetupCommand(parser.Setup);
            
            var parseResult = parser.Parse(args);

            parseResult.CommandHandler((ConvertGuidCommandOptions options) => ConvertGuidCommand.ConvertGuid(options));
            parseResult.CommandHandler((DateCommandOptions options) => DateCommand.GetDate(options));
            parseResult.CommandHandler((FileReplaceCommandOptions options) => FileReplaceCommand.ReplaceFile(options));
            parseResult.CommandHandler((LongestWordCommandOptions options) => LongestWordCommand.LongestWord(options));
            parseResult.CommandHandler((QueryWebCommandOptions options) => QueryWebCommand.QueryWeb(options));
            parseResult.ErrorHandler((ParseResult result) => 1);

            return parseResult.Handle();
        }
    }
}
