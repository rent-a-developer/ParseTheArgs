using System;
using System.Linq;
using ParseTheArgs.Setup;

namespace ParseTheArgs.Demo
{
    public static class LongestWordCommand
    {
        public static Int32 LongestWord(LongestWordCommandOptions options)
        {
            try
            {
                var longestWord = options.Words.OrderBy(a => a.Length).Last();
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
                .Command<LongestWordCommandOptions>()
                .Name("longestWord")
                .Help("Gets the longest word of a list of words.")
                .ExampleUsage("Tool longestWord --words cat dog fish crocodile");

            command
                .Option(a => a.Words)
                .Name("words")
                .Help("The list of words.")
                .IsRequired();
        }
    }
}
