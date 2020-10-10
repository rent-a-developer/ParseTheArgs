using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ParseTheArgs.Errors;
using ParseTheArgs.Tokens;

namespace ParseTheArgs.Parsers.Options
{
    /// <summary>
    /// Parses a valueless (switch) command line option.
    /// The target property will be set to true when the option is present, otherwise the target property will be set to false.
    /// </summary>
    public class BooleanOptionParser : OptionParser
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="targetProperty">The property where the value of the option will be stored.</param>
        /// <param name="optionName">The name of the option the parser parses.</param>
        /// <exception cref="ArgumentNullException"><paramref name="targetProperty"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="targetProperty"/> does not have the property type <see cref="Boolean"/> or <see cref="Nullable{Boolean}"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="optionName"/> is null or an empty string.</exception>
        public BooleanOptionParser(PropertyInfo targetProperty, String optionName) : base(targetProperty, optionName)
        {
            if (targetProperty.PropertyType != typeof(Boolean) && targetProperty.PropertyType != typeof(Nullable<Boolean>))
            {
                throw new ArgumentException($"The given target property has an incompatible property type. Expected type is System.Boolean or System.Nullable<System.Boolean>, actual type was {targetProperty.PropertyType.FullName}.", nameof(targetProperty));
            }
        }

        /// <summary>
        /// The type of the option the parser parses.
        /// </summary>
        public override OptionType OptionType => OptionType.ValuelessOption;

        /// <summary>
        /// Determines if the option is required.
        /// Since this option type does not have a value (it is a switch option) it can never be required.
        /// </summary>
        public override Boolean IsOptionRequired => false;

        /// <summary>
        /// Parses the given tokens and puts the result of the parsing into the given parse result object.
        /// </summary>
        /// <param name="tokens">The tokens to parse.</param>
        /// <param name="parseResult">The object where to put result of the parsing into.</param>
        public override void Parse(List<Token> tokens, ParseResult parseResult)
        {
            var optionToken = tokens.OfType<OptionToken>().FirstOrDefault(a => this.OptionName == a.OptionName);

            if (optionToken != null)
            {
                optionToken.IsParsed = true;

                if (optionToken.OptionValues.Count == 0)
                {
                    this.TargetProperty.SetValue(parseResult.CommandOptions, true);
                }
                else
                {
                    parseResult.AddError(new InvalidOptionError(this.OptionName, "This option does not support any values."));
                }
            }
        }
    }
}