using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ParseTheArgs.Errors;
using ParseTheArgs.Tokens;

namespace ParseTheArgs.Parsers.Arguments
{
    /// <summary>
    /// Parses a valueless (switch) command line argument.
    /// The target property will be set to true when the argument is present, otherwise the target property will be set to false.
    /// </summary>
    public class BooleanArgumentParser : ArgumentParser
    {
        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="targetProperty">The property where the value of the argument will be stored.</param>
        /// <param name="argumentName">The name of the argument the parser parses.</param>
        public BooleanArgumentParser(PropertyInfo targetProperty, ArgumentName argumentName) : base(targetProperty, argumentName)
        {
        }

        /// <summary>
        /// The type of the argument the parser parses.
        /// </summary>
        public override ArgumentType ArgumentType => ArgumentType.ValuelessArgument;

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
        public override void Parse(List<Token> tokens, ParseResult parseResult)
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
        }
    }
}