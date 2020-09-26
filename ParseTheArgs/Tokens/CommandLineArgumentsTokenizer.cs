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
            var leftArgs = new List<String>(args);

            while (leftArgs.Any())
            {
                var arg = leftArgs[0];
                if (arg.StartsWith("--") || arg.StartsWith("-"))
                {
                    var argumentName = arg.StartsWith("--") ? arg.Substring(2) : arg.Substring(1);
                    var argumentValues = leftArgs.Skip(1).TakeWhile(a => !a.StartsWith("--") && !a.StartsWith("-")).ToList();

                    if (argumentValues.Count == 0)
                    {
                        yield return new ArgumentToken(argumentName);
                    }
                    else
                    {
                        yield return new ArgumentToken(argumentName, argumentValues);
                    }

                    leftArgs.RemoveAt(0);
                    leftArgs.RemoveRange(0, argumentValues.Count);
                }
                else
                {
                    var commandName = arg;
                    yield return new CommandToken(commandName);

                    leftArgs.RemoveAt(0);
                }
            }
        }
    }
}