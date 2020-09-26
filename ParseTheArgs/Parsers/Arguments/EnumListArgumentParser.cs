using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ParseTheArgs.Errors;

namespace ParseTheArgs.Parsers.Arguments
{
    /// <summary>
    /// Parses a command line argument that accepts one or more enum members of the enum <typeparamref name="TEnum" />.
    /// </summary>
    /// <typeparam name="TEnum">The type of the enum the parser accepts as argument values.</typeparam>
    public class EnumListArgumentParser<TEnum> : MultiValueArgumentParser<TEnum>
        where TEnum : struct, Enum
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="targetProperty">The property where the value of the argument will be stored.</param>
        /// <param name="argumentName">The name of the argument the parser parses.</param>
        public EnumListArgumentParser(PropertyInfo targetProperty, ArgumentName argumentName) : base(targetProperty, argumentName)
        {
            this.EnumValuesHelps = new Dictionary<TEnum, String>();
        }

        /// <summary>
        /// Defines the help texts for individual enum member of the type <typeparamref name="TEnum" />.
        /// </summary>
        public Dictionary<TEnum, String> EnumValuesHelps { get; }

        /// <summary>
        /// Gets the help text of the argument.
        /// </summary>
        /// <returns>The help text of the argument.</returns>
        public override String GetHelpText()
        {
            var result = new StringBuilder();
            result.Append(this.ArgumentHelp);

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
        /// Parses the given value to the desired argument value type of the argument parser.
        /// </summary>
        /// <param name="argumentValue">The argument value to parse.</param>
        /// <param name="parseResult">The parse result to put parse errors in if a parse error occurred.</param>
        /// <param name="resultValue">The parsed value.</param>
        /// <returns>True if the given argument value could be parsed; otherwise false.</returns>
        protected override Boolean TryParseValue(String argumentValue, ParseResult parseResult, out TEnum resultValue)
        {
            if (!Enum.TryParse(argumentValue, true, out resultValue))
            {
                parseResult.AddError(new ArgumentValueFormatError(this.ArgumentName, argumentValue, "One of the valid options (see help)"));
                resultValue = default;
                return false;
            }

            return true;
        }
    }
}