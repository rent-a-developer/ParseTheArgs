using System;
using System.Linq;
using ParseTheArgs.Setup;

namespace ParseTheArgs.Demo
{
    public static class LongestWordCommand
    {
        public static Int32 LongestWord(LongestWordCommandArguments arguments)
        {
            try
            {
                var longestWord = arguments.Words.OrderBy(a => a.Length).Last();
                Console.WriteLine($"The longest word is: {longestWord}");

                return 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Unhandled exception:");
                Console.Error.WriteLine(ex.ToString());
                return 1;
            }
        }

        public static void SetupCommand(ParserSetup parserSetup)
        {
            var command = parserSetup
                .Command<LongestWordCommandArguments>()
                .Name("longestWord")
                .Help("Gets the longest word of a list of words.")
                .ExampleUsage("Tool longestWord --words cat dog fish crocodile");

            command
                .Argument(a => a.Words)
                .Name("words")
                .ShortName('w')
                .Help("The list of words.")
                .IsRequired();
        }
    }
}
