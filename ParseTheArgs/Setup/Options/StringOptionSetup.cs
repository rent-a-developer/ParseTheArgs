using System;
using ParseTheArgs.Parsers.Commands;
using ParseTheArgs.Parsers.Options;

namespace ParseTheArgs.Setup.Options
{
    /// <summary>
    /// Represents the configuration of an option that accepts a single <see cref="string" /> value.
    /// </summary>
    /// <typeparam name="TCommandOptions">The type in which the options of the command the option belongs to will be stored.</typeparam>
    public class StringOptionSetup<TCommandOptions> : SingleValueOptionSetup<TCommandOptions, StringOptionParser, StringOptionSetup<TCommandOptions>, String>
        where TCommandOptions : class
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="commandParser">The parser for the command the option belongs to.</param>
        /// <param name="optionParser">The parser for the option.</param>
        /// <exception cref="ArgumentNullException"><paramref name="commandParser" /> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="optionParser" /> is null.</exception>
        public StringOptionSetup(CommandParser<TCommandOptions> commandParser, StringOptionParser optionParser) : base(commandParser, optionParser)
        {
        }
    }
}