﻿using System;
using System.Collections.Generic;
using ParseTheArgs.Parsers.Commands;
using ParseTheArgs.Parsers.Options;

namespace ParseTheArgs.Setup.Options
{
    /// <summary>
    /// Represents the configuration of an option that accepts one or more values of the type <typeparamref name="TOptionValue" />.
    /// </summary>
    /// <typeparam name="TCommandOptions">The type in which the options of the command the option belongs to will be stored.</typeparam>
    /// <typeparam name="TOptionParser">The type of parser for the option.</typeparam>
    /// <typeparam name="TOptionSetup">The type of setup for the option.</typeparam>
    /// <typeparam name="TOptionValue">The type of the option value(s).</typeparam>
#pragma warning disable S2436 // Types and methods should not have too many generic parameters
    public abstract class MultiValueOptionSetup<TCommandOptions, TOptionParser, TOptionSetup, TOptionValue> : OptionSetup<TCommandOptions, TOptionParser, TOptionSetup>
#pragma warning restore S2436 // Types and methods should not have too many generic parameters
        where TCommandOptions : class
        where TOptionParser : MultiValueOptionParser<TOptionValue>
        where TOptionSetup : MultiValueOptionSetup<TCommandOptions, TOptionParser, TOptionSetup, TOptionValue>
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="commandParser">The parser for the command the option belongs to.</param>
        /// <param name="optionParser">The parser for the option.</param>
        /// <exception cref="ArgumentNullException"><paramref name="commandParser" /> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="optionParser" /> is null.</exception>
        protected MultiValueOptionSetup(CommandParser<TCommandOptions> commandParser, TOptionParser optionParser) : base(commandParser, optionParser)
        {
        }

        /// <summary>
        /// Sets the default value for the option.
        /// If the option is not specified on the command line the target property will be set to the given value.
        /// </summary>
        /// <param name="defaultValue">The default value for the option.</param>
        /// <returns>A reference to this instance for further configuration.</returns>
        public TOptionSetup DefaultValue(List<TOptionValue> defaultValue)
        {
            this.optionParser.OptionDefaultValue = defaultValue;
            return (TOptionSetup) this;
        }

        /// <summary>
        /// Marks that the option as required.
        /// If the option is marked as required and is not specified on the command line a parse error will be caused (<see cref="ParseResult.Errors" />).
        /// </summary>
        /// <returns>A reference to this instance for further configuration.</returns>
        public TOptionSetup IsRequired()
        {
            this.optionParser.IsOptionRequired = true;
            return (TOptionSetup) this;
        }
    }
}