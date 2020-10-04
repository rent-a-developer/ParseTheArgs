using System;
using System.Globalization;
using System.Reflection;

namespace ParseTheArgs.Parsers.Options
{
    /// <summary>
    /// Parses a command line option that accepts one or more numeric values of the type <typeparamref name="TValue" />.
    /// </summary>
    /// <typeparam name="TValue">The specific numeric type the parser parses.</typeparam>
    public abstract class NumericListOptionParser<TValue> : MultiValueOptionParser<TValue>
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="targetProperty">The property where the value of the option will be stored.</param>
        /// <param name="optionName">The name of the option the parser parses.</param>
        protected NumericListOptionParser(PropertyInfo targetProperty, String optionName) : base(targetProperty, optionName)
        {
            this.FormatProvider = CultureInfo.CurrentCulture;
            this.NumberStyles = NumberStyles.Any;
        }

        /// <summary>
        /// Defines the format provider to use to parse the option value to a numeric value.
        /// The default is <see cref="CultureInfo.CurrentCulture" />.
        /// </summary>
        public virtual IFormatProvider FormatProvider { get; set; }

        /// <summary>
        /// Defines the number styles that are permitted when parsing the option value to a numeric value.
        /// The default is <see cref="System.Globalization.NumberStyles.Any" />.
        /// </summary>
        public virtual NumberStyles NumberStyles { get; set; }
    }
}