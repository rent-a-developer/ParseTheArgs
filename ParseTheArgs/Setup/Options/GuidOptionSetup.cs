using System;
using ParseTheArgs.Parsers.Commands;
using ParseTheArgs.Parsers.Options;

namespace ParseTheArgs.Setup.Options
{
    /// <summary>
    /// Represents the configuration of an option that accepts a single <see cref="Guid" /> value.
    /// </summary>
    /// <typeparam name="TCommandOptions">The type in which the options of the command the option belongs to will be stored.</typeparam>
    public class GuidOptionSetup<TCommandOptions> : SingleValueOptionSetup<TCommandOptions, GuidOptionParser, GuidOptionSetup<TCommandOptions>, Guid>
        where TCommandOptions : class
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="commandParser">The parser for the command the option belongs to.</param>
        /// <param name="optionParser">The parser for the option.</param>
        /// <exception cref="ArgumentNullException"><paramref name="commandParser" /> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="optionParser" /> is null.</exception>
        public GuidOptionSetup(CommandParser<TCommandOptions> commandParser, GuidOptionParser optionParser) : base(commandParser, optionParser)
        {
        }

        /// <summary>
        /// Sets the format that is accepted when the option value is parsed to a <see cref="Guid" />.
        /// For supported formats see the documentation of <see cref="Guid.Parse(string)" />.
        /// </summary>
        /// <param name="guidFormat">The format that is accepted when the option value is parsed to a <see cref="Guid" />.</param>
        /// <returns>A reference to this instance for further configuration.</returns>
        public GuidOptionSetup<TCommandOptions> Format(String guidFormat)
        {
            this.optionParser.GuidFormat = guidFormat;
            return this;
        }
    }
}