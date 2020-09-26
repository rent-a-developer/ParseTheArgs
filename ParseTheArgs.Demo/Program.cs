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

            parseResult.Handle(
                (ConvertGuidCommandArguments arguments) =>
                {
                    return "1";
                },
                (ParseResult result) =>
                {
                    return "1";
                }
            );

            return parseResult.Handle(
                (ConvertGuidCommandArguments arguments) => ConvertGuidCommand.ConvertGuid(arguments),
                (DateCommandArguments arguments) => DateCommand.GetDate(arguments),
                (FileReplaceCommandArguments arguments) => FileReplaceCommand.ReplaceFile(arguments),
                (LongestWordCommandArguments arguments) => LongestWordCommand.LongestWord(arguments),
                (QueryWebCommandArguments arguments) => QueryWebCommand.QueryWeb(arguments),
                (ParseResult result) =>
                {
                    return 1;
                }
            );
        }
    }
}
