﻿using System;
using System.Globalization;
using System.Linq.Expressions;
using ParseTheArgs.Parsers.Commands;
using ParseTheArgs.Parsers.Options;

namespace ParseTheArgs.Setup.Options
{
    /// <summary>
    /// Represents the configuration of an option that accepts one or more <see cref="long" /> values.
    /// </summary>
    /// <typeparam name="TCommandOptions">The type in which the options of the command the option belongs to will be stored.</typeparam>
    public class Int64ListOptionSetup<TCommandOptions> : MultiValueOptionSetup<TCommandOptions, Int64ListOptionParser, Int64ListOptionSetup<TCommandOptions>, Int64>
        where TCommandOptions : class
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="commandParser">The parser for the command the option belongs to.</param>
        /// <param name="propertyExpression">An expression that points to a property (the target property) of the <typeparamref name="TCommandOptions" /> type in which the option value should be stored.</param>
        public Int64ListOptionSetup(CommandParser<TCommandOptions> commandParser, LambdaExpression propertyExpression) : base(commandParser, propertyExpression)
        {
        }

        /// <summary>
        /// Sets the format provider to be used to parse the option value to <see cref="Int64" /> values.
        /// The default is <see cref="CultureInfo.CurrentCulture" />.
        /// </summary>
        /// <param name="formatProvider">The format provider to be used to parse the option value to <see cref="Int64" /> values.</param>
        /// <returns>A reference to this instance for further configuration.</returns>
        public Int64ListOptionSetup<TCommandOptions> FormatProvider(IFormatProvider formatProvider)
        {
            this.OptionParser.FormatProvider = formatProvider;
            return this;
        }

        /// <summary>
        /// Sets the number styles that are permitted when parsing the option value to <see cref="Int64" /> values.
        /// The default is <see cref="NumberStyles.Any" />.
        /// </summary>
        /// <param name="numberStyles">The number styles that are permitted when parsing the option value to <see cref="Int64" /> values.</param>
        /// <returns>A reference to this instance for further configuration.</returns>
        public Int64ListOptionSetup<TCommandOptions> Styles(NumberStyles numberStyles)
        {
            this.OptionParser.NumberStyles = numberStyles;
            return this;
        }
    }
}