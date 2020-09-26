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

            parseResult.CommandHandler((ConvertGuidCommandArguments arguments) => ConvertGuidCommand.ConvertGuid(arguments));
            parseResult.CommandHandler((DateCommandArguments arguments) => DateCommand.GetDate(arguments));
            parseResult.CommandHandler((FileReplaceCommandArguments arguments) => FileReplaceCommand.ReplaceFile(arguments));
            parseResult.CommandHandler((LongestWordCommandArguments arguments) => LongestWordCommand.LongestWord(arguments));
            parseResult.CommandHandler((QueryWebCommandArguments arguments) => QueryWebCommand.QueryWeb(arguments));
            parseResult.ErrorHandler((ParseResult result) => 1);

            return parseResult.Handle();
        }
    }
}
