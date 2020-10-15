using System;
using ParseTheArgs.Parsers.Commands;
using ParseTheArgs.Parsers.Options;

namespace ParseTheArgs.Setup.Options
{
    /// <summary>
    /// Represents the configuration of a valueless (switch) command line option.
    /// The target property will be set to true when the option is present, otherwise the target property will be set to false.
    /// </summary>
    /// <typeparam name="TCommandOptions">The type in which the options of the command the option belongs to will be stored.</typeparam>
    public class BooleanOptionSetup<TCommandOptions> : OptionSetup<TCommandOptions, BooleanOptionParser, BooleanOptionSetup<TCommandOptions>>
        where TCommandOptions : class
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="commandParser">The parser for the command the option belongs to.</param>
        /// <param name="optionParser">The parser for the option.</param>
        /// <exception cref="ArgumentNullException"><paramref name="commandParser" /> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="optionParser" /> is null.</exception>
        public BooleanOptionSetup(CommandParser<TCommandOptions> commandParser, BooleanOptionParser optionParser) : base(commandParser, optionParser)
        {
        }
    }
}