using System;
using System.Collections.Generic;
using System.Linq;

namespace ParseTheArgs.Tokens
{
    /// <summary>
    /// A tokenizer for command line arguments.
    /// </summary>
    public static class CommandLineArgumentsTokenizer
    {
        /// <summary>
        /// Converts the given command line arguments to a sequence of tokens.
        /// </summary>
        /// <param name="args">The command line arguments to tokenize.</param>
        /// <returns>The tokens that represent the given command line arguments.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="args" /> is null.</exception>
        public static IEnumerable<Token> Tokenize(String[] args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            return TokenizeIterator(args);
        }

        private static IEnumerable<Token> TokenizeIterator(String[] args)
        {
            var argumentsToTokenize = new List<String>(args);

            while (argumentsToTokenize.Any())
            {
                var argument = argumentsToTokenize[0];
                
                if (argument.StartsWith("--") || argument.StartsWith("-"))
                {
                    var optionName = argument.StartsWith("--") ? argument.Substring(2) : argument.Substring(1);
                    var optionValues = argumentsToTokenize.Skip(1).TakeWhile(a => !a.StartsWith("--") && !a.StartsWith("-")).ToList();

                    if (optionValues.Count == 0)
                    {
                        yield return new OptionToken(optionName);
                    }
                    else
                    {
                        yield return new OptionToken(optionName, optionValues);
                    }

                    argumentsToTokenize.RemoveAt(0);
                    argumentsToTokenize.RemoveRange(0, optionValues.Count);
                }
                else
                {
                    var commandName = argument;
                    yield return new CommandToken(commandName);

                    argumentsToTokenize.RemoveAt(0);
                }
            }
        }
    }
}