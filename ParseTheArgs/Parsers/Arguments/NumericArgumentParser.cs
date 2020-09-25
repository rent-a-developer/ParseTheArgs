using System;
using System.Globalization;

namespace ParseTheArgs.Parsers.Arguments
{
    /// <summary>
    /// Parses a command line argument that accepts a single numeric value of the type <typeparamref name="TValue" />.
    /// </summary>
    /// <typeparam name="TCommandArguments">The type in which the value of the argument (of the command the argument belongs to) will be stored.</typeparam>
    /// <typeparam name="TValue">The specific numeric type the parser parses.</typeparam>
    public abstract class NumericArgumentParser<TCommandArguments, TValue> : SingleValueArgumentParser<TCommandArguments, TValue>
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        protected NumericArgumentParser()
        {
            this.NumberStyles = NumberStyles.Any;
            this.FormatProvider = CultureInfo.CurrentCulture;
        }

        /// <summary>
        /// Defines the format provider to use to parse the argument value to a numeric value.
        /// The default is <see cref="CultureInfo.CurrentCulture" />.
        /// </summary>
        public IFormatProvider FormatProvider { get; set; }

        /// <summary>
        /// Defines the number styles that are permitted when parsing the argument value to a numeric value.
        /// The default is <see cref="System.Globalization.NumberStyles.Any" />.
        /// </summary>
        public NumberStyles NumberStyles { get; set; }
    }
}