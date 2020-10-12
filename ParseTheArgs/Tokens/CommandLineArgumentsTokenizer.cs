using System;
using System.Collections.Generic;
using System.Linq;

namespace ParseTheArgs.Tokens
{
    /// <summary>
    /// A tokenizer for command line arguments.
    /// </summary>
    public class CommandLineArgumentsTokenizer
    {
        /// <summary>
        /// Converts the given command line arguments to a sequence of tokens.
        /// </summary>
        /// <param name="args">The command line arguments to tokenize.</param>
        /// <returns>The tokens that represent the given command line arguments.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="args" /> is null.</exception>
        public virtual List<Token> Tokenize(params String[] args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            var argumentsToTokenize = new List<String>(args);

            var result = new List<Token>();

            while (argumentsToTokenize.Any())
            {
                var argument = argumentsToTokenize[0];
                
                if (argument.StartsWith("--") || argument.StartsWith("-"))
                {
                    var optionName = argument.StartsWith("--") ? argument.Substring(2) : argument.Substring(1);
                    var optionValues = argumentsToTokenize.Skip(1).TakeWhile(a => !a.StartsWith("--") && !a.StartsWith("-")).ToList();

                    if (optionValues.Count == 0)
                    {
                        result.Add(new OptionToken(optionName));
                    }
                    else
                    {
                        result.Add(new OptionToken(optionName, optionValues));
                    }

                    argumentsToTokenize.RemoveAt(0);
                    argumentsToTokenize.RemoveRange(0, optionValues.Count);
                }
                else
                {
                    var commandName = argument;
                    result.Add(new CommandToken(commandName));

                    argumentsToTokenize.RemoveAt(0);
                }
            }

            return result;
        }
    }
}