using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using ParseTheArgs.Errors;
using ParseTheArgs.Parsers.Options;
using ParseTheArgs.Tests.TestData;
using ParseTheArgs.Tokens;

namespace ParseTheArgs.Tests.Parsers.Options
{
    [TestFixture]
    public class BooleanOptionParserTests
    {
        [Test(Description = "Parse should add an InvalidOptionError error to the parse result when an option value was specified for a Boolean (switch) option.")]
        public void Parse_ValuePresent_ShouldAddError()
        {
            var parser = new BooleanOptionParser(typeof(DataTypesCommandOptions).GetProperty("Boolean"), "boolean");

            var tokens = new List<Token>
            {
                new OptionToken("boolean")
                {
                    OptionValues = { "value" }
                }
            };
            var parseResult = new ParseResult();
            parseResult.CommandOptions = new DataTypesCommandOptions();

            parser.Parse(tokens, parseResult);

            parseResult.HasErrors.Should().BeTrue();
            parseResult.Errors.Should().HaveCount(1);
            parseResult.Errors[0].Should().BeOfType<InvalidOptionError>();

            var error = (InvalidOptionError) parseResult.Errors[0];
            error.OptionName.Should().Be("boolean");
            error.GetErrorMessage().Should().Be("The option --boolean is invalid: This option does not support any values.");
        }
    }
}