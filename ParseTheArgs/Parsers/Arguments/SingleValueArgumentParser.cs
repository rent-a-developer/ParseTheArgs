using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ParseTheArgs.Errors;
using ParseTheArgs.Tokens;

namespace ParseTheArgs.Parsers.Arguments
{
    /// <summary>
    /// Parses a command line argument that accepts a single <typeparamref name="TArgumentValue" /> value.
    /// </summary>
    /// <typeparam name="TArgumentValue">The type of the argument value.</typeparam>
    public abstract class SingleValueArgumentParser<TArgumentValue> : ArgumentParser
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="targetProperty">The property where the value of the argument will be stored.</param>
        /// <param name="argumentName">The name of the argument the parser parses.</param>
        protected SingleValueArgumentParser(PropertyInfo targetProperty, ArgumentName argumentName) : base(targetProperty, argumentName)
        {
            this.argumentDefaultValue = default!;
        }

        /// <summary>
        /// Defines the default value to use for the argument when the argument is not given.
        /// </summary>
        public TArgumentValue ArgumentDefaultValue
        {
            get => this.argumentDefaultValue;
            set
            {
                this.argumentDefaultValue = value;
                this.isArgumentDefaultValueSet = true;
            }
        }

        /// <summary>
        /// The type of the argument the parser parses.
        /// </summary>
        public override ArgumentType ArgumentType => ArgumentType.SingleValueArgument;

        /// <summary>
        /// Parses the given tokens and puts the result of the parsing into the given parse result object.
        /// </summary>
        /// <param name="tokens">The tokens to parse.</param>
        /// <param name="parseResult">The parse result to put result of the parsing into.</param>
        public override void Parse(List<CommandLineArgumentsToken> tokens, ParseResult parseResult)
        {
            var argumentToken = tokens.OfType<ArgumentToken>().FirstOrDefault(a => this.ArgumentName.EqualsNameOrShortName(a.ArgumentName));

            if (argumentToken != null)
            {
                argumentToken.IsParsed = true;

                if (argumentToken.ArgumentValues.Count == 0)
                {
                    parseResult.AddError(new ArgumentValueMissingError(this.ArgumentName));
                }
                else if (argumentToken.ArgumentValues.Count > 1)
                {
                    parseResult.AddError(new ArgumentMultipleValuesError(this.ArgumentName));
                }
                else
                {
                    if (this.TryParseValue(argumentToken.ArgumentValues[0], parseResult, out var resultValue))
                    {
                        this.TargetProperty.SetValue(parseResult.CommandArguments, resultValue);
                    }
                }
            }
            else if (this.IsArgumentRequired)
            {
                parseResult.AddError(new ArgumentMissingError(this.ArgumentName));
            }
            else if (this.isArgumentDefaultValueSet)
            {
                this.TargetProperty.SetValue(parseResult.CommandArguments, this.ArgumentDefaultValue);
            }
        }

        /// <summary>
        /// Parses the given value to the desired argument value type of the argument parser.
        /// </summary>
        /// <param name="argumentValue">The argument value to parse.</param>
        /// <param name="parseResult">The parse result to put parse errors in if a parse error occurred.</param>
        /// <param name="resultValue">The parsed value.</param>
        /// <returns>True if the given argument value could be parsed; otherwise false.</returns>
        protected abstract Boolean TryParseValue(String argumentValue, ParseResult parseResult, out TArgumentValue resultValue);

        private Boolean isArgumentDefaultValueSet;
        private TArgumentValue argumentDefaultValue;
    }
}