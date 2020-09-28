using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ParseTheArgs.Errors;
using ParseTheArgs.Tokens;

namespace ParseTheArgs.Parsers.Options
{
    /// <summary>
    /// Parses a command line option that accepts a single <typeparamref name="TOptionValue" /> value.
    /// </summary>
    /// <typeparam name="TOptionValue">The type of the option value.</typeparam>
    public abstract class SingleValueOptionParser<TOptionValue> : OptionParser
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="targetProperty">The property where the value of the option will be stored.</param>
        /// <param name="optionName">The name of the option the parser parses.</param>
        protected SingleValueOptionParser(PropertyInfo targetProperty, String optionName) : base(targetProperty, optionName)
        {
            this.optionDefaultValue = default!;
        }

        /// <summary>
        /// Defines the default value to use for the option when the option is not given.
        /// </summary>
        public TOptionValue OptionDefaultValue
        {
            get => this.optionDefaultValue;
            set
            {
                this.optionDefaultValue = value;
                this.isOptionDefaultValueSet = true;
            }
        }

        /// <summary>
        /// The type of the option the parser parses.
        /// </summary>
        public override OptionType OptionType => OptionType.SingleValueOption;

        /// <summary>
        /// Parses the given tokens and puts the result of the parsing into the given parse result object.
        /// </summary>
        /// <param name="tokens">The tokens to parse.</param>
        /// <param name="parseResult">The parse result to put result of the parsing into.</param>
        public override void Parse(List<Token> tokens, ParseResult parseResult)
        {
            var optionToken = tokens.OfType<OptionToken>().FirstOrDefault(a => this.OptionName == a.OptionName);

            if (optionToken != null)
            {
                optionToken.IsParsed = true;

                if (optionToken.OptionValues.Count == 0)
                {
                    parseResult.AddError(new OptionValueMissingError(this.OptionName));
                }
                else if (optionToken.OptionValues.Count > 1)
                {
                    parseResult.AddError(new OptionMultipleValuesError(this.OptionName));
                }
                else
                {
                    if (this.TryParseValue(optionToken.OptionValues[0], parseResult, out var resultValue))
                    {
                        this.TargetProperty.SetValue(parseResult.CommandOptions, resultValue);
                    }
                }
            }
            else if (this.IsOptionRequired)
            {
                parseResult.AddError(new OptionMissingError(this.OptionName));
            }
            else if (this.isOptionDefaultValueSet)
            {
                this.TargetProperty.SetValue(parseResult.CommandOptions, this.OptionDefaultValue);
            }
        }

        /// <summary>
        /// Parses the given value to the desired option value type of the option parser.
        /// </summary>
        /// <param name="optionValue">The option value to parse.</param>
        /// <param name="parseResult">The parse result to put parse errors in if a parse error occurred.</param>
        /// <param name="resultValue">The parsed value.</param>
        /// <returns>True if the given option value could be parsed; otherwise false.</returns>
        protected abstract Boolean TryParseValue(String optionValue, ParseResult parseResult, out TOptionValue resultValue);

        private Boolean isOptionDefaultValueSet;
        private TOptionValue optionDefaultValue;
    }
}