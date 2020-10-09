using System;
using ParseTheArgs.Parsers.Commands;
using ParseTheArgs.Parsers.Options;

namespace ParseTheArgs.Setup.Options
{
    /// <summary>
    /// Represents the configuration of an option that accepts one or more <see cref="Guid" /> values.
    /// </summary>
    /// <typeparam name="TCommandOptions">The type in which the options of the command the option belongs to will be stored.</typeparam>
    public class GuidListOptionSetup<TCommandOptions> : MultiValueOptionSetup<TCommandOptions, GuidListOptionParser, GuidListOptionSetup<TCommandOptions>, Guid>
        where TCommandOptions : class
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="commandParser">The parser for the command the option belongs to.</param>
        /// <param name="optionParser">The parser for the option.</param>
        /// <exception cref="ArgumentNullException"><paramref name="commandParser"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="optionParser"/> is null.</exception>
        public GuidListOptionSetup(CommandParser<TCommandOptions> commandParser, GuidListOptionParser optionParser) : base(commandParser, optionParser)
        {
        }

        /// <summary>
        /// Sets the format that is accepted when the option values are parsed to <see cref="Guid" /> values.
        /// For supported formats see the documentation of <see cref="Guid.Parse(string)" />.
        /// </summary>
        /// <param name="guidFormat">The format that is accepted when the option values are parsed to <see cref="Guid" /> values.</param>
        /// <returns>A reference to this instance for further configuration.</returns>
        public GuidListOptionSetup<TCommandOptions> Format(String guidFormat)
        {
            this.optionParser.GuidFormat = guidFormat;
            return this;
        }
    }
}