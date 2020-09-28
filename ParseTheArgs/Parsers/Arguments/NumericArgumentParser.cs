using System;
using System.Globalization;
using System.Reflection;

namespace ParseTheArgs.Parsers.Arguments
{
    /// <summary>
    /// Parses a command line argument that accepts a single numeric value of the type <typeparamref name="TValue" />.
    /// </summary>
    /// <typeparam name="TValue">The specific numeric type the parser parses.</typeparam>
    public abstract class NumericArgumentParser<TValue> : SingleValueArgumentParser<TValue>
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="targetProperty">The property where the value of the argument will be stored.</param>
        /// <param name="argumentName">The name of the argument the parser parses.</param>
        protected NumericArgumentParser(PropertyInfo targetProperty, String argumentName) : base(targetProperty, argumentName)
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