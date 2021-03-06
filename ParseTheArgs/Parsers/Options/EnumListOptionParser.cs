using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ParseTheArgs.Errors;

namespace ParseTheArgs.Parsers.Options
{
    /// <summary>
    /// Parses a command line option that accepts one or more enum members of the enum <typeparamref name="TEnum" />.
    /// </summary>
    /// <typeparam name="TEnum">The type of the enum the parser accepts as option values.</typeparam>
    public class EnumListOptionParser<TEnum> : MultiValueOptionParser<TEnum>
        where TEnum : struct, Enum
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="targetProperty">The property where the value of the option will be stored.</param>
        /// <param name="optionName">The name of the option the parser parses.</param>
        /// <exception cref="ArgumentNullException"><paramref name="targetProperty" /> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="targetProperty" /> does not have the property type <see cref="List{T}" />.</exception>
        /// <exception cref="ArgumentException"><paramref name="optionName" /> is null or an empty string.</exception>
        public EnumListOptionParser(PropertyInfo targetProperty, String optionName) : base(targetProperty, optionName)
        {
            if (targetProperty.PropertyType != typeof(List<TEnum>))
            {
                throw new ArgumentException($"The given target property has an incompatible property type. Expected type is System.Collections.Generic.List<{typeof(TEnum).FullName}>, actual type was {targetProperty.PropertyType.FullName}.", nameof(targetProperty));
            }

            this.EnumValuesHelps = new Dictionary<TEnum, String>();
        }

        /// <summary>
        /// Defines the help texts for individual enum member of the type <typeparamref name="TEnum" />.
        /// </summary>
        public virtual IDictionary<TEnum, String> EnumValuesHelps { get; }

        /// <summary>
        /// Gets the help text of the option.
        /// </summary>
        /// <returns>The help text of the option.</returns>
        public override String GetHelpText()
        {
            var result = new StringBuilder();
            result.Append(this.OptionHelp);

            if (this.EnumValuesHelps.Any())
            {
                result.AppendLine($" Possible values: {String.Join(", ", this.EnumValuesHelps.Keys)}.");

                foreach (var enumValuesHelp in this.EnumValuesHelps)
                {
                    result.AppendLine($"{enumValuesHelp.Key}: {enumValuesHelp.Value}");
                }
            }
            else
            {
                var enumMembers = Enum.GetNames(typeof(TEnum));
                result.Append($" Possible values: {String.Join(", ", enumMembers)}.");
            }

            return result.ToString();
        }

        /// <summary>
        /// Parses the given value to the desired option value type of the option parser.
        /// </summary>
        /// <param name="optionValue">The option value to parse.</param>
        /// <param name="parseResult">The object where to put parse errors in if a parse error occurred.</param>
        /// <param name="resultValue">The parsed value.</param>
        /// <returns>True if the given option value could be parsed; otherwise false.</returns>
        protected override Boolean TryParseValue(String optionValue, ParseResult parseResult, out TEnum resultValue)
        {
            if (!this.ValueParser.TryParseEnum(optionValue, out resultValue))
            {
                parseResult.AddError(new OptionValueInvalidFormatError(this.OptionName, optionValue, "One of the valid values (see help)"));
                resultValue = default;
                return false;
            }

            return true;
        }
    }
}