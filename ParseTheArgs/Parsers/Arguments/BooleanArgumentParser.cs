using System;
using System.Collections.Generic;
using System.Linq;
using ParseTheArgs.Errors;
using ParseTheArgs.Tokens;

namespace ParseTheArgs.Parsers.Arguments
{
    /// <summary>
    /// Parses a valueless (switch) command line argument.
    /// The target property will be set to true when the argument is present, otherwise the target property will be set to false.
    /// </summary>
    /// <typeparam name="TCommandArguments">The type in which the value of the argument (of the command the argument belongs to) will be stored.</typeparam>
    public class BooleanArgumentParser<TCommandArguments> : ArgumentParser<TCommandArguments>
    {
        /// <summary>
        /// Defines the default value for the argument.
        /// The default value will be assigned to the target property when the argument is not given.
        /// </summary>
        public Boolean ArgumentDefaultValue
        {
            get => this.argumentDefaultValue;
            set
            {
                this.argumentDefaultValue = value;
                this.IsArgumentDefaultValueSet = true;
            }
        }

        /// <summary>
        /// The type of the argument the parser parses.
        /// </summary>
        public override ArgumentType ArgumentType => ArgumentType.ValuelessArgument;

        /// <summary>
        /// Determines if the argument default value (<see cref="ArgumentDefaultValue" />) is set.
        /// </summary>
        public Boolean IsArgumentDefaultValueSet { get; set; }

        /// <summary>
        /// Determines if the argument is required.
        /// Since this argument type does not have a value (it is a switch argument) it can never be required.
        /// </summary>
        public override Boolean IsArgumentRequired => false;

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
                    this.TargetProperty.SetValue(parseResult.CommandArguments, true);
                }
                else
                {
                    parseResult.AddError(new InvalidArgumentError(this.ArgumentName, "This argument does not support any values."));
                }
            }
            else if (this.IsArgumentDefaultValueSet)
            {
                this.TargetProperty.SetValue(parseResult.CommandArguments, this.ArgumentDefaultValue);
            }
        }

        private Boolean argumentDefaultValue;
    }
}