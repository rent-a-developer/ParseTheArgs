using System;
using ParseTheArgs.Parsers.Commands;
using ParseTheArgs.Parsers.Options;

namespace ParseTheArgs.Setup.Options
{
    /// <summary>
    /// Represents the configuration of an option that accepts one or more <see cref="String" /> values.
    /// </summary>
    /// <typeparam name="TCommandOptions">The type in which the options of the command the option belongs to will be stored.</typeparam>
    public class StringListOptionSetup<TCommandOptions> : MultiValueOptionSetup<TCommandOptions, StringListOptionParser, StringListOptionSetup<TCommandOptions>, String>
        where TCommandOptions : class
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="commandParser">The parser for the command the option belongs to.</param>
        /// <param name="optionParser">The parser for the option.</param>
        /// <exception cref="ArgumentNullException"><paramref name="commandParser"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="optionParser"/> is null.</exception>
        public StringListOptionSetup(CommandParser<TCommandOptions> commandParser, StringListOptionParser optionParser) : base(commandParser, optionParser)
        {
        }
    }
}